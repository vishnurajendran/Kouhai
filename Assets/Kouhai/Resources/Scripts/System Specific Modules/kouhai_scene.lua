--- This script adds intellisense and wrapper for
--- Kouhai Dialog System


--- set scene background
---@param bgName string
function background(bgName)
    Scene.SetBackgroundImage(bgName)
end

--- plays a background music
function playmusic(musicName)
    Scene.PlayBackgroundMusic(musicName)
end

--- plays a Ambiance
function playambiance(ambianceName)
    Scene.PlayAmbiance(ambianceName)
end

--- plays a SFX
function playsfx(sfxName)
    Scene.PlaySFX(sfxName)
end