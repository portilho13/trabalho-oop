namespace trabalho_oop;

public interface IStorable
{
    string ConvertToJson();  // Common function in every class that needs to be saved
    string GetIdentifier();  // Unique identifier of every class
    
    EntityType GetEntityType();
}