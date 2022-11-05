using GameBlocks.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GameBlocks.Views
{
    /// <summary>
    /// Logika interakcji dla klasy LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        /// <summary>
        /// Background task which doesn't stop main thread.
        /// </summary>
        private Task BackgroundTask { get; set; }

        /// <summary>
        /// Token usd to stop background task from another task.
        /// </summary>
        private CancellationTokenSource ts = new();

        /// <summary>
        /// Name of a stream in which the waitingroom is located.
        /// </summary>
        private string StreamName { get; set; }

        /// <summary>
        /// Name of the waiting room in which the player is located.
        /// </summary>
        private string WaitingRoomName { get; set; }

        /// <summary>
        /// Intiger defying the purpose of loading window (0 - waiting for player, 1 - joining player).
        /// </summary>
        private int PurposeIndex { get; set; }


        /// <summary>
        /// Constructor of LoadingWindow class.
        /// </summary>
        /// <param name="purposeIndex">Intiger defying the purpose of loading window (0 - waiting for player, 1 - joining player).</param>
        /// <param name="informations">Text displayed d loading window.</param>
        /// <param name="streamName">Name of a stream in which the waitingroom is located.</param>
        /// <param name="waitingRoomName">Name of the waiting room in which the player is located.</param>
        public LoadingWindow(int purposeIndex, string informations, string streamName, string waitingRoomName)
        {
            InitializeComponent();
            WaitingRoomName = waitingRoomName;
            StreamName = streamName;
            PurposeIndex = purposeIndex;
            LoadingInformaitonTextBlock.Text = informations;

            CancellationToken ct = ts.Token;

            switch (purposeIndex)
            {
                case 0:
                    BackgroundTask = Task.Factory.StartNew(() => WaitForPlayer(streamName, waitingRoomName, ct), ct).ContinueWith((tsk) =>
                    {
                        Close();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case 1:
                    BackgroundTask = Task.Factory.StartNew(() => MatchmakingStartingAsync(5000, streamName, waitingRoomName, ct), ct).ContinueWith((tsk) =>
                    {
                        Close();
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
            }
        }

        /// <summary>
        /// Checks in loop if a new player joined waiing room.
        /// </summary>
        /// <param name="streamName">Name of a stream where waiting room is located.</param>
        /// <param name="waitingRoomName">Name of a waiting room in which player is located.</param>
        /// <param name="cancellationToken">Token that can be used by other threads to cancel the loop.</param>
        public static void WaitForPlayer(string streamName, string waitingRoomName, CancellationToken cancellationToken)
        {
            while (true)
            {
                (var output, _) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeys QueueTicTacToe {waitingRoomName}");
                List<string> players = ExtensionsMethods.SearchInJson(output, "items");

                // When someone joined
                if (int.Parse(players[0]) == 2)
                {
                    // Start Gry (utworzenie klucza z nazwami graczy)
                    GlobalVariables.IsMatchmakingComplete = true;
                    break;
                }

                // When window clossed or cancel button pressed
                if (cancellationToken.IsCancellationRequested)
                {
                    GlobalVariables.IsMatchmakingComplete = false;
                    // Adding Item status:cancelled
                    MultiChainClient.PublishToStream(streamName, waitingRoomName, $"{{\"\"\"json\"\"\":{{\"\"\"status\"\"\":\"\"\"cancelled\"\"\"}}}}");
                    break;
                }
            }
        }

        /// <summary>
        /// Gives player some time to cancel matchmaking. After this time starts game.
        /// </summary>
        /// <param name="milisecondsToStart">Time in miliseconds when player can cancel matchmaking.</param>
        /// <param name="streamName">Name of a stream where waiting room is located.</param>
        /// <param name="waitingRoomName">Name of a waiting room in which player is located.</param>
        /// <param name="cancellationToken">Token that can be used by other threads to cancel the loop.</param>
        /// <returns></returns>
        public static async Task MatchmakingStartingAsync(int milisecondsToStart, string streamName, string waitingRoomName, CancellationToken cancellationToken)
        {
            int loops = 0;
            while (true)
            {
                Thread.Sleep(1000);
                loops++;

                if (loops == ((milisecondsToStart - 1)/1000))
                {
                    // Dołączenie do waitingRoomu i Start gry po upłynięciu kilku sekund
                    MultiChainClient.PublishToStream(streamName, waitingRoomName, $"{{\"\"\"json\"\"\":{{\"\"\"login\"\"\":\"\"\"{GlobalVariables.UserAccount!.Login}\"\"\"}}}}");
                    GlobalVariables.IsMatchmakingComplete = true;
                    await Task.Delay(1000);
                    break;
                }

                // When window clossed or cancel button pressed
                if (cancellationToken.IsCancellationRequested)
                {
                    // Nothing should happen. Matchmaking cancelled
                    Console.WriteLine("task canceled");
                    GlobalVariables.IsMatchmakingComplete = false;
                    await Task.Delay(1000);
                    break;
                }
            }
        }

        /// <summary>
        /// Cancels loading. Works like window close button
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ts.Cancel();
            Close();  // closes window and exeutes Window_Closed method
        }

        /// <summary>
        /// Handling an instance where the window was closed.
        /// </summary>
        /// <param name="sender">Reference to the sender e.g. button.</param>
        /// <param name="e">Additional information object and event handler.</param>
        private void Window_Closed(object sender, EventArgs e)
        {
            ts.Cancel();
        }
    }
}
