using System;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {
            FMS fms = new FMS();
            fms.Start();

            Airplane ryanair = new Airplane("Ryanair", "EI-GSG");

            Flight flight = new Flight("RYR4703","Porto", "Milan", ryanair, fms);
            flight.SaveFlight();
        }
    }
};

