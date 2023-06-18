--- This script adds intellisense and wrapper for
--- Kouhai Serialisation System

KouhaiPreferences = {}

---returns true if this key is available
---@param keyName string
---@return boolean
function KouhaiPreferences:HasKey(keyName)
    return SaveSystem.HasKey(keyName)
end

---Saves value with key
---@param key string
---@param value any
function KouhaiPreferences:Save(key, value)
    SaveSystem.SaveData(key, value)
end

---gets the saved data set by key
---@param key string
---@return any
function KouhaiPreferences:Load(key)
    return SaveSystem.GetData(key)
end