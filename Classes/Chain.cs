using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing a chain.
    /// </summary>
    internal class Chain
    {
        /// <summary>
        /// The name of a chain.
        /// </summary>
        public string ChainName { get; set; }
        
        /// <summary>
        /// List of streams used as queues (names of which always start with "Queue")
        /// </summary>
        public List<Stream> QueueStreams { get; set; }

        /// <summary>
        /// List of streams used as games (names of which always start with "Game")
        /// </summary>
        public List<Stream> GameStreams { get; set; }

        /// <summary>
        /// Constructor of chain object.
        /// </summary>
        /// <param name="chainName">Name of a chain.</param>
        /// <param name="queueStreams">List of streams used as queues.</param>
        /// <param name="gameStreams">List of streams used as games.</param>
        public Chain(string chainName, List<Stream> queueStreams, List<Stream> gameStreams)
        {
            ChainName = chainName;
            QueueStreams = queueStreams;
            GameStreams = gameStreams;
        }
    }
}
