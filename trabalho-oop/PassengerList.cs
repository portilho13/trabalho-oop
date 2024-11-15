namespace trabalho_oop;

public class PassengerList
{
    private List<string> _passengerNames = FMS.Instance.GetPassengerNames();
    private List<string> _passengerSurnames = FMS.Instance.GetPassengerSurnames();

    private string GenerateRandomPassenger()
    {
        // Check that both lists contain elements before proceeding
        if (_passengerNames.Count == 0 || _passengerSurnames.Count == 0)
        {
            Console.WriteLine("Passenger names or surnames list is empty.");
            return "";
        }

        // Create a single Random instance
        Random random = new Random();

        // Get random indices
        int randomNameIndex = random.Next(0, _passengerNames.Count);
        int randomSurnameIndex = random.Next(0, _passengerSurnames.Count);

        // Retrieve the random name and surname
        string passengerName = _passengerNames[randomNameIndex];
        string passengerSurname = _passengerSurnames[randomSurnameIndex];

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
                r.ReservationCode = NumberGenerator.GenerateRandomNumber();
            } while(passengersReservatonList.ContainsKey(r.ReservationCode));
            
            passengersReservatonList.Add(r.ReservationCode, r);

        }
        return passengersReservatonList;
    }

}