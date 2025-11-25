using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biograf.Data;
using Biograf.Models;

namespace Biograf.Controllers
{
    /// <summary>
    /// Controller för att hantera bokningar
    /// </summary>
    public class BokningarController : Controller
    {
        private readonly BiografContext _context;

        public BokningarController(BiografContext context)
        {
            _context = context;
        }

        
        // Visar lediga och upptagna platser för en föreställning
        public async Task<IActionResult> ValjPlats(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forestallning = await _context.Forestallningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .Include(f => f.Bokningar)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (forestallning == null)
            {
                return NotFound();
            }

            // Skapa lista med information om varje stol
            var stolar = new List<dynamic>();
            var bokatdeStolar = forestallning.Bokningar.Select(b => b.Stolnummer).ToList();

            for (int i = 1; i <= forestallning.Salong.AntalStolar; i++)
            {
                stolar.Add(new
                {
                    Nummer = i,
                    Bokad = bokatdeStolar.Contains(i)
                });
            }

            ViewBag.Stolar = stolar;
            return View(forestallning);
        }

        // Visar bokningsformulär för vald plats
        public async Task<IActionResult> BokaPlatsen(int forestallningId, int stolnummer)
        {
            var forestallning = await _context.Forestallningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .Include(f => f.Bokningar)
                .FirstOrDefaultAsync(f => f.Id == forestallningId);

            if (forestallning == null)
            {
                return NotFound();
            }

            // Kontrollera att stolnumret är giltigt
            if (stolnummer < 1 || stolnummer > forestallning.Salong.AntalStolar)
            {
                ModelState.AddModelError("", "Ogiltigt stolnummer.");
                return RedirectToAction(nameof(ValjPlats), new { id = forestallningId });
            }

            // Kontrollera att stolen inte redan är bokad
            if (forestallning.Bokningar.Any(b => b.Stolnummer == stolnummer))
            {
                ModelState.AddModelError("", "Denna plats är redan bokad.");
                return RedirectToAction(nameof(ValjPlats), new { id = forestallningId });
            }

            // Skapa ny bokning för formuläret
            var bokning = new Bokning
            {
                ForestallningId = forestallningId,
                Stolnummer = stolnummer
            };

            ViewBag.Forestallning = forestallning;
            return View(bokning);
        }

        // POST: Bokningar/BokaPlatsen
        // Sparar bokningen i databasen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BokaPlatsen([Bind("ForestallningId,Stolnummer,KundNamn,KundEmail")] Bokning bokning)
        {
            // Hämta föreställningen med relaterad data
            var forestallning = await _context.Forestallningar
                .Include(f => f.Film)
                .Include(f => f.Salong)
                .Include(f => f.Bokningar)
                .FirstOrDefaultAsync(f => f.Id == bokning.ForestallningId);

            if (forestallning == null)
            {
                return NotFound();
            }

            // Validering av indata
            if (string.IsNullOrWhiteSpace(bokning.KundNamn))
            {
                ModelState.AddModelError("KundNamn", "Kundens namn är obligatoriskt.");
            }
            else if (bokning.KundNamn.Length > 200)
            {
                ModelState.AddModelError("KundNamn", "Namn får vara max 200 tecken.");
            }

            if (string.IsNullOrWhiteSpace(bokning.KundEmail))
            {
                ModelState.AddModelError("KundEmail", "E-postadress är obligatorisk.");
            }
            else if (bokning.KundEmail.Length > 200)
            {
                ModelState.AddModelError("KundEmail", "E-postadress får vara max 200 tecken.");
            }
            else if (!IsValidEmail(bokning.KundEmail))
            {
                ModelState.AddModelError("KundEmail", "Ogiltig e-postadress.");
            }

            // Validera stolnummer
            if (bokning.Stolnummer < 1 || bokning.Stolnummer > forestallning.Salong.AntalStolar)
            {
                ModelState.AddModelError("Stolnummer", "Ogiltigt stolnummer för denna salong.");
            }

            if (ModelState.IsValid)
            {
                // Kontrollera igen att stolen inte är bokad (för att förhindra race condition)
                var existingBokning = await _context.Bokningar
                    .FirstOrDefaultAsync(b => b.ForestallningId == bokning.ForestallningId && 
                                            b.Stolnummer == bokning.Stolnummer);

                if (existingBokning != null)
                {
                    ModelState.AddModelError("", "Denna plats har redan blivit bokad av någon annan. Vänligen välj en annan plats.");
                    ViewBag.Forestallning = forestallning;
                    return View(bokning);
                }

                // Generera unikt bokningsnummer
                bokning.Bokningsnummer = GenerateBokningsnummer();

                try
                {
                    _context.Add(bokning);
                    await _context.SaveChangesAsync();
                    
                    // Redirecta till bekräftelsesida
                    return RedirectToAction(nameof(Bekraftelse), new { id = bokning.Id });
                }
                catch (DbUpdateException)
                {
                    // Om det uppstår en databaskonflikt (t.ex. dubbelbokning)
                    ModelState.AddModelError("", "Ett fel uppstod vid bokningen. Platsen kan redan vara bokad. Försök igen.");
                    ViewBag.Forestallning = forestallning;
                    return View(bokning);
                }
            }

            ViewBag.Forestallning = forestallning;
            return View(bokning);
        }

        
        // Visar bekräftelse av bokning
        public async Task<IActionResult> Bekraftelse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bokning = await _context.Bokningar
                .Include(b => b.Forestallning)
                    .ThenInclude(f => f.Film)
                .Include(b => b.Forestallning)
                    .ThenInclude(f => f.Salong)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bokning == null)
            {
                return NotFound();
            }

            return View(bokning);
        }

        // Visar formulär för att söka bokning
        public IActionResult SokBokning()
        {
            return View();
        }

        // POST: Bokningar/SokBokning
        // Söker efter bokning med bokningsnummer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SokBokning(string bokningsnummer)
        {
            if (string.IsNullOrWhiteSpace(bokningsnummer))
            {
                ModelState.AddModelError("", "Vänligen ange ett bokningsnummer.");
                return View();
            }

            var bokning = await _context.Bokningar
                .Include(b => b.Forestallning)
                    .ThenInclude(f => f.Film)
                .Include(b => b.Forestallning)
                    .ThenInclude(f => f.Salong)
                .FirstOrDefaultAsync(b => b.Bokningsnummer == bokningsnummer.Trim());

            if (bokning == null)
            {
                ModelState.AddModelError("", "Ingen bokning hittades med detta bokningsnummer.");
                return View();
            }

            return View("VisaBokning", bokning);
        }

        /// Genererar ett unikt bokningsnummer
        private string GenerateBokningsnummer()
        {
            return $"BK{DateTime.Now.Ticks.ToString().Substring(8)}";
        }

        /// Validerar e-postadress
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
