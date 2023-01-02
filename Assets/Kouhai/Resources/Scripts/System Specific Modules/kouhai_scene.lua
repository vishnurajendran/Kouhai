--- This script adds intellisense and wrapper for
--- Kouhai Dialog System
require "kouhai_essentials"

--- set scene background
---@param bgName string
function background(bgName)
    Scene.SetBackgroundImage(bgName)
end

--- plays a background music
--- @param musicName string
function playmusic(musicName)
    Scene.PlayBackgroundMusic(musicName)
end

--- plays a Ambiance
--- @param ambianceName string
function playambiance(ambianceName)
    Scene.PlayAmbiance(ambianceName)
end

--- plays a SFX
--- @param sfxName string
function playsfx(sfxName)
    Scene.PlaySFX(sfxName)
end

--- does a screen shake
--- takes a tuple with 2 numbers
--- first value is intensity, 
---second value is duration of shake
---@param shakeData tuple
function shakescreen(shakeData)
    Scene.ShakeScreen(shakeData[1], shakeData[2])
end

--- does a fade to black
--- takes a tuple of size 1
--- value defines duration of 
--- @param durTuple tuple
function fadetoblack(durTuple)
    Scene.FadeToBlack(durTuple[1])
end

--- does a fade from black
--- takes a tuple of size 1
--- value defines duration of fade
--- @param durTuple tuple
function fadefromblack(durTuple)
    Scene.FadeFromBlack(durTuple[1])
end

--- flash screen with a specific color
--- takes a tuple of Color and number
--- @param flashData tuple 
function flashscreen(flashData)
    Scene.FlashScreen(flashData[1], flashData[2])
end

--- set bloom intensity
--- takes a tuple of intensity and duration
--- @param bloomData tuple
function setbloom(bloomData)
    Scene.SetBloom(bloomData[1], bloomData[2])
end

--- sets film grain intensity
--- takes a tuple of intensity and duration
--- @param grainData tuple
function setgrain(grainData)
    Scene.SetGrain(grainData[1], grainData[2])
end

--- sets vignette intensity
--- takes a tuple of intensity and duration
--- @param vignetteintensityData tuple
function setvignetteintensity(vignetteintensityData)
    Scene.SetVignetteIntensity(vignetteintensityData[1], vignetteintensityData[2])
end

--- sets vignette color
--- takes a tuple of color and duration
--- @param vignettecolorData tuple
function setvignettecolor(vignettecolorData)
    Scene.SetVignetteColor(vignettecolordata[1], vignettecolordata[2])
end

--- sets saturation
--- takes a tuple of intensity and duration
--- @param saturationData tuple
function setsaturation(saturationData)
    Scene.SetSaturation(saturationData[1], saturationData[2])
end

--- set chromatic abberation
--- takes a tuple of intensity and duration
--- @param caData tuple
function setchromaticaberration(caData)
    Scene.SetChromaticAberration(caData[1], caData[2])
end