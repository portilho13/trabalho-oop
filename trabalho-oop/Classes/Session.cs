//-----------------------------------------------------------------
//    <copyright file="Session.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

namespace trabalho_oop
{
    using System;

    /// <summary>
    /// The Session class manages the session state for a logged-in person. 
    /// It keeps track of the logged-in user and the session's creation time.
    /// </summary>
    public class Session
    {
        // The private field holding the logged-in person
        private Person _loggedInPerson;

        // Logger instance for logging session activities
        private static ILogger _logger;

        /// <summary>
        /// The logged-in person for this session.
        /// If no one is logged in, an exception is thrown when accessed.
        /// </summary>
        public Person LoggedInPerson
        {
            get
            {
                // Throws an exception if no person is logged in
                if (_loggedInPerson == null)
                {
                    throw new InvalidOperationException("No person is currently logged in");
                }
                return _loggedInPerson;
            }
            private set
            {
                // Ensures that the logged-in person is not null
                _loggedInPerson = value ?? throw new ArgumentNullException(nameof(value), "Logged in person cannot be null");
            }
        }

        /// <summary>
        /// The creation timestamp of the session.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Initializes a new session for a given person.
        /// This constructor logs the creation of the session and sets up the session time.
        /// </summary>
        /// <param name="person">The person to be logged in for this session.</param>
        /// <param name="logger">Logger for session-related activities.</param>
        public Session(Person person, ILogger logger)
        {
            // Check if the person parameter is null and throw an exception if so
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Cannot create session with null person");
            }

            // Initialize the logger
            _logger = logger;

            // Set the logged-in person and session creation time
            LoggedInPerson = person;
            CreatedAt = DateTime.Now;

            // Log the session creation
            _logger.Info($"Session created for person: {person.GetType().Name}");
        }
        
        /// <summary>
        /// Destructor for the Session class.
        /// Logs when a session is being destroyed (when the object is being garbage collected).
        /// </summary>
        ~Session()
        {
            try
            {
                // Log the destruction of the session if a person was logged in
                if (_loggedInPerson != null)
                {
                    _logger.Info($"Session for person {_loggedInPerson.GetType().Name} is being destroyed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any errors during the destructor, but do not throw exceptions
                Console.Error.WriteLine($"Error in Session destructor: {ex.Message}");
            }
        }
    }
}