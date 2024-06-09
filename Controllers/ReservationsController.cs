using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DeskJockey.Data;
using DeskJockey.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DeskJockey.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetReservationsForDesk(int deskId)
        {
            var reservations = _context.Reservations
                .Where(r => r.DeskId == deskId)
                .Select(r => new {
                    StartDate = r.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = r.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    r.FirstName,
                    r.LastName
                })
                .ToList();

            return Json(new { reservations });
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reservations.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeskId,StartDate,EndDate,FirstName,LastName,Status")] Reservation reservation)
        {

            if (User.Identity.IsAuthenticated)
            {

                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userIdString))
                {
                    if (int.TryParse(userIdString, out int userId))
                    {
                        reservation.UserId = userId;
                    }
                    else
                    {
 
                        return Json(new { success = false, errors = new[] { "Nieprawidłowy identyfikator użytkownika." } });
                    }
                }
            }

            if (ModelState.IsValid)
            {

                var overlappingReservations = _context.Reservations
                    .Where(r => r.DeskId == reservation.DeskId &&
                                ((r.StartDate <= reservation.StartDate && r.EndDate >= reservation.StartDate) ||
                                 (r.StartDate <= reservation.EndDate && r.EndDate >= reservation.EndDate) ||
                                 (r.StartDate >= reservation.StartDate && r.EndDate <= reservation.EndDate)))
                    .ToList();

                if (overlappingReservations.Any())
                {

                    return Json(new { success = false, message = "Biurko jest już zarezerwowane w wybranym czasie." });
                }

                try
                {
                    _context.Add(reservation);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
     
                    Console.WriteLine(ex.Message);
                    return Json(new { success = false, errors = new[] { ex.Message } });
                }
            }
            else
            {
                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(int id, [Bind("ReservationId,UserId,DeskId,StartDate,EndDate,Status,FirstName,LastName")] Reservation reservation)
        {
            if (id != reservation.ReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
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
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return _context.Reservations.Any(e => e.ReservationId == id);
        }
    }
}
