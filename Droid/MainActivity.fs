namespace Sample.Droid
open System

open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS
open Xamarin.Forms.Platform.Android
open System.IO

open MvvmCross.Forms.Platforms.Android.Core
open MvvmCross.Forms.Platforms.Android.Views
open MvvmCross
open Sample.Core
open Sample.CoreLogin
open System.Reflection
open System.Collections.Generic
open MvvmCross.Views

type Resources = Sample.Droid.Resource

type AndroidDatabasePath() = 
    interface IDatabasePath with
        member this.Directory = 
            Environment.GetFolderPath(Environment.SpecialFolder.Personal)

type Setup() = 
    inherit MvxFormsAndroidSetup<App, FormsApp>()

    //https://stackoverflow.com/questions/41669428/providing-additional-view-assemblies-to-mvvmcross
    override this.InitializeViewLookup() =
        let viewsLookup = new Dictionary<Type, Type>()
        viewsLookup.[typeof<MainViewModel>] <- typeof<MainPage>
        viewsLookup.[typeof<LoginViewModel>] <- typeof<LoginPage>

        let container = Mvx.IoCProvider.Resolve<IMvxViewsContainer> ()        
        container.AddAll(viewsLookup)


    override this.InitializeIoC() = 
        base.InitializeIoC()

        Mvx.IoCProvider.RegisterType<IDatabasePath, AndroidDatabasePath>()

[<Activity (Label = "Sample.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type RootActivity() =
    inherit MvxFormsAppCompatActivity<Setup, App, FormsApp>()

    override this.OnCreate (bundle: Bundle) =
        FormsAppCompatActivity.TabLayoutResource <- Resources.Layout.Tabbar
        FormsAppCompatActivity.ToolbarResource <- Resources.Layout.Toolbar

        base.OnCreate (bundle)
