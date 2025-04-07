using Eventeaze.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eventeaze.Controllers
{
    public class VenueController : Controller
    {
        private readonly EventEazeDbContext _context;
        public VenueController(EventEazeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var venues = await _context.Venue.ToListAsync();
            return View(venues);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Venue venues)
        {


            if (ModelState.IsValid)
            {

                _context.Add(venues);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(venues);
        }
        public async Task<IActionResult> Details(int? id)
        {

            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueId == id);

            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var venue = await _context.Venue.FirstOrDefaultAsync(m => m.VenueId == id);


            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var venue = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(venue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CompanyExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            return View(venue);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Venue venue)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(venue.VenueId))
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

            return View(venue);
        }

    }

}
