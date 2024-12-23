﻿//-----------------------------------------------------------------
//    <copyright file="Airplane.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>1.0</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// Represents an airplane with properties such as company, registration, capacity, and model.
    /// Provides functionality for managing occupation status and serializing to JSON.
    /// </summary>
    public class Airplane : IStorable
    {
        #region Private Properties
        
        //-----------------------------------------------------------------
        //    <summary>
        //        Private logger instance to log information and errors 
        //        related to airplane operations.
        //    </summary>
        //-----------------------------------------------------------------
        private ILogger _logger;
        
        #endregion
        
        #region Public Properties

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the company that owns the airplane.
        //    </summary>
        //-----------------------------------------------------------------
        public string Company { get; set; }

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the unique registration number of the airplane.
        //    </summary>
        //-----------------------------------------------------------------
        public string Registration { get; set; }

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the occupation status of the airplane.
        //        Indicates whether the airplane is occupied or not.
        //    </summary>
        //-----------------------------------------------------------------
        public bool IsOccupied { get; set; }

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the maximum capacity of the airplane (e.g., number of seats).
        //    </summary>
        //-----------------------------------------------------------------
        public int Capacity { get; set; }

        //-----------------------------------------------------------------
        //    <summary>
        //        Gets or sets the model of the airplane.
        //    </summary>
        //-----------------------------------------------------------------
        public string Model { get; set; }

        #endregion
        
        #region Constructors
        
        //-----------------------------------------------------------------
        //    <summary>
        //        Default constructor for the Airplane class. 
        //        Initializes an empty instance without any parameters.
        //    </summary>
        //-----------------------------------------------------------------
        public Airplane() {}

        //-----------------------------------------------------------------
        //    <summary>
        //        Constructor to initialize the essential properties of the airplane.
        //        Validates the parameters and logs the airplane creation.
        //    </summary>
        //    <param name="company">Company owning the airplane.</param>
        //    <param name="registration">Unique registration number of the airplane.</param>
        //    <param name="capacity">Capacity of the airplane.</param>
        //    <param name="model">Model of the airplane.</param>
        //    <param name="logger">Logger instance to log information.</param>
        //-----------------------------------------------------------------
        public Airplane(string company, string registration, int capacity, string model, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "logger cannot be null");
            // Validate the constructor parameters before initializing
            ValidateConstructorParameters(company, registration, capacity, model);

            // Initialize properties
            Company = company;
            Registration = registration;
            Capacity = capacity;
            IsOccupied = false;  // Default value for IsOccupied
            Model = model;
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Sets the logger instance for the airplane.
        //    </summary>
        //    <param name="logger">Logger instance to log information.</param>
        //-----------------------------------------------------------------
        public void SetLogger(ILogger logger) => _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");

        #endregion
        
        #region Private Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Validates the constructor parameters to ensure they meet required conditions.
        //        Throws appropriate exceptions for invalid inputs.
        //    </summary>
        //    <param name="company">Company owning the airplane.</param>
        //    <param name="registration">Unique registration number of the airplane.</param>
        //    <param name="capacity">Capacity of the airplane.</param>
        //    <param name="model">Model of the airplane.</param>
        //-----------------------------------------------------------------
        private void ValidateConstructorParameters(string company, string registration, int capacity, string model)
        {
            // Ensure company is not empty or whitespace
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("Company name cannot be empty or whitespace.", nameof(company));

            // Ensure registration is not empty or whitespace and within valid length
            if (string.IsNullOrWhiteSpace(registration))
                throw new ArgumentException("Registration cannot be empty or whitespace.", nameof(registration));

            if (registration.Length < 5 || registration.Length > 10)
                throw new ArgumentException("Registration must be between 2 and 10 characters.", nameof(registration));

            // Ensure capacity is a positive value
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive number.");

            // Ensure model is not empty or whitespace
            if (string.IsNullOrWhiteSpace(model))
                throw new ArgumentException("Model cannot be empty or whitespace.", nameof(model));
        }

        #endregion

        #region Public Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Toggles the occupation status of the airplane and logs the change.
        //    </summary>
        //-----------------------------------------------------------------
        public void ChangeOccupiedStatus()
        {
            // Toggle the occupation status
            IsOccupied = !IsOccupied;
            // Log the change in occupation status (log functionality can be added here)
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Converts the airplane object to a JSON string for storage.
        //    </summary>
        //    <returns>A JSON string representing the airplane object.</returns>
        //-----------------------------------------------------------------
        public string ConvertToJson()
        {
            try
            {
                // Serialize the object to a formatted JSON string
                return JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            }
            catch (JsonException ex)
            {
                // Log the JSON serialization error and throw a detailed exception
                throw new JsonException($"Failed to serialize airplane {Registration} to JSON", ex);
            }
            catch (Exception ex)
            {
                // Catch any other unexpected errors during serialization
                throw new InvalidOperationException($"Unexpected error while converting airplane {Registration} to JSON", ex);
            }
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Returns the identifier of the airplane, which is its registration number.
        //    </summary>
        //    <returns>The registration number of the airplane.</returns>
        //-----------------------------------------------------------------
        public string GetIdentifier() => Registration;

        //-----------------------------------------------------------------
        //    <summary>
        //        Returns the entity type for this object, which is Airplane.
        //    </summary>
        //    <returns>The entity type of the object.</returns>
        //-----------------------------------------------------------------
        public EntityType GetEntityType() => EntityType.Airplane;

        #endregion
    }
}
