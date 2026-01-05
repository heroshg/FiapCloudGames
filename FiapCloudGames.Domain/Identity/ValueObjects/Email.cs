using FiapCloudGames.Domain.Common;
using System.Text.RegularExpressions;

namespace FiapCloudGames.Domain.Identity.ValueObjects
{
    public class Email
    {
        public Email(string address)
        {
            if(string.IsNullOrWhiteSpace(address) )
            {
                throw new DomainException("Email cannot be empty!");
            }

            if(!IsValid(address))
            {
                throw new DomainException("Email format is invalid!");
            }
            Address = address;
        }
        public string Address { get; private set; }


        private static bool IsValid(string address)
        {
            return Regex.IsMatch(address, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
