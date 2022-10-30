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
        /// Root stream to comunicate with the seed node/admin.
        /// </summary>
        public Stream RootStream { get; set; }

        /// <summary>
        /// Stream containing users ogins and hashed passwords.
        /// </summary>
        public Stream UsersStream { get; set; }

        /// <summary>
        /// List of streams used as queues (names of which always start with "Queue").
        /// </summary>
        public List<Stream> QueueStreams { get; set; }

        /// <summary>
        /// List of streams used as games (names of which always start with "Game").
        /// </summary>
        public List<Stream> GameStreams { get; set; }

        /// <summary>
        /// Constructor of chain object.
        /// </summary>
        /// <param name="chainName">Name of a chain.</param>
        /// <param name="rootStream">Root stream to comunicate with the seed node/admin.</param>
        /// <param name="usersStream">Stream containing users ogins and hashed passwords.</param>
        /// <param name="queueStreams">List of streams used as queues.</param>
        /// <param name="gameStreams">List of streams used as games.</param>
        public Chain(string chainName, Stream rootStream, Stream usersStream, List<Stream> queueStreams, List<Stream> gameStreams)
        {
            ChainName = chainName;
            RootStream = rootStream;
            UsersStream = usersStream;
            QueueStreams = queueStreams;
            GameStreams = gameStreams;
        }

        /// <summary>
        /// Performs encryption and checks if such login and password hash are inside he chain.
        /// </summary>
        /// <param name="login">login from a text box.</param>
        /// <param name="password">password from a text box.</param>
        /// <returns>true if successfuly logged in, else false.</returns>
        public bool LogIntoChainSuccess(string myLogin, string myPassword)
        {
            string output, err;
            (output, err) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamitems {UsersStream.StreamName} false 10000");
            List<string> chainLogins = ExtensionsMethods.SearchInJson(output, "login");
            List<string> chainPasswordHashes = ExtensionsMethods.SearchInJson(output, "password");

            // reversing lists to search newest information first
            chainLogins.Reverse();
            chainPasswordHashes.Reverse();

            var myPasswordHash = EasyEncryption.SHA.ComputeSHA256Hash(myPassword);  // using sha256 encryption algorithm ho hash my password.

            foreach (var (passwordHash, i) in chainPasswordHashes.Select((passwordHash, i) => (passwordHash, i)))
            {
                if(passwordHash == myPasswordHash && chainLogins[i] == myLogin)
                {
                    // TODO: jeszcze sprawdz nazwy
                    return true;
                }
            }

            return false;
        }
    }
}
