--- This script adds intellisense and wrapper for
--- Kouhai Character

--- Define positions for character on screen
CharacterPositions = { Left="left",Right="right",Centre="centre" }

--- A table that represents a character
KouhaiCharacter = {name="", position=CharacterPositions.Left}

--- Create a kouhai character
--- @param name string
--- @param position CharacterPositions
function KouhaiCharacter:New(name, position)
    local o = {}
    setmetatable(o, self)
    self.__index = self
    self.name = name or ""
    self.position = position or CharacterPositions.Left
    return o
end

--- show character
--- @param expression string
function KouhaiCharacter:Show(expression)
    Character.Show = {self.name, expression or "",self.position}
end

--- show character
function KouhaiCharacter:Hide()
    Character.HideCharacter(self.name)
end

--- Changes position of the character on the screen
--- @param position CharacterPositions
function KouhaiCharacter:SetPosition(newPosition, shiftSpeed)
    self.position = newPosition
    Character.ShiftCharacter({self.name, newPosition, shiftSpeed or 0})
end

--- Make character speak a dialog
--- @param dialog string
function KouhaiCharacter:Speak(dialog)
    Dialog.Say = {self.name, dialog or ""}
end