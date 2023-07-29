using System.Collections.Generic;
using UnityEngine;

namespace RuntimeDeveloperConsole
{
    public struct ConsoleCommand
    {
        public string Command;
        public string[] Arguments;
    }

    public class ConsoleSystem
    {
        private static IConsoleWindow consoleWindow;
        public static void SetConsoleWindow(IConsoleWindow console)
        {
            consoleWindow = console;
            Application.logMessageReceived +=  HandleLog;
        }

        private static void HandleLog(string message, string stackTrace, LogType type)
        {
            string msg = "";
            switch(type)
            {
                case LogType.Error:
                    msg = "<color=red>ERROR:: {0}</color>";
                    break;
                case LogType.Warning:
                    msg = "<color=yellow>WARN:: {0}</color>";
                    break;
                case LogType.Exception:
                    msg = "<color=red>EXCEPTION:: {0}</color>";
                    break;
                default:
                    msg = "LOG:: {0}";
                    break;
            }
            
            if(consoleWindow != null)
                consoleWindow.PrintLineToConsole(string.Format(msg, message));
        }

        public static void ClearConsoleWindow()
        {
            if (consoleWindow == null) return;

            consoleWindow.Clear();
        }

        public static string GetCommandSuggestion(string searchString)
        {
            return CommandDatabase.GetCommandSuggestion(searchString);
        }

        public static void HandleCommand(string commandString)
        {
            if(string.IsNullOrEmpty(commandString)) {
                return;
            }

            foreach(string cmd in commandString.Split(ConsoleConstants.COMMAND_SEPERATOR))
            {
                ExecuteCommand(Parse(cmd));
            }
        }

        private static ConsoleCommand Parse(string commandString)
        {
            if (string.IsNullOrEmpty(commandString))
                return default;

            ConsoleCommand command = new ConsoleCommand();
            var components = commandString.Split(ConsoleConstants.COMMAND_COMPONENT_SEPERATOR);
            command.Command = components[0];
            
            List<string> args = new List<string>();
            for(int i = 1;i< components.Length; i++)
            {
                args.Add(components[i].Trim());
            }

            command.Arguments = args.ToArray();
            return command;
        }

        private static void ExecuteCommand(ConsoleCommand command)
        {
            consoleWindow.PrintLineToConsole(CommandDatabase.Run(command));
        }
    }
}
