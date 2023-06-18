--- This script adds intellisense and wrapper for
--- Kouhai Scene System
require "kouhai_essentials"

KouhaiScene={}

--- sets up scene in one go
function KouhaiScene:SetupScene(bgName, musicName, ambianceName)
    KouhaiScene:SetBackground(bgName or "")
    KouhaiScene:PlayMusic(musicName or "")
    KouhaiScene:PlayAmbiance(ambianceName or "")
end

--- set scene background
---@param bgName string
function KouhaiScene:SetBackground(bgName)
    Scene.SetBackgroundImage(bgName)
end

--- plays a background music
--- @param musicName string
function KouhaiScene:PlayMusic(musicName)
    Scene.PlayBackgroundMusic(musicName)
end

--- plays a Ambiance
--- @param ambianceName string
function KouhaiScene:PlayAmbiance(ambianceName)
    Scene.PlayAmbiance(ambianceName)
end

--- plays a SFX
--- @param sfxName string
function KouhaiScene:PlaySfx(sfxName)
    Scene.PlaySFX(sfxName)
end

--- does a screen shake
--- @param color Color
--- @param duration number
function KouhaiScene:ShakeScreen(instensity,duration)
    Scene.ShakeScreen(instensity, duration)
end

--- does a fade to black
--- @param duration number
function KouhaiScene:FadeToBlack(duration)
    Scene.FadeToBlack(duration)
end

--- does a fade from black
--- @param duration number
function KouhaiScene:FadeFromBlack(duration)
    Scene.FadeFromBlack(duration)
end

--- flash screen with a specific color
--- @param color Color
--- @param duration number
function KouhaiScene:FlashScreen(color, duration)
    Scene.FlashScreen(color, duration)
end

--- sets scenebloom intensity
--- @param instensity number
--- @param duration number
function KouhaiScene:SetBloom(intensity, duration)
    Scene.SetBloom(intensity, duration)
end

--- sets scenefilm grain intensity
--- @param instensity number
--- @param duration number
function KouhaiScene:SetFilmGrain(intensity, duration)
    Scene.SetGrain(intensity, duration)
end

--- sets scenevignette intensity
--- @param instensity Color
--- @param duration number
--- @param instensity number
function KouhaiScene:SetVignette(intensity, color, duration)
    Scene.SetVignetteIntensity(intensity, duration)
    Scene.SetVignetteColor(color, duration)
end

--- sets scenesaturation
--- @param instensity number
--- @param duration number
function KouhaiScene:SetSaturation(intensity, duration)
    Scene.SetSaturation(intensity, duration)
end

--- sets scenechromatic abberation
--- @param instensity number
--- @param duration number
function KouhaiScene:SetChromaticAberration(intensity, duration)
    Scene.SetChromaticAberration(intensity, duration)
end