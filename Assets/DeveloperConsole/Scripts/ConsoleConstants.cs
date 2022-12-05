using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeDeveloperConsole
{
    public class ConsoleConstants
    {
        public const string COMMAND_NOT_FOUND_STRING = "<color=red>{0} is not a registered command with the console.</color>";
        public const string COMMAND_INCORRECT_SIGNATURE = "Command is not of correct input argument signature";
        public const string COMMAND_NOT_FOUND = "Could not find specified command";

        public const string TERM_KEY = ">> ";
        public const string COMMAND_COMPONENT_SEPERATOR = " ";
        public const string COMMAND_SEPERATOR = ";";

        public const string TOO_MANY_ARGS_TEXT = "Too many arguments provided than specified";
        public const string TOO_FEW_ARGS_TEXT = "Too few arguments provided than specified";

        public const string SystemInfoString = @"<color=#00FFFF>-- Stats --        
          [Basic]
          DeviceModel: {0}
          DeviceName : {1}
          DeviceType : {2}
          
          [CPU]
          Proccessors: {3}
          Memory     : {4}
    
          [GPU]
          GPU Device : {5}
          GPU Memory : {6} 
          API        : {7}</color>";
    }
}
