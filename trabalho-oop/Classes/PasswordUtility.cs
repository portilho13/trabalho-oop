//-----------------------------------------------------------------
//    <copyright file="PasswordUtility.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;

namespace trabalho_oop
{
    #region Class Documentation

    /// <summary>
    /// Provides utility methods for handling passwords.
    /// Includes functionality to hash a password using SHA-256.
    /// </summary>
    public static class PasswordUtility
    {
        #endregion

        #region Public Methods

        /// <summary>
        /// Hashes a given password using the SHA-256 algorithm.
        /// </summary>
        /// <param name="password">The password to be hashed.</param>
        /// <returns>The hashed password as a hexadecimal string.</returns>
        /// <exception cref="ArgumentException">Thrown if the password is null or empty.</exception>
        public static string HashPassword(string password)
        {
            // Validate that the password is not null or whitespace
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            // Use SHA256 to hash the password
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Convert password to bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                
                // Compute the hash
                byte[] hashBytes = sha256Hash.ComputeHash(passwordBytes);

                // Create a StringBuilder to hold the resulting hash string
                StringBuilder hashString = new StringBuilder();
            
                // Convert each byte in the hash to a hexadecimal string
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }
            
                // Return the hashed password as a string
                return hashString.ToString();
            }
        }

        #endregion
    }
}
