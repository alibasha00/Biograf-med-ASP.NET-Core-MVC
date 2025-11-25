using System.ComponentModel.DataAnnotations;

namespace Biograf.Models
{
    
    public class Forestallning
    {
        public int Id { get; set; }

        public int FilmId { get; set; }

        public int SalongId { get; set; }

        public DateTime DatumTid { get; set; }

        // Navigation properties
        public Film? Film { get; set; }
        public Salong? Salong { get; set; }
        public ICollection<Bokning> Bokningar { get; set; } = new List<Bokning>();
    }
}
