namespace Bookstore.Domain.AdminUser
{
    public class DbSecrets
    {
        public string? Host { get; set; }

        public int Port { get; set; }

        public string? Username { get; set; }

        public string? Password { get; set; }

        public string? DbInstanceIdentifier { get; set; }

        public string? Engine { get; set; }
    }
}