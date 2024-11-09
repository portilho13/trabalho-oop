namespace trabalho_oop;

public class PassangerList
{
    private List<string> passangerNames = new List<string>();
    private List<string> passangerSurnames = new List<string>();
    private FMS Fms;

    public PassangerList(FMS fms)
    {
        Fms = fms;
        passangerNames = Fms.GetPassangerNames();
        passangerSurnames = Fms.GetPassangerSurnames();
    }

    private string GenerateRandomPassanger()
    {
        // Check that both lists contain elements before proceeding
        if (passangerNames.Count == 0 || passangerSurnames.Count == 0)
        {
            Console.WriteLine("Passenger names or surnames list is empty.");
            return "";
        }

        // Create a single Random instance
        Random random = new Random();

        // Get random indices
        int randomNameIndex = random.Next(0, passangerNames.Count);
        int randomSurnameIndex = random.Next(0, passangerSurnames.Count);

        // Retrieve the random name and surname
        string passangerName = passangerNames[randomNameIndex];
        string passangerSurname = passangerSurnames[randomSurnameIndex];

        // Concatenate with a space in between
        string passanger = passangerName + " " + passangerSurname;

        // Output the result
        return passanger;
    }
    


    public Dictionary<string, Reservation> GeneratePassangerList(int passangerCount)
    {
        Dictionary<string, Reservation> passangersReservatonList = new Dictionary<string, Reservation>();
        for (int i = 0; i < passangerCount; i++)
        {
            Reservation r = new Reservation();
            Passanger p = new Passanger();
            string passangerName = GenerateRandomPassanger();
            p.Name = passangerName;
            
            r.Passanger = p;
            do
            {
                r.GenerateReservationCode();
            } while(passangersReservatonList.ContainsKey(r.ReservationCode));
            
            passangersReservatonList.Add(r.ReservationCode, r);

        }
        return passangersReservatonList;
    }

}
