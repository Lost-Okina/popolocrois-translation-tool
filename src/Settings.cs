namespace PopoloCroisTranslationTool
{
    /// <summary>
    /// Various settings that can be loaded in to modify some actions the tool takes
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// The character that denotes the start of an emoticon for PopoloCrois Monogatari II only
        /// </summary>
        public char EmoticonCharacter { get; set; } = '&';

        /// <summary>
        /// Maximum number of characters allowed to be read by the tool when loading
        /// </summary>
        public int MaxReadableDialogueLength { get; set; } = 48 * 33;

        /// <summary>
        /// Maximum number of lines allowed to be read by the tool when loading
        /// </summary>
        public byte MaxReadableLines { get; set; } = 0x30;

        /// <summary>
        /// The character that denotes the beginning of an in-line hex code
        /// </summary>
        public char RawHexadecimalCharacter { get; set; } = '*';

        /// <summary>
        /// Maximum range for parsing out scene titles from binary file
        /// </summary>
        public int SceneTitleExpectedLength { get; set; } = 64;
    }
}