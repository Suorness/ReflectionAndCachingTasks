using NUnit.Framework;
using ReflectionTask;
using System;
using System.Collections.Generic;

namespace ReflactionTaskTests
{
    /// <summary>
    /// Attention, this class demonstrates the work of the class <see cref="CompareHelper"/>.
    /// These methods are not tests.
    /// </summary>
    [TestFixture]
    public class CompareHelpersTests
    {
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
                Array = new Category[1]
            };

            listA.Array[0] = new Category()
            {
                Id = 1,
                Name = "Name"
            };

            var listB = new CustomList()
            {
                Array = new Category[1]
            };
            listB.Array[0] = new Category()
            {
                Id = 1,
                Name = "Name"
            };

            var result = CompareHelper.Compare(listA, listB);
            Assert.AreEqual(true, result);
        }

        [Test]
        public void Compare_WithAllMember()
        {
            var time = DateTime.Now;
            var c = new C()
            {
                Id = 100
            };

            var product = new Product()
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


            var categoryArray = new Category[1];
            categoryArray[0] = new Category()
            {
                Id = 1,
                Name = "Name"
            };

            var compereA = new CompererExampleClass()
            {
                Id = 1,
                CProperty = c,
                Product = product,
                Array = categoryArray,
                DateTime = time,
                IntArray = new int[] { 1, 2 }
            };

            var compereB = new CompererExampleClass()
            {
                Id = 1,
                CProperty = c,
                Product = product,
                Array = categoryArray,
                DateTime = time,
                IntArray = new int[] { 1, 2 }
            };

            var result = CompareHelper.Compare(compereA, compereB);
            Assert.AreEqual(true, result);

            //Change each field.
            compereA.Id = 2;
            result = CompareHelper.Compare(compereA, compereB);
            Assert.AreEqual(false, result);

            compereB.Id = 2;
            compereA.CProperty = new C()
            {
                Id = 101
            };
            result = CompareHelper.Compare(compereA, compereB);
            Assert.AreEqual(false, result);

            compereA.CProperty = c;
            compereA.DateTime = DateTime.Now;
            result = CompareHelper.Compare(compereA, compereB);
            Assert.AreEqual(false, result);

            compereA.DateTime = time;
            compereA.Product = new Product();
            result = CompareHelper.Compare(compereA, compereB);
            Assert.AreEqual(false, result);

            compereA.Product = product;
            compereA.IntArray = new int[] { 1, 4 };
            result = CompareHelper.Compare(compereA, compereB);
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
        public Category[] Array { get; set; }
    }

    internal class CompererExampleClass
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public C CProperty { get; set; }

        public Product Product { get; set; }

        public Category[] Array { get; set; }

        public int[] IntArray { get; set; }
    }
}
