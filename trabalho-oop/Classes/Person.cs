﻿//-----------------------------------------------------------------
//    <copyright file="Person.cs" company="Ryanair">
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
    /// Represents a person with basic contact information such as name, email, and phone number.
    /// This class serves as a base class for other entities that require personal details.
    /// </summary>
    public abstract class Person
    {
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the full name of the person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the person.
        /// </summary>
        public string Phone { get; set; }

        #endregion
    }
}