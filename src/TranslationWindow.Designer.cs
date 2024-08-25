namespace PopoloCroisTranslationTool
{
    partial class TranslationWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TranslationWindow));
            SceneList = new ListBox();
            MenuStrip = new MenuStrip();
            FileToolStripMenuItem = new ToolStripMenuItem();
            LoadToolStripMenuItem = new ToolStripMenuItem();
            CloseToolStripMenuItem = new ToolStripMenuItem();
            SaveToolStripMenuItem = new ToolStripMenuItem();
            SaveAllScenesToolStripMenuItem = new ToolStripMenuItem();
            SaveAsToolStripMenuItem = new ToolStripMenuItem();
            ExportToolStripMenuItem = new ToolStripMenuItem();
            ImportToolStripMenuItem = new ToolStripMenuItem();
            ToolsToolStripMenuItem = new ToolStripMenuItem();
            SearchToolStripMenuItem = new ToolStripMenuItem();
            CenterTextToolStripMenuItem = new ToolStripMenuItem();
            TranslateAllSpeakersToolStripMenuItem = new ToolStripMenuItem();
            OpenFileDialog = new OpenFileDialog();
            SceneLabel = new Label();
            DialogueList = new ListBox();
            DialogueTextBox = new RichTextBox();
            DialogueLineLabel = new Label();
            SpeakerNameTextBox = new RichTextBox();
            FormattingDropDown = new ComboBox();
            SpeakerLabel = new Label();
            FormattingLabel = new Label();
            DialogueLabel = new Label();
            EditAllCheckbox = new CheckBox();
            MatchingCountTextBox = new TextBox();
            LineCountTextBox = new TextBox();
            LineCountLabel = new Label();
            MaxLineLengthLabel = new Label();
            SaveFileDialog = new SaveFileDialog();
            OriginalDialogueTextBox = new RichTextBox();
            OriginalSpeakerNameTextBox = new RichTextBox();
            OriginalFormattingDropDown = new ComboBox();
            OriginalLineCountTextBox = new TextBox();
            OriginalLabel = new Label();
            NewLabel = new Label();
            MenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // SceneList
            // 
            SceneList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            SceneList.Font = new Font("Arial", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SceneList.FormattingEnabled = true;
            SceneList.HorizontalScrollbar = true;
            SceneList.Location = new Point(0, 53);
            SceneList.Margin = new Padding(2, 1, 2, 1);
            SceneList.Name = "SceneList";
            SceneList.ScrollAlwaysVisible = true;
            SceneList.SelectionMode = SelectionMode.MultiExtended;
            SceneList.Size = new Size(291, 452);
            SceneList.TabIndex = 1;
            SceneList.SelectedIndexChanged += SceneListSelectedIndexChanged;
            // 
            // MenuStrip
            // 
            MenuStrip.BackColor = SystemColors.ControlDark;
            MenuStrip.ImageScalingSize = new Size(32, 32);
            MenuStrip.Items.AddRange(new ToolStripItem[] { FileToolStripMenuItem, ToolsToolStripMenuItem });
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Padding = new Padding(2, 1, 0, 1);
            MenuStrip.Size = new Size(922, 24);
            MenuStrip.TabIndex = 2;
            // 
            // FileToolStripMenuItem
            // 
            FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { LoadToolStripMenuItem, CloseToolStripMenuItem, SaveToolStripMenuItem, SaveAllScenesToolStripMenuItem, SaveAsToolStripMenuItem, ExportToolStripMenuItem, ImportToolStripMenuItem });
            FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            FileToolStripMenuItem.Size = new Size(37, 22);
            FileToolStripMenuItem.Text = "File";
            // 
            // LoadToolStripMenuItem
            // 
            LoadToolStripMenuItem.Name = "LoadToolStripMenuItem";
            LoadToolStripMenuItem.Size = new Size(226, 22);
            LoadToolStripMenuItem.Text = "Load";
            LoadToolStripMenuItem.Click += LoadFile;
            // 
            // CloseToolStripMenuItem
            // 
            CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
            CloseToolStripMenuItem.Size = new Size(226, 22);
            CloseToolStripMenuItem.Text = "Close";
            CloseToolStripMenuItem.Click += CloseFiles;
            // 
            // SaveToolStripMenuItem
            // 
            SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            SaveToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.S;
            SaveToolStripMenuItem.Size = new Size(226, 22);
            SaveToolStripMenuItem.Text = "Save Scene";
            SaveToolStripMenuItem.Click += SaveScene;
            // 
            // SaveAllScenesToolStripMenuItem
            // 
            SaveAllScenesToolStripMenuItem.Name = "SaveAllScenesToolStripMenuItem";
            SaveAllScenesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            SaveAllScenesToolStripMenuItem.Size = new Size(226, 22);
            SaveAllScenesToolStripMenuItem.Text = "Save All Scenes";
            SaveAllScenesToolStripMenuItem.Click += SaveAllScenes;
            // 
            // SaveAsToolStripMenuItem
            // 
            SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            SaveAsToolStripMenuItem.Size = new Size(226, 22);
            SaveAsToolStripMenuItem.Text = "Save All As";
            SaveAsToolStripMenuItem.Click += SaveScenesAs;
            // 
            // ExportToolStripMenuItem
            // 
            ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            ExportToolStripMenuItem.Size = new Size(226, 22);
            ExportToolStripMenuItem.Text = "Export As";
            ExportToolStripMenuItem.Click += ExportScenes;
            // 
            // ImportToolStripMenuItem
            // 
            ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            ImportToolStripMenuItem.Size = new Size(226, 22);
            ImportToolStripMenuItem.Text = "Import";
            ImportToolStripMenuItem.Click += ImportScenes;
            // 
            // ToolsToolStripMenuItem
            // 
            ToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SearchToolStripMenuItem, CenterTextToolStripMenuItem, TranslateAllSpeakersToolStripMenuItem });
            ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            ToolsToolStripMenuItem.Size = new Size(46, 22);
            ToolsToolStripMenuItem.Text = "Tools";
            // 
            // SearchToolStripMenuItem
            // 
            SearchToolStripMenuItem.Name = "SearchToolStripMenuItem";
            SearchToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.F;
            SearchToolStripMenuItem.Size = new Size(186, 22);
            SearchToolStripMenuItem.Text = "Search";
            SearchToolStripMenuItem.Click += SearchScenes;
            // 
            // CenterTextToolStripMenuItem
            // 
            CenterTextToolStripMenuItem.Name = "CenterTextToolStripMenuItem";
            CenterTextToolStripMenuItem.Size = new Size(186, 22);
            CenterTextToolStripMenuItem.Text = "Center Text";
            CenterTextToolStripMenuItem.Click += CenterDialogue;
            // 
            // TranslateAllSpeakersToolStripMenuItem
            // 
            TranslateAllSpeakersToolStripMenuItem.Name = "TranslateAllSpeakersToolStripMenuItem";
            TranslateAllSpeakersToolStripMenuItem.Size = new Size(186, 22);
            TranslateAllSpeakersToolStripMenuItem.Text = "Translate All Speakers";
            TranslateAllSpeakersToolStripMenuItem.Click += TranslateAllSpeakers;
            // 
            // OpenFileDialog
            // 
            OpenFileDialog.FileName = string.Empty;
            OpenFileDialog.Filter = "Binary files|*.BIN";
            OpenFileDialog.Multiselect = true;
            OpenFileDialog.SupportMultiDottedExtensions = true;
            // 
            // SceneLabel
            // 
            SceneLabel.AutoSize = true;
            SceneLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SceneLabel.Location = new Point(7, 29);
            SceneLabel.Margin = new Padding(2, 0, 2, 0);
            SceneLabel.Name = "SceneLabel";
            SceneLabel.Size = new Size(125, 18);
            SceneLabel.TabIndex = 3;
            SceneLabel.Text = "Scene / Location";
            // 
            // DialogueList
            // 
            DialogueList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            DialogueList.Font = new Font("Arial", 10.875F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DialogueList.FormattingEnabled = true;
            DialogueList.Location = new Point(296, 53);
            DialogueList.Margin = new Padding(2, 1, 2, 1);
            DialogueList.Name = "DialogueList";
            DialogueList.ScrollAlwaysVisible = true;
            DialogueList.Size = new Size(76, 452);
            DialogueList.TabIndex = 4;
            DialogueList.SelectedIndexChanged += DialogueListSelectedIndexChanged;
            // 
            // DialogueTextBox
            // 
            DialogueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            DialogueTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DialogueTextBox.Location = new Point(659, 178);
            DialogueTextBox.Margin = new Padding(2, 1, 2, 1);
            DialogueTextBox.Name = "DialogueTextBox";
            DialogueTextBox.Size = new Size(250, 206);
            DialogueTextBox.TabIndex = 5;
            DialogueTextBox.Text = "";
            DialogueTextBox.TextChanged += DialogueTextModified;
            // 
            // DialogueLineLabel
            // 
            DialogueLineLabel.AutoSize = true;
            DialogueLineLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DialogueLineLabel.Location = new Point(287, 29);
            DialogueLineLabel.Margin = new Padding(2, 0, 2, 0);
            DialogueLineLabel.Name = "DialogueLineLabel";
            DialogueLineLabel.Size = new Size(82, 18);
            DialogueLineLabel.TabIndex = 6;
            DialogueLineLabel.Text = "Text Line #";
            // 
            // SpeakerNameTextBox
            // 
            SpeakerNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            SpeakerNameTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SpeakerNameTextBox.Location = new Point(659, 53);
            SpeakerNameTextBox.Margin = new Padding(2, 1, 2, 1);
            SpeakerNameTextBox.Name = "SpeakerNameTextBox";
            SpeakerNameTextBox.Size = new Size(248, 32);
            SpeakerNameTextBox.TabIndex = 7;
            SpeakerNameTextBox.Text = "";
            SpeakerNameTextBox.TextChanged += SpeakerTextModified;
            // 
            // FormattingDropDown
            // 
            FormattingDropDown.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            FormattingDropDown.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormattingDropDown.FormattingEnabled = true;
            FormattingDropDown.Items.AddRange(new object[] { "none", "small", "normal", "large" });
            FormattingDropDown.Location = new Point(659, 117);
            FormattingDropDown.Margin = new Padding(2, 1, 2, 1);
            FormattingDropDown.Name = "FormattingDropDown";
            FormattingDropDown.Size = new Size(250, 23);
            FormattingDropDown.TabIndex = 8;
            FormattingDropDown.SelectedIndexChanged += FormattingChanged;
            // 
            // SpeakerLabel
            // 
            SpeakerLabel.AutoSize = true;
            SpeakerLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SpeakerLabel.Location = new Point(401, 29);
            SpeakerLabel.Margin = new Padding(2, 0, 2, 0);
            SpeakerLabel.Name = "SpeakerLabel";
            SpeakerLabel.Size = new Size(68, 18);
            SpeakerLabel.TabIndex = 9;
            SpeakerLabel.Text = "Speaker";
            // 
            // FormattingLabel
            // 
            FormattingLabel.AutoSize = true;
            FormattingLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormattingLabel.Location = new Point(401, 93);
            FormattingLabel.Margin = new Padding(2, 0, 2, 0);
            FormattingLabel.Name = "FormattingLabel";
            FormattingLabel.Size = new Size(83, 18);
            FormattingLabel.TabIndex = 10;
            FormattingLabel.Text = "Formatting";
            // 
            // DialogueLabel
            // 
            DialogueLabel.AutoSize = true;
            DialogueLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            DialogueLabel.Location = new Point(401, 155);
            DialogueLabel.Margin = new Padding(2, 0, 2, 0);
            DialogueLabel.Name = "DialogueLabel";
            DialogueLabel.Size = new Size(71, 18);
            DialogueLabel.TabIndex = 11;
            DialogueLabel.Text = "Dialogue";
            // 
            // EditAllCheckbox
            // 
            EditAllCheckbox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            EditAllCheckbox.AutoSize = true;
            EditAllCheckbox.Font = new Font("Arial", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            EditAllCheckbox.Location = new Point(383, 485);
            EditAllCheckbox.Margin = new Padding(2, 1, 2, 1);
            EditAllCheckbox.Name = "EditAllCheckbox";
            EditAllCheckbox.Size = new Size(124, 20);
            EditAllCheckbox.TabIndex = 12;
            EditAllCheckbox.Text = "Edit All Identical";
            EditAllCheckbox.UseVisualStyleBackColor = true;
            EditAllCheckbox.CheckedChanged += EditAllCheckedChanged;
            // 
            // MatchingCountTextBox
            // 
            MatchingCountTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MatchingCountTextBox.Font = new Font("Arial", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MatchingCountTextBox.Location = new Point(383, 508);
            MatchingCountTextBox.Margin = new Padding(2, 1, 2, 1);
            MatchingCountTextBox.Name = "MatchingCountTextBox";
            MatchingCountTextBox.ReadOnly = true;
            MatchingCountTextBox.Size = new Size(524, 23);
            MatchingCountTextBox.TabIndex = 13;
            // 
            // LineCountTextBox
            // 
            LineCountTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            LineCountTextBox.Font = new Font("Arial", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LineCountTextBox.Location = new Point(659, 453);
            LineCountTextBox.Margin = new Padding(2, 1, 2, 1);
            LineCountTextBox.Name = "LineCountTextBox";
            LineCountTextBox.ReadOnly = true;
            LineCountTextBox.Size = new Size(248, 23);
            LineCountTextBox.TabIndex = 14;
            // 
            // LineCountLabel
            // 
            LineCountLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LineCountLabel.AutoSize = true;
            LineCountLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LineCountLabel.Location = new Point(401, 426);
            LineCountLabel.Margin = new Padding(2, 0, 2, 0);
            LineCountLabel.Name = "LineCountLabel";
            LineCountLabel.Size = new Size(83, 18);
            LineCountLabel.TabIndex = 15;
            LineCountLabel.Text = "Line Count";
            // 
            // MaxLineLengthLabel
            // 
            MaxLineLengthLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MaxLineLengthLabel.AutoSize = true;
            MaxLineLengthLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaxLineLengthLabel.ForeColor = Color.Firebrick;
            MaxLineLengthLabel.Location = new Point(378, 389);
            MaxLineLengthLabel.Margin = new Padding(2, 0, 2, 0);
            MaxLineLengthLabel.Name = "MaxLineLengthLabel";
            MaxLineLengthLabel.Size = new Size(0, 18);
            MaxLineLengthLabel.TabIndex = 16;
            // 
            // OriginalDialogueTextBox
            // 
            OriginalDialogueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            OriginalDialogueTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OriginalDialogueTextBox.Location = new Point(393, 178);
            OriginalDialogueTextBox.Margin = new Padding(2, 1, 2, 1);
            OriginalDialogueTextBox.Name = "OriginalDialogueTextBox";
            OriginalDialogueTextBox.ReadOnly = true;
            OriginalDialogueTextBox.Size = new Size(250, 206);
            OriginalDialogueTextBox.TabIndex = 17;
            OriginalDialogueTextBox.Text = "";
            // 
            // OriginalSpeakerNameTextBox
            // 
            OriginalSpeakerNameTextBox.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OriginalSpeakerNameTextBox.Location = new Point(393, 53);
            OriginalSpeakerNameTextBox.Margin = new Padding(2, 1, 2, 1);
            OriginalSpeakerNameTextBox.Name = "OriginalSpeakerNameTextBox";
            OriginalSpeakerNameTextBox.ReadOnly = true;
            OriginalSpeakerNameTextBox.Size = new Size(248, 32);
            OriginalSpeakerNameTextBox.TabIndex = 18;
            OriginalSpeakerNameTextBox.Text = "";
            // 
            // OriginalFormattingDropDown
            // 
            OriginalFormattingDropDown.Font = new Font("Arial", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OriginalFormattingDropDown.FormattingEnabled = true;
            OriginalFormattingDropDown.Items.AddRange(new object[] { "none", "small", "normal", "large" });
            OriginalFormattingDropDown.Location = new Point(393, 117);
            OriginalFormattingDropDown.Margin = new Padding(2, 1, 2, 1);
            OriginalFormattingDropDown.Name = "OriginalFormattingDropDown";
            OriginalFormattingDropDown.Size = new Size(250, 23);
            OriginalFormattingDropDown.TabIndex = 19;
            // 
            // OriginalLineCountTextBox
            // 
            OriginalLineCountTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            OriginalLineCountTextBox.Font = new Font("Arial", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OriginalLineCountTextBox.Location = new Point(393, 453);
            OriginalLineCountTextBox.Margin = new Padding(2, 1, 2, 1);
            OriginalLineCountTextBox.Name = "OriginalLineCountTextBox";
            OriginalLineCountTextBox.ReadOnly = true;
            OriginalLineCountTextBox.Size = new Size(248, 23);
            OriginalLineCountTextBox.TabIndex = 20;
            // 
            // OriginalLabel
            // 
            OriginalLabel.AutoSize = true;
            OriginalLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OriginalLabel.Location = new Point(558, 29);
            OriginalLabel.Margin = new Padding(2, 0, 2, 0);
            OriginalLabel.Name = "OriginalLabel";
            OriginalLabel.Size = new Size(72, 18);
            OriginalLabel.TabIndex = 21;
            OriginalLabel.Text = "(Original)";
            // 
            // NewLabel
            // 
            NewLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            NewLabel.AutoSize = true;
            NewLabel.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NewLabel.Location = new Point(850, 29);
            NewLabel.Margin = new Padding(2, 0, 2, 0);
            NewLabel.Name = "NewLabel";
            NewLabel.Size = new Size(49, 18);
            NewLabel.TabIndex = 22;
            NewLabel.Text = "(New)";
            // 
            // TranslationWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(922, 542);
            Controls.Add(NewLabel);
            Controls.Add(OriginalLabel);
            Controls.Add(OriginalLineCountTextBox);
            Controls.Add(OriginalFormattingDropDown);
            Controls.Add(OriginalSpeakerNameTextBox);
            Controls.Add(OriginalDialogueTextBox);
            Controls.Add(MaxLineLengthLabel);
            Controls.Add(LineCountLabel);
            Controls.Add(LineCountTextBox);
            Controls.Add(MatchingCountTextBox);
            Controls.Add(EditAllCheckbox);
            Controls.Add(DialogueLabel);
            Controls.Add(FormattingLabel);
            Controls.Add(SpeakerLabel);
            Controls.Add(FormattingDropDown);
            Controls.Add(SpeakerNameTextBox);
            Controls.Add(DialogueLineLabel);
            Controls.Add(DialogueTextBox);
            Controls.Add(DialogueList);
            Controls.Add(SceneLabel);
            Controls.Add(SceneList);
            Controls.Add(MenuStrip);
            AllowDrop = true;
            DragDrop += DragDropCallback;
            DragEnter += DragEnterCallback;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2, 1, 2, 1);
            Name = "TranslationWindow";
            Text = DefaultWindowTitle;
            Load += TranslationWindowLoad;
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.ListBox SceneList;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.Label SceneLabel;
        private System.Windows.Forms.ListBox DialogueList;
        private System.Windows.Forms.RichTextBox DialogueTextBox;
        private System.Windows.Forms.Label DialogueLineLabel;
        private System.Windows.Forms.RichTextBox SpeakerNameTextBox;
        private System.Windows.Forms.ComboBox FormattingDropDown;
        private System.Windows.Forms.Label SpeakerLabel;
        private System.Windows.Forms.Label FormattingLabel;
        private System.Windows.Forms.Label DialogueLabel;
        private System.Windows.Forms.ToolStripMenuItem CloseToolStripMenuItem;
        private System.Windows.Forms.CheckBox EditAllCheckbox;
        private System.Windows.Forms.TextBox MatchingCountTextBox;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.TextBox LineCountTextBox;
        private System.Windows.Forms.Label LineCountLabel;
        private System.Windows.Forms.Label MaxLineLengthLabel;
        private System.Windows.Forms.ToolStripMenuItem ToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAllScenesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.RichTextBox OriginalDialogueTextBox;
        private System.Windows.Forms.RichTextBox OriginalSpeakerNameTextBox;
        private System.Windows.Forms.ComboBox OriginalFormattingDropDown;
        private System.Windows.Forms.TextBox OriginalLineCountTextBox;
        private System.Windows.Forms.Label OriginalLabel;
        private System.Windows.Forms.Label NewLabel;
        private System.Windows.Forms.ToolStripMenuItem CenterTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TranslateAllSpeakersToolStripMenuItem;
    }
}

