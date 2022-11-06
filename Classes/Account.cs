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
        /// User's login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// User's password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Constructor of an account class.
        /// </summary>
        /// <param name="login">User's login.</param>
        /// <param name="password">User's password.</param>
        public Account(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
