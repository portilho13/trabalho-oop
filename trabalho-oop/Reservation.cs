namespace trabalho_oop;

public class Reservation
{
    public FMS Fms { private get; set; }
    
    public string ReservationCode { get; private set; }
    public Person Passanger {get; set;}
    
    public Reservation() {}
    
    public void GenerateReservationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var reservationCode = new char[6];

        for (int i = 0; i < reservationCode.Length; i++)
        {
            reservationCode[i] = chars[random.Next(chars.Length)];
        }
        
        this.ReservationCode = new string(reservationCode);
    }
    
    ~Reservation() {}
}