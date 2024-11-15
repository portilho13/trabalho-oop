//-----------------------------------------------------------------
//    <copyright file="EntityType.cs" company="Ryanair">
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
    /// Represents the different types of entities within the system.
    /// Used to categorize various objects such as flights, airplanes, passengers, etc.
    /// </summary>
    public enum EntityType
    {
        /// <summary>
        /// Represents a flight entity.
        /// </summary>
        Flight,

        /// <summary>
        /// Represents an airplane entity.
        /// </summary>
        Airplane,

        /// <summary>
        /// Represents a passenger entity.
        /// </summary>
        Passenger,

        /// <summary>
        /// Represents a staff entity.
        /// </summary>
        Staff,

        /// <summary>
        /// Represents a reservation entity.
        /// </summary>
        Reservation,

        /// <summary>
        /// Represents an unknown entity type. Used as a default or fallback value.
        /// </summary>
        Unknown
    }
}