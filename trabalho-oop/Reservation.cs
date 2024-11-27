//-----------------------------------------------------------------
//    <copyright file="Reservation.cs" company="Ryanair">
//     Copyright Ryanair. All rights reserved.
//    </copyright>
//    <date>15-11-2024</date>
//    <time>17:00</time>
//    <version>0.1</version>
//    <author>Mario Portilho @a27989</author>
//-----------------------------------------------------------------

using System;
using System.Text.Json;

namespace trabalho_oop
{
    /// <summary>
    /// The Reservation class represents a reservation made by a passenger for a flight.
    /// It contains reservation details like the reservation code and the passenger information.
    /// </summary>
    public class Reservation
    {

        // The reservation code (unique identifier), will be generated automatically\
        
        public string ReservationCode { get; set; }

        
    }
}