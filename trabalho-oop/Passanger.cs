using System;

namespace trabalho_oop
{
    public class Passanger
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public Passanger(string name, int age) {
            Name = name;
            Age = age;
        }

        ~Passanger() { }
    }
}

