--- This script adds intellisense and wrapper for
--- Kouhai Choice System

DialogSystem = {}

--- Display Dialog, Can be used for any dialogs that are not character related
--- For any character dialogs, Character.Speak is recommended.
--- @param title string
--- @param dialog string
function DialogSystem:DisplayDialog(title, dialog)
    Dialog.Choices = {title or "", dialog or ""}
end

--- show choices and wait for player input
--- format{"choice 1","choice 2","choice 3",...}
--- example {"Yes","No"}
---@param choicesTable table
function DialogSystem:ShowChoices(choicesTable)
    Dialog.Choices = choicesTable
end

--- gets player choice
--- @return number
function DialogSystem:GetPlayerChoice()
    return Dialog.PlayerChoice
end