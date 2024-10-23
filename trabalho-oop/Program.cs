using System;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {
            FMS fms = new FMS(); // Start new file managment system
            fms.Start();
            

            Airplane ryanair = new Airplane("Ryanair", "EI-GSG", fms);
            fms.SaveAirplane(ryanair);

            Flight flight = new Flight("RYR4703","Porto", "Milan", ryanair); 
            fms.SaveFlight(flight);
            
        }
    }
};

