using GameBlocks.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
        /// Creates or joins a waiting room inside a chain.
        /// </summary>
        /// <param name="gameName">Selected game name.</param>
        /// <returns>True if Game should start, false if matchmaking cancelled.</returns>
        internal static bool CreateOrJoinWaitingRoom(string gameName)
        {
            string streamName = $"Queue{gameName}";
            var keys = MultiChainClient.ReadStreamKeys(streamName);

            int lastRoomNumber = int.Parse(keys.Last().KeyName.Remove(0, 11));  // removes "waitingRoom" part from the name, leaving only number
            int lastRoomItems = keys.Last().KeyItems;

            if (lastRoomItems >= 2) // Creating waiting room
            {
                lastRoomNumber++;
                string newWaitingRoomName = $"waitingRoom{lastRoomNumber}";
                // TODO: Może to zmienić, żeby nie był od razu tworony waiting room, tylko np po 3 sekundach jeśli racz nie anuluje
                MultiChainClient.PublishToStream(streamName, newWaitingRoomName, $"{{\"\"\"json\"\"\":{{\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"}}}}");

                LoadingWindow loadingWindow = new LoadingWindow(0, "Waiting for another player...", streamName, newWaitingRoomName);
                loadingWindow.ShowDialog();

                if (!GlobalVariables.IsMatchmakingComplete)  // when matchmaking was cancelled
                {
                    Trace.WriteLine("Game cancelled");
                    return false;
                }
                if (GlobalVariables.IsMatchmakingComplete)
                {
                    Trace.WriteLine("Game starts");
                    GlobalVariables.IsMatchmakingComplete = false;

                    string gameKey = MultiChainClient.CreateNewGameKey(gameName, newWaitingRoomName);
                    // TODO: Start of a game with gameKey 
                    return true;
                }
            }
            else if (lastRoomItems == 1) // Joining waitnig room
            {
                string newWaitingRoomName = $"waitingRoom{lastRoomNumber}"; // New waiting room is actually the last waiting room found in keys

                LoadingWindow loadingWindow = new LoadingWindow(1, "Matchmaking...", streamName, newWaitingRoomName);
                loadingWindow.ShowDialog();

                if (!GlobalVariables.IsMatchmakingComplete)  // when matchmaking was cancelled
                {
                    Trace.WriteLine("Game cancelled");
                    return false;
                }
                if (GlobalVariables.IsMatchmakingComplete)
                {
                    Trace.WriteLine("Game starts");
                    GlobalVariables.IsMatchmakingComplete = false;

                    string gameKey = MultiChainClient.CreateNewGameKey(gameName, newWaitingRoomName);
                    // TODO: Start of a game with gameKey 
                    return true;
                }
            }
            Trace.WriteLine("Something went wrong\nExtensionMethods.CreateOrJoinWaitingRoom()");
            return false;
        }

        /// <summary>
        /// Chooses game title from available game titles List, using index.
        /// </summary>
        /// <param name="selectedGameIndex">Index defining which title to choose.</param>
        /// <param name="availableGames">List of available game titles</param>
        /// <returns>Game title.</returns>
        public static (int, string) ChooseGameTitle(int selectedGameIndex, List<string> availableGames)
        {
            if (selectedGameIndex >= availableGames.Count)  // going up, right arrow
            {
                selectedGameIndex = 0;
            }
            else if (selectedGameIndex < 0)  // going down, left arrow
            {
                selectedGameIndex = availableGames.Count - 1;
            }

            return (selectedGameIndex, $"{availableGames[selectedGameIndex]}");
        }

        /// <summary>
        /// Gets full path to an image.
        /// </summary>
        /// <param name="resourcePath">Path to an image from project perspective e.g. /Views/image.jpg.</param>
        /// <returns>Uri identifiator of an image.</returns>
        public static Uri GetFullImageSource(string resourcePath)
        {
            var uri = string.Format($"pack://application:,,,/{Assembly.GetExecutingAssembly().GetName().Name};component/{resourcePath}");

            return new Uri(uri);
        }

        /// <summary>
        /// Stops Node and exits application.
        /// </summary>
        public static void ExitApplication()
        {
            // Stopping a Node
            MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, "stop");
            System.Windows.Application.Current.Shutdown();
        }
    }
}
