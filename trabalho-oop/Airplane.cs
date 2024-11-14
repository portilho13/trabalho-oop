using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Airplane : IStorable
    {
        // Public properties with private setters to enforce immutability after construction
        public string Company { get;  set; }
        public string Registration { get; set; }
        public bool IsOccupied { get; set; }
        public int Capacity { get; set; }
        
        public string Model { get; set; }
        
        public Airplane() {}

        // Constructor to initialize the essential properties, with validation
        public Airplane(string company, string registration, int capacity, string model)
        {
            try
            {
                ValidateConstructorParameters(company, registration, capacity);

                Company = company;
                Registration = registration;
                Capacity = capacity;
                IsOccupied = false;  // Default value
                Model = model;

                Logger.Instance().Info($"Airplane {Registration} created successfully. Company: {Company}, Capacity: {Capacity}");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Failed to create airplane due to invalid parameters", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unexpected error occurred while creating airplane", ex);
            }
        }

        private void ValidateConstructorParameters(string company, string registration, int capacity)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(company))
                    throw new ArgumentException("Company name cannot be empty or whitespace.", nameof(company));

                if (string.IsNullOrWhiteSpace(registration))
                    throw new ArgumentException("Registration cannot be empty or whitespace.", nameof(registration));

                if (registration.Length < 2 || registration.Length > 10)
                    throw new ArgumentException("Registration must be between 2 and 10 characters.", nameof(registration));

                if (capacity <= 0)
                    throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive number.");
                
            }
            catch (Exception ex) when (ex is ArgumentException || ex is ArgumentOutOfRangeException)
            {
                throw; // Re-throw the validation exceptions
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unexpected error during parameter validation", ex);
            }
        }

        // Method to toggle the occupation status of the airplane
        public void ChangeOccupiedStatus()
        {
            try
            {
                IsOccupied = !IsOccupied;
                Logger.Instance().Info($"Airplane {Registration} occupied status changed to {IsOccupied}.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to change occupation status for airplane {Registration}", ex);
            }
        }

        // Convert the object to JSON for storage
        public string ConvertToJson()
        {
            try
            {
                return JsonSerializer.Serialize(this, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });
            }
            catch (JsonException ex)
            {
                throw new JsonException($"Failed to serialize airplane {Registration} to JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unexpected error while converting airplane {Registration} to JSON", ex);
            }
        }

        // Return the identifier for this entity (used for saving, etc.)
        public string GetIdentifier()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Registration))
                {
                    throw new InvalidOperationException("Airplane registration is not set or is invalid.");
                }
                return Registration;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to get identifier for airplane", ex);
            }
        }

        // Return the entity type for FMS operations
        public EntityType GetEntityType()
        {
            try
            {
                return EntityType.Airplane;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to get entity type", ex);
            }
        }

        // Destructor (Finalizer)
        ~Airplane()
        {
            Logger.Instance().Info($"Airplane {Registration} instance is being destroyed.");
        }
    }
}