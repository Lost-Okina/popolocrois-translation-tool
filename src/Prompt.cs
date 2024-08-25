namespace PopoloCroisTranslationTool
{
    /// <summary>
    /// A common prompt used for simple user querying
    /// </summary>
    public class Prompt
    {
        public static bool ShowDialog(string text, string caption)
        {
            using (Form prompt = new Form()
            {
                Width = 275,
                Height = 100,
                FormBorderStyle = FormBorderStyle.Sizable,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                SizeGripStyle = SizeGripStyle.Auto,
                AutoSizeMode = AutoSizeMode.GrowOnly,
                AutoSize = false,
            })
            {
                Label textLabel = new Label() { Left = 30, Top = 10, Text = text, Width = 600 };
                Button confirmation = new Button() { Text = "Ok", Left = 75, Width = 50, Top = 30, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 135, Width = 50, Top = 30, DialogResult = DialogResult.Cancel };

                confirmation.Click += (sender, e) =>
                {
                    prompt.Close();
                };

                prompt.Controls.Add(cancel);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? true : false;
            }
        }
    }

    /// <summary>
    /// A common prompt with a text entry box for user searches
    /// </summary>
    internal class TextPrompt : Form
    {
        public TextPrompt()
        {
            Width = 200;
            Height = 125;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Text = "Search";
            StartPosition = FormStartPosition.CenterParent;

            Label textLabel = new Label() { Left = 25, Top = 10, Text = "String:", Width = 200 };
            Button confirmation = new Button() { Text = "Search", Left = 130, Width = 50, Top = 50, DialogResult = DialogResult.OK };
            TextBox textInput = new TextBox() { Text = Memory, Left = 25, Width = 150, Top = 30 };
            Label textLabel2 = new Label() { Left = 25, Top = 50, Text = "Continued:", Width = 70 };
            CheckBox checkBox = new CheckBox() { CheckState = CheckboxMemory, Left = 100, Top = 50 };

            confirmation.Click += (sender, e) =>
            {
                ReturnValue = textInput.Text;
                FromLocation = checkBox.Checked;
                Memory = textInput.Text;
                CheckboxMemory = checkBox.CheckState;
                Close();
            };

            Controls.Add(textInput);
            Controls.Add(confirmation);
            Controls.Add(textLabel);
            Controls.Add(textLabel2);
            Controls.Add(checkBox);
            AcceptButton = confirmation;
        }

        public static CheckState CheckboxMemory { get; set; }
        public static string Memory { get; set; }
        public bool FromLocation { get; set; }
        public string ReturnValue { get; set; }
    }
}