using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using WPFCustomMessageBox;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Methods which extend the main Classes and give additional functionalities.
    /// </summary>
    internal static class ExtensionsMethods
    {
        //public static (string, List<string>, List<string>) ReadSetupFile()
        public static string ReadSetupFile()
        {
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader("../../../Setup.txt"))    // <-- być może trzeba to będzie zmienić kiedyś jeśli będzie plik exe
                {
                    var output = sr.ReadToEnd();
                    string[] separatingStrings = { "Chain name: " };
                    string[] words = output.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                    return words[0];

                    //string[] separatingStrings = { "Chain name: ", "Chain queues: ", "Chain games: ", ", " };

                    //string[] words = output.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                    //if (!words.Any())
                    //{
                    //    MessageBox.Show($"Setup.txt is empty.", "File error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                    //else
                    //{
                    //    string patternQueue = "Queue";
                    //    string patternGame = "Game";
                    //    Regex rgQueue = new Regex(patternQueue);
                    //    Regex rgGame = new Regex(patternGame);
                    //    foreach (var word in rgQueue.Matches(words.ToString()))
                    //    {
                    //    }
                    //    return (null, null, null);
                    //}
                }
            }
            catch (IOException e)
            {
                CustomMessageBox.Show($"Setup.txt is missing. It should contain informaitions about the chain\n{e.Message}",
                    "File error", MessageBoxButton.OK, MessageBoxImage.Error);

                ExitApplication();

                return "";
                //return (null, null, null);
            }
        }

        private static void ExitApplication()
        {
            // Stopping a Node
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
            System.Windows.Application.Current.Shutdown();
        }
    }
}
