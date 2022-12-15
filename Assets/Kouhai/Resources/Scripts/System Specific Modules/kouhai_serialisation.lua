--- This script adds intellisense and wrapper for
--- Kouhai Serialisation System

---returns true if this key is available
---@param keyName string
---@return boolean
function haskey(keyName)
    return SaveSystem.HasKey(keyName)
end

---Saves value with key
---@param dataTable table
function save(dataTable)
    SaveSystem.SaveData(dataTable[1], dataTable[2])
end

---gets the saved data set by key
---@param key string
---@return any
function load(key)
    return SaveSystem.GetData(key)
end