using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBlocks.Classes
{
    /// <summary>
    /// Class representing user account.
    /// </summary>
    internal class Account
    {
        /// <summary>
        /// Users login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Users password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Constructor of an account class.
        /// </summary>
        /// <param name="login">Users login.</param>
        /// <param name="password">Users password.</param>
        public Account(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
