require "kouhai"

--This is a Test script to test Lua execution
--This is not intended to be used as an example script
--start here
::start::
background "highschool_bg_1"
playmusic "normal_day_bgmusic"
say {"Test", "Umm...? Hello?"}
show {"kieren","normal","left"}
say {"Test", "Hello? Is this is working?...."}
show {"kieren","derp","left"}
say {"Test", "Can you select an answer? I want tor check if this system is working"}
choices {"Yes", "No"}
--Handle player choice
if(player_choice == 1) then
   show {"kieren","grin","left"}
   say {"Test", "This is Yes!"}
elseif (player_choice == 2) then
   show {"kieren","disgust","left"}
   say {"Test", "This is No!"}
end
show {"kieren","smile","left"}
say {"Test", "This works"}
show {"kieren","normal","left"}
say {"Test", "Thanks for the Help"}
