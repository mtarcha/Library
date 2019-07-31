using System.Linq;
using System.Reflection;

namespace Library.Domain
{
    public abstract class ValueObject
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            return EqualsInternal(obj);
        }

        public override int GetHashCode()
        {
            const int startValue = 17;
            const int multiplier = 59;

            int hashCode = startValue;
            unchecked
            {
                var fields = GetType().GetRuntimeFields();
                foreach (FieldInfo field in fields)
                {
                    object value = field.GetValue(this);

                    if (value != null)
                        hashCode = hashCode * multiplier + value.GetHashCode();
                }
            }
           
            return hashCode;
        }

        public virtual bool EqualsInternal(object other)
        {
            if (other == null)
                return false;

            var fields = other.GetType().GetRuntimeFields().ToArray();
            foreach (var field in fields)
            {
                var value1 = field.GetValue(other);
                var value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (value2 == null)
                {
                    return false;
                }
                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }

        public static bool operator ==(ValueObject x, ValueObject y)
        {
            if (ReferenceEquals(x, null))
                return ReferenceEquals(y, null);

            return x.Equals(y);
        }

        public static bool operator !=(ValueObject x, ValueObject y)
        {
            return !(x == y);
        }
    }
}