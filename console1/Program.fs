open Helpers
    exception UserNeedsHelp of string
    let programHelp = 
        [|
            "This program prints an important message a number of times."
            ;"The default message is 'hello world.' The default number of times is 1."
        |]
    
    let defaultBaseOptions = createNewBaseOptions "console1" "Important Message Printer" programHelp
    let defaulMessageToOutput = createNewConfigEntry "M" "Message to print (Optional)" [|"/M:<message> -> This is the message you want printed."|] "Hello World"
    let defaultNumberOfTimesToRepeat = createNewConfigEntry "N" "Number of times to print (Optional)" [|"/N:<integer> -> How many times you want it printed."|] 1
    
    let loadConfigFromCommandLine (args:string []):console1ProgramConfig =
        if args.Length>0 && (args.[0]="?"||args.[0]="/?"||args.[0]="-?"||args.[0]="--?"||args.[0]="help"||args.[0]="/help"||args.[0]="-help"||args.[0]="--help") then raise (UserNeedsHelp args.[0]) else
        let newConfigBase = defaultBaseOptions
        let newMessageToOutput= ConfigEntry<_>.populateValueFromCommandLine(defaulMessageToOutput,args)
        let newNumberOfTimesToRepeat=ConfigEntry<_>.populateValueFromCommandLine(defaultNumberOfTimesToRepeat,args)
        { 
            configBase = newConfigBase
            messageToOutput=newMessageToOutput
            numberOfTimesToRepeat=newNumberOfTimesToRepeat
        }
    
    let doStuff (opts:console1ProgramConfig)=
        let messagePrint()=printfn "%s" opts.messageToOutput.parameterValue
        List.replicate(opts.numberOfTimesToRepeat.parameterValue) 0 |> List.iter(fun x->messagePrint())

[<EntryPoint>]
let main argv = 
//    printfn "%A" argv
//    0 // return an integer exit code
        try
            let opts = loadConfigFromCommandLine argv
            doStuff opts 
            0
        with
            | :? UserNeedsHelp as hex ->
                defaultBaseOptions.printThis
                0
            | ex ->
                System.Console.WriteLine ("Program terminated abnormally " + ex.Message)
                System.Console.WriteLine (ex.StackTrace)
                if ex.InnerException = null
                    then
                        0
                    else
                        System.Console.WriteLine("---   Inner Exception   ---")
                        System.Console.WriteLine (ex.InnerException.Message)
                        System.Console.WriteLine (ex.InnerException.StackTrace)
                        0
