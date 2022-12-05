using System;

namespace RuntimeDeveloperConsole
{
    /// <summary>
    /// Assign attribute to public static methods 
    /// to register them to console window 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCommandAttribute : Attribute
    {
        /// <summary>
        /// Display argument format for help
        /// </summary>
        public string ArgHelpText;
        public string Description;

        public ConsoleCommandAttribute(string description, string argHelpText)
        {
            this.Description = description;
            this.ArgHelpText = argHelpText;
        }
    }
}
