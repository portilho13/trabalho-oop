//-----------------------------------------------------------------
//    <copyright file="SessionManager.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace trabalho_oop
{

    /// <summary>
    /// The SessionManager class handles the management of both staff and passengers' login sessions, 
    /// user authentication, and user data persistence. 
    /// It supports staff and passenger registration, login, session management, and data saving/loading.
    /// </summary>
    public class SessionManager
    {
        // The active session for the logged-in user (either staff or passenger)
        public Session ActiveSession;

        // Lists to hold staff and passenger data
        private readonly List<Staff> _staff;
        private readonly List<Passenger> _passengers;

        // Logger instance for logging events
        private readonly Logger _logger;

        /// <summary>
        /// Initializes the SessionManager with empty lists for staff and passengers
        /// and a logger to log all activities.
        /// </summary>
        public SessionManager(Logger logger)
        {
            try
            {
                _staff = new List<Staff>();
                _passengers = new List<Passenger>();
                _logger = logger;
                _logger.Info("SessionManager initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to initialize SessionManager: {ex.Message}");
                throw new InvalidOperationException("Failed to initialize SessionManager", ex);
            }
        }

        /// <summary>
        /// Handles the staff login process by verifying staff credentials (staff code and password).
        /// Creates a new session if the login is successful.
        /// </summary>
        public bool LoginStaff(string staffCode, string password)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(staffCode))
                    throw new ArgumentException("Staff code cannot be empty", nameof(staffCode));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_staff == null)
                    throw new InvalidOperationException("Staff list not initialized");

                // Hash the password and attempt to find the staff member with the provided credentials
                string hashedPassword = CreateHashPassword(password);
                Staff staff = _staff.Find(s => s.staffCode == staffCode && s.password == hashedPassword);

                // If staff found, create session and log the success
                if (staff != null)
                {
                    ActiveSession = new Session(staff, _logger);
                    _logger.Info($"Staff login successful: {staff.Name} ({staff.Email})");
                    return true;
                }

                // If no staff found, log the failed login attempt
                _logger.Warn($"Staff login failed: Invalid credentials for {staffCode}");
                return false;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                // Log errors and rethrow as a general exception
                _logger.Error($"Error during staff login: {ex.Message}");
                throw new InvalidOperationException("Failed to process staff login", ex);
            }
        }

        /// <summary>
        /// Creates a hashed version of the password using SHA256 for secure storage.
        /// </summary>
        private string CreateHashPassword(string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = sha256Hash.ComputeHash(passwordBytes);
                    StringBuilder hashString = new StringBuilder();
                    
                    // Convert the byte array to a hex string
                    foreach (byte b in hashBytes)
                    {
                        hashString.Append(b.ToString("x2"));
                    }
                    
                    return hashString.ToString();
                }
            }
            catch (Exception ex) when (ex is not ArgumentException)
            {
                // Log any errors encountered during password hashing
                _logger.Error("Error creating password hash");
                throw new InvalidOperationException("Failed to create password hash", ex);
            }
        }

        /// <summary>
        /// Loads staff and passenger data from the FMS (File Management System).
        /// </summary>
        public void Load()
        {
            try
            {
                _staff.Clear();
                string[] files = FMS.Instance.ReadStaffFromFolder();
                foreach (string file in files)
                {
                    string json = FMS.Instance.ReadFromJson(file);
                    Staff staff = JsonSerializer.Deserialize<Staff>(json);
                    if (staff != null)
                        this._staff.Add(staff);
                }

                _logger.Info("Staff data loaded from FMS");

                _passengers.Clear();
                files = FMS.Instance.ReadPassengersFromFolder();
                foreach (string file in files)
                {
                    string json = FMS.Instance.ReadFromJson(file);
                    Passenger passenger = JsonSerializer.Deserialize<Passenger>(json);
                    if (passenger != null)
                        this._passengers.Add(passenger);
                }

                _logger.Info("Passenger data loaded from FMS");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to load staff data: {ex.Message}");
                throw new InvalidOperationException("Failed to load staff data from FMS", ex);
            }
        }

        /// <summary>
        /// Handles passenger login by checking the email and password.
        /// Creates a new session for the passenger if the login is successful.
        /// </summary>
        public bool LoginPassenger(string email, string password)
        {
            try
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty", nameof(email));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_passengers == null)
                    throw new InvalidOperationException("Passenger list not initialized");

                // Check if a passenger exists with the provided email and password
                Passenger passenger = _passengers.Find(s => s.Email == email && s.password == password);

                if (passenger != null)
                {
                    ActiveSession = new Session(passenger, _logger);
                    _logger.Info($"Passenger login successful: {passenger.Name} ({passenger.Email})");
                    return true;
                }

                // If no passenger found, log the failed login attempt
                _logger.Warn($"Passenger login failed: Invalid credentials for {email}");
                return false;
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                _logger.Error($"Error during passenger login: {ex.Message}");
                throw new InvalidOperationException("Failed to process passenger login", ex);
            }
        }

        /// <summary>
        /// Logs the current user out of the session.
        /// </summary>
        public void Logout()
        {
            try
            {
                if (ActiveSession?.LoggedInPerson != null)
                {
                    _logger.Info($"User {ActiveSession.LoggedInPerson.Name} logged out");
                }
                ActiveSession = null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error during logout: {ex.Message}");
                throw new InvalidOperationException("Failed to process logout", ex);
            }
        }

        /// <summary>
        /// Checks if there is an active session.
        /// </summary>
        public bool IsAuthenticated()
        {
            try
            {
                return ActiveSession != null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error checking authentication status: {ex.Message}");
                throw new InvalidOperationException("Failed to check authentication status", ex);
            }
        }

        /// <summary>
        /// Displays all registered staff members.
        /// </summary>
        public void DisplayStaff()
        {
            foreach (Staff staff in _staff)
            {
                Console.WriteLine(staff.ToString());
            }
        }

        /// <summary>
        /// Displays all registered passengers.
        /// </summary>
        public void DisplayPassengers()
        {
            foreach (Passenger passenger in _passengers)
            {
                Console.WriteLine($"Passenger: {passenger.ToString()}");
            }
        }

        /// <summary>
        /// Returns the entity type (either Passenger or Staff) of the currently logged-in user.
        /// </summary>
        public EntityType GetEntityType()
        {
            Person person = GetLoggedInPerson();
            if (person is Passenger)
            {
                return EntityType.Passenger;
            }
            return EntityType.Staff;
        }

        /// <summary>
        /// Retrieves the person currently logged into the session.
        /// </summary>
        public Person GetLoggedInPerson()
        {
            try
            {
                return ActiveSession?.LoggedInPerson;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving logged in person: {ex.Message}");
                throw new InvalidOperationException("Failed to retrieve logged in person", ex);
            }
        }

        /// <summary>
        /// Registers a new staff member with the given details.
        /// </summary>
        public void RegisterStaff(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty", nameof(name));

                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty", nameof(email));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_staff == null)
                    throw new InvalidOperationException("Staff list not initialized");

                Staff existingStaff = _staff.Find(s => s.Email == email);
                if (existingStaff != null)
                {
                    _logger.Warn($"Staff registration failed: Email {email} already exists");
                    throw new InvalidOperationException($"Email {email} is already registered");
                }

                Staff newStaff = new Staff(_logger)
                {
                    Name = name,
                    Email = email
                };
                newStaff.SetPassword(password);

                _staff.Add(newStaff);
                _logger.Info($"New staff registered: {name} ({email})");
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                _logger.Error($"Error during staff registration: {ex.Message}");
                throw new InvalidOperationException("Failed to register staff", ex);
            }
        }

        /// <summary>
        /// Registers a new passenger with the given details.
        /// </summary>
        public void RegisterPassanger(string name, string email, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty", nameof(name));

                if (string.IsNullOrWhiteSpace(email))
                    throw new ArgumentException("Email cannot be empty", nameof(email));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Password cannot be empty", nameof(password));

                if (_passengers == null)
                    throw new InvalidOperationException("Staff list not initialized");

                Passenger p = _passengers.Find(s => s.Email == email);
                if (p != null)
                {
                    _logger.Warn($"Staff registration failed: Email {email} already exists");
                    throw new InvalidOperationException($"Email {email} is already registered");
                }

                Passenger passenger = new Passenger(_logger)
                {
                    Name = name,
                    Email = email
                };
                passenger.password = password;

                _passengers.Add(passenger);
                _logger.Info($"New passenger registered: {name} ({email})");
            }
            catch (Exception ex) when (ex is not ArgumentException && ex is not InvalidOperationException)
            {
                _logger.Error($"Error during passenger registration: {ex.Message}");
                throw new InvalidOperationException("Failed to register passenger", ex);
            }
        }

        /// <summary>
        /// Saves all staff and passenger data to the FMS (File Management System).
        /// </summary>
        public void Save()
        {
            try
            {
                if (_staff == null || _passengers == null)
                    throw new InvalidOperationException("Staff or passenger list not initialized");

                foreach (Staff staff in _staff)
                {
                    if (staff != null)
                        FMS.Instance.Save(staff);
                }

                foreach (Passenger passenger in _passengers)
                {
                    if (passenger != null)
                        FMS.Instance.Save(passenger);
                }

                _logger.Info("Staff and Passenger data saved to FMS");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save data: {ex.Message}");
                throw new InvalidOperationException("Failed to save data to FMS", ex);
            }
        }
    }
}
