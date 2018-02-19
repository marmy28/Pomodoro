module public Pomodoro.Models.Interval
open FsUnit.MsTest
open Microsoft.FSharp.Core
open Microsoft.VisualStudio.TestTools.UnitTesting
open Pomodoro.Localization.My.Resources
open Pomodoro.Models.TimeMeasure


type public IInterval = 
    abstract Duration : int<das>
    abstract Message : string
    abstract MinimizeWindow : bool
    abstract MessageDuration : int<ms>
    
type private T = 
    { Duration : unit -> int<min>;
        Message : unit -> string;
        MinimizeWindow : bool;
        MessageDuration : int<ms> }
    interface IInterval with
        member this.Duration = convertMinuteToDecasecond (this.Duration())
        member this.Message = this.Message()
        member this.MinimizeWindow = this.MinimizeWindow
        member this.MessageDuration = this.MessageDuration
    
let private createInterval d m mw md = 
    { Duration = d;
        Message = m;
        MinimizeWindow = mw;
        MessageDuration = convertSecondToMillisecond md }
    
let private updateDurationAndMessage d m i = 
    { i with Duration = d;
                Message = m }
    
let private settings = Pomodoro.Localization.My.MySettings.Default
let private workInterval = 
    createInterval (fun () -> 1<min> * settings.WorkIntervalInMin) (fun () -> Resources.WorkMessage) true 5<sec>
let private shortBreakInterval = 
    createInterval (fun () -> 1<min> * settings.ShortBreakIntervalInMin) (fun () -> Resources.ShortBreakMessage) 
        false 5<sec>
let private longBreakInterval = 
    shortBreakInterval 
    |> updateDurationAndMessage (fun () -> 1<min> * settings.LongBreakIntervalInMin) 
            (fun () -> Resources.LongBreakMessage)
let private sitInterval = 
    createInterval (fun () -> 1<min> * settings.SitIntervalInMin) (fun () -> Resources.SitMessage) false 10<sec>
let private standInterval = 
    sitInterval 
    |> updateDurationAndMessage (fun () -> 1<min> * settings.StandIntervalInMin) (fun () -> Resources.StandMessage)
    
let private workShortBreakInterval = 
    [ workInterval;
        shortBreakInterval ]
    
let private workLongBreakInterval = 
    [ workInterval;
        longBreakInterval ]
    
let private sitStandInterval = 
    [ standInterval;
        sitInterval ]
    
let private completeWorkShortBreakInterval = 
    workShortBreakInterval
    |> List.replicate 3
    |> List.concat
    
let private workShortLongBreakInterval = List.append completeWorkShortBreakInterval workLongBreakInterval
[<CompiledName("PomodoroIntervalCollection")>]
let public pomodoroIntervalCollection : seq<IInterval> = Seq.cast workShortLongBreakInterval
[<CompiledName("SitStandIntervalCollection")>]
let public sitStandIntervalCollection : seq<IInterval> = Seq.cast sitStandInterval
    
[<TestClass>]
type Tester() = 
        
    [<TestMethod>]
    member this.``longBreak is longer than shortBreak``() = 
        (longBreakInterval :> IInterval).Duration 
        |> should be (greaterThan (shortBreakInterval :> IInterval).Duration)
        
    [<TestMethod>]
    member this.``sitInterval is longer than standInterval``() = 
        (sitInterval :> IInterval).Duration |> should be (greaterThan (standInterval :> IInterval).Duration)
        
    [<TestMethod>]
    member this.``workShortBreakInterval should have unique members``() = workShortBreakInterval |> should be unique
        
    [<TestMethod>]
    member this.``workLongBreakInterval should have unique members``() = workLongBreakInterval |> should be unique
        
    [<TestMethod>]
    member this.``sitStandInterval should have unique members``() = sitStandInterval |> should be unique
        
    [<TestMethod>]
    member this.``workShortLongBreakInterval should have repeating members``() = 
        workShortLongBreakInterval |> should not' (be unique)
        
    [<TestMethod>]
    member this.``pomodoroIntervalCollection should be of type IInterval``() = 
        Seq.iter (fun x -> x |> should be instanceOfType<IInterval>) pomodoroIntervalCollection
        
    [<TestMethod>]
    member this.``sitStandIntervalCollection should be of type IInterval``() = 
        Seq.iter (fun x -> x |> should be instanceOfType<IInterval>) sitStandIntervalCollection
        
    [<TestMethod>]
    member this.``workShortLongBreakInterval should have 8 intervals``() = 
        workShortLongBreakInterval |> should haveLength 8
        
    [<TestMethod>]
    member this.``sitStandInterval should have 2 intervals``() = sitStandInterval |> should haveLength 2
