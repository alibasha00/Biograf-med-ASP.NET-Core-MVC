using Biograf.Models;
using Microsoft.EntityFrameworkCore;

namespace Biograf.Data
{
    /// <summary>
    /// Seeder-klass för att fylla databasen med startdata
    /// </summary>
    public static class DbSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BiografContext(
                serviceProvider.GetRequiredService<DbContextOptions<BiografContext>>()))
            {
                // Skapa databasen om den inte finns
                context.Database.EnsureCreated();

                // Om det redan finns data, avsluta
                if (context.Filmer.Any())
                {
                    return;
                }

                // Lägg till filmer
                var filmer = new Film[]
                {
                    new Film
                    {
                        Titel = "Inception",
                        Genre = "Science Fiction",
                        Beskrivning = "En tjuv som stjäl företagshemligheter genom att använda teknologi för att dela drömmar erbjuds en chans att få sitt gamla liv tillbaka som betalning för att plantera en idé i en företagsledares undermedvetna.",
                        Langd = 148,
                       Pris = 120
                    },
                    new Film
                    {
                        Titel = "The Shawshank Redemption",
                        Genre = "Drama",
                        Beskrivning = "Två fängslade män skapar en stark vänskap över flera år och finner tröst och slutlig återlösning genom handling av vardaglig vänlighet.",
                        Langd = 142,
                        Pris = 110
                    },
                    new Film
                    {
                        Titel = "The Dark Knight",
                        Genre = "Action",
                        Beskrivning = "När hotet känt som Jokern föder kaos och förödelse över folket i Gotham, måste den maskerade vakten Batman acceptera ett av de största psykologiska och fysiska testerna.",
                        Langd = 152,
                        Pris = 130
                    },
                    new Film
                    {
                        Titel = "Forrest Gump",
                        Genre = "Drama",
                        Beskrivning = "Decennierna av livet för Forrest Gump, en naiv man från Alabama som bevittnar och omedvetet påverkar flera avgörande historiska händelser i 1900-talets USA.",
                        Langd = 142,
                        Pris = 110
                    },
                    new Film
                    {
                        Titel = "Pulp Fiction",
                        Genre = "Thriller",
                        Beskrivning = "Livets vägar för två gangsters, en boxare, en gangsterhustru och två restauranggrånare sammanflätas i fyra berättelser om våld och återlösning.",
                        Langd = 154,
                        Pris = 115
                    },
                    new Film
                    {
                        Titel = "Interstellar",
                        Genre = "Science Fiction",
                        Beskrivning = "En grupp utforskare använder en nyupptäckt passage för att överstiga begränsningarna för mänsklig rymdfärd och erövra de enorma avstånden som är inblandade i en interstellär resa.",
                        Langd = 169,
                        Pris = 125
                    }
                };
                context.Filmer.AddRange(filmer);
                context.SaveChanges();

                // Lägg till salonger
                var salonger = new Salong[]
                {
                    new Salong { Salongsnummer = 1, AntalStolar = 50 },
                    new Salong { Salongsnummer = 2, AntalStolar = 80 },
                    new Salong { Salongsnummer = 3, AntalStolar = 120 },
                    new Salong { Salongsnummer = 4, AntalStolar = 40 }
                };
                context.Salonger.AddRange(salonger);
                context.SaveChanges();

                // Lägg till föreställningar (olika filmer, olika salonger, olika tider)
                // Använd fast datum i januari 2026 för att säkerställa att föreställningarna inte har passerat
                var startDatum = new DateTime(2026, 1, 5); // Börja 5 januari 2026
                var forestallningar = new List<Forestallning>();

                // Inception - Salong 1 (flera visningar)
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[0].Id,
                    SalongId = salonger[0].Id,
                    DatumTid = startDatum.AddDays(1).AddHours(18) // 6 jan 2026, 18:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[0].Id,
                    SalongId = salonger[0].Id,
                    DatumTid = startDatum.AddDays(2).AddHours(21) // 7 jan 2026, 21:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[0].Id,
                    SalongId = salonger[0].Id,
                    DatumTid = startDatum.AddDays(3).AddHours(16).AddMinutes(30) // 8 jan 2026, 16:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[0].Id,
                    SalongId = salonger[0].Id,
                    DatumTid = startDatum.AddDays(5).AddHours(19).AddMinutes(15) // 10 jan 2026, 19:15
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[0].Id,
                    SalongId = salonger[0].Id,
                    DatumTid = startDatum.AddDays(7).AddHours(20).AddMinutes(30) // 12 jan 2026, 20:30
                });

                // The Shawshank Redemption - Salong 2 (flera visningar)
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[1].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(1).AddHours(19).AddMinutes(30) // 6 jan 2026, 19:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[1].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(3).AddHours(17) // 8 jan 2026, 17:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[1].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(4).AddHours(15).AddMinutes(45) // 9 jan 2026, 15:45
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[1].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(6).AddHours(18).AddMinutes(30) // 11 jan 2026, 18:30
                });

                // The Dark Knight - Salong 3 (flera visningar)
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[2].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(1).AddHours(20) // 6 jan 2026, 20:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[2].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(2).AddHours(17).AddMinutes(30) // 7 jan 2026, 17:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[2].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(4).AddHours(19) // 9 jan 2026, 19:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[2].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(5).AddHours(21).AddMinutes(15) // 10 jan 2026, 21:15
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[2].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(7).AddHours(18) // 12 jan 2026, 18:00
                });

                // Forrest Gump - Salong 4 (flera visningar)
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[3].Id,
                    SalongId = salonger[3].Id,
                    DatumTid = startDatum.AddDays(2).AddHours(18).AddMinutes(30) // 7 jan 2026, 18:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[3].Id,
                    SalongId = salonger[3].Id,
                    DatumTid = startDatum.AddDays(3).AddHours(20) // 8 jan 2026, 20:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[3].Id,
                    SalongId = salonger[3].Id,
                    DatumTid = startDatum.AddDays(6).AddHours(16).AddMinutes(45) // 11 jan 2026, 16:45
                });

                // Pulp Fiction - Salong 2 (flera visningar)
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[4].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(2).AddHours(20).AddMinutes(30) // 7 jan 2026, 20:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[4].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(4).AddHours(21).AddMinutes(30) // 9 jan 2026, 21:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[4].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(5).AddHours(19).AddMinutes(45) // 10 jan 2026, 19:45
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[4].Id,
                    SalongId = salonger[1].Id,
                    DatumTid = startDatum.AddDays(7).AddHours(17).AddMinutes(30) // 12 jan 2026, 17:30
                });

                // Interstellar - Salong 3 (flera visningar)
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[5].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(1).AddHours(16) // 6 jan 2026, 16:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[5].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(3).AddHours(18).AddMinutes(30) // 8 jan 2026, 18:30
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[5].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(5).AddHours(20) // 10 jan 2026, 20:00
                });
                forestallningar.Add(new Forestallning
                {
                    FilmId = filmer[5].Id,
                    SalongId = salonger[2].Id,
                    DatumTid = startDatum.AddDays(6).AddHours(21).AddMinutes(30) // 11 jan 2026, 21:30
                });

                context.Forestallningar.AddRange(forestallningar);
                context.SaveChanges();

                // Lägg till några testbokningar
                var bokningar = new Bokning[]
                {
                    new Bokning
                    {
                        ForestallningId = forestallningar[0].Id,
                        Stolnummer = 15,
                        Bokningsnummer = GenerateBokningsnummer(),
                        KundNamn = "Anna Andersson",
                        KundEmail = "anna@example.com"
                    },
                    new Bokning
                    {
                        ForestallningId = forestallningar[0].Id,
                        Stolnummer = 16,
                        Bokningsnummer = GenerateBokningsnummer(),
                        KundNamn = "Erik Eriksson",
                        KundEmail = "erik@example.com"
                    },
                    new Bokning
                    {
                        ForestallningId = forestallningar[2].Id,
                        Stolnummer = 25,
                        Bokningsnummer = GenerateBokningsnummer(),
                        KundNamn = "Maria Svensson",
                        KundEmail = "maria@example.com"
                    }
                };
                context.Bokningar.AddRange(bokningar);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Genererar ett unikt bokningsnummer
        /// </summary>
        private static string GenerateBokningsnummer()
        {
            return $"BK{DateTime.Now.Ticks.ToString().Substring(8)}";
        }
    }
}
