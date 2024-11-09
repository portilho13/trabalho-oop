using System;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            FMS fms = new FMS(); // Start new file managment system
            fms.Start();

            fms.GetPassangerSurnames();
            
            SessionManager sessionManager = new SessionManager();
            sessionManager.fms = fms;

            sessionManager.Load();
            
            sessionManager.DisplayStaffList();
            
            sessionManager.RegisterStaff("Junior", "junior.portilho2005@gmail.com", "junior");
            
            Airplane ryanair = new Airplane("Ryanair", "EI-GSG", fms, 186);
            fms.SaveAirplane(ryanair);

            Flight flight = new Flight("RYR4703","Porto", "Milan", ryanair, fms);

            flight.DisplayPassengers();
            fms.SaveFlight(flight);
            
            sessionManager.Save();
            
        }
    }
};

