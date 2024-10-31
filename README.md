# PopoloCrois Translation Tool
A user-friendly tool for editing dialogue and cutscenes in the Japanese-only PSX games PopoloCrois Monogatari II and PopoRogue.

If you are looking for currently released patches made via this tool, please see:

PopoloCrois Monogatari II English - https://romhackplaza.org/translations/popolocrois-monogatari-ii-english-translation-playstation/

PopoRogue English - TBD Very Soon

## How to Use
After opening the executable, you will be able to open one of the game's scene files directly into the editor. This does mean there is a dependency on an image extracting tool, otherwise the tool will not handle disk format compression and fail to load all relevant game data. So, when using this tool, please extract out the file that you want to work with first, then import it back when finished.

I tend to use the simple CDMage for this: https://www.romhacking.net/utilities/1435/

The bin file for PSX games should be loaded with M2/2353 track format.

The files that contain dialogue information are:

**Popolocrois Monogatari II**

EVENT\EVT_0.BIN

EVENT\EVT_1.BIN

EVENT\EVT_2.BIN

EVENT\EVT_3.BIN

EVENT\EVT_4.BIN

EVENT\EVT_5.BIN

EVENT\EVT_S.BIN

FIELD\EPISODE0.BIN

FIELD\EPISODE1.BIN

FIELD\EPISODE2.BIN

FIELD\EPISODE3.BIN

FIELD\EPISODE4.BIN

FIELD\EPISODE5.BIN

**PopoRogue**

EVENT\J_EVENT.BIN

EVENT\K_EVENT.BIN

EVENT\V_EVENT.BIN

EVENT\NPC1.BIN

EVENT\NPC2.BIN

Once one of these files are loaded, you are able to select which Scene and Line you want to see/edit.

## Encoding
You may notice some odd characters appearing depending on which game you are editing. Here is a list of the default encoding characters and what they do:

**Emotes - &**

PopoloCrois Monogatari II has animated emotes the can display in a text box. The default character is an ampersand to denote the start and end of an emote. The usable values are 1-9 to display the various animations in-line.
Example: &1&

__Hex Codes - \*__

This is the catch-all for anytime a set of binary information is included in a string in the game. Some of these are well known, and some of these have totally unknown uses in the game. In general, it is safest to keep these in place when seen, but feel free to test with removing them if they are ugly.
Example: \*0x0507\* will center the text in the dialogue box.

The characters that denote these can be modified in a generated file named settings.json if you need to make use of these characters in game for any reason.

## Handy Features

**Crtl-F**

Of course! Works for searching in Japanese or English for text in the dialogue or speaker section!

**Edit All identical**

This is a checkbox that can be enabled at any time and will copy over any changes made to the currently opened dialogue box to every other identical textbox in the game. There are a lot of duplicate signposts and NPC's all over PopoloCrois!

**Center Text**

This will automatically add the hex codes required to center your text perfectly to length for either game. It does not auto update, so if you change the dialogue, remove the hex codes and hit it again.
