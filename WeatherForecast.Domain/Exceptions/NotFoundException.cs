using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string username)
            : base($"User '{username}' already exists.") { }
    }

    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException()
            : base("Invalid username or password.") { }
    }

    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message) { }
    }


    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message = "Unauthorized access.")
            : base(message) { }
    }

}
