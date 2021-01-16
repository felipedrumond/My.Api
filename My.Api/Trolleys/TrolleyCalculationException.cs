using System;

namespace My.Api.Trolleys
{
    public class TrolleyCalculationException : Exception
    {
        public TrolleyCalculationException() : base("Trolley failed to calculate its total.") { }
    }
}