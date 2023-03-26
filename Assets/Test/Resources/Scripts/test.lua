require "kouhai"

-- start variable declaration block
-- any variable declarations under this block will appear in the inspector
::vars::

numberVar = 0
boolVar = false
stringVar = "Hello World"

::endvars::
--- end variable declaration block

--This is a Test script to test Lua execution
--This is not intended to be used as an example script
--start here
::start::

if not b then
   -- something comes here --
end

background "highschool_bg_1"
playmusic "normal_day_bgmusic"
say {"Test", "Umm...? Hello?"}
playsfx "thud"
show {"kieren","normal",CharacterPositions.Left}
say {"Test", "Hello? Is this is working?...."}
show {"kieren","derp",CharacterPositions.Left}
say {"Test", "Can you select an answer? I want tor check if this system is working"}
choices {"Yes", "No"}

--Handle player choice
if(player_choice == 1) then
   show {"kieren","grin",CharacterPositions.Left}
   playsfx "thud"
   say {"Test", "This is Yes!"}
elseif (player_choice == 2) then
   show {"kieren","disgust",CharacterPositions.Left}
   playsfx "thud"
   say {"Test", "This is No!"}
end

-- continue with story
show {"kieren","smile",CharacterPositions.Left}
say {"Test", "This works"}
playsfx "thud"
show {"kieren","normal",CharacterPositions.Left}
say {"Test", "Thanks for the Help, now lets try the movement"}
show {"kieren","smile",CharacterPositions.Left}
say {"Test", "I'm gonna move to centre now"}
playsfx "thud"
shift {"kieren",CharacterPositions.Centre,0.25}
say {"Test", "Alright so far so good"}
say {"Test", "moving on..."}
shift {"kieren",CharacterPositions.Right,0.5}
say {"Test", "Awesome, now lets move to left again"}
shift {"kieren",CharacterPositions.Left,0.5}
say {"Test", "cool Imma peace out now!!"}
say {"Test", "BYE!!"}
hide "kieren"

log "Game Over"