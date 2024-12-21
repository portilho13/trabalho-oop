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
        #region Private Properties

        //-----------------------------------------------------------------
        //    <summary>
        //        Dictionary to hold the fleet of airplanes, indexed by their registration code.
        //    </summary>
        //-----------------------------------------------------------------
        private Dictionary<string, Airplane> _fleet = new Dictionary<string, Airplane>();

        //-----------------------------------------------------------------
        //    <summary>
        //        Logger instance to log actions performed on the fleet.
        //    </summary>
        //-----------------------------------------------------------------
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        //-----------------------------------------------------------------
        //    <summary>
        //        Constructor to initialize the Fleet with a logger instance.
        //        Throws an exception if the logger is null.
        //    </summary>
        //    <param name="logger">Logger instance to log fleet operations.</param>
        //-----------------------------------------------------------------
        public Fleet(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
        }

        #endregion

        #region Private Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Checks whether an airplane with the given registration already exists in the fleet.
        //    </summary>
        //    <param name="registration">The registration code of the airplane.</param>
        //    <returns>True if the airplane exists, false otherwise.</returns>
        //-----------------------------------------------------------------
        private bool DoesPlaneExist(string registration) => _fleet.ContainsKey(registration);

        #endregion

        #region Public Methods

        //-----------------------------------------------------------------
        //    <summary>
        //        Retrieves an airplane by its registration code.
        //        If the airplane is not found, returns null.
        //    </summary>
        //    <param name="registration">The registration code of the airplane.</param>
        //    <returns>The Airplane object corresponding to the registration, or null if not found.</returns>
        //-----------------------------------------------------------------
        public Airplane? GetAirplane(string registration)
        {
            if (_fleet.TryGetValue(registration, out var airplane))
            {
                return airplane;
            }

            return null; // Return null if the registration does not exist
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Adds a new airplane to the fleet.
        //        Throws an exception if an airplane with the same registration already exists.
        //    </summary>
        //    <param name="airplane">The Airplane object to be added.</param>
        //    <exception cref="InvalidOperationException">Thrown if the airplane already exists in the fleet.</exception>
        //-----------------------------------------------------------------
        public void AddAirplane(Airplane airplane)
        {
            if (airplane == null) throw new ArgumentNullException(nameof(airplane));
            if (DoesPlaneExist(airplane.Registration))
            {
                throw new InvalidOperationException($"An airplane with registration {airplane.Registration} already exists.");
            }
            
            _fleet.Add(airplane.Registration, airplane);
            _logger.Info($"Airplane {airplane.Registration} added to fleet.");
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Loads the fleet of airplanes from external files stored in the FMS system.
        //        Deserializes each file into an Airplane object and adds them to the fleet.
        //    </summary>
        //-----------------------------------------------------------------
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

        //-----------------------------------------------------------------
        //    <summary>
        //        Retrieves a list of all airplane registration numbers in the fleet.
        //    </summary>
        //    <returns>A list of registration numbers of all airplanes in the fleet.</returns>
        //-----------------------------------------------------------------
        public List<string> GetAirplaneRegistrations()
        {
            List<string> aircraftList = new List<string>();
            foreach (Airplane airplane in _fleet.Values)
            {
                aircraftList.Add(airplane.Registration);
            }
            return aircraftList;
        }

        //-----------------------------------------------------------------
        //    <summary>
        //        Removes an airplane from the fleet by its registration code.
        //        Also deletes the airplane data from the FMS system.
        //    </summary>
        //    <param name="registration">The registration of the airplane to remove.</param>
        //    <exception cref="KeyNotFoundException">Thrown if the airplane does not exist.</exception>
        //-----------------------------------------------------------------
        public void RemoveAirplane(string registration)
        {
            // Retrieves the airplane to be removed
            Airplane airplane = GetAirplane(registration);
            if (airplane == null) throw new KeyNotFoundException($"The airplane with registration {registration} does not exist.");

            // Deletes the airplane data from the FMS system
            FMS.Instance.DeleteAirplane(airplane);

            // Removes the airplane from the fleet
            _fleet.Remove(registration);

            _logger.Info($"Airplane {registration} removed from fleet.");
        }

        #endregion
    }
}
