using System;
using System.Runtime.Serialization;

namespace AppStrap
{
    public sealed class AppStrapException : Exception
    {
        public string Name { get; private set; }
        public AppStrapException(string domainName, string message)
            : base(message)
        {
            Name = domainName;
        }

        public AppStrapException(string domainName, string message, Exception innerException)
            : base(message, innerException)
        {
            Name = domainName;
        }

        private AppStrapException(string domainName, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Name = domainName;
        }
    }
}
