using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing a single stream in a chain.
    /// </summary>
    internal class Stream
    {
        /// <summary>
        /// Name of a stream.
        /// </summary>
        public string StreamName { get; set; }
        
        /// <summary>
        /// List of keys used in a stream.
        /// </summary>
        public List<Key> Keys { get; set; }
    }
}
