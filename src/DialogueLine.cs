namespace PopoloCroisTranslationTool
{
    /// <summary>
    /// A set of text lines that get rendered in game. Can be longer than the 3 lines that get rendered at a time.
    /// </summary>
    public class DialogueLine
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DialogueLine(string dialogue, long dialoguePointerLocation, long dialoguePointerOffsetValue, bool isPointer, int lineCount, string speakerName, TextFormatting textFormat, Scene parentScene)
        {
            Dialogue = dialogue;
            DialoguePointerLocation = dialoguePointerLocation;
            DialoguePointerOffsetValue = dialoguePointerOffsetValue;
            IsPointer = isPointer;
            LineCount = lineCount;
            OriginalDialogue = dialogue;
            OriginalSpeakerName = speakerName;
            ParentScene = parentScene;
            SpeakerName = speakerName;
            TextFormat = textFormat;
        }

        /// <summary>
        /// Only has functionality in PopoloCrois Monogatari II, modifies text size in game (also JP only on default ROM)
        /// </summary>
        public enum TextFormatting
        { none = 0, small, normal, large };

        /// <summary>
        /// The dialogue that will be rendered in game
        /// </summary>
        public string Dialogue { get; private set; }

        /// <summary>
        /// Memory location of the pointer to the dialogue
        /// </summary>
        public long DialoguePointerLocation { get; private set; }

        /// <summary>
        /// The value of the pointer. Will be modified when user changes any dialogue length in the scene and saves
        /// </summary>
        public long DialoguePointerOffsetValue { get; private set; }

        /// <summary>
        /// Whether the user has edited this dialogue line
        /// </summary>
        public bool IsEdited { get; private set; } = false;

        /// <summary>
        /// Whether this dialogue line just points to another pointer instead of dialogue text
        /// </summary>
        public bool IsPointer { get; private set; }

        /// <summary>
        /// The total count of lines currently in this dialogue line. Matters because it is stored alongside the pointer in memory.
        /// </summary>
        public int LineCount { get; private set; }

        /// <summary>
        /// The original dialogue when the scene was loaded by the user
        /// </summary>
        public string OriginalDialogue { get; private set; }

        /// <summary>
        /// The original line count when the scene was loaded by the user
        /// </summary>
        public int OriginalLineCount { get; private set; }

        /// <summary>
        /// The original speaker name when the scene was loaded by the user
        /// </summary>
        public string OriginalSpeakerName { get; private set; }

        /// <summary>
        /// Parent scene for tracking the IsEdited flag
        /// </summary>
        public Scene ParentScene { get; private set; }

        /// <summary>
        /// The speaker name of this dialogue line. Will appear in game as a name plate above the dialogue box.
        /// </summary>
        public string SpeakerName { get; private set; }

        /// <summary>
        /// The text formatting on the dialogue box
        /// </summary>
        public TextFormatting TextFormat { get; private set; }

        /// <summary>
        /// Applies the hex code to center the text for this dialogue line
        /// </summary>
        public void CenterDialogue()
        {
            string newDialogue = string.Empty;

            if (Dialogue.Last() == '\n')
            {
                Dialogue = Dialogue.Remove(Dialogue.Length - 1);
            }

            foreach (string textLine in Dialogue.Split('\n'))
            {
                if (textLine.Length > 0)
                {
                    int dialogueBoxCharacterLength;
                    if (ParentScene.LoadedGameType == GameType.PopoRogue)
                    {
                        dialogueBoxCharacterLength = 132;
                    }
                    else
                    {
                        dialogueBoxCharacterLength = 128;
                    }

                    string centeredHexString = "*0x09" + (dialogueBoxCharacterLength - (4 * textLine.Length)).ToString("X2") + "*";
                    newDialogue += centeredHexString + textLine + "\n";
                }
                else
                {
                    newDialogue += "\n";
                }
            }

            Dialogue = newDialogue;
            IsEdited = true;
            ParentScene.IsEdited = true;
        }

        /// <summary>
        /// Modifies the dialogue
        /// </summary>
        /// <param name="newDialogue">The new dialogue</param>
        public void EditDialogue(string newDialogue)
        {
            if (IsPointer)
            {
                return;
            }

            if (string.IsNullOrEmpty(newDialogue) || newDialogue.Last() != '\n')
            {
                newDialogue += '\n';
            }

            Dialogue = newDialogue;
            IsEdited = true;
            ParentScene.IsEdited = true;
            LineCount = newDialogue.Count(ch => ch == '\n');
        }

        /// <summary>
        /// Modifies the formatting applied to this line
        /// </summary>
        /// <param name="newFormatting">The new formatting</param>
        public void EditFormatting(int newFormatting)
        {
            if (IsPointer)
            {
                return;
            }

            TextFormat = (TextFormatting)newFormatting;
            IsEdited = true;
            ParentScene.IsEdited = true;
        }

        /// <summary>
        /// Modifies the speaker of the line
        /// </summary>
        /// <param name="newSpeaker">The new speaker</param>
        public void EditSpeaker(string newSpeaker)
        {
            if (IsPointer)
                return;

            SpeakerName = newSpeaker;
            IsEdited = true;
            ParentScene.IsEdited = true;
        }

        /// <summary>
        /// Removes IsEdited flag after saving
        /// </summary>
        public void Save()
        {
            IsEdited = false;
        }
    }
}