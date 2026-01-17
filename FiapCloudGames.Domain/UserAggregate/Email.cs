using FiapCloudGames.Domain.Common;
using System.Text.RegularExpressions;

namespace FiapCloudGames.Domain.UserAggregate
{
    public class Email
    {
        public Email(string address)
        {
            if(string.IsNullOrWhiteSpace(address) )
            {
                throw new DomainException("Email cannot be empty!");
            }

            Address = address;
        }
        public string Address { get; private set; }


    }
}
