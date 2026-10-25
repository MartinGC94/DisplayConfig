using System;

namespace MartinGC94.DisplayConfig.API
{
    [Serializable]
    public sealed class ApiException : Exception
    {
        public ApiException() { }
        public ApiException(string message) : base(message) { }
        public ApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}