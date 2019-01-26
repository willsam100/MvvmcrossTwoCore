namespace Sample.Core

open Microsoft.FSharp.Quotations

module NameOf = 

    let get(p:Quotations.Expr<_>) = 
        match p with 
        | Patterns.PropertyGet(_,pi,_) -> pi.Name
        | _ -> invalidArg "Invalid arg" "p"

    [<CompiledName("GetOption")>]
    let getOption () = 
        Some "ItWorked"


[<System.Runtime.CompilerServices.Extension>]
module OptionExtension = 
    open System
    open System.Runtime.CompilerServices

    [<Extension>]
    let Match(opt : Option<'a>, success:Func<'a, 'b>,  failure:Func<'b>) =
        match opt with
        | Some x -> success.Invoke x
        | None -> failure.Invoke ()

    [<Extension>]
    let MatchAction(opt : Option<'a>, success:Action<'a>,  failure:Action) =
        match opt with
        | Some x -> success.Invoke x
        | None -> failure.Invoke ()