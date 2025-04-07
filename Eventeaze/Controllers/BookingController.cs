using Eventeaze.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Eventeaze.Controllers
{
    public class BookingController : Controller
    {
        private readonly EventEazeDbContext _context;
        public BookingController(EventEazeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()

        {
            var book = await _context.Booking.ToListAsync();
            return View(book);
        }
        public IActionResult Create()
        {
            ViewBag.EventId = new SelectList(_context.Event, "EventId", "EventName");
            ViewBag.VenueId = new SelectList(_context.Venue, "VenueId", "VenueName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {


            if (ModelState.IsValid)
            {

                _context.Add(booking);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));

            }

            return View(booking);
        }
        public async Task<IActionResult> Details(int? id)
        {

            var book = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var book = await _context.Booking.FirstOrDefaultAsync(m => m.BookingId == id);


            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool CompanyExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Booking.FindAsync(id);
            if (id == null)
            {
                return NotFound();
            }

            return View(book);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Booking book)
        {
            if (id != book.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(book.BookingId))
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

            return View(book);
        }

    }
}
