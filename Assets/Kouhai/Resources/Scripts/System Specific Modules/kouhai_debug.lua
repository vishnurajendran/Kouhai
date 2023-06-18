--- This script adds intellisense and wrapper for
--- Kouhai Debugging System

Debugging = {}

--log message in cyan
function Debugging:Log(msg)
    Debug.Log(msg)
end

--log warning message
function Debugging:LogWarning(msg)
    Debug.LogWarning(msg)
end

--log error message
function Debugging:LogError(msg)
    Debug.LogError(msg)
end