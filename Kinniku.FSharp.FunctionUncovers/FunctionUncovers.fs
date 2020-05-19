module Kinniku.FSharp.FunctionUncovers
open System


let inline (|ResultOf|) (_: ^T -> ^R) (x: ^R) = x 

let inline inject< ^t, ^f, ^r when (^t or ^f): (static member Inject: ^f -> ^r)> fn =
    ((^t or ^f): (static member Inject: ^f -> ^r) fn)  

let inline inject2< ^t, ^f, ^r, ^p when (^t or ^f): (static member Inject: ^f * ^p -> ^r)> p fn =
    ((^t or ^f): (static member Inject: ^f * ^p-> ^r) (fn, p))  

let inline getInstance< ^t, ^a when (^t or ^a): (static member GetInstance: ^a -> ^a)> (_: ^t) =
    ((^t or ^a): (static member GetInstance: ^a -> ^a) Unchecked.defaultof<_> )



/// 生成柯里化Func
type CurryingFunc<'T, 'R when 'R: equality> =
        
    static member inline Inject f = Func<'T,'R> f

    static member inline Inject f = 

        let inline inject x = inject<CurryingFunc<_,_>,_,_>  x 
        Func<_,_>(fun x -> x |> f |> inject)


/// 生成柯里化Func，最后一个为Action
type CurryingFunc<'T> =
        
    static member inline Inject f = Action<'T> f

    static member inline Inject f = 

        let inline inject x = inject<CurryingFunc<_>,_,_>  x 
        Func<_,_>(fun x -> x |> f |> inject)


/// 测试柯里化函数的最终值类型
type FuncTest<'T when 'T: equality> =
        
    static member inline Inject x: 'T = x

    static member inline Inject f = 

        let inline inject x = inject<FuncTest<_>,_,_>  x 

        Unchecked.defaultof<_> |> f  |> inject


type Dependency<'P, 'T when 'T: equality> =

    static member inline Inject (x, _: 'P): 'T = x

    static member inline Inject (fn, p) = 

        let inline inject x = inject2<Dependency<_,_>,_,_,_> p x

        getInstance(p) |> fn |> inject