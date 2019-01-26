namespace Sample.Core

open Xamarin.Forms
open Xamarin.Forms.Xaml

open MvvmCross.Forms.Views
open MvvmCross.Forms.Presenters.Attributes

[<MvxContentPagePresentation(NoHistory = true)>]
type MainPage() =
    inherit MvxContentPage<MainViewModel>()
    let _ = base.LoadFromXaml(typeof<MainPage>)

    do base.SetBinding(MainPage.ErrorMessageProperty, "ErrorMessage")

    static member ErrorMessageProperty = 
        BindableProperty.Create("ErrorMessage", typeof<option<string>>, typeof<MainPage>, None, 
            propertyChanged = new BindableProperty.BindingPropertyChangedDelegate(MainPage.ErrorMessageChanged))


    static member ErrorMessageChanged(bindable:obj) (oldValue:obj) (newValue:obj) = 
        let mainPage = bindable :?> MainPage

        match newValue with 
        | :? option<string> as x -> 
            x |> Option.iter(fun x -> 
                async {
                    do! mainPage.DisplayAlert("Error", x, "Ok") |> Async.AwaitTask
                    mainPage.ViewModel.ErrorMessage <- None
                } |> Async.StartImmediate
            )
        | _ -> ()
