using CachTask;

namespace CachTaskTests
{
    [Cach(hour: 0, min: 0, sec: 2)]
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Person()
        {
        }

        public Person(string name, int year)
        {
            Name = name;
            Age = year;
        }
    }
}
