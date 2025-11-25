using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biograf.Data;

namespace Biograf.Controllers
{
    /// <summary>
    /// Controller för att hantera föreställningar
    /// </summary>
    public class ForestallningarController : Controller
    {
        private readonly BiografContext _context;

        public ForestallningarController(BiografContext context)
        {
            _context = context;
        }

        // GET: Forestallningar
        // Visar alla kommande föreställningar
        public async Task<IActionResult> Index()
        {
            var forestallningar = await _context.Forestallningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .Where(f => f.DatumTid > DateTime.Now)
                .OrderBy(f => f.DatumTid)
                .ToListAsync();

            return View(forestallningar);
        }
    }
}
