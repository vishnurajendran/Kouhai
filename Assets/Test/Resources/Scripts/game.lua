require "kouhai"

-- start variable declaration block
-- any variable declarations under this block will appear in the inspector
::vars::

numberVar = 0
boolVar = false
stringVar = "Hello World"
canSidJump = false

::endvars::
--- end variable declaration block

--This is a Test script to test Lua execution
--This is not intended to be used as an example script
--start here
::start::
kieren = KouhaiCharacter:New("Kieren",CharacterPositions.Left)

-- KouhaiScene:SetBackground("highschool_bg_1")
-- KouhaiScene:PlayMusic("normal_day_bgmusic")
KouhaiScene:SetupScene("highschool_bg_1","normal_day_bgmusic")
KouhaiScene:FadeFromBlack(2)
Utility:Waitforseconds(2)

kieren:Speak("Umm...? Hello?")
KouhaiScene:PlaySfx("thud")
kieren:Show("normal")
kieren:Speak("Hello? Is this is working?....")
kieren:Show("derp")
kieren:Speak("Can you select an answer? I want tor check if this system is working")
DialogSystem:ShowChoices ({"Yes", "No"})

--Handle player choice
choice = DialogSystem:GetPlayerChoice()
if(choice == 1) then
   kieren:Show("grin")
   kieren:Speak( "This is Yes!")
   KouhaiScene:PlaySfx("thud")
elseif (choice == 2) then
   kieren:Show("disgust")
   kieren:Speak( "This is No!")
   KouhaiScene:PlaySfx("thud")
end

-- continue with story
kieren:Show("smile")
kieren:Speak("This works")
KouhaiScene:PlaySfx("thud")
kieren:Show("normal")
kieren:Speak("Thanks for the Help, now lets try the movement")
kieren:Show("smile")
kieren:Speak("I'm gonna move to centre now")
KouhaiScene:PlaySfx("thud")
kieren:SetPosition(CharacterPositions.Centre,0.25)
kieren:Speak("Alright so far so good")
kieren:Speak("moving on...")
kieren:SetPosition(CharacterPositions.Right,0.5)
kieren:Speak("Awesome, now lets move to left again")
kieren:SetPosition(CharacterPositions.Left,0.5)
kieren:Speak("cool Imma peace out now!!")
kieren:Speak("BYE!!")
kieren:Hide()

Utility:Waitforseconds(1)
KouhaiScene:FadeToBlack(2)

Debugging:Log("Game Over")