using System.ComponentModel.DataAnnotations;

namespace Biograf.Models
{
 
    public class Salong
    {
        public int Id { get; set; }
        public int Salongsnummer { get; set; }
        public int AntalStolar { get; set; }

        // Navigation property
        public ICollection<Forestallning> Forestallningar { get; set; } = new List<Forestallning>();
    }
}
