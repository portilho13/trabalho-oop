using System;
using System.Text.Json;

namespace trabalho_oop
{
    public class Airplane : IStorable
    {
        // Public properties with private setters to enforce immutability after construction
        public string Company { get; private set; }
        public string Registration { get; private set; }
        public bool IsOccupied { get; private set; }
        public int Capacity { get; private set; }

        // Constructor to initialize the essential properties, with validation
        public Airplane(string company, string registration, int capacity)
        {
            // Validation to ensure valid data is passed to constructor
            if (string.IsNullOrWhiteSpace(company))
                throw new ArgumentException("Company name cannot be empty.", nameof(company));

            if (string.IsNullOrWhiteSpace(registration))
                throw new ArgumentException("Registration cannot be empty.", nameof(registration));

            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive number.");

            // Initializing properties
            Company = company;
            Registration = registration;
            Capacity = capacity;
            IsOccupied = false;  // Default value

            Logger.Instance().Info($"Airplane {Registration} created successfully. Company: {Company}, Capacity: {Capacity}");
        }

        // Method to toggle the occupation status of the airplane
        public void ChangeOccupiedStatus()
        {
            IsOccupied = !IsOccupied;
            Logger.Instance().Info($"Airplane {Registration} occupied status changed to {IsOccupied}.");
        }

        // Convert the object to JSON for storage
        public string ConvertToJson() => JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });

        // Return the identifier for this entity (used for saving, etc.)
        public string GetIdentifier() => Registration;

        // Return the entity type for FMS operations
        public EntityType GetEntityType() => EntityType.Airplane;

        // Destructor (Finalizer)
        ~Airplane()
        {
            Logger.Instance().Info($"Airplane {Registration} instance is being destroyed.");
        }
    }
}
