module public Pomodoro.Models.TimeMeasure

open FsUnit.MsTest
open Microsoft.FSharp.Core
open Microsoft.VisualStudio.TestTools.UnitTesting

/// Minute
[<Measure>]
type internal min
    
/// Decaseconds
[<Measure>]
type internal das
    
/// Seconds
[<Measure>]
type internal sec
    
/// Milliseconds
[<Measure>]
type internal ms
    
/// Seconds per minute
[<Literal>]
let internal secPerMin = 60<sec/min>
    
/// Milliseconds per second
[<Literal>]
[<CompiledName("MillisecondPerSecond")>]
let public millisecondPerSecond = 1000<ms/sec>
    
/// Seconds per decaseconds
[<Literal>]
[<CompiledName("SecondPerDecasecond")>]
let public secondPerDecasecond = 10<sec/das>
    
/// Converts seconds to milliseconds
let internal convertSecondToMillisecond (x : int<sec>) = x * millisecondPerSecond
    
/// Converts minutes to decaseconds
let internal convertMinuteToDecasecond (x : int<min>) = x * (secPerMin / secondPerDecasecond)
    
[<TestClass>]
type Tester() = 
    [<TestMethod>]
    member this.``seconds to ms should have larger number``() = 
        let number = 3
        let numberInSec : int<sec> = LanguagePrimitives.Int32WithMeasure number
        let numberInMs : int<ms> = LanguagePrimitives.Int32WithMeasure number
        convertSecondToMillisecond numberInSec |> should be (greaterThan numberInMs)
