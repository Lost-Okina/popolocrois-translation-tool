using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace PopoloCroisTranslationTool
{
    /// <summary>
    /// The user interface implementation
    /// </summary>
    public partial class TranslationWindow : Form
    {
        private const string DefaultWindowTitle = "PopoloCrois Translation Tool";

        /// <summary>
        /// The list of duplicate text box locations when editing all identical text is enabled
        /// </summary>
        private List<Tuple<int, int>>? DuplicateEditingIndices = null;

        /// <summary>
        /// The loaded game files
        /// </summary>
        private GameFile GameData;

        /// <summary>
        /// Used as a semaphore of sorts, blocks certain async actions when modifying the '*' that indicates a user's unsaved changes
        /// </summary>
        private bool UpdatingTitles = false;

        /// <summary>
        /// Entry Point
        /// </summary>
        public TranslationWindow()
        {
            InitializeComponent();

            GameData = new GameFile(LoadOrCreateSettings());
        }

        public void CenterDialogue(object sender, EventArgs e)
        {
            if (!GameData.IsLoaded())
                return;

            GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].CenterDialogue();

            SynchronizeSelectedScene();

            UpdateEditedFlagsSafely();
        }

        public void CloseFiles(object sender, EventArgs e)
        {
            if (GameData == null || (GameData.ChangesMade() && !Prompt.ShowDialog("Discard Unsaved Changes?", "")))
            {
                return;
            }

            UpdatingTitles = true;
            SceneList.DataSource = null;
            DialogueList.DataSource = null;
            DialogueTextBox.Text = string.Empty;
            SpeakerNameTextBox.Text = string.Empty;
            OriginalDialogueTextBox.Text = string.Empty;
            OriginalSpeakerNameTextBox.Text = string.Empty;
            FormattingDropDown.SelectedIndex = 0;
            OriginalFormattingDropDown.SelectedIndex = 0;
            Text = DefaultWindowTitle;
            UpdatingTitles = false;

            GameData.CloseFiles();
        }

        public void DialogueListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (UpdatingTitles || !GameData.IsLoaded())
            {
                return;
            }

            SynchronizeSelectedScene();

            if (EditAllCheckbox.Checked)
            {
                DuplicateEditingIndices = GameData.FindAllDuplicates(DialogueTextBox.Text, SpeakerNameTextBox.Text, FormattingDropDown.SelectedIndex);
                MatchingCountTextBox.Text = DuplicateEditingIndices.Count.ToString() + " duplicates";
            }
        }

        public void DialogueTextModified(object sender, EventArgs e)
        {
            if (SceneList.SelectedIndex > -1 && !GameData.SceneList[SceneList.SelectedIndex].IsEmptyScene() && DialogueTextBox.Text != GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].Dialogue)
            {
                if (DuplicateEditingIndices == null)
                {
                    GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].EditDialogue(DialogueTextBox.Text);
                }
                else
                {
                    foreach (var duplicate in DuplicateEditingIndices)
                    {
                        GameData.SceneList[duplicate.Item1].DialogueLines[duplicate.Item2].EditDialogue(DialogueTextBox.Text);
                    }
                }

                LineCountTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].LineCount.ToString();

                if (CheckLineLengths(DialogueTextBox.Text))
                {
                    if (GameData.SceneList[SceneList.SelectedIndex].LoadedGameType == GameType.PopolocroisMonogatariII)
                    {
                        MaxLineLengthLabel.Text = "Max Line Length is 32!";
                    }
                    else
                    {
                        MaxLineLengthLabel.Text = "Max Line Length is 33!";
                    }
                }
                else
                {
                    MaxLineLengthLabel.Text = string.Empty;
                }

                UpdateEditedFlagsSafely();
            }
        }

        public void DragDropCallback(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];

            if (files != null && files.Any())
            {
                LoadFiles(files);
            }
        }

        public void DragEnterCallback(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public void EditAllCheckedChanged(object sender, EventArgs e)
        {
            if (UpdatingTitles || !GameData.IsLoaded())
            {
                return;
            }

            if (EditAllCheckbox.Checked)
            {
                DuplicateEditingIndices = GameData.FindAllDuplicates(DialogueTextBox.Text, SpeakerNameTextBox.Text, FormattingDropDown.SelectedIndex);
                MatchingCountTextBox.Text = DuplicateEditingIndices.Count.ToString() + " duplicates";
            }
            else
            {
                MatchingCountTextBox.Text = string.Empty;
                DuplicateEditingIndices = null;
            }
        }

        public void ExportScenes(object sender, EventArgs e)
        {
            if (!GameData.IsLoaded())
                return;

            SaveFileDialog.Filter = "Text files|*.txt";

            try
            {
                SaveFileDialog.ShowDialog();

                if (!string.IsNullOrWhiteSpace(SaveFileDialog.FileName))
                {
                    GameData.ExportScenes(SaveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog(ex.Message, "");
            }
        }

        public void FormattingChanged(object sender, EventArgs e)
        {
            if (SceneList.SelectedIndex > -1 && !GameData.SceneList[SceneList.SelectedIndex].IsEmptyScene() && FormattingDropDown.SelectedIndex != (int)GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].TextFormat)
            {
                if (DuplicateEditingIndices == null)
                {
                    GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].EditFormatting(FormattingDropDown.SelectedIndex);
                }
                else
                {
                    foreach (var duplicate in DuplicateEditingIndices)
                    {
                        GameData.SceneList[duplicate.Item1].DialogueLines[duplicate.Item2].EditFormatting(FormattingDropDown.SelectedIndex);
                    }
                }

                UpdateEditedFlagsSafely();
            }
        }

        public void ImportScenes(object sender, EventArgs e)
        {
            if (!GameData.IsLoaded())
            {
                return;
            }

            OpenFileDialog.Filter = "Text files|*.txt";

            try
            {
                OpenFileDialog.ShowDialog();

                GameData.ImportScenes(OpenFileDialog.FileName);
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog(ex.Message, "");
            }

            UpdateEditedFlagsSafely();

            SynchronizeSelectedScene();

            if (EditAllCheckbox.Checked)
            {
                DuplicateEditingIndices = GameData.FindAllDuplicates(DialogueTextBox.Text, SpeakerNameTextBox.Text, FormattingDropDown.SelectedIndex);
                MatchingCountTextBox.Text = DuplicateEditingIndices.Count.ToString() + " duplicates";
            }
        }

        public void LoadFile(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(OpenFileDialog.FileName))
            {
                OpenFileDialog.FileName = OpenFileDialog.FileName.Substring(OpenFileDialog.FileName.LastIndexOf("\\") + 1);
            }

            OpenFileDialog.Filter = "Binary files|*.BIN";

            OpenFileDialog.ShowDialog();

            LoadFiles(OpenFileDialog.FileNames);
        }

        public void SaveAllScenes(object sender, EventArgs e)
        {
            if (GameData == null)
            {
                return;
            }

            if (GameData.ChangesMade() && Prompt.ShowDialog("Permanently Save Changes to File?", ""))
            {
                try
                {
                    GameData.SaveScenes();
                }
                catch (Exception ex)
                {
                    Prompt.ShowDialog(ex.Message, "");
                    return;
                }
            }
            else
            {
                return;
            }

            RemoveEditedFlagsSafely(true);

            DialogueTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].Dialogue;
            LineCountTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].LineCount.ToString();
        }

        public void SaveScene(object sender, EventArgs e)
        {
            if (GameData == null)
            {
                return;
            }

            if (GameData.ChangesMade() && Prompt.ShowDialog("Permanently Save Changes to File?", ""))
            {
                try
                {
                    GameData.SaveScenes(SceneList.SelectedIndex);
                }
                catch (Exception ex)
                {
                    Prompt.ShowDialog(ex.Message, "");
                    return;
                }
            }
            else
            {
                return;
            }

            RemoveEditedFlagsSafely();

            DialogueTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].Dialogue;
            LineCountTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].LineCount.ToString();
        }

        public void SaveScenesAs(object sender, EventArgs e)
        {
            if (GameData == null)
            {
                return;
            }

            SaveFileDialog.FileName = OpenFileDialog.FileName.Substring(OpenFileDialog.FileName.LastIndexOf("\\") + 1);
            SaveFileDialog.Filter = "Binary files|*.BIN";

            SaveFileDialog.ShowDialog();

            if (string.IsNullOrWhiteSpace(SaveFileDialog.FileName))
            {
                return;
            }

            string currentFile = GameData.SceneList[SceneList.SelectedIndex].ParentFile;

            if (!Prompt.ShowDialog($"Save file {currentFile.Substring(OpenFileDialog.FileName.LastIndexOf("\\") + 1)} as {SaveFileDialog.FileName.Substring(OpenFileDialog.FileName.LastIndexOf("\\") + 1)}?", ""))
            {
                return;
            }

            try
            {
                GameData.SaveScenes(newFile: currentFile, oldFile: SaveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                Prompt.ShowDialog(ex.Message, "");
                return;
            }

            Text = Text.Replace(currentFile.Substring(currentFile.LastIndexOf("\\") + 1), SaveFileDialog.FileName.Substring(SaveFileDialog.FileName.LastIndexOf("\\") + 1));

            RemoveEditedFlagsSafely(true);
        }

        public void SceneListSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!UpdatingTitles && SceneList.SelectedIndex > -1)
            {
                BindingList<string> dialogueListText = [];

                for (int i = 0; i < GameData.SceneList[SceneList.SelectedIndex].DialogueLines.Count; i++)
                {
                    if (!GameData.SceneList[SceneList.SelectedIndex].DialogueLines[i].IsEdited)
                    {
                        dialogueListText.Add((i + 1).ToString());
                    }
                    else
                    {
                        dialogueListText.Add((i + 1).ToString() + "*");
                    }
                }

                DialogueList.DataSource = dialogueListText;

                if (SceneList.SelectedIndex > -1 && GameData.SceneList[SceneList.SelectedIndex].IsEmptyScene())
                {
                    DialogueTextBox.Text = string.Empty;
                    SpeakerNameTextBox.Text = string.Empty;
                    OriginalDialogueTextBox.Text = string.Empty;
                    OriginalSpeakerNameTextBox.Text = string.Empty;
                    FormattingDropDown.SelectedIndex = 0;
                    MatchingCountTextBox.Text = string.Empty;
                    LineCountTextBox.Text = string.Empty;
                    OriginalLineCountTextBox.Text = string.Empty;
                }
            }
        }

        public void SearchScenes(object sender, EventArgs e)
        {
            using (var form = new TextPrompt())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK && form.ReturnValue.Length != 0)
                {
                    Tuple<int, int> stringLocation;

                    if (form.FromLocation)
                    {
                        stringLocation = GameData.SearchForString(form.ReturnValue, new Tuple<int, int>(SceneList.SelectedIndex, DialogueList.SelectedIndex));
                    }
                    else
                    {
                        stringLocation = GameData.SearchForString(form.ReturnValue);
                    }

                    if (stringLocation != null)
                    {
                        UpdatingTitles = true;
                        SceneList.SelectedIndices.Clear();
                        UpdatingTitles = false;

                        SceneList.SetSelected(stringLocation.Item1, true);
                        DialogueList.SelectedIndex = stringLocation.Item2;

                        int textLocation = DialogueTextBox.Text.IndexOf(form.ReturnValue);
                        if (textLocation == -1)
                        {
                            textLocation = OriginalDialogueTextBox.Text.IndexOf(form.ReturnValue);

                            if (textLocation != -1)
                            {
                                OriginalDialogueTextBox.Focus();
                                OriginalDialogueTextBox.Select(textLocation, form.ReturnValue.Length);
                            }
                            else
                            {
                                textLocation = SpeakerNameTextBox.Text.IndexOf(form.ReturnValue);
                                SpeakerNameTextBox.Focus();
                                SpeakerNameTextBox.Select(textLocation, form.ReturnValue.Length);
                            }
                        }
                        else
                        {
                            DialogueTextBox.Focus();
                            DialogueTextBox.Select(textLocation, form.ReturnValue.Length);
                        }
                    }
                }
            }
        }

        public void SpeakerTextModified(object sender, EventArgs e)
        {
            if (SceneList.SelectedIndex > -1 && !GameData.SceneList[SceneList.SelectedIndex].IsEmptyScene() && SpeakerNameTextBox.Text != GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].SpeakerName)
            {
                if (DuplicateEditingIndices == null)
                {
                    GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].EditSpeaker(SpeakerNameTextBox.Text);
                }
                else
                {
                    foreach (var duplicate in DuplicateEditingIndices)
                    {
                        GameData.SceneList[duplicate.Item1].DialogueLines[duplicate.Item2].EditSpeaker(SpeakerNameTextBox.Text);
                    }
                }

                UpdateEditedFlagsSafely();
            }
        }

        private bool CheckLineLengths(string dialogue)
        {
            int maxLineLength = dialogue.Split('\n').Max(x => x.Length);

            if ((maxLineLength > 32 && GameData.SceneList[SceneList.SelectedIndex].LoadedGameType == GameType.PopolocroisMonogatariII) ||
                (maxLineLength > 33 && GameData.SceneList[SceneList.SelectedIndex].LoadedGameType == GameType.PopoRogue))
            {
                return true;
            }

            return false;
        }

        private BindingList<string> GetSceneTitles()
        {
            BindingList<string> titles = [];

            foreach (Scene scene in GameData.SceneList)
            {
                if (!scene.IsEdited)
                {
                    titles.Add(scene.SceneTitle);
                }
                else
                {
                    titles.Add(scene.SceneTitle + "*");
                }
            }

            return titles;
        }

        private void LoadFiles(string[] fileNames)
        {
            if (fileNames.Length > 1)
            {
                foreach (var path in fileNames)
                {
                    if (GameData.LoadFile(path))
                    {
                        if (Text == DefaultWindowTitle)
                        {
                            Text = DefaultWindowTitle + " (" + path.Split('\\').Last() + ")";
                        }
                        else
                        {
                            Text = Text.Remove(Text.Length - 1);
                            Text += " & " + path.Split('\\').Last() + ")";
                        }

                        SceneList.DataSource = GetSceneTitles();
                    }
                }
            }
            else if (fileNames.Length == 1 && !string.IsNullOrWhiteSpace(fileNames[0]) && GameData.LoadFile(fileNames[0]))
            {
                if (Text == DefaultWindowTitle)
                {
                    Text = DefaultWindowTitle + " (" + fileNames[0].Split('\\').Last() + ")";
                }
                else
                {
                    Text = Text.Remove(Text.Length - 1);
                    Text += " & " + fileNames[0].Split('\\').Last() + ")";
                }

                SceneList.DataSource = GetSceneTitles();
            }
        }

        private Settings LoadOrCreateSettings()
        {
            string settingsFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "settings.json");

            if (File.Exists(settingsFilePath))
            {
                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsFilePath));
            }
            else
            {
                Settings settings = new();

                File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(settings));

                return settings;
            }
        }

        private void RemoveEditedFlagsSafely(bool removeAll = false)
        {
            UpdatingTitles = true;

            if (SceneList.DataSource != null && DialogueList.DataSource != null)
            {
                var sceneListData = (BindingList<string>)SceneList.DataSource;
                var dialogueListData = (BindingList<string>)DialogueList.DataSource;

                if (removeAll)
                {
                    for (int i = 0; i < sceneListData.Count; i++)
                    {
                        if (sceneListData[i].Last() == '*')
                        {
                            sceneListData[i] = sceneListData[i].Remove(sceneListData[i].Length - 1);
                        }
                    }
                }
                else
                {
                    if (sceneListData[SceneList.SelectedIndex].Last() == '*')
                    {
                        sceneListData[SceneList.SelectedIndex] = sceneListData[SceneList.SelectedIndex].Remove(sceneListData[SceneList.SelectedIndex].Length - 1);
                    }
                }

                for (int i = 0; i < dialogueListData.Count; i++)
                {
                    if (dialogueListData[i].Last() == '*')
                    {
                        dialogueListData[i] = dialogueListData[i].Remove(dialogueListData[i].Length - 1);
                    }
                }
            }

            UpdatingTitles = false;
        }

        private void SynchronizeSelectedScene()
        {
            DialogueTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].Dialogue;
            OriginalDialogueTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].OriginalDialogue;
            SpeakerNameTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].SpeakerName;
            OriginalSpeakerNameTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].OriginalSpeakerName;
            FormattingDropDown.SelectedIndex = (int)GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].TextFormat;
            OriginalFormattingDropDown.SelectedIndex = (int)GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].TextFormat;
            LineCountTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].LineCount.ToString();
            OriginalLineCountTextBox.Text = GameData.SceneList[SceneList.SelectedIndex].DialogueLines[DialogueList.SelectedIndex].OriginalLineCount.ToString();
        }

        private void TranslateAllSpeakers(object sender, EventArgs e)
        {
            if (!GameData.IsLoaded())
                return;

            GameData.TranslateAllSpeakers(SceneList.SelectedIndex, DialogueList.SelectedIndex);

            UpdateEditedFlagsSafely();
        }

        private void TranslationWindowLoad(object sender, EventArgs e)
        {
        }

        private void UpdateEditedFlagsSafely()
        {
            UpdatingTitles = true;

            if (SceneList.DataSource != null && DialogueList.DataSource != null)
            {
                var sceneListObject = (BindingList<string>)SceneList.DataSource;
                var dialogueListObject = (BindingList<string>)DialogueList.DataSource;

                for (int i = 0; i < sceneListObject.Count; i++)
                {
                    if (GameData.SceneList[i].IsEdited && sceneListObject[i].Last() != '*')
                    {
                        sceneListObject[i] += '*';
                    }
                }

                for (int i = 0; i < dialogueListObject.Count; i++)
                {
                    if (GameData.SceneList[SceneList.SelectedIndex].DialogueLines[i].IsEdited && dialogueListObject[i].Last() != '*')
                    {
                        dialogueListObject[i] += '*';
                    }
                }
            }

            SceneList.Refresh();
            DialogueList.Refresh();

            UpdatingTitles = false;
        }
    }
}