using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        /// Runns a multichain command using CMD (warning: multichain must be already running).
        /// </summary>
        /// <param name="prefix">E.g. multichain-cli or multichaind.</param>
        /// <param name="chainName">Name of the chain.</param>
        /// <param name="command">instructions after chain name (e.g. liststreams or liststreamkeys streamName).</param>
        /// <returns>Output informations (like in CMD) or error infrmations if something goes wrong.</returns>
        public static (string, string) RunCommand(string prefix, string chainName, string command)
        {
            Process process = new Process();
            process.StartInfo.WorkingDirectory = @"d:\Multichain\";              // <-- to powinno być sczytywane z Setup.txt
            process.StartInfo.FileName = $@"d:\Multichain\{prefix}.exe";
            process.StartInfo.Arguments = $"{chainName} {command}";
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
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            return (output, err);
        }

        /// <summary>
        /// Gathers information about the chain and its component. Creates classes.
        /// </summary>
        /// <param name="chainName">Name of a chain.</param>
        /// <returns>Chain object.</returns>
        internal static Chain InitializeChain(string chainName)
        {
            //GlobalVariables.ChainName = ExtensionsMethods.ReadSetupFile();
            //(List<Stream> queueStreams, List<Stream> gameStreams) = MultiChainClient.ReadChainStreams();
            //var chain = new Chain(GlobalVariables.ChainName, queueStreams, gameStreams);

            //return chain;
            return null;
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
