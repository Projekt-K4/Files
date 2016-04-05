using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Projekt_K4
{
    class Person
    {
        private int time_to_live = 600; //Is the time the person has left to live
        public Person()
        {
            Console.WriteLine("Person with default life expectancy(600) created");
        }
        public Person(int assign_time) //Overloaded Constructor
        {
            time_to_live = assign_time;
            Console.WriteLine("Person with life expectancy:" + time_to_live + "created");
        }


    }

    class Person_gen
    {
        public Random global_time = new Random();
        private List<Person> person_list = new List<Person>();
        public Person_gen()
        {
            Console.WriteLine("Catastrophe engages");
        }
        public Person_gen(int number_of_causualities)
        {
            generate_causualities(number_of_causualities);
            Console.WriteLine(number_of_causualities + "involved in catastrophe");
        }
        private void generate_causualities(int number_of_persons)
        {
            for (int i = 0; i < number_of_persons; i++)
            {
                add_person(person_list, get_random_time());
                //person_list.Add(new Person(get_random_time()));
            }
        }

        public int get_random_time()
        {

            int random_time = global_time.Next(1, 400);
            return random_time;
        }
        public bool add_person(List<Person> plist, int random_time)
        {
            plist.Add(new Person(random_time));
            return true;
        }
    }

    class Program
    {
        static void Main()
        {
            Person person1 = new Person(400);

            Person_gen catastrophe = new Person_gen(10);
        }
    }

}
