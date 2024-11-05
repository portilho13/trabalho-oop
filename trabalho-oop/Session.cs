namespace trabalho_oop;

using System;

public class Session
{
    public Person LoggedInPerson { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Session(Person person)
    {
        LoggedInPerson = person;
        CreatedAt = DateTime.Now;
    }
    
    ~Session() {}
}
