namespace trabalho_oop;

public class PassangerList
{
    
    List<Passanger> passangerList = new List<Passanger>();
    
    private List<string> passangerNames = new List<string>();
    private List<string> passangerSurnames = new List<string>();
    private FMS Fms;

    public PassangerList(FMS fms)
    {
        Fms = fms;
        passangerNames = Fms.GetPassangerNames();
        passangerSurnames = Fms.GetPassangerSurnames();
    }

    private string GenerateRandomPassangers()
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


    public List<Passanger> GeneratePassangerList(int passangerCount)
    {
        List<Passanger> passangerList = new List<Passanger>();
        for (int i = 0; i < passangerCount; i++)
        {
            Passanger p = new Passanger();
            string passangerName = GenerateRandomPassangers();
            if (passangerName != "")
            {
                p.Name = passangerName;
                passangerList.Add(p);
            }
            else
            {
                Console.WriteLine("Passanger name is empty.");
                return passangerList;
            }
        }
        return passangerList;
    }

}
