//-----------------------------------------------------------------
//    <copyright file="NumberGenerator.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

namespace trabalho_oop
{
    #region Class Documentation

    /// <summary>
    /// Provides functionality for generating random alphanumeric strings.
    /// </summary>
    public static class NumberGenerator
    {
        #endregion

        #region Constants

        // Alphanumeric characters (uppercase letters and digits) used for the random string
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        #endregion

        #region Methods

        /// <summary>
        /// Generates a random alphanumeric string consisting of uppercase letters and digits.
        /// The generated string is 6 characters long.
        /// </summary>
        /// <returns>A random 6-character alphanumeric string.</returns>
        public static string GenerateRandomNumber()
        {
            // Random number generator instance to select random characters
            var random = new Random();
            
            // Array to hold the randomly selected characters
            var randomId = new char[6];

            // Loop to generate a 6-character string
            for (int i = 0; i < randomId.Length; i++)
            {
                // Select a random character from the 'Chars' string and add it to the array
                randomId[i] = Chars[random.Next(Chars.Length)];
            }

            // Return the generated string
            return new string(randomId);
        }

        #endregion
    }

}