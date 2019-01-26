namespace Sample.CoreLogin

open Xamarin.Forms
open Xamarin.Forms.Xaml
open MvvmCross.Forms.Views

type LoginPage() =
    inherit MvxContentPage<LoginViewModel>()
    let _ = base.LoadFromXaml(typeof<LoginPage>)
