using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CNetProjectAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        // In-memory list to simulate a data source
        private static readonly List<Book> Books = new List<Book>
        {
            new Book { Title = "The Great Gatsby", Format = "Hardcover", Author = "F. Scott Fitzgerald", Pages = 180 },
            new Book { Title = "1984", Format = "Paperback", Author = "George Orwell", Pages = 328 },
            new Book { Title = "Clean Code", Format = "eBook", Author = "Robert C. Martin", Pages = 464 }
        };

        // GET: api/books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            return Ok(Books);
        }

        // GET: api/books/{title}
        [HttpGet("{title}")]
        public ActionResult<Book> GetBookByTitle(string title)
        {
            var book = Books.Find(b => b.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (book == null)
            {
                return NotFound(new { Message = "Book not found." });
            }
            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public ActionResult AddBook([FromBody] Book newBook)
        {
            if (newBook == null || string.IsNullOrEmpty(newBook.Title))
            {
                return BadRequest(new { Message = "Invalid book data." });
            }

            Books.Add(newBook);
            return CreatedAtAction(nameof(GetBookByTitle), new { title = newBook.Title }, newBook);
        }

        // DELETE: api/books/{title}
        [HttpDelete("{title}")]
        public ActionResult DeleteBook(string title)
        {
            var book = Books.Find(b => b.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (book == null)
            {
                return NotFound(new { Message = "Book not found." });
            }

            Books.Remove(book);
            return Ok(new { Message = $"Book '{title}' deleted successfully." });
        }
        
        // PUT: api/books/{title}
        [HttpPut("{title}")]
        public ActionResult UpdateBook(string title, [FromBody] Book updatedBook)
        {
            if (updatedBook == null || string.IsNullOrEmpty(updatedBook.Title))
                return BadRequest(new { Message = "Invalid book data." });

            var existingBook = Books.Find(b => b.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (existingBook == null)
                return NotFound(new { Message = "Book not found." });

            // Update the existing book details
            existingBook.Title = updatedBook.Title;
            existingBook.Format = updatedBook.Format;
            existingBook.Author = updatedBook.Author;
            existingBook.Pages = updatedBook.Pages;

            return Ok(new { Message = $"Book '{title}' updated successfully.", Book = existingBook });
        }
    }
}