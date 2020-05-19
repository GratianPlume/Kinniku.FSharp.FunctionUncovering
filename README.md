## About

This is a single-file project that implements some function base on
[Statically Resolved Type Parameters](https://docs.microsoft.com/en-us/dotnet/fsharp/language-reference/generics/statically-resolved-type-parameters)
for handle currying function in F#.
It is free and unencumbered software released into the public domain.

You can use it to convert a currying function to a currying `System.Func`.
## Usage:

```fsharp
open System
open System.Runtime.CompilerServices
open Kinniku.FSharp.FunctionUncovers

[<Extension>]
type Test =
    [<Extension>]
    static member Currying(fn: Func<_,_,_,_>) =
        // Generate a currying System.Func
        CurryingFunc<_,_>.Inject(fun a b c -> fn.Invoke(a, b, c))

    [<Extension>]
    static member Currying(fn: Action<_,_,_>) =
        // Generate a currying Delegate end with System.Action
        CurryingFunc<_>.Inject(fun a b c -> fn.Invoke(a, b, c))        

```

Or you can get a function return type, and bind it to a function's
argument. 
## Usage:
```fsharp
module Test2 =
    open System
    open Kinniku.FSharp.FunctionUncovers

    let foo (n: int)  = {| Value = string n |}

    // Get the type of one argument funtion
    let handle (ResultOf foo x) =  x.Value


    let foo2 (a: int) (b: string) (c: DateTime) = 
        {| A = a; B = b; C = c |}

    // Get the last return type of a currying funtion.
    let handle2 x =        
        let inline fn() = FuncTest<_>.Inject foo2
        let (ResultOf fn x) = x
        x.A

```

And then you can get a lite dependency injection serve too.
## Usage:
```fsharp
module Test3 =
    open System
    open Kinniku.FSharp.FunctionUncovers

    // build you instance provider, write static methods name with
    // `GetInstance`, and it has one argument, type equals return 
    // type.
    type Instances = 

        | Instances

        static member GetInstance(_: int) = 1

        static member GetInstance(_: int64) = 2L

        static member GetInstance(_: string) = "AH"

        static member GetInstance(_: DateTime) = DateTime.Now

    // the function who want to be injected with instance.
    let foo (a: int) (b: int64) (c: DateTime) (d: string)= 
        printf "a: %d\nb: %d\nc: %A\nd: %s" a b c d

    // Run foo with my instance.
    let serveFunc () = 
        Dependency<_,_>.Inject(foo, Instances) // parse a default 
                                               // value to bind the
                                               // instance provider
```



