--- This script adds intellisense and wrapper for
--- Kouhai Debugging System

--log message in cyan
function log(msg)
    Debug.Log(msg)
end

--log warning message
function logwarning(msg)
    Debug.LogWarning(msg)
end

--log error message
function logerror(msg)
    Debug.LogError(msg)
end