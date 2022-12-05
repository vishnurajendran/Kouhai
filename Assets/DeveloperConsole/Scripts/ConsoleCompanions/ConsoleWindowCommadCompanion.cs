using RuntimeDeveloperConsole;

public class ConsoleWindowCommadCompanion
{
    [ConsoleCommand("Clears console window","clear")]
    public static void Clear()
    {
        ConsoleSystem.ClearConsoleWindow();
    }
}
