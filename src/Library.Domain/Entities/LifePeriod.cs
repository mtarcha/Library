using System;
using Library.Domain.Common;

namespace Library.Domain.Entities
{
    public class LifePeriod : ValueObject
    {
        public LifePeriod(DateTime dateOfBirth)
            : this(dateOfBirth, null)
        {
        }

        public LifePeriod(DateTime dateOfBirth, DateTime? dateOfDeath)
        {
            if (dateOfBirth.Date > DateTime.Today.Date)
            {
                throw new ArgumentException("Let the child enjoy the childhood.");
            }

            if (dateOfDeath.HasValue && dateOfBirth > dateOfDeath.Value)
            {
                throw new ArgumentException("Date of birth must be less than date of death.");
            }

            DateOfBirth = dateOfBirth;
            DateOfDeath = dateOfDeath;
        }

        public DateTime DateOfBirth { get; }

        public DateTime? DateOfDeath { get; }

        public bool IsAlive()
        {
            return !DateOfDeath.HasValue;
        }
    }
}