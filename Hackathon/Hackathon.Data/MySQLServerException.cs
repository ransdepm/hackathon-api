using System;

namespace Hackathon.Data.Exceptions
{
    public class MySQLServerException : Exception
    {
        public MySQLServerException() : base()
        {

        }

        public MySQLServerException(string message)
        : base(message)
        {

        }
    }
}
