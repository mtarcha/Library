using System;
using System.Linq;

namespace Library.Application.Common
{
    public class RequestResult
    {
        public static RequestResult Success => new RequestResult(null);

        public RequestResult(Exception exception)
            : this(new AggregateException(exception))
        {
        }

        public RequestResult(AggregateException exceptions)
        {
            Exceptions = exceptions;
            HasErrors = exceptions?.InnerExceptions.Any() ?? false;
        }

        public bool HasErrors { get; }

        public AggregateException Exceptions { get; }
    }
}