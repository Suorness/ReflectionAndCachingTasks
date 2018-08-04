using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace ReflectionTask
{
    public class CompareHelper
    {
        public static bool Compare<T>(T objectA, T objectB)
        {
            if (ReferenceEquals(objectA, null))
            {
                return false;
            }

            if (ReferenceEquals(objectB, null))
            {
                return false;
            }

            if (objectA.GetType() != objectB.GetType())
            {
                return false;
            }

            bool equalsObject = true;
            object valueA;
            object valueB;

            var properties = objectA.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanRead);

            foreach (var property in properties)
            {
                valueA = property.GetValue(objectA);
                valueB = property.GetValue(objectB);
                Type type = property.PropertyType;
                
                if (IsComparableType(type) || IsPrimitiveType(type))
                {
                    equalsObject &= CompareValue(valueA, valueB);
                }
                else if (IsEnumerableType(type))
                {
                    equalsObject &= CompareEnumerable((IEnumerable)valueA, (IEnumerable)valueB);
                }
                else if (IsClass(property) || IsValueType(type))
                {
                    equalsObject &= Compare(valueA, valueB);
                }
            }

            return equalsObject;
        }

        private static bool CompareEnumerable(IEnumerable enumA, IEnumerable enumB)
        {
            if (ReferenceEquals(enumA, null))
            {
                return false;
            }

            if (ReferenceEquals(enumB, null))
            {
                return false;
            }

            var enumValueA = enumA.Cast<object>();
            var enumValueB = enumB.Cast<object>();

            if (enumValueA.Count() != enumValueB.Count())
            {
                return false;
            }

            bool equalsEnum = true;
            var enumeratorA = enumValueA.GetEnumerator();
            var enumeratorB = enumValueB.GetEnumerator();

            for (int i = 0; i < enumValueA.Count(); i++)
            {
                enumeratorA.MoveNext();
                enumeratorB.MoveNext();

                var type = enumeratorA.Current.GetType();

                if (IsComparableType(type) || IsPrimitiveType(type))
                {
                    equalsEnum &= CompareValue(enumeratorA.Current, enumeratorB.Current);
                }
                else
                {
                    equalsEnum &= Compare(enumeratorA.Current, enumeratorB.Current);
                }
                
            }

            return equalsEnum;
        }

        private static bool CompareValue(object valueA, object valueB)
        {
            IComparable valueComparer = valueA as IComparable;

            if (ReferenceEquals(valueComparer, null))
            {
                return false;
            }

            bool valueEqual = true;

            if (valueComparer.CompareTo(valueB) != 0)
            {
                valueEqual = false;
            }

            return valueEqual;
        }

        private static bool IsAssignableFrom(Type type, Type typeToAssignable)
        {
            return typeToAssignable.IsAssignableFrom(type);
        }

        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive;
        }

        private static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        private static bool IsEnumerableType(Type type)
        {
            return IsAssignableFrom(type, typeof(IEnumerable));
        }

        private static bool IsComparableType(Type type)
        {
            return IsAssignableFrom(type, typeof(IComparable));
        }

        private static bool IsClass(PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsClass;
        }
    }
}
