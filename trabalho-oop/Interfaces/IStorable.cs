//-----------------------------------------------------------------
//    <copyright file="IStorable.cs" company="Ryanair">
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
    /// The IStorable interface is implemented by classes that need to be stored in the system.
    /// Any class that implements this interface will provide methods for converting its data to 
    /// JSON format, retrieving a unique identifier for storage, and specifying the entity type.
    /// </summary>
    public interface IStorable
    {
        /// <summary>
        /// Converts the object to a JSON string representation.
        /// This method is necessary for storing the object's data in JSON format.
        /// </summary>
        /// <returns>A JSON string representation of the object.</returns>
        string ConvertToJson();  // Common function in every class that needs to be saved

        /// <summary>
        /// Retrieves the unique identifier of the object.
        /// This method returns an identifier that uniquely identifies the object
        /// and is used for storing the object, for example, in file names.
        /// </summary>
        /// <returns>The unique identifier of the object.</returns>
        string GetIdentifier();  // Unique identifier of every class
        
        /// <summary>
        /// Returns the entity type of the object.
        /// This method is used to determine what type of entity the object represents
        /// (e.g., Airplane, Flight, etc.) so that the correct handling and storage
        /// logic can be applied.
        /// </summary>
        /// <returns>The entity type of the object.</returns>
        EntityType GetEntityType();
    }
}