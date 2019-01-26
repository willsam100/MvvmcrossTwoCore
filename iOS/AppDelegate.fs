namespace Sample.iOS

open System
open UIKit
open Foundation
open Xamarin.Forms
open Xamarin.Forms.Platform.iOS

open MvvmCross.Forms.Platforms.Ios.Core
open System.IO
open Sample.Core
open Sample.CoreLogin
open System.Collections.Generic
open MvvmCross.Views
open MvvmCross

type IosDatabasePath() = 
    interface IDatabasePath with 
        member this.Directory = 
            let docs  = Environment.GetFolderPath(Environment.SpecialFolder.Personal)
            Path.Combine(docs, "..", "Library")

type Setup() = 
    inherit MvxFormsIosSetup<App, FormsApp>()

    //https://stackoverflow.com/questions/41669428/providing-additional-view-assemblies-to-mvvmcross
    override this.InitializeViewLookup() =
        let viewsLookup = new Dictionary<Type, Type>()
        viewsLookup.[typeof<MainViewModel>] <- typeof<MainPage>
        viewsLookup.[typeof<LoginViewModel>] <- typeof<LoginPage>

        let container = Mvx.IoCProvider.Resolve<IMvxViewsContainer> ()        
        container.AddAll(viewsLookup)

    override this.InitializeIoC() = 
        base.InitializeIoC()
        Mvx.IoCProvider.RegisterType<IDatabasePath, IosDatabasePath>()

[<Register ("AppDelegate")>]
type AppDelegate () =
    inherit MvxFormsApplicationDelegate<Setup, App, FormsApp> ()

    override this.FinishedLaunching (app, options) =
        base.FinishedLaunching(app, options)

module Main =
    [<EntryPoint>]
    let main args =
        UIApplication.Main(args, null, "AppDelegate")
        0
