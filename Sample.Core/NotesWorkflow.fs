namespace Sample.Core
open System.Diagnostics

type INoteDownloadFlow = 
    abstract member DownloadNotes: unit -> Note seq option Async
    
type NoteDownloadFlow(network: INetwork, db:IDatabase) = 

    let baseUrl = "http://127.0.0.1"

    let getAllNotes() = 
        async {
            let request = Note.createGetAllNotesRequest baseUrl
            let! response = network.RunRequest request

            let notes = response |> Note.parseGetAllNotesResponse
            match notes with 
            | Ok notes -> 
                notes |> PersitenceeNote.saveNote |> db.Run
                return notes |> Seq.ofList |> Some
            | Error e -> 
                sprintf "Error: %A" e |> Debug.WriteLine
                return None
        }

    interface INoteDownloadFlow with
        member this.DownloadNotes () = 
            getAllNotes()

type INoteDbFlow = 
    abstract member LoadNotes: unit -> Note seq Async

type NoteDbFlow(db:IDatabase) = 

    let loadNotes () = 
        db.Run PersitenceeNote.loadNotes

    interface INoteDbFlow with
        member this.LoadNotes () = 
            loadNotes()