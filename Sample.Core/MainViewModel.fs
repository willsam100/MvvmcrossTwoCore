namespace Sample.Core

open MvvmCross.ViewModels
open MvvmCross.Commands
open System.Collections.ObjectModel
open System

type MainViewModel(noteDownloadFlow:INoteDownloadFlow, noteDbFlow:INoteDbFlow) as this = 
    inherit MvxViewModel()

    let notesCollection = new ObservableCollection<_>()
    let mutable errorMessage = None

    let showNotes notes = 
        notesCollection.Clear()

        notes 
        |> Seq.map (fun x -> sprintf "%s %A %d" x.Message x.Created x.Id)
        |> Seq.iter notesCollection.Add

    let getNotesCommand = MvxCommand(fun () -> 
        async {
            let! notes = noteDownloadFlow.DownloadNotes()
            match notes with 
            | Some notes -> showNotes notes
            | None -> this.ErrorMessage <- Some "Failed to download notes"
        } |> Async.StartImmediate )

    let loadNotesCommand = MvxCommand(fun () -> 
        async {
            let! notes = noteDbFlow.LoadNotes()
            showNotes notes
        } |> Async.StartImmediate)

    member this.Notes = notesCollection
    member this.ErrorMessage 
        with get() = errorMessage
        and set(value) = 
            this.RaiseAndSetIfChanged(&errorMessage, value, NameOf.get <@ this.ErrorMessage @>) |> ignore