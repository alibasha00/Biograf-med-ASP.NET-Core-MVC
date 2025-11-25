using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biograf.Data;

namespace Biograf.Controllers
{
    /// <summary>
    /// Controller för att hantera filmvisning
    /// </summary>
    public class FilmerController : Controller
    {
        private readonly BiografContext _context;

        public FilmerController(BiografContext context)
        {
            _context = context;
        }

        // Visar alla filmer
        public async Task<IActionResult> Index()
        {
            var filmer = await _context.Filmer.ToListAsync();
            return View(filmer);
        }

        
        // Visar detaljer för en specifik film samt kommande föreställningar
        public async Task<IActionResult> Detaljer(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var film = await _context.Filmer
                .Include(f => f.Forestallningar)
                    .ThenInclude(fo => fo.Salong)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (film == null)
            {
                return NotFound();
            }

            // Filtrera kommande föreställningar
            ViewBag.KommandeForestallningar = film.Forestallningar
                .Where(f => f.DatumTid > DateTime.Now)
                .OrderBy(f => f.DatumTid)
                .ToList();

            return View(film);
        }
    }
}
