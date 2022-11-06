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
    internal class Setup
    {
        /// <summary>
        /// Name of a chain provided in Setup file.
        /// </summary>
        public string ChainName { get; set; } = "";
    }
}
