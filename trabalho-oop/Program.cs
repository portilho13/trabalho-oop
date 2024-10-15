using System;

namespace trabalho_oop
{
    public class Program
    {
        static void Main(string[] args)
        {
            Airplane ryanair = new Airplane("Ryanair", "9H-GSF");
            Console.WriteLine(ryanair.Registration);
        }
    }
};

