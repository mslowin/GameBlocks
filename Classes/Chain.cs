using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    internal class Chain
    {
        public string ChainName { get; set; }
        public List<Stream> QueueStreams { get; set; }
        public List<Stream> GameStreams { get; set; }

        public Chain(string chainName, List<Stream> queueStreams, List<Stream> gameStreams)
        {
            ChainName = chainName;
            QueueStreams = queueStreams;
            GameStreams = gameStreams;
        }
    }
}
