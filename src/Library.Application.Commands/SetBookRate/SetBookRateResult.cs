using System;

namespace Library.Application.Commands.SetBookRate
{
    public class SetBookRateResult
    {
        public Guid Id { get; set; }
        
        public double Rate { get; set; }
    }
}