namespace trabalho_oop;

public class Passanger: Person
{
    public string password;
    
    public Dictionary<string, Reservation> reservations { get; set; } = new Dictionary<string, Reservation>(); //Ensure to create reservation Hashmap to store real users reservations

    private bool DoesReservationExists(string reservationCode) => reservations.ContainsKey(reservationCode);
    public void AddReservation(Reservation reservation)
    {
        if (!DoesReservationExists(reservation.ReservationCode))
            reservations.Add(reservation.ReservationCode, reservation);
    }
    public Passanger() {}
    ~Passanger() {}
}