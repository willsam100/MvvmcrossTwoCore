namespace Sample.Droid

open System
open Android.App
open Android.Widget

open System.Windows.Input
open System.Collections.Specialized
open MvvmCross.Navigation
open MvvmCross.Binding.BindingContext
open MvvmCross.ViewModels
open Android.Views

[<Android.Runtime.Preserve(AllMembers = true)>]
type LinkerPleaseInclude() = 

    member this.Include(button:Button) = 
        button.Click.AddHandler (new EventHandler(fun s e -> button.Text <- button.Text + "" ))

    member this.Include(checkBox:CheckBox) = 
        checkBox.CheckedChange.Add (fun args -> checkBox.Checked <- not checkBox.Checked )

    member this.Include(switch:Switch) = 
        switch.CheckedChange.Add (fun args -> switch.Checked <- not switch.Checked )

    member this.Include(view:View) = 
        view.Click.Add (fun e -> view.ContentDescription <- view.ContentDescription + "" )

    member this.Include(text:TextView) = 
        text.AfterTextChanged.Add (fun args -> text.Text <- "" + text.Text )
        text.Hint <- "" + text.Hint

    member this.Include(text:CheckedTextView) = 
        text.AfterTextChanged.Add (fun args -> text.Text <- "" + text.Text )
        text.Hint <- "" + text.Hint

    member this.Include(cb:CompoundButton) = 
        cb.CheckedChange.Add (fun args -> cb.Checked <- not cb.Checked )

    member this.Include(sb:SeekBar) = 
        sb.ProgressChanged.Add (fun args -> sb.Progress <- sb.Progress + 1 )

    member this.Include(radioGroup:RadioGroup) = 
        radioGroup.CheckedChange.Add (fun args -> radioGroup.Check(args.CheckedId) )

    member this.Include(radioButton:RadioButton) = 
        radioButton.CheckedChange.Add (fun args -> radioButton.Checked <- args.IsChecked )

    member this.Include(ratingBar:RatingBar) = 
        ratingBar.RatingBarChange.Add (fun args -> ratingBar.Rating <- (float32 0) + ratingBar.Rating )

    member this.Include(act:Activity) = 
        act.Title <- act.Title + ""

    member this.Include(changed:INotifyCollectionChanged) = 
        changed.CollectionChanged.Add (fun e -> 
            let  test = sprintf "%A %A %A %A %A" e.Action e.NewItems e.NewStartingIndex e.OldItems e.OldStartingIndex
            ()
            )

    member this.Include(command:ICommand) = 
        command.CanExecuteChanged.Add (fun e -> if (command.CanExecute(null)) then command.Execute(null) )

    member this.Include(injector: byref<MvvmCross.IoC.MvxPropertyInjector>) = 
        injector <- new MvvmCross.IoC.MvxPropertyInjector()

    member this.Include(changed:System.ComponentModel.INotifyPropertyChanged) = 
        changed.PropertyChanged.Add (fun e -> 
                let test = e.PropertyName
                ()
            )

    member this.Include(context:MvxTaskBasedBindingContext) = 
        context.Dispose()
        let context2 = new MvxTaskBasedBindingContext()
        context2.Dispose()

    member this.Include(service:byref<MvxNavigationService>,loader:IMvxViewModelLoader) = 
        service <- new MvxNavigationService(null, loader)

    member this.Include(color:byref<ConsoleColor>) = 
        Console.Write("")
        Console.WriteLine("")
        color <- Console.ForegroundColor
        Console.ForegroundColor <- ConsoleColor.Red
        Console.ForegroundColor <- ConsoleColor.Yellow
        Console.ForegroundColor <- ConsoleColor.Magenta
        Console.ForegroundColor <- ConsoleColor.White
        Console.ForegroundColor <- ConsoleColor.Gray
        Console.ForegroundColor <- ConsoleColor.DarkGray

    // TODO: uncommment if using MvvmCross.Plugin.Json
    //member this.Include(plugin:MvvmCross.Plugin.Json.Plugin) = 
        //plugin.Load()