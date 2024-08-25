namespace PopoloCroisTranslationTool
{
    /// <summary>
    /// A collection of dialogue lines from a single in-game data object representing either a single cutscene or a location filled with NPC's
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// The list of dialogue lines inside of this scene
        /// </summary>
        public List<DialogueLine> DialogueLines { get; set; }

        /// <summary>
        /// Whether the user has edited a dialogue line in this scene
        /// </summary>
        public bool IsEdited { get; set; }

        /// <summary>
        /// Whether this scene is from PopoRogue or PopoloCoris Monogatari II
        /// </summary>
        public GameType LoadedGameType { get; set; }

        /// <summary>
        /// The file where this scene data came from
        /// </summary>
        public string ParentFile { get; set; }

        /// <summary>
        /// The memory location of the beginning of the dialogue pointer list
        /// </summary>
        public long PointerListLocation { get; set; }

        /// <summary>
        /// The memory location of the beginning of the scene data
        /// </summary>
        public long SceneDataLocation { get; set; }

        /// <summary>
        /// Title of the scene taken from bytes 16-48 of the scene. More reliable in Monogatari II than PopoRogue.
        /// </summary>
        public string SceneTitle { get; set; }

        /// <summary>
        /// Whether the scene has any actual dialogue in it
        /// </summary>
        /// <returns>Whether the scene is empty</returns>
        public bool IsEmptyScene()
        {
            return DialogueLines.Count == 0;
        }

        /// <summary>
        /// Sets the IsEdited flag to false for self and owned dialogue lines
        /// </summary>
        public void Save()
        {
            IsEdited = false;

            foreach (DialogueLine line in DialogueLines)
            {
                line.Save();
            }
        }
    }
}