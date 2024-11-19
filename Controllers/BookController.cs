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
    public class BookController : ControllerBase
    {
        private readonly ApibooksContext _context;

        public BookController(ApibooksContext context)
        {
            _context = context;
        }

        [HttpGet("GetBooks")]
        public async Task<ActionResult<List<Book>>> Get()
        {
            var books = await _context.Books
                .Include(b => b.Reservations)
                .Select(
                    s => new Book
                    {
                        Id = s.Id,
                        Title = s.Title,
                        Author = s.Author,
                        Available = s.Available,
                        Reservations = s.Reservations.Select(r => new Reservation
                        {
                            Id = r.Id,
                            BookId = r.BookId,
                            UserName = r.UserName,
                            ReservationDate = r.ReservationDate,
                            ReturnDate = r.ReturnDate
                        }).ToList()
                    }
                ).ToListAsync();

            if (books.Count == 0)
            {
                return NotFound();
            }
            else
            {
                return books;
            }
        }

        [HttpPost("CreateBook")]
        [SwaggerOperation(Summary = "Create a new book", Description = "Creates a new book.")]
        [SwaggerResponse(201, "Book created successfully", typeof(Book))]
        [SwaggerResponse(400, "Invalid input")]
        public async Task<ActionResult<Book>> Create(
            [SwaggerParameter(Description = "The title of the book", Required = true)] string title,
            [SwaggerParameter(Description = "The author of the book", Required = true)] string author,
            [SwaggerParameter(Description = "The availability of the book", Required = false)] bool? available)
        {
            var book = new Book
            {
                Title = title,
                Author = author,
                Available = available
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }

        [HttpPut("UpdateBook/{id}")]
        [SwaggerOperation(Summary = "Update an existing book", Description = "Updates an existing book.")]
        [SwaggerResponse(204, "Book updated successfully")]
        [SwaggerResponse(400, "Invalid input")]
        [SwaggerResponse(404, "Book not found")]
        public async Task<IActionResult> Update(
            [SwaggerParameter(Description = "The ID of the book", Required = true)] int id,
            [SwaggerParameter(Description = "The title of the book", Required = true)] string title,
            [SwaggerParameter(Description = "The author of the book", Required = true)] string author,
            [SwaggerParameter(Description = "The availability of the book", Required = false)] bool? available)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = title;
            book.Author = author;
            book.Available = available;

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Books.Any(e => e.Id == book.Id))
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

        [HttpDelete("DeleteBook/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}