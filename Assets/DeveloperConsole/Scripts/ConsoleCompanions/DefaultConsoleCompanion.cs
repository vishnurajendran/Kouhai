using UnityEngine;
using RuntimeDeveloperConsole;

public class DefaultConsoleCompanion
{
    [ConsoleCommand("print message to screen", "echo <message>")]
    public static void Echo(string[] args)
    {
        if (args == null || args.Length <= 0)
            return ;

        Debug.Log(string.Join(" ",args));
    }

    [ConsoleCommand("Display commands available", "help <filter>")]
    public static string Help(string[] args)
    {
        return CommandDatabase.GetAvailableCommandsHelp(args);
    }

    [ConsoleCommand("Display system info", "sysinfo")]
    public static string SysInfo()
    {
        return string.Format(ConsoleConstants.SystemInfoString, SystemInfo.deviceModel,
                             SystemInfo.deviceName, SystemInfo.deviceType, SystemInfo.processorCount,
                             SystemInfo.systemMemorySize, SystemInfo.graphicsDeviceName, SystemInfo.graphicsMemorySize,
                             SystemInfo.graphicsDeviceType);

    }

}
