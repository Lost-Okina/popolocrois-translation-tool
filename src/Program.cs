using System.Text;

namespace PopoloCroisTranslationTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // SHIFT-JIS encoding is not included by default in dotnet core
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ApplicationConfiguration.Initialize();
            Application.Run(new TranslationWindow());
        }
    }
}