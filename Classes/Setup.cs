using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Setup class to store information from Setup.json file.
    /// </summary>
    public class Setup
    {
        /// <summary>
        /// Path to a folder where multichain.exe is located.
        /// </summary>
        public string PathToMultichainFolder { get; set; } = ""; 
        
        /// <summary>
        /// Name of a chain provided in Setup file.
        /// </summary>
        public string ChainName { get; set; } = "";
    }
}
