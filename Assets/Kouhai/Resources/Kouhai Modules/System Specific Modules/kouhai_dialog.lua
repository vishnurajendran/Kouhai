--- This script adds intellisense and wrapper for
--- Kouhai Dialog System

--- say dialog
--- format{"name to diplay","dialog"}
--- example {"kieren","Hi!"}
---@param speechTable table
function say(speechTable)
    Dialog.Say = speechTable
end

--- show choices and wait for player input
--- format{"choice 1","choice 2","choice 3",...}
--- example {"Yes","No"}
---@param choicesTable table
function choices(choicesTable)
    Dialog.Choices = choicesTable
end

--- gets player choice
---@param characterTable table
function player_choice()
    return Dialog.PlayerChoice
end