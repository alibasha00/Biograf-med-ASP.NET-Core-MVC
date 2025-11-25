using System.ComponentModel.DataAnnotations;

namespace Biograf.Models
{
 
    public class Film
    {
        public int Id { get; set; }
        public string Titel { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Beskrivning { get; set; } = string.Empty;
        public int Langd { get; set; } // i minuter
        public decimal Pris { get; set; }

        // Navigation property
        public ICollection<Forestallning> Forestallningar { get; set; } = new List<Forestallning>();
    }
}
