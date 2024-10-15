using System;

namespace trabalho_oop
{
    public class Airplane
    {
        public string Company {  get; set; }

        public string Registration { get; set; }

        public bool isOccupied = false;

        public Airplane(string company, string registration)
        {
            Company = company;
            Registration = registration;
        }

        public void ChangeOccupiedStatus()
        {
            isOccupied = !isOccupied;
        }

        ~Airplane() { }
    }

}
