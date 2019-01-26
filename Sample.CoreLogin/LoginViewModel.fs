namespace Sample.CoreLogin

open System
open MvvmCross.ViewModels
open MvvmCross.Navigation
open MvvmCross.Commands
open Sample.Core

type LoginViewModel(navigationSerice:IMvxNavigationService) = 
    inherit MvxViewModel()

    let mutable username = ""
    let mutable password = ""

    let navigateToMainViewModel () = 

        if username.ToLower() = "admin" && password.ToLower() = "admin" then 
            navigationSerice.Navigate<MainViewModel>() |> ignore

    let loginCommand = new MvxCommand(Action(navigateToMainViewModel)) 

    member this.Username 
        with get() = username 
        and set(value) = 
            this.SetProperty(&username, value, NameOf.get( <@ this.Username @> )) |> ignore
            this.Login.RaiseCanExecuteChanged()

    member this.Password
        with get() = password
        and set(value) = 
            this.SetProperty(&password, value, NameOf.get( <@ this.Password @> )) |> ignore
            this.Login.RaiseCanExecuteChanged()

    member this.Login:MvxCommand = loginCommand