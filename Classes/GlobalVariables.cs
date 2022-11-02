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
        /// Name of the chain from Setup file.
        /// </summary>
        public static string ChainName { get; set; } = "";

        /// <summary>
        /// Chain object of the chain (has all vital streams and keys stored inside).
        /// </summary>
        public static Chain? MainChain { get; set; } = null;

        /// <summary>
        /// Account object with credentials inside.
        /// </summary>
        public static Account? UserAccount { get; set; } = null;
    }
}
