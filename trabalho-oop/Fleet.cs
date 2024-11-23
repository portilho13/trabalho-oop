//-----------------------------------------------------------------
//    <copyright file="Fleet.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// Represents a fleet of airplanes.
    /// Manages the collection of Airplane objects and provides methods to add, remove, and retrieve airplanes.
    /// </summary>
    public class Fleet
    {
        // Dictionary to hold the fleet of airplanes, indexed by their registration code.
        private Dictionary<string, Airplane> _fleet = new Dictionary<string, Airplane>();

        // Logger instance to log actions performed on the fleet
        private readonly ILogger _logger;

        public Fleet(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }

        /// <summary>
        /// Checks if an airplane with the specified registration exists in the fleet.
        /// </summary>
        /// <param name="registration">The registration of the airplane to check.</param>
        /// <returns>True if the airplane exists, otherwise false.</returns>
        private bool DoesPlaneExist(string registration) => _fleet.ContainsKey(registration);

        /// <summary>
        /// Retrieves an airplane from the fleet by its registration code.
        /// Throws a KeyNotFoundException if the airplane does not exist.
        /// </summary>
        /// <param name="registration">The registration of the airplane to retrieve.</param>
        /// <returns>The airplane corresponding to the given registration.</returns>
        public Airplane GetAirplane(string registration)
        {
            return _fleet[registration];
        }

        /// <summary>
        /// Adds a new airplane to the fleet.
        /// If an airplane with the same registration already exists, it will throw an exception.
        /// </summary>
        /// <param name="airplane">The airplane to add to the fleet.</param>
        public void AddAirplane(Airplane airplane)
        {
            if (DoesPlaneExist(airplane.Registration))
            {
                throw new InvalidOperationException($"An airplane with registration {airplane.Registration} already exists.");
            }
            
            _fleet.Add(airplane.Registration, airplane);
            _logger.Info($"Airplane {airplane.Registration} added to fleet.");
        }

        /// <summary>
        /// Loads the fleet of airplanes from the files stored in the FMS system.
        /// Deserializes the JSON files into Airplane objects and adds them to the fleet.
        /// </summary>
        public void LoadFleet()
        {
            // Retrieves a list of files containing airplane data
            string[] files = FMS.Instance.ReadAirplaneFromFolder();

            foreach (string file in files)
            {
                // Reads the JSON data from the file
                string json = FMS.Instance.ReadFromJson(file);

                // Deserializes the JSON data into an Airplane object
                Airplane airplane = JsonSerializer.Deserialize<Airplane>(json);

                // Adds the deserialized airplane to the fleet
                this.AddAirplane(airplane);
            }
        }

        /// <summary>
        /// Displays the registration numbers of all airplanes in the fleet.
        /// </summary>
        public void ShowAircraftList()
        {
            foreach (Airplane airplane in _fleet.Values)
            {
                Console.WriteLine(airplane.Registration);
            }
        }

        /// <summary>
        /// Removes an airplane from the fleet by its registration code.
        /// Also deletes the airplane data from the FMS system.
        /// </summary>
        /// <param name="registration">The registration of the airplane to remove.</param>
        public void RemoveAirplane(string registration)
        {
            // Retrieves the airplane to be removed
            Airplane airplane = GetAirplane(registration);

            // Deletes the airplane data from the FMS system
            FMS.Instance.DeleteAirplane(airplane);

            // Removes the airplane from the fleet
            _fleet.Remove(registration);

            _logger.Info($"Airplane {registration} removed from fleet.");
        }
    }
}
