using CachTask;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace CachTaskTests
{
    [TestFixture]
    public class CachHelperTests
    {
        private Person _person;
        private string _key;

        public CachHelperTests()
        {
            _person = new Person("Vasya", 21);
            _key = "key";
        }
        [Test]
        public void SetAndGetValue()
        {
            CachHelper.SetInCach(_person, _key);
            var newPerson = CachHelper.GetFromCach<Person>(_key);

            Assert.AreEqual(_person.Age, newPerson.Age);
            Assert.AreEqual(_person.Name, newPerson.Name);
        }

        [Test]
        public async Task SetAndEndLifeTime()
        {
            CachHelper.SetInCach(_person, _key);

            //The lifetime is set to 2 seconds.
            await Task.Delay(new TimeSpan(0, 0, 3));
            var newPerson = CachHelper.GetFromCach<Person>(_key);

            Assert.IsNull(newPerson);

        }

        [Test]
        public void SetAndGetValueCustomSerealize()
        {
            CachHelper.SetInCachWithCustomSerilizable(_person, _key);
            var newPerson = CachHelper.GetFromCachWithCustomSerilizable<Person>(_key);

            Assert.AreEqual(_person.Age, newPerson.Age);
            Assert.AreEqual(_person.Name, newPerson.Name);
        }

        [Test]
        public async Task SetAndEndLifeTimeCustomSerealize()
        {
            CachHelper.SetInCachWithCustomSerilizable(_person, _key);

            //The lifetime is set to 2 seconds.
            await Task.Delay(new TimeSpan(0, 0, 3));
            var newPerson = CachHelper.GetFromCachWithCustomSerilizable<Person>(_key);

            Assert.IsNull(newPerson);

        }
    }
}
