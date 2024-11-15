//-----------------------------------------------------------------
//    <copyright file="NumberGenerator.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

namespace trabalho_oop
{
    /// <summary>
    /// Provides functionality for generating random alphanumeric strings.
    /// </summary>
    public static class NumberGenerator
    {
        /// <summary>
        /// Generates a random alphanumeric string consisting of uppercase letters and digits.
        /// The generated string is 6 characters long.
        /// </summary>
        /// <returns>A random 6-character alphanumeric string.</returns>
        public static string GenerateRandomNumber()
        {
            // Alphanumeric characters (uppercase letters and digits) used for the random string
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            // Random number generator instance to select random characters
            var random = new Random();
            
            // Array to hold the randomly selected characters
            var randomId = new char[6];

            // Loop to generate a 6-character string
            for (int i = 0; i < randomId.Length; i++)
            {
                // Select a random character from the 'chars' string and add it to the array
                randomId[i] = chars[random.Next(chars.Length)];
            }

            // Return the generated string
            return new string(randomId);
        }
    }
}