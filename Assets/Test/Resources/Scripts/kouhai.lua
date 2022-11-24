
--- set scene background
---@param bgName string
function background(bgName)
    Scene.Background = bgName
end

--- plays a background music
function playmusic(musicName)
    Scene.Music = musicName
end

--- say dialog
---@param speechTable table
function say(speechTable)
    Dialog.Say = speechTable
end

--- show character
--- format{"name","expression","position"}
--- example {"kieren","normal","left"}
---@param characterTable table 
function show(characterTable)
    Character.Show = characterTable
end

--- show choices and wait for player input
---@param choicesTable table
function choices(choicesTable)
    Dialog.Choices = choicesTable
end

--- gets player choice
---@param characterTable table
function player_choice()
    return Dialog.PlayerChoice
end