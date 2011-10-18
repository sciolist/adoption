using System;
using System.Runtime.Serialization;

namespace Adoption
{
    [Serializable]
    public class AdoptionException : Exception
    {
        public AdoptionException() { }
        public AdoptionException(string message) : base(message) { }
        public AdoptionException(string message, Exception inner) : base(message, inner) { }
        protected AdoptionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ParseException : AdoptionException
    {
        public ParseException() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception inner) : base(message, inner) { }
        protected ParseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class ValueRequiredException : AdoptionException
    {
        public ValueRequiredException(string parameterName) : this("Required value not supplied", parameterName) { }
        public ValueRequiredException(string message, string parameterName) : base(string.Format("{0}: {1}", message, parameterName)) { }
        public ValueRequiredException(string message, string parameterName, Exception inner) : base(string.Format("{0}: {1}", message, parameterName), inner) { }
        protected ValueRequiredException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
