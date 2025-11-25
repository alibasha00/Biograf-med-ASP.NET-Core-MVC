using System.ComponentModel.DataAnnotations;

namespace Biograf.Models
{

    public class Bokning
    {
        public int Id { get; set; }
        public int ForestallningId { get; set; }
        public int Stolnummer { get; set; }
        public string Bokningsnummer { get; set; } = string.Empty;
        public string KundNamn { get; set; } = string.Empty;
        public string KundEmail { get; set; } = string.Empty;

        // Navigation property
        public Forestallning? Forestallning { get; set; }
    }
}
