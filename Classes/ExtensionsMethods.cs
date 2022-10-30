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
        /// <summary>
        /// Reads Setup.json file and Creates Setup object.
        /// </summary>
        /// <returns>Setup object.</returns>
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

        /// <summary>
        /// Reads json files.
        /// </summary>
        /// <param name="pathToFile">path to a jason file.</param>
        /// <returns>Contents of a file in a single string.</returns>
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

        /// <summary>
        /// Performs a search in a json file. Finds attributes and corrseponding values.
        /// </summary>
        /// <param name="jsonText">Text to perform a search on.</param>
        /// <param name="attributeToSearchFor">Attribute to search for.</param>
        /// <returns>List of corresponding values.</returns>
        public static List<string> SearchInJson(string jsonText, string attributeToSearchFor)
        {
            bool desiredTypeFlag = false;
            List<string> correspondingValues = new List<string>();
            JsonTextReader reader = new JsonTextReader(new StringReader(jsonText));
            while (reader.Read())
            {
                if (desiredTypeFlag)
                {
                    correspondingValues.Add(reader.Value.ToString());
                    desiredTypeFlag = false;
                }
                if (reader.Value != null && reader.Value.ToString() == attributeToSearchFor)
                {
                    desiredTypeFlag = true;
                }
            }

            return correspondingValues;
        }

        /// <summary>
        /// Stops Node and exits application.
        /// </summary>
        private static void ExitApplication()
        {
            // Stopping a Node
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
            System.Windows.Application.Current.Shutdown();
        }
    }
}
