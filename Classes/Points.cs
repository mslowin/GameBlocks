using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing users Points.
    /// </summary>
    internal class Points
    {
        /// <summary>
        /// Reads the number of users points from the chain.
        /// </summary>
        public static string ReadPointsFromStream()
        {
            string output, err;
            (output, err) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, 
                $"liststreamkeyitems {GlobalVariables.MainChain!.PointsStream.StreamName} {GlobalVariables.UserAccount!.Login}");

            List<string> usersPointsHistory = ExtensionsMethods.SearchInJson(output, "points");

            // reversing the list to search newest information first
            usersPointsHistory.Reverse();

            return usersPointsHistory.First();
        }

        /// <summary>
        /// Adds points to the chain.
        /// </summary>
        /// <param name="pointsToBeAdded">Points that will be added to the total amount.</param>
        public static void AddPoints(string pointsToBeAdded)
        {
            int NewPoints = int.Parse(GlobalVariables.NumberOfPoints) + int.Parse(pointsToBeAdded);
            MultiChainClient.PublishToStream(GlobalVariables.MainChain!.PointsStream.StreamName, GlobalVariables.UserAccount!.Login, 
                $"{{\"\"\"json\"\"\":{{\"\"\"points\"\"\":\"\"\"{NewPoints.ToString()}\"\"\"}}}}");
        }
    }
}
