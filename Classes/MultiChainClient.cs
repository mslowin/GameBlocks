using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class containing a collection of methods used to perform operations on the Chain.
    /// </summary>
    internal static class MultiChainClient
    {
        /// <summary>
        /// Gathers information about the chain and its component. Creates classes.
        /// </summary>
        /// <param name="chainName">Name of a chain.</param>
        /// <returns>Chain object.</returns>
        public static Chain InitializeChain(string chainName)
        {
            MultiChainClient.RunCommand("multichaind", chainName, "-daemon");
            System.Threading.Thread.Sleep(3000);                                // <-- sleep to make sure the node is running

            List<Stream> allStreams = MultiChainClient.ReadChainStreams();
            Chain chain = CreateChainObjet(allStreams);

            return chain;
        }

        /// <summary>
        /// Creates Chain object from gathered Streams.
        /// </summary>
        /// <param name="allStreams">List of all Streams in a chain.</param>
        /// <returns>Chain object.</returns>
        private static Chain CreateChainObjet(List<Stream> allStreams)
        {
            Stream rootStream = allStreams.Single(x => x.StreamName == "root");
            Stream usersStream = allStreams.Single(x => x.StreamName == "UsersStream");
            List<Stream> queueStreams = allStreams.Where(x => x.StreamName.StartsWith("Queue")).ToList();
            List<Stream> gameStreams = allStreams.Where(x => x.StreamName.StartsWith("Game")).ToList();

            return new Chain(GlobalVariables.ChainName, rootStream, usersStream, queueStreams, gameStreams);
        }

        /// <summary>
        /// Gathers information about streams in a chain and creates Stream classes.
        /// </summary>
        /// <returns>List of all streams in a chain.</returns>
        public static List<Stream> ReadChainStreams()
        {
            string output, err;
            List<Stream> allStreams = new List<Stream>();
            (output, err) = RunCommand("multichain-cli", GlobalVariables.ChainName, "liststreams");

            List<string> streamNames = ExtensionsMethods.SearchInJson(output, "name");

            foreach (string streamName in streamNames)
            {
                List<Key> keys = ReadStreamKeys(streamName);
                allStreams.Add(new Stream { StreamName = streamName, Keys = keys });
            }

            return allStreams;
        }

        /// <summary>
        /// Reads all keys in a single Stream and creates Key objects.
        /// </summary>
        /// <param name="streamName">Name of a stream to check for keys.</param>
        /// <returns>List of all Keys objects in a Stream.</returns>
        public static List<Key> ReadStreamKeys(string streamName)
        {
            List <Key> keys = new List<Key>();
            string output, err;
            (output, err) = MultiChainClient.RunCommand("multichain-cli", GlobalVariables.ChainName, $"liststreamkeys {streamName}");

            List<string> streamKeysNames = ExtensionsMethods.SearchInJson(output, "key");
            List<string> streamKeysItems = ExtensionsMethods.SearchInJson(output, "items");

            int i = 0;
            streamKeysNames.ForEach(x => keys.Add(new Key { KeyName = x, KeyItems = int.Parse(streamKeysItems[i++]) }));

            return keys;

        }

        internal static void PublishToStream(string streamName, string keyName, string jsonItems)
        {
            RunCommand("multichain-cli", GlobalVariables.ChainName, $"publish {streamName} {keyName} {jsonItems}");
        }

        /// <summary>
        /// Runns a multichain command using CMD (warning: multichain must be already running).
        /// </summary>
        /// <param name="prefix">E.g. multichain-cli or multichaind.</param>
        /// <param name="chainName">Name of the chain.</param>
        /// <param name="command">instructions after chain name (e.g. liststreams or liststreamkeys streamName).</param>
        /// <returns>Output informations (like in CMD) or error infrmations if something goes wrong.</returns>
        public static (string, string) RunCommand(string prefix, string chainName, string command)
        {
            Process process = new Process();
            process.StartInfo.WorkingDirectory = @"d:\Multichain\";              // TODO: to powinno być sczytywane z pliku Setup
            process.StartInfo.FileName = $@"d:\Multichain\{prefix}.exe";
            process.StartInfo.Arguments = $"{chainName} {command}";
            //string fullCommand = $"{prefix} {chainName} {command}";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            if (prefix == "multichaind" && command == "-daemon")  // when the user wants to start the node
            {
                _ = StartNodeAsync(process);
                return ("", "");
            }
            process.Start();
            // Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return (output, err);
        }

        /// <summary>
        /// Asynchronicly starts a Node.
        /// </summary>
        /// <param name="process">Process that starts a node.</param>
        private async static Task StartNodeAsync(Process process)
        {
            process.Start();
            await process.WaitForExitAsync();  // this needs to run untill the app is closed
        }
    }
}
