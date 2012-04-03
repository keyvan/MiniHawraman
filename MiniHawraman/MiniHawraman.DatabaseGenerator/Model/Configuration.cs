using System;

namespace MiniHawraman.DatabaseGenerator.Model
{
    public class Configuration
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Config { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
