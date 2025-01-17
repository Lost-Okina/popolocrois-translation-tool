using System.Text;

namespace PopoloCroisTranslationTool
{
    /// <summary>
    /// Converts binary game files into data structures for the editor to interface with
    /// </summary>
    public class GameFile
    {
        /// <summary>
        /// Default text to display when the scene is not a pointer, but has no dialogue
        /// </summary>
        public const string PointerTextString = "Empty Non-Pointer Line\nDo Not Modify";

        /// <summary>
        /// Various settings that can be modified by the user for specific needs
        /// </summary>
        private readonly Settings UserSettings;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="settings">User settings for some functions</param>
        public GameFile(Settings settings)
        {
            UserSettings = settings;
        }

        /// <summary>
        /// The full list of currently loaded scenes
        /// </summary>
        public List<Scene> SceneList { get; private set; } = new List<Scene>();

        /// <summary>
        /// Whether any user changes have been made
        /// </summary>
        /// <returns>True if user has made changes</returns>
        public bool ChangesMade()
        {
            if (!IsLoaded())
                return false;

            return SceneList.Any(s => s.IsEdited || s.DialogueLines.Any(d => d.IsEdited));
        }

        /// <summary>
        /// Closes all open files
        /// </summary>
        public void CloseFiles()
        {
            SceneList.Clear();
        }

        /// <summary>
        /// Exports the currently opened game files to a text dump
        /// </summary>
        /// <param name="fileName">The name of the file to create and write to</param>
        public void ExportScenes(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                // Preface to warn users
                sw.WriteLine("Warning: Do not modify anything except for the speaker and dialogue sections of the file or data corruption is possible upon importing.");
                sw.WriteLine();

                foreach (Scene scene in SceneList)
                {
                    sw.WriteLine("***** Scene Metadata *****");
                    sw.WriteLine(scene.ParentFile.Substring(scene.ParentFile.LastIndexOf("\\") + 1));
                    sw.WriteLine(scene.SceneTitle);
                    sw.WriteLine(scene.PointerListLocation);
                    sw.WriteLine("**************************");
                    sw.WriteLine();

                    int dialogueCounter = 0;

                    foreach (var dialogue in scene.DialogueLines)
                    {
                        if (dialogue.IsPointer)
                        {
                            dialogueCounter++;
                            continue;
                        }

                        sw.WriteLine(++dialogueCounter);
                        sw.WriteLine("Speaker:");
                        sw.WriteLine(dialogue.SpeakerName);
                        sw.WriteLine();
                        sw.WriteLine("Dialogue:");
                        sw.WriteLine(dialogue.Dialogue);
                    }
                }
            }
        }

        /// <summary>
        /// Finds every duplicate of the given dialogue and speaker in the loaded files
        /// </summary>
        /// <param name="dialogue">Dialogue to match</param>
        /// <param name="speakerName">Speaker to match</param>
        /// <param name="formatting">Formatting to match</param>
        /// <returns>The indices in the Scene List of every single duplicate dialogue box</returns>
        public List<Tuple<int, int>> FindAllDuplicates(string dialogue, string speakerName, int formatting)
        {
            var duplicates = new List<Tuple<int, int>>();
            for (int i = 0; i < SceneList.Count; i++)
            {
                for (int j = 0; j < SceneList[i].DialogueLines.Count; j++)
                {
                    if (string.Compare(SceneList[i].DialogueLines[j].Dialogue, dialogue) == 0 &&
                        string.Compare(SceneList[i].DialogueLines[j].SpeakerName, speakerName) == 0 &&
                        formatting == (int)SceneList[i].DialogueLines[j].TextFormat)
                    {
                        duplicates.Add(new Tuple<int, int>(i, j));
                    }
                }
            }

            return duplicates;
        }

        /// <summary>
        /// Imports a text file into an already opened game file
        /// </summary>
        /// <param name="fileName">The text file to import from</param>
        /// <exception cref="Exception">Can throw if the Scene Title can't be matched in an open game file</exception>
        public void ImportScenes(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception("File does not exist");
            }

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                sr.ReadLine();
                SeekNextNonWhiteSpace(sr);

                // Main Scene Loop
                string sceneMetadataHeader = sr.ReadLine();
                while (!sr.EndOfStream && sceneMetadataHeader.StartsWith("***** Scene Metadata *****"))
                {
                    string parentFileName = sr.ReadLine();
                    string sceneTitle = sr.ReadLine();
                    long pointerListStartLocation = long.Parse(sr.ReadLine());
                    sr.ReadLine();

                    // verify this exists in current loaded data
                    if (!SceneList.Any(s => s.SceneTitle == sceneTitle && s.PointerListLocation == pointerListStartLocation))
                    {
                        throw new Exception($"Importing a scene that does not exist in currently loaded files. ({sceneTitle})");
                    }

                    int activeSceneIndex = SceneList.IndexOf(SceneList.First(s => s.SceneTitle == sceneTitle && s.PointerListLocation == pointerListStartLocation));

                    // Main Dialogue Loop
                    SeekNextNonWhiteSpace(sr);
                    string sceneCountString = sr.ReadLine();
                    while (int.TryParse(sceneCountString, out int sceneCount))
                    {
                        SeekNextNonWhiteSpace(sr);
                        string speakerHeader = sr.ReadLine();
                        string speaker = sr.ReadLine();

                        SeekNextNonWhiteSpace(sr);
                        string dialogueHeader = sr.ReadLine();

                        // Loop to get all of the dialogue string
                        int lineCount = 0;
                        string fullDialogue = string.Empty;
                        string dialogueLine = sr.ReadLine();
                        while (!sr.EndOfStream && !int.TryParse(dialogueLine, out int dialogueLineInt) && !dialogueLine.StartsWith("*****"))
                        {
                            lineCount++;
                            fullDialogue += dialogueLine + "\n";
                            dialogueLine = sr.ReadLine();
                        }

                        // bug fix, remove 1 line
                        if (!sr.EndOfStream)
                        {
                            lineCount--;
                            fullDialogue = fullDialogue.Remove(fullDialogue.Length - 1);
                        }

                        sceneCountString = dialogueLine;

                        // Modify scenes that have been changed
                        if (SceneList[activeSceneIndex].DialogueLines[sceneCount - 1].SpeakerName != speaker ||
                            SceneList[activeSceneIndex].DialogueLines[sceneCount - 1].Dialogue != fullDialogue)
                        {
                            SceneList[activeSceneIndex].DialogueLines[sceneCount - 1].EditSpeaker(speaker);
                            SceneList[activeSceneIndex].DialogueLines[sceneCount - 1].EditDialogue(fullDialogue);
                        }
                    }

                    sceneMetadataHeader = sceneCountString;
                }
            }
        }

        /// <summary>
        /// Whether any game files have been loaded
        /// </summary>
        /// <returns>True if any files were loaded</returns>
        public bool IsLoaded()
        {
            return SceneList != null && SceneList.Count > 0;
        }

        /// <summary>
        /// Perform a load of a single game file
        /// </summary>
        /// <param name="filePath">The game file being loaded</param>
        /// <returns>Whether the load was successful</returns>
        public bool LoadFile(string filePath)
        {
            if (IsLoaded() && SceneList.Any(s => s.ParentFile == filePath))
            {
                return false;
            }

            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                LoadScenes(filePath, file);
            }

            return true;
        }

        /// <summary>
        /// Performs the write of all user changes back into the Game Files
        /// </summary>
        /// <param name="individualScene">Nullable location if user is only saving one scene</param>
        /// <param name="oldFile">When using the Save As function, represents the currently opened file</param>
        /// <param name="newFile">When using the Save As function, represents the new file to be created</param>
        /// <returns>Error description if anything wrong happened</returns>
        public void SaveScenes(int? individualScene = null, string? oldFile = null, string? newFile = null)
        {
            if (!string.IsNullOrWhiteSpace(newFile) && !String.IsNullOrWhiteSpace(oldFile))
            {
                // Don't allow Save As to a file thats currently open
                if (!SceneList.Any(s => s.ParentFile == newFile))
                {
                    File.Copy(oldFile, newFile, true);

                    foreach (var scene in SceneList)
                    {
                        if (scene.ParentFile == oldFile)
                        {
                            scene.ParentFile = newFile;
                        }
                    }
                }
            }

            for (int i = 0; i < SceneList.Count; i++)
            {
                Scene scene = SceneList[i];

                if (!scene.IsEdited ||
                    (individualScene != null && individualScene != i) ||
                    (!string.IsNullOrWhiteSpace(newFile) && scene.ParentFile != newFile))
                {
                    continue;
                }

                // Find the first non-pointer memory location in the string pool
                long stringPoolPointer = -1;

                foreach (DialogueLine dialogueLine in scene.DialogueLines)
                {
                    if (dialogueLine.IsPointer)
                    {
                        continue;
                    }
                    else
                    {
                        stringPoolPointer = dialogueLine.DialoguePointerOffsetValue + scene.PointerListLocation;
                        break;
                    }
                }

                if (stringPoolPointer == -1)
                {
                    throw new Exception($"Could not find the start of the string pool in memory for scene {scene.SceneTitle}");
                }

                List<byte> pointerMemoryBuffer = new List<byte>();
                List<byte> stringPoolMemoryBuffer = new List<byte>();

                foreach (DialogueLine dialogue in scene.DialogueLines)
                {
                    if (dialogue.IsPointer || dialogue.LineCount == 0)
                    {
                        byte[] oldPointerData = CreatePointerBytes(dialogue.LineCount, dialogue.DialoguePointerOffsetValue);
                        pointerMemoryBuffer.AddRange(oldPointerData);
                        continue;
                    }

                    long newPointerOffset = stringPoolPointer - scene.PointerListLocation;
                    byte[] newPointerData = CreatePointerBytes(dialogue.LineCount, newPointerOffset);
                    pointerMemoryBuffer.AddRange(newPointerData);

                    if (!string.IsNullOrWhiteSpace(dialogue.SpeakerName))
                    {
                        byte[] newSpeakerData = UnicodeToShiftJIS(dialogue.SpeakerName);

                        if (scene.LoadedGameType == GameType.PopolocroisMonogatariII)
                        {
                            stringPoolMemoryBuffer.Add(0x3E);
                        }
                        else
                        {
                            stringPoolMemoryBuffer.Add(0x3C);
                        }
                        stringPoolMemoryBuffer.AddRange(newSpeakerData);
                        stringPoolMemoryBuffer.Add(0x00);
                        stringPoolPointer += newSpeakerData.Length + 2;
                    }

                    if (dialogue.TextFormat != DialogueLine.TextFormatting.none)
                    {
                        stringPoolMemoryBuffer.Add(0x05);
                        stringPoolMemoryBuffer.Add(0x03);
                        stringPoolMemoryBuffer.Add(0x00);
                        switch (dialogue.TextFormat)
                        {
                            case DialogueLine.TextFormatting.small:
                                stringPoolMemoryBuffer.Add(0x00);
                                break;

                            case DialogueLine.TextFormatting.normal:
                                stringPoolMemoryBuffer.Add(0x01);
                                break;

                            case DialogueLine.TextFormatting.large:
                                stringPoolMemoryBuffer.Add(0x02);
                                break;
                        }
                        stringPoolPointer += 4;
                    }

                    byte[] newDialogueData = UnicodeToShiftJIS(dialogue.Dialogue);
                    stringPoolMemoryBuffer.AddRange(newDialogueData);
                    stringPoolPointer += newDialogueData.Length;
                }

                using (FileStream writeFileStream = new FileStream(scene.ParentFile, FileMode.Open))
                {
                    // Check how much memory space is available to write to, a 16 byte buffer is preserved for safety
                    byte[] fileDataBlock = new byte[16];
                    writeFileStream.Seek(scene.PointerListLocation + 2, SeekOrigin.Begin);
                    writeFileStream.Read(fileDataBlock);
                    int writableDataSize = 16;
                    bool reachedPadding = false;

                    while (writableDataSize < pointerMemoryBuffer.Count + stringPoolMemoryBuffer.Count)
                    {
                        // If we have passed all the relevant data and zero padding, we have run out of space
                        if (reachedPadding && !IsAllZeroes(fileDataBlock))
                        {
                            throw new Exception($"Not enough memory available to save! (Scene {scene.SceneTitle})");
                        }
                        else if (!reachedPadding && IsAllZeroes(fileDataBlock))
                        {
                            reachedPadding = true;
                        }

                        // Can also check for the signature of another event starting
                        if (ContainsSceneStartCode(fileDataBlock, out int _, out GameType _, out int _, true))
                        {
                            throw new Exception($"Not enough memory available to save! (Scene {scene.SceneTitle})");
                        }

                        writeFileStream.Seek(writeFileStream.Position - 12, SeekOrigin.Begin);
                        writeFileStream.Read(fileDataBlock);
                        writableDataSize += 4;
                    }

                    writeFileStream.Seek(scene.PointerListLocation + 2, SeekOrigin.Begin);
                    writeFileStream.Write(pointerMemoryBuffer.ToArray(), 0, pointerMemoryBuffer.Count);
                    writeFileStream.Write(stringPoolMemoryBuffer.ToArray(), 0, stringPoolMemoryBuffer.Count);
                }

                scene.Save();
            }
        }

        /// <summary>
        /// Searches for a string in the game's dialogue
        /// </summary>
        /// <param name="searchString">The string to find in the game</param>
        /// <param name="currentSceneIndex">If continuing a search, the current selected index to continue from</param>
        /// <returns>The location of the first match found</returns>
        public Tuple<int, int> SearchForString(string searchString, Tuple<int, int> currentSceneIndex = null)
        {
            int i = 0, j = 0;
            if (currentSceneIndex != null)
            {
                i = currentSceneIndex.Item1;
                j = currentSceneIndex.Item2 + 1;
            }

            for (; i < SceneList.Count; i++)
            {
                for (; j < SceneList[i].DialogueLines.Count; j++)
                {
                    if (SceneList[i].DialogueLines[j].Dialogue.Contains(searchString) || SceneList[i].DialogueLines[j].OriginalDialogue.Contains(searchString) ||
                        SceneList[i].DialogueLines[j].SpeakerName.Contains(searchString))
                        return new Tuple<int, int>(i, j);
                }

                j = 0;
            }

            return null;
        }

        /// <summary>
        /// Convenient tool to just modify every instance of the same Speaker like edit all identical flag can do for Dialogue
        /// </summary>
        /// <param name="sceneIndex">The scene that has the speaker being modified</param>
        /// <param name="dialogueIndex">The specific dialogue line with the speaker being modified</param>
        public void TranslateAllSpeakers(int sceneIndex, int dialogueIndex)
        {
            if (string.IsNullOrWhiteSpace(SceneList[sceneIndex].DialogueLines[dialogueIndex].SpeakerName) ||
                SceneList[sceneIndex].DialogueLines[dialogueIndex].SpeakerName == SceneList[sceneIndex].DialogueLines[dialogueIndex].OriginalSpeakerName)
            {
                return;
            }

            string oldText = SceneList[sceneIndex].DialogueLines[dialogueIndex].OriginalSpeakerName;
            string newText = SceneList[sceneIndex].DialogueLines[dialogueIndex].SpeakerName;

            foreach (Scene scene in SceneList)
            {
                foreach (DialogueLine line in scene.DialogueLines)
                {
                    if (line.SpeakerName == oldText)
                    {
                        line.EditSpeaker(newText);
                    }
                }
            }
        }

        private static bool ContainsPointerStartSignature(byte[] bytes, out int signatureOffset)
        {
            for (signatureOffset = 0; signatureOffset < bytes.Length - 1; signatureOffset++)
            {
                if (bytes[signatureOffset] == 0xF1 && (bytes[signatureOffset + 1] <= 0x20 ||
                  bytes[signatureOffset + 1] % 3 == 0) && bytes[signatureOffset + 1] <= 0x20)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ContainsSceneStartCode(byte[] bytes, out int location, out GameType gameType, out int textPointer, bool checkAllSignatures = false)
        {
            gameType = GameType.PopolocroisMonogatariII;
            textPointer = 0;

            for (int i = 0; i < bytes.Length - 9; i++)
            {
                if ((bytes[i + 0] == 'E' && bytes[i + 1] == 'V' && bytes[i + 2] == '2' && bytes[i + 3] == '\0') ||
                    (bytes[i + 0] == 'N' && bytes[i + 1] == 'P' && bytes[i + 2] == '2' && bytes[i + 3] == '\0') ||
                    (bytes[i + 0] == 'E' && bytes[i + 1] == 'V' && bytes[i + 2] == 'X' && bytes[i + 3] == '\0') ||
                    (bytes[i + 0] == 'N' && bytes[i + 1] == 'P' && bytes[i + 2] == 'X' && bytes[i + 3] == '\0'))
                {
                    location = i;

                    if ((bytes[i + 0] == 'E' && bytes[i + 1] == 'V' && bytes[i + 2] == 'X' && bytes[i + 3] == '\0') ||
                        (bytes[i + 0] == 'N' && bytes[i + 1] == 'P' && bytes[i + 2] == 'X' && bytes[i + 3] == '\0'))
                    {
                        gameType = GameType.PopoRogue;
                    }

                    textPointer = bytes[i + 8] + (bytes[i + 9] * 256);

                    return true;
                }

                if (checkAllSignatures &&
                    (bytes[i + 0] == 'T' && bytes[i + 1] == 'I' && bytes[i + 2] == 'X' && bytes[i + 3] == '\0' ||
                    bytes[i + 0] == 'A' && bytes[i + 1] == 'N' && bytes[i + 2] == '2' && bytes[i + 3] == '\0' ||
                    bytes[i + 0] == 'A' && bytes[i + 1] == 'N' && bytes[i + 2] == 'X' && bytes[i + 3] == '\0'))
                {
                    location = i;

                    return true;
                }
            }

            location = 0;
            return false;
        }

        private static byte[] CreatePointerBytes(int lineCount, long pointerOffset)
        {
            byte[] pointerData = new byte[]
            {
                (byte)lineCount,
                0x00,
                (byte)(pointerOffset % 256),
                (byte)(pointerOffset / 256),
            };

            return pointerData;
        }

        private static bool IsAllZeroes(byte[] bytes)
        {
            return bytes.All(b => b == 0x00);
        }

        private static void SeekNextNonWhiteSpace(StreamReader sr)
        {
            int nextChar = sr.Peek();

            while (!sr.EndOfStream && (nextChar == '\r' || nextChar == '\n' || nextChar == ' ' || nextChar == '\t'))
            {
                sr.Read();
                nextChar = sr.Peek();
            }
        }

        private void LoadScenes(string filePath, FileStream file)
        {
            file.Seek(0, SeekOrigin.Begin);

            SceneList ??= new List<Scene>();

            byte[] readBuffer = new byte[16];

            while (file.Position < file.Length && file.Position < 90000000)
            {
                file.Read(readBuffer);

                if (ContainsSceneStartCode(readBuffer, out int sceneStartByte, out GameType gameType, out int textPointer))
                {
                    // Advance position in case the scene does not start on an even 16-bit memory location
                    if (sceneStartByte > 0)
                    {
                        file.Position += sceneStartByte;
                    }

                    byte[] sceneTitleBuffer = new byte[UserSettings.SceneTitleExpectedLength];
                    file.Read(sceneTitleBuffer);
                    string sceneTitle;

                    // Extremely rare, but some scenes have no title
                    if (sceneTitleBuffer[0] == 0x00 && sceneTitleBuffer[1] == 0x00 && sceneTitleBuffer[2] == 0x00)
                    {
                        sceneTitle = SceneList.Count + "_" + file.Position;
                    }
                    else
                    {
                        sceneTitle = ShiftJISBytesToString(sceneTitleBuffer);
                    }

                    // PopoloCrois Monogatari II actually has helpful titles, clean them so they are readable
                    sceneTitle = sceneTitle.Replace('\0', ' ');

                    if (sceneTitle.Contains("   "))
                    {
                        sceneTitle = sceneTitle.Substring(0, sceneTitle.IndexOf("   "));
                    }

                    SceneList.Add(new Scene()
                    {
                        DialogueLines = new List<DialogueLine>(),
                        IsEdited = false,
                        LoadedGameType = gameType,
                        ParentFile = filePath,
                        SceneDataLocation = file.Position - UserSettings.SceneTitleExpectedLength,
                        SceneTitle = sceneTitle
                    });

                    int falsePositive = 0;

                    if (textPointer > 0)
                    {
                        if (gameType == GameType.PopoRogue)
                        {
                            file.Position += textPointer - 32;
                        }
                        else
                        {
                            file.Position += textPointer - 96;
                        }
                    }

                    // Begin looping through memory looking for the memory signature of the pointer list beginning
                    List<long> potentialPointerListStarts = new List<long>();

                    while (true)
                    {
                        byte[] pointerStartBuffer = new byte[65]; // In case F1 is last byte in series, need an odd number
                        file.Read(pointerStartBuffer);
                        file.Seek(-1, SeekOrigin.Current);

                        if (ContainsPointerStartSignature(pointerStartBuffer, out int signatureOffset))
                        {
                            potentialPointerListStarts.Add(file.Position - pointerStartBuffer.Length + signatureOffset);

                            file.Seek(signatureOffset - pointerStartBuffer.Length + 2, SeekOrigin.Current);
                            file.Seek(16 - file.Position % 16, SeekOrigin.Current);
                        }

                        if (IsAllZeroes(pointerStartBuffer) || ContainsSceneStartCode(pointerStartBuffer, out int _, out GameType _, out int _, checkAllSignatures: true))
                        {
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    potentialPointerListStarts.Reverse();

                    foreach (long pointerListStart in potentialPointerListStarts)
                    {
                        SceneList.Last().PointerListLocation = pointerListStart;
                        file.Seek(pointerListStart + 2, SeekOrigin.Begin);
                        byte[] dialoguePointerBuffer = new byte[4];
                        file.Read(dialoguePointerBuffer);

                        // another attempt at finding false positives, if the first is huge, it must be incorrect
                        bool pointerValidationIssue = false;
                        if (SceneList.Last().DialogueLines.Count == 0 && (dialoguePointerBuffer[3] > 0x03 || (dialoguePointerBuffer[3] == 0x00 && dialoguePointerBuffer[2] <= 0x04) || dialoguePointerBuffer[1] != 0x00))
                        {
                            pointerValidationIssue = true;
                        }

                        while (dialoguePointerBuffer[0] <= UserSettings.MaxReadableLines && dialoguePointerBuffer[1] == 0x00 && (dialoguePointerBuffer[2] != 0x00 || dialoguePointerBuffer[3] != 0x00) && !pointerValidationIssue)
                        {
                            int lineCount = dialoguePointerBuffer[0];
                            long dialoguePointerLocation = file.Position - dialoguePointerBuffer.Length;
                            long dialoguePointerOffsetValue = dialoguePointerBuffer[2] + dialoguePointerBuffer[3] * 256;
                            string speakerName = string.Empty;
                            string dialogue;
                            DialogueLine.TextFormatting format = DialogueLine.TextFormatting.none;

                            file.Seek(pointerListStart + dialoguePointerOffsetValue, SeekOrigin.Begin);
                            file.Read(dialoguePointerBuffer);
                            bool isPointer = dialoguePointerBuffer[0] <= UserSettings.MaxReadableLines && dialoguePointerBuffer[1] == 0x00 && (dialoguePointerBuffer[2] != 0x00 || dialoguePointerBuffer[3] != 0x00);

                            // PopoloCrois Monogatari II Episode 4 edge case where text string starts with an empty hex code
                            if (gameType == GameType.PopolocroisMonogatariII && sceneTitle.StartsWith("CBA_08") && dialoguePointerBuffer[0] == 0x05 && dialoguePointerBuffer[1] == 0x00)
                            {
                                isPointer = false;
                            }

                            if (!isPointer)
                            {
                                file.Seek(-dialoguePointerBuffer.Length, SeekOrigin.Current);
                                byte[] dialogueTextBuffer = new byte[UserSettings.MaxReadableDialogueLength];
                                file.Read(dialogueTextBuffer);

                                string decodedDialogue = ShiftJISBytesToString(dialogueTextBuffer, true);

                                if ((gameType == GameType.PopolocroisMonogatariII && decodedDialogue[0] == '>') ||
                                    (gameType == GameType.PopoRogue && decodedDialogue[0] == '<'))
                                {
                                    speakerName = decodedDialogue.Substring(1, decodedDialogue.IndexOf('\0') - 1);
                                    decodedDialogue = decodedDialogue.Substring(decodedDialogue.IndexOf('\0') + 1);
                                }

                                if ((decodedDialogue[0] == 0x05 && decodedDialogue[1] == 0x03 && decodedDialogue[2] == 0x00) ||
                                    (decodedDialogue[0] == 0x00 && decodedDialogue[1] == 0x05 && decodedDialogue[2] == 0x03 && decodedDialogue[3] == 0x00))
                                {
                                    switch (decodedDialogue[0] == 0x00 ? (byte)decodedDialogue[4] : (byte)decodedDialogue[3])
                                    {
                                        case 0:
                                            format = DialogueLine.TextFormatting.small;
                                            break;

                                        case 1:
                                            format = DialogueLine.TextFormatting.normal;
                                            break;

                                        case 2:
                                            format = DialogueLine.TextFormatting.large;
                                            break;
                                    }

                                    if (decodedDialogue[0] != 0x00)
                                    {
                                        decodedDialogue = decodedDialogue.Substring(4);
                                    }
                                    else
                                    {
                                        decodedDialogue = decodedDialogue.Substring(5);
                                    }
                                }

                                if (lineCount != 0)
                                {
                                    dialogue = string.Join('\n', decodedDialogue.Replace('\0', '\n').Split('\n').Take(lineCount)) + "\n";
                                }
                                else
                                {
                                    dialogue = PointerTextString;
                                    speakerName = string.Empty;
                                }
                            }
                            else
                            {
                                dialogue = "Reused Dialogue!\n\nThis is likely a piece of dialogue spoken by several NPC's.\n(It is listed on a later number)";
                                speakerName = string.Empty;
                            }

                            SceneList.Last().DialogueLines.Add(new DialogueLine(
                                dialogue,
                                dialoguePointerLocation,
                                dialoguePointerOffsetValue,
                                isPointer,
                                lineCount,
                                speakerName,
                                format,
                                SceneList.Last()));

                            file.Seek(dialoguePointerLocation + 4, SeekOrigin.Begin);
                            file.Read(dialoguePointerBuffer);
                        }

                        // Allow for multiple failed attempts at finding the pointer list beginning until giving up
                        if (SceneList.Last().DialogueLines.Count == 0)
                        {
                            if (falsePositive < 100)
                            {
                                falsePositive++;
                                continue;
                            }
                        }
                        // Extremely rare case, if all that was found was these empty non-pointers, try a little more
                        else if (SceneList.Last().DialogueLines.All(d => d.Dialogue == PointerTextString))
                        {
                            if (falsePositive < 100)
                            {
                                SceneList.Last().DialogueLines.Clear();

                                falsePositive++;
                                continue;
                            }
                        }

                        break;
                    }

                    // reset pointer to be 16 byte aligned and continue file search
                    if (potentialPointerListStarts.Any())
                    {
                        file.Seek(SceneList.Last().PointerListLocation + 128, SeekOrigin.Begin);
                    }
                    else
                    {
                        file.Seek(SceneList.Last().SceneDataLocation + 128, SeekOrigin.Begin);
                    }

                    file.Seek(16 - file.Position % 16, SeekOrigin.Current);

                    // Removing empty scenes from the list
                    if (!SceneList.Last().DialogueLines.Any())
                    {
                        SceneList.RemoveAt(SceneList.Count - 1);
                    }
                }
            }
        }

        private string ShiftJISBytesToString(byte[] bytes, bool checkEmoticons = false)
        {
            string returnString = string.Empty;

            byte[] UnicodeBytes = Encoding.Convert(Encoding.GetEncoding(932), Encoding.Unicode, bytes);

            for (int i = 0; i < UnicodeBytes.Length; i += 2)
            {
                returnString += (char)((char)UnicodeBytes[i] + ((char)UnicodeBytes[i + 1] * 256));
            }

            if (checkEmoticons)
            {
                for (int i = 0; i < returnString.Length - 3; i++)
                {
                    if (returnString[i] == 0x05 && returnString[i + 1] == 0x09 && returnString[i + 2] == 0x00)
                    {
                        byte emoticonNum = (byte)returnString[i + 3];
                        returnString = returnString.Replace(returnString.Substring(i, 4), UserSettings.EmoticonCharacter + emoticonNum.ToString() + UserSettings.EmoticonCharacter);
                    }

                    // Check for recognized hexadecimal patterns that should be maintained but need converting for use in the editor
                    if (returnString[i] == 0x05 && returnString[i + 1] == 0x02)
                    {
                        returnString = returnString.Replace(returnString.Substring(i, 2), $"{UserSettings.RawHexadecimalCharacter}0x0502{UserSettings.RawHexadecimalCharacter}");
                    }
                    else if (returnString[i] == 0x05 && returnString[i + 1] == 0x04)
                    {
                        returnString = returnString.Replace(returnString.Substring(i, 2), $"{UserSettings.RawHexadecimalCharacter}0x0504{UserSettings.RawHexadecimalCharacter}");
                    }
                    else if (returnString[i] == 0x05 && returnString[i + 1] == 0x07)
                    {
                        returnString = returnString.Replace(returnString.Substring(i, 2), $"{UserSettings.RawHexadecimalCharacter}0x0507{UserSettings.RawHexadecimalCharacter}");
                    }
                    else if (returnString[i] == 0x05 && returnString[i + 1] == 0x01)
                    {
                        returnString = returnString.Replace(returnString.Substring(i, 4), $"{UserSettings.RawHexadecimalCharacter}0x05010" + ((byte)returnString[i + 2]).ToString() + "0" + ((byte)returnString[i + 3]).ToString() + UserSettings.RawHexadecimalCharacter);
                    }
                    else if (returnString[i] == 0x09)
                    {
                        returnString = returnString.Replace(returnString.Substring(i, 2), $"{UserSettings.RawHexadecimalCharacter}0x09" + ((byte)returnString[i + 1]).ToString("X2") + UserSettings.RawHexadecimalCharacter);
                    }
                }
            }

            return returnString;
        }

        private byte[] UnicodeToShiftJIS(string Dialogue)
        {
            List<byte> dialogueData = new List<byte>();

            for (int i = 0; i < Dialogue.Length; i++)
            {
                if (Dialogue[i] == UserSettings.RawHexadecimalCharacter)
                {
                    i += 3;
                    while (Dialogue[i] != UserSettings.RawHexadecimalCharacter)
                    {
                        dialogueData.Add(byte.Parse($"{Dialogue[i]}{Dialogue[i + 1]}", System.Globalization.NumberStyles.AllowHexSpecifier));
                        i += 2;
                    }
                    continue;
                }
                else if (Dialogue[i] == UserSettings.EmoticonCharacter && SceneList.Any(sc => sc.LoadedGameType == GameType.PopolocroisMonogatariII))
                {
                    dialogueData.Add(0x05);
                    dialogueData.Add(0x09);
                    dialogueData.Add(0x00);
                    dialogueData.Add((byte)(Dialogue[i + 1] - '0'));
                    i += 2;

                    continue;
                }
                else if (Dialogue[i] == '\n')
                {
                    dialogueData.Add(0x00);
                    continue;
                }
                else if (Dialogue[i] <= 'z' + 15) // Some buffer for other English punctuation
                {
                    dialogueData.Add((byte)Dialogue[i]);
                    continue;
                }
                else
                {
                    byte[] UnicodeBytes = new byte[] { (byte)(Dialogue[i] % 256), (byte)(Dialogue[i] / 256) };
                    byte[] JISBytes = Encoding.Convert(Encoding.Unicode, Encoding.GetEncoding(932), UnicodeBytes);

                    dialogueData.Add(JISBytes[0]);

                    if (JISBytes.Length > 1) // Unicode to Shift-JIS can produce 1 or 2 byte characters
                    {
                        dialogueData.Add(JISBytes[1]);
                    }

                    continue;
                }
            }

            return dialogueData.ToArray();
        }
    }
}