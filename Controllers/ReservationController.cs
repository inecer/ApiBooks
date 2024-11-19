using ApiBooks.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiBooks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ApibooksContext _context;

        public ReservationController(ApibooksContext context)
        {
            _context = context;
        }

        [HttpGet("GetReservations")]
        public async Task<ActionResult<List<Reservation>>> Get()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Book)
                .Select(
                    s => new Reservation
                    {
                        Id = s.Id,
                        BookId = s.BookId,
                        UserName = s.UserName,
                        ReservationDate = s.ReservationDate,
                        ReturnDate = s.ReturnDate,
                        Book = new Book
                        {
                            Id = s.Book.Id,
                            Title = s.Book.Title,
                            Author = s.Book.Author,
                            Available = s.Book.Available
                        }
                    }
                ).ToListAsync();

            if (reservations.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return reservations;
            }
        }

        [HttpPost("CreateReservation")]
        [SwaggerOperation(Summary = "Create a new reservation", Description = "Creates a new reservation for a book.")]
        [SwaggerResponse(201, "Reservation created successfully", typeof(Reservation))]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<Reservation>> Create(
            [SwaggerParameter(Description = "The ID of the reservation", Required = false)] int id,
            [SwaggerParameter(Description = "The ID of the book being reserved", Required = true)] int bookId,
            [SwaggerParameter(Description = "The username of the person making the reservation", Required = true)] string userName,
            [SwaggerParameter(Description = "The date the reservation was made", Required = true)] DateTime reservationDate,
            [SwaggerParameter(Description = "The date the book is expected to be returned", Required = false)] DateTime? returnDate)
        {
            var reservation = new Reservation
            {
                Id = id,
                BookId = bookId,
                UserName = userName,
                ReservationDate = reservationDate,
                ReturnDate = returnDate ?? default(DateTime)
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = reservation.Id }, reservation);
        }
        
        [HttpPut("UpdateReservation")]
        [SwaggerOperation(Summary = "Update an existing reservation", Description = "Updates an existing reservation for a book.")]
        [SwaggerResponse(204, "Reservation updated successfully")]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "Reservation not found")]
        public async Task<IActionResult> Update(
            [SwaggerParameter(Description = "The ID of the reservation", Required = true)] int id,
            [SwaggerParameter(Description = "The ID of the book being reserved", Required = true)] int bookId,
            [SwaggerParameter(Description = "The username of the person making the reservation", Required = true)] string userName,
            [SwaggerParameter(Description = "The date the reservation was made", Required = true)] DateTime reservationDate,
            [SwaggerParameter(Description = "The date the book is expected to be returned", Required = false)] DateTime? returnDate)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.BookId = bookId;
            reservation.UserName = userName;
            reservation.ReservationDate = reservationDate;
            reservation.ReturnDate = returnDate ?? reservation.ReturnDate;

            _context.Entry(reservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Reservations.Any(e => e.Id == reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("DeleteReservation/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}