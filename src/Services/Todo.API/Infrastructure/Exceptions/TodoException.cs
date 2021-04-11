namespace Todo.API.Infrastructure.Exceptions
{
    using System;

    /// <summary>
    /// Exception type for todo exceptions
    /// </summary>
    public class TodoException : Exception
    {
        public TodoException() { }
        public TodoException(string message) : base(message) { }
        public TodoException(string message, Exception innerException) : base(message, innerException) { }
    }
}
