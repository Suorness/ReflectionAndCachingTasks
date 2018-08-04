using NUnit.Framework;
using ReflectionTask;

namespace ReflactionTaskTests
{
    [TestFixture]
    public class CompareHelpersTests
    {
        private Product productA;

        public CompareHelpersTests()
        {

        }

        [Test]
        public void StartTest()
        {
            object objA = 7;
            object objB = 7;

            var result = CompareHelper.Compare(objA, objB);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void CompareNull_Test()
        {
            var result = CompareHelper.Compare(null, new object());
            var result2 = CompareHelper.Compare(new object(), null);

            Assert.AreEqual(false, result);
            Assert.AreEqual(false, result2);
        }

        [Test]
        public void Compare_TypeTest()
        {
            var result = CompareHelper.Compare(new A(), new B());

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Compare_EqualObjectWithComparebleAndPrimitiveType()
        {
            var valueA = new B()
            {
                Name = "Name",
                Number = 42
            };

            var valueB = new B()
            {
                Name = "Name",
                Number = 42
            };

            var result = CompareHelper.Compare(valueA, valueB);

            Assert.AreEqual(true, result);
        }

        [Test]
        public void Compare_NotEqualObjectWithComparebleAndPrimitiveType()
        {
            var valueA = new B()
            {
                Name = "Name",
                Number = 42
            };

            var valueB = new B()
            {
                Name = "Name",
                Number = 43
            };

            var result = CompareHelper.Compare(valueA, valueB);
            Assert.AreEqual(false, result);

            var valueC = new B()
            {
                Name = "NameC",
                Number = 43
            };

            result = CompareHelper.Compare(valueC, valueB);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Compare_EqualsObjectWithValueTypeAndClasses()
        {
            var productA = new Product()
            {
                Id = 1,
                Name = "Name",
                Category = new Category()
                {
                    Id = 1,
                    Name = "CategoryName"
                },
                Company = new Company()
                {
                    Id = 1
                }
            };

            var productB = new Product()
            {
                Id = 1,
                Name = "Name",
                Category = new Category()
                {
                    Id = 1,
                    Name = "CategoryName"
                },
                Company = new Company()
                {
                    Id = 1
                }
            };

            var result = CompareHelper.Compare(productA, productB);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Compare_NotEqualsObjectWithValueTypeAndClasses()
        {
            var productA = new Product()
            {
                Id = 1,
                Name = "Name",
                Category = new Category()
                {
                    Id = 1,
                    Name = "CategoryName"
                },
                Company = new Company()
                {
                    Id = 1
                }
            };

            var productB = new Product()
            {
                Id = 1,
                Name = "Name",
                Category = new Category()
                {
                    Id = 1,
                    Name = "CategoryName"
                },
                Company = new Company()
                {
                    Id = 2
                }
            };

            var result = CompareHelper.Compare(productA, productB);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Compare_EnumerableItem()
        {
            var listA = new CustomList()
            {
                Array = new int[] { 1, 2 }
            };

            var listB = new CustomList()
            {
                Array = new int[] { 1, 3 }
            };

            var result = CompareHelper.Compare(listA, listB);
            Assert.AreEqual(false, result);
        }
    }

    internal class A
    {
        public int Number { get; set; }
    }

    internal class B : A
    {
        public string Name { get; set; }
    }

    internal class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public Company Company { get; set; }
    }

    internal class Company
    {
        public int Id { get; set; }
    }

    internal class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    internal struct C
    {
        public int Id { get; set; }
    }

    internal class CustomList
    {
        public int[] Array { get; set; }
    }
}
