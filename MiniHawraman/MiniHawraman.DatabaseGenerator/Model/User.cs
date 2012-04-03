using System;

namespace MiniHawraman.DatabaseGenerator.Model
{
    public class User
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string VerificationCode { get; set; }

        public string Biography { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
