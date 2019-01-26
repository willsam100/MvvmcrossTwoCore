namespace Sample.CoreLogin

open Xamarin.Forms

open MvvmCross.ViewModels
open MvvmCross
open FSharp.Data
open System
open Sample.Core

type App() = 
    inherit MvxApplication() 

    let r = Random()

    override this.Initialize() = 
        let response () = 
            sprintf """[{"Id": %d, "Message":"Learn F#", "Created":"2018-06-17T03:00:00Z"}]""" (r.Next(1, 5))

        Mvx.IoCProvider.RegisterSingleton<INetwork>(fun () -> 
        {
            new INetwork with 
                member this.RunRequest _ =
                    async {
                        return {
                            HttpResponse.Cookies = Map.empty
                            HttpResponse.Headers = Map.empty
                            HttpResponse.ResponseUrl = ""
                            HttpResponse.StatusCode = 200
                            HttpResponse.Body = response () |> Text 

                        }
                    }
        })

        Mvx.IoCProvider.RegisterType<INoteDownloadFlow, NoteDownloadFlow>()
        Mvx.IoCProvider.RegisterType<INoteDbFlow, NoteDbFlow>()
        Mvx.IoCProvider.RegisterType<IDatabase, Database>()
        
        let db = Mvx.IoCProvider.Resolve<IDatabase>()

        db.Run(fun db -> 
            async {
#if DEBUG
            do! db.DropTableAsync<PersitenceeNote.PersitenceNote>() |> Async.AwaitTask |> Async.Ignore
#endif
            do! db.CreateTableAsync<PersitenceeNote.PersitenceNote>() |> Async.AwaitTask |> Async.Ignore

            } |> Async.StartImmediate
        )

        this.RegisterAppStart<LoginViewModel>()
       
type FormsApp() =
    inherit Application()

