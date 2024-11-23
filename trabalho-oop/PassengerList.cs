//-----------------------------------------------------------------
//    <copyright file="PassengerList.cs" company="Ryanair">
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
    /// Represents a list of passengers, capable of generating random passengers and assigning reservations.
    /// </summary>
    public class PassengerList
    {
        // Fields to hold passenger names and surnames, fetched from FMS instance
        private List<string> _passengerNames = FMS.Instance.GetPassengerNames();
        private List<string> _passengerSurnames = FMS.Instance.GetPassengerSurnames();

        // Logger instance to log actions
        private readonly ILogger _logger;
        
        /// <summary>
        /// Constructor to initialize the PassengerList with a logger instance.
        /// </summary>
        /// <param name="logger">The logger instance used for logging actions in the passenger list.</param>
        public PassengerList(ILogger logger) => _logger = logger;

        /// <summary>
        /// Generates a random passenger name by selecting a random name and surname from predefined lists.
        /// </summary>
        /// <returns>A random passenger's full name.</returns>
        private string GenerateRandomPassenger()
        {
            // Ensure that both lists (names and surnames) are populated before proceeding
            if (_passengerNames.Count == 0 || _passengerSurnames.Count == 0)
            {
                Console.WriteLine("Passenger names or surnames list is empty.");
                return "";
            }

            // Create a single Random instance to avoid repetitive random seed generation
            Random random = new Random();

            // Get random indices from each list
            int randomNameIndex = random.Next(0, _passengerNames.Count);
            int randomSurnameIndex = random.Next(0, _passengerSurnames.Count);

            // Retrieve the random name and surname based on the generated indices
            string passengerName = _passengerNames[randomNameIndex];
            string passengerSurname = _passengerSurnames[randomSurnameIndex];

            // Concatenate the name and surname with a space in between to form the full name
            string passenger = passengerName + " " + passengerSurname;

            // Return the generated passenger name
            return passenger;
        }
        
        /// <summary>
        /// Generates a list of passengers with assigned reservations.
        /// </summary>
        /// <param name="passengerCount">The number of passengers to generate.</param>
        /// <returns>A dictionary with reservation codes as keys and corresponding reservations as values.</returns>
        public Dictionary<string, Reservation> GeneratePassengerList(int passengerCount)
        {
            // Dictionary to store the generated passengers and their reservations
            Dictionary<string, Reservation> passengersReservatonList = new Dictionary<string, Reservation>();

            // Loop to generate the specified number of passengers
            for (int i = 0; i < passengerCount; i++)
            {
                // Create a new passenger and assign a random name
                Passenger p = new Passenger(_logger);
                string passengerName = GenerateRandomPassenger();
                p.Name = passengerName;

                // Create a new reservation for the passenger
                Reservation r = new Reservation(p, _logger);

                // Ensure the reservation code is unique by generating it until it's not found in the dictionary
                do
                {
                    r.ReservationCode = NumberGenerator.GenerateRandomNumber();
                } while (passengersReservatonList.ContainsKey(r.ReservationCode));

                // Add the reservation to the dictionary
                passengersReservatonList.Add(r.ReservationCode, r);
            }

            // Return the list of generated passengers and their reservations
            return passengersReservatonList;
        }
    }
}
