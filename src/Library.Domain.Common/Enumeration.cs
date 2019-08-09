using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Library.Domain.Common
{
    public abstract class Enumeration<T> : ValueObject, IComparable where T : Enumeration<T>
    {
        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }

        public int Id { get; private set; }


        public override string ToString() => Name;

        public static IEnumerable<T> GetAll()
        {
            var fields = typeof(T).GetRuntimeFields();

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration<T>;

            if (otherValue == null)
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration<T> firstValue, Enumeration<T> secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromValue(int value) 
        {
            var matchingItem = Parse<int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        public static T FromName(string name) 
        {
            var matchingItem = Parse<string>(name, "display name", item => item.Name == name);
            return matchingItem;
        }

        private static T Parse<K>(K value, string description, Func<T, bool> predicate)
        {
            var matchingItem = GetAll().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }

        public int CompareTo(object other) => Id.CompareTo(((Enumeration<T>)other).Id);
    }
}