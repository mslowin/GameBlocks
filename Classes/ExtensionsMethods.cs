using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static Setup? ReadSetupFile()
        {
            try
            {
                // Open the text file using a stream reader.
                using (var sr = new StreamReader("../../../Setup.json"))    // <-- być może trzeba to będzie zmienić kiedyś jeśli będzie plik exe
                {
                    string json = sr.ReadToEnd();
                    Setup? setup = JsonConvert.DeserializeObject<Setup>(json);
                    
                    return setup;
                }
            }
            catch (IOException e)
            {
                CustomMessageBox.Show($"Setup.txt is missing. It should contain informaitions about the chain\n{e.Message}",
                    "File error", MessageBoxButton.OK, MessageBoxImage.Error);

                ExitApplication();

                return null;
            }
        }

        public static string ReadJsonFile(string pathToFile)
        {
            try
            {
                using (var sr = new StreamReader(pathToFile))
                {
                    string json = sr.ReadToEnd();
                    JsonTextReader reader = new JsonTextReader(new StringReader(json));
                    while (reader.Read())
                    {
                        if (reader.Value != null)
                        {
                            Trace.WriteLine($"Token: {reader.TokenType}, Value: {reader.Value}");
                        }
                        else
                        {
                            Trace.WriteLine($"Token: {reader.TokenType}");
                        }
                    }

                    return json;
                }
            }
            catch (IOException e)
            {
                CustomMessageBox.Show($"Json file is missing.\n{e.Message}",
                    "File error", MessageBoxButton.OK, MessageBoxImage.Error);

                return "";
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
