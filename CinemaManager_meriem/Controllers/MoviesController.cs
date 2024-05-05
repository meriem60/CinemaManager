using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CinemaManager_meriem.Models.Cinema;

namespace CinemaManager_meriem.Controllers
{
    public class MoviesController : Controller
    {
        private readonly CinemaContext _context;

        public MoviesController(CinemaContext context)
        {
            _context = context;
        }
        
        public IActionResult MoviesAndTheirProdds(CinemaContext context)
        {
            var moviesWithProducers = _context.Movies.Include(m => m.Producer).ToList();

            return View(moviesWithProducers);
        }
        public ActionResult MoviesAndTheirProds_UsingModel()
        {
            var movies = from m in _context.Movies
                         join p in _context.Producers
                         on m.ProducerId equals p.Id
                         select new ProdMovie
                         {
                             mTitle = m.Title,
                             pNat = p.Nationality,
                             mGenre = m.Genre,
                             pName = p.Name
                         };

            return View(movies);
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Movies.Include(m => m.Producer);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Genre,ProducerId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "Id", movie.ProducerId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        public ActionResult SearchByTitle(string titre = "")

        {

            var moviesbytitle = from m in _context.Movies.Include(n => n.Producer)

                                where m.Title.Contains(titre)

                                select m;

            return View(moviesbytitle);

        }
        public ActionResult SearchByGenre(string Genre = "")

        {

            var moviesbygenre = from m in _context.Movies.Include(n => n.Producer)

                                where m.Genre.Contains(Genre)

                                select m;

            return View(moviesbygenre);

        }

        public ActionResult SearchBy2(string titre = "", string genre = "")
        {

            var Genres = (from m in _context.Movies
                          select m.Genre).Distinct();
            ViewBag.genre = new SelectList(Genres);


            if (titre != null && genre != null)
            {
                var titre_genre = from m in _context.Movies.Include(n => n.Producer)
                                  where m.Genre.Contains(genre) && m.Title.Contains(titre)
                                  select m;


            }
            else if (titre == null && genre == null)
            {

                var titre_genre = from m in _context.Movies.Include(n => n.Producer) select m;

            }
            else if (titre == null)
            {
                var titre_genre = from m in _context.Movies.Include(n => n.Producer)
                                  where m.Genre.Contains(genre)
                                  select m;
                ViewBag.genre = new SelectList(Genres);


            }
            else if (genre == null)
            {
                var titre_genre = from m in _context.Movies
                                  where m.Genre.Contains(titre)
                                  select m;



            }


            return View(movies);
        }
    }
}
