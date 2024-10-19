using System;

namespace trabalho_oop
{
    public class Program
    {

        static void AircraftMenu(FMS Fms)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("|    1 - Create Aircraft    |");
                Console.WriteLine("-----------------------------");
                int option = int.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        string companyName;
                        string registration; 
                        Console.WriteLine("Company Name: ");
                        companyName = Console.ReadLine();
                        Console.WriteLine("Registration: ");
                        registration = Console.ReadLine();
                        
                        Console.WriteLine(companyName);
                        Console.WriteLine(registration);
                        
                        Airplane a = new Airplane(companyName, registration, Fms);
                        a.SaveAircraft();
                        break;
                }
            }
        }
        static int MainMenu()
        {
            Console.WriteLine("----------------------------");
            Console.WriteLine("|    1 - Aircraft Menu     |");
            Console.WriteLine("|    0 - Leave             |");
            Console.WriteLine("----------------------------");
            int option = int.Parse(Console.ReadLine());
            return option;
        }
        static void Main(string[] args)
        {
            FMS fms = new FMS();
            fms.Start();
            
            bool exit = false;
            while (!exit)
            {
                int option = MainMenu();
                switch (option)
                {
                    case 1:
                        AircraftMenu(fms);
                        break;
                    case 0:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Wrong option");
                        break;
                }
            }

            Airplane ryanair = new Airplane("Ryanair", "EI-GSG", fms);
            ryanair.SaveAircraft();

            Flight flight = new Flight("RYR4703","Porto", "Milan", ryanair, fms);
            flight.SaveFlight(fms);
            
            Fleet fleet = new Fleet(fms);
            fleet.ReadAircraftListFromAirplaneFolder();
            fleet.ShowAircraftList();
        }
    }
};

