namespace Sample.Core
open FSharp.Data
open System
open Newtonsoft.Json
open System.Diagnostics
open SQLite

[<Measure>]
type utcSecondsSinceEpoch

type WebError = 
    | Deserialize of exn
    | ParseNote

type Note = {
    Id:int64
    Message:string
    Created: int64<utcSecondsSinceEpoch>
}

type NetworkMethod = Get | Post of string

type RawRequest = {
    Uri: string
    Method:NetworkMethod
}

module List = 

    let traverseResultM xs = 

        match xs with 
        | [] -> Ok []
        | _ -> 

            let folder x result = 
                match result, x with 
                | Error e, _ -> Error e
                | _, Error e -> Error e
                | Ok xs, Ok x -> x :: xs |> Ok
                
            List.foldBack folder xs (Ok [])


module Time = 

    let toUtcSeocndsSinceEpoch (dateTime:DateTime) = 
        let epoch = DateTime(1970, 1, 1, 0, 0,0, 0, DateTimeKind.Utc)
        let span = (dateTime.ToUniversalTime()) - epoch
        let seconds = span.TotalSeconds |> int64
        seconds * 1L<utcSecondsSinceEpoch>
        
module Note =

    type ServerNote() = 
        member val Id = -1L with get,set
        member val Message = "" with get,set
        member val Created = DateTime.MinValue with get,set

    let createGetAllNotesRequest baseUrl = 
        {
            Uri = sprintf "%s/note" baseUrl
            Method = Get
        }

    let private parseResponse response = 
        try 
            JsonConvert.DeserializeObject<seq<ServerNote>> response |> Ok
        with 
        | e -> 
            e |> Deserialize |> Error

    let private parseServerNoteToDomainNote (serverNote:ServerNote) = 

        if String.IsNullOrEmpty serverNote.Message |> not && serverNote.Created <> DateTime.MinValue then
            {
                Id = serverNote.Id
                Message = serverNote.Message
                Created = Time.toUtcSeocndsSinceEpoch serverNote.Created
            } |> Ok
        else
            ParseNote |> Error

    let parseGetAllNotesResponse response = 
        match response.Body with 
        | Binary _ -> [] |> Ok
        | Text t -> 
            t 
            |> parseResponse
            |> Result.map (Seq.map parseServerNoteToDomainNote)
            |> Result.map List.ofSeq
            |> Result.bind List.traverseResultM

module PersitenceeNote = 

    type PersitenceNote() = 
        [<PrimaryKey>]
        member val Id = -1L with get,set
        member val Message = "" with get,set
        member val Created = -1L with get, set

    let domainNoteToPersistentNote (note:Note) = 
        PersitenceNote(
                        Id = note.Id,
                        Message = note.Message,
                        Created = note.Created / 1L<utcSecondsSinceEpoch>
            )

    let persistentNoteToDomainNote (pNote:PersitenceNote) = 
        {
            Id = pNote.Id
            Message = pNote.Message
            Created = pNote.Created * 1L<utcSecondsSinceEpoch>
        }

    let saveNote (notes:Note list) (db:SQLiteAsyncConnection) = 

        db.RunInTransactionAsync (fun db -> 
            notes 
            |> List.map domainNoteToPersistentNote
            |> List.map db.InsertOrReplace
            |> ignore

        ) |> Async.AwaitTask |> Async.StartImmediate

    let loadNotes (db:SQLiteAsyncConnection) = 
        async {
            let! dbNotes = db.Table<PersitenceNote>().ToListAsync() |> Async.AwaitTask
            return dbNotes |> Seq.map persistentNoteToDomainNote
        }


type INetwork = 
    abstract member RunRequest: RawRequest -> HttpResponse Async
