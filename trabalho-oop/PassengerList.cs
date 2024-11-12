namespace trabalho_oop;

public class PassengerList
{
    private List<string> passengerNames = new List<string>();
    private List<string> passengerSurnames = new List<string>();
    private FMS Fms;

    public PassengerList(FMS fms)
    {
        Fms = fms;
        passengerNames = Fms.GetPassengerNames();
        passengerSurnames = Fms.GetPassengerSurnames();
    }

    private string GenerateRandomPassenger()
    {
        // Check that both lists contain elements before proceeding
        if (passengerNames.Count == 0 || passengerSurnames.Count == 0)
        {
            Console.WriteLine("Passenger names or surnames list is empty.");
            return "";
        }

        // Create a single Random instance
        Random random = new Random();

        // Get random indices
        int randomNameIndex = random.Next(0, passengerNames.Count);
        int randomSurnameIndex = random.Next(0, passengerSurnames.Count);

        // Retrieve the random name and surname
        string passengerName = passengerNames[randomNameIndex];
        string passengerSurname = passengerSurnames[randomSurnameIndex];

        // Concatenate with a space in between
        string passenger = passengerName + " " + passengerSurname;

        // Output the result
        return passenger;
    }
    


    public Dictionary<string, Reservation> GeneratePassengerList(int passengerCount)
    {
        Dictionary<string, Reservation> passengersReservatonList = new Dictionary<string, Reservation>();
        for (int i = 0; i < passengerCount; i++)
        {
            Reservation r = new Reservation();
            Passenger p = new Passenger();
            string passengerName = GenerateRandomPassenger();
            p.Name = passengerName;
            
            r.Passenger = p;
            do
            {
                r.GenerateReservationCode();
            } while(passengersReservatonList.ContainsKey(r.ReservationCode));
            
            passengersReservatonList.Add(r.ReservationCode, r);

        }
        return passengersReservatonList;
    }

}
