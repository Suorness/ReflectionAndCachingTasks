using System.Runtime.Serialization;

namespace ConsolePLCachTask
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Age { get; set; }

        public Person(string name, int year)
        {
            Name = name;
            Age = year;
        }
    }
}
