namespace FiapCloudGames.Domain.Identity.ValueObjects
{
    public class Role
    {
        public static readonly Role User = new("User");
        public static readonly Role Admin = new("Admin");

        public string Value { get; }

        private Role(string value) => Value = value;
    }
}
