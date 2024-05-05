using CinemaManager_meriem.Models.Cinema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CinemaManager_meriem.Controllers
{
    public class ProducersController : Controller
    {
        CinemaContext _context;
        public ProducersController(CinemaContext context)
        {
            _context = context;
        }
        // GET: ProducersController
        public ActionResult Index()
        {
            
            return View(_context.Producers.ToList());
        }

        // GET: ProducersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProducersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProducersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Producer producer)
        {
            try
            {
                _context.Producers.Add(producer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult ProdsAndTheirMovies()
        {
            var cinemaDbContext = _context.Producers.Include(m => m.Movies);
            return View(cinemaDbContext);
        }

        public ActionResult ProdsAndTheirMovies_UsingModel()
        {
            var movies = from m in _context.Movies
                         join p in _context.Producers
                         on m.ProducerId equals p.Id
                         select new ProdMovie
                         {
                             mTitle = m.Title,
                             mGenre = m.Genre,
                             pName = p.Name,
                             pNat = p.Nationality,
                         };
            return View(movies);
        }

        // GET: ProducersController/Edit/5
        public ActionResult Edit(int id)
        {
            var producer = _context.Producers.Find(id);
            if (producer == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: ProducersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Producer producer)
        {
            if (id != producer.Id)
            {
                return BadRequest();
            }
            try
            {
                _context.Producers.Update(producer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProducersController/Delete/5
        public ActionResult Delete(int id)
        {
            var producer = _context.Producers.Find(id);
            if (producer == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: ProducersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Producer producer)
        {
            if (id != producer.Id)
            {
                return BadRequest();
            }
            try
            {
                _context.Producers.Remove(producer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult MyMovies(int id)
        {
            var movies = from m in _context.Movies
                         join p in _context.Producers
                         on m.ProducerId equals p.Id
                         where p.Id == id
                         select new ProdMovie
                         {
                             mTitle = m.Title,
                             mGenre = m.Genre,
                             pName = p.Name,
                             pNat = p.Nationality,
                         };

            return View(movies);
        }
    }
}
