using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CNetProjectAPI.Data;

namespace CNetProjectAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ApiDbContext _context;

        // Constructor to inject ApplicationDbContext
        public BooksController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooks()
        {
            // Retrieve books from the database asynchronously
            var books = await _context.Books.ToListAsync();
            return Ok(books);
        }

        // GET: api/books/{title}
        [HttpGet("{title}")]
        public async Task<ActionResult<Book>> GetBookByTitle(string title)
        {
            var book = await _context.Books
                                      .FirstOrDefaultAsync(b => b.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));

            if (book == null)
            {
                return NotFound(new { Message = "Book not found." });
            }

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<Book>> AddBook([FromBody] Book newBook)
        {
            if (newBook == null || string.IsNullOrEmpty(newBook.Title))
            {
                return BadRequest(new { Message = "Invalid book data." });
            }

            // Add the new book to the database
            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookByTitle), new { title = newBook.Title }, newBook);
        }

        // DELETE: api/books/{title}
        // DELETE: api/books/{title}
        [HttpDelete("{title}")]
        public async Task<ActionResult> DeleteBook(string title)
        {
            try
            {
                // Log the incoming request and title
                Console.WriteLine($"Attempting to delete book with title: {title}");

                // Use ToLower() for case-insensitive comparison
                var book = await _context.Books
                    .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());

                // Debugging message: if book is not found
                if (book == null)
                {
                    Console.WriteLine($"Book with title '{title}' not found in the database.");
                    return NotFound(new { Message = "Book not found." });
                }

                // Debugging message: if book is found
                Console.WriteLine($"Book found: {book.Title}, {book.Author}, {book.Pages} pages.");

                // Attempt to delete the book
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();

                // Debugging message: successful deletion
                Console.WriteLine($"Book '{title}' deleted successfully from the database.");

                return Ok(new { Message = $"Book '{title}' deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.WriteLine($"An error occurred while deleting the book: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while deleting the book." });
            }
        }


        // PUT: api/books/{title}
        [HttpPut("{title}")]
        public async Task<ActionResult> UpdateBook(string title, [FromBody] Book updatedBook)
        {
            if (updatedBook == null || string.IsNullOrEmpty(updatedBook.Title))
                return BadRequest(new { Message = "Invalid book data." });

            var existingBook = await _context.Books
                .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());

            if (existingBook == null)
                return NotFound(new { Message = "Book not found." });

            // Update the existing book's details
            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Format = updatedBook.Format;
            existingBook.Pages = updatedBook.Pages;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = $"Book '{title}' updated successfully.", Book = existingBook });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while updating the book." });
            }
        }
    }
}