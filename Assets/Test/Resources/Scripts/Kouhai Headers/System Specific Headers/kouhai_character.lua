--- This script adds intellisense and wrapper for
--- Kouhai Character System

CharacterPositions = { Left="left",Right="right",Centre="centre" }

--- show character
--- format{"name","expression","position"}
--- example {"kieren","normal","left"}
---@param characterTable table 
function show(characterTable)
    Character.Show = characterTable
end

--- Hides characters
function hide(charactername)
    Character.HideCharacter(charactername)
end

--- Shifts character
--- format{"name","position","time"}
--- example {"kieren","left","0.5"}
function shift(characterTable)
    Character.ShiftCharacter(characterTable)
end