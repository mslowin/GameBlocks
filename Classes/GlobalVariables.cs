using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class storing globally accessible data.
    /// </summary>
    internal static class GlobalVariables
    {
        /// <summary>
        /// Path to a folder where multichain.exe is located.
        /// </summary>
        public static string PathToMultichainFolder { get; set; } = "";

        /// <summary>
        /// Name of the chain from Setup file.
        /// </summary>
        public static string ChainName { get; set; } = "";

        /// <summary>
        /// Address of a node (from getaddresses command).
        /// </summary>
        public static string NodeAddress { get; set; } = "";

        /// <summary>
        /// Chain object of the game chain (has all vital streams and keys stored inside).
        /// </summary>
        public static Chain? MainChain { get; set; } = null;

        /// <summary>
        /// Account object with credentials inside.
        /// </summary>
        public static Account? UserAccount { get; set; } = null;

        /// <summary>
        /// Intiger containing the total number of users points taken from the chain.
        /// </summary>
        public static string NumberOfPoints { get; set; } = "";

        /// <summary>
        /// Indicates whetcher a waitingroom was succesfully filled or the matchmaking was cancelled.
        /// </summary>
        public static bool IsMatchmakingComplete { get; set; } = false;
    }
}
