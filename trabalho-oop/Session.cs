namespace trabalho_oop;

using System;

public class Session
{
    private Person _loggedInPerson;
    public Person LoggedInPerson
    {
        get
        {
            if (_loggedInPerson == null)
            {
                throw new InvalidOperationException("No person is currently logged in");
            }
            return _loggedInPerson;
        }
        private set
        {
            _loggedInPerson = value ?? throw new ArgumentNullException(nameof(value), "Logged in person cannot be null");
        }
    }

    public DateTime CreatedAt { get; private set; }

    public Session(Person person)
    {
        try
        {
            if (person == null)
            {
                throw new ArgumentNullException(nameof(person), "Cannot create session with null person");
            }

            LoggedInPerson = person;
            CreatedAt = DateTime.Now;

            Logger.Instance().Info($"Session created for person: {person.GetType().Name}");
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            Logger.Instance().Error($"Failed to create session: {ex.Message}");
            throw new InvalidOperationException("Failed to initialize session", ex);
        }
    }
    
    ~Session()
    {
        try
        {
            if (_loggedInPerson != null)
            {
                Logger.Instance().Info($"Session for person {_loggedInPerson.GetType().Name} is being destroyed.");
            }
        }
        catch (Exception ex)
        {
            // Don't throw exceptions in destructors
            Console.Error.WriteLine($"Error in Session destructor: {ex.Message}");
        }
    }
}