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
    public class MoviesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        // Constructor to inject ApplicationDbContext
        public MoviesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            // Retrieve movies from the database asynchronously
            var movies = await _context.Movies.ToListAsync();
            return Ok(movies);
        }

        // GET: api/movies/{title}
        [HttpGet("{title}")]
        public async Task<ActionResult<Movie>> GetMovieByTitle(string title)
        {
            var movie = await _context.Movies
                                      .FirstOrDefaultAsync(b => b.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));

            if (movie == null)
            {
                return NotFound(new { Message = "Movie not found." });
            }

            return Ok(movie);
        }

        // POST: api/movies
        [HttpPost]
        public async Task<ActionResult<Movie>> AddMovie([FromBody] Movie newMovie)
        {
            if (newMovie == null || string.IsNullOrEmpty(newMovie.Title))
            {
                return BadRequest(new { Message = "Invalid movie data." });
            }

            // Add the new movie to the database
            _context.Movies.Add(newMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMovieByTitle), new { title = newMovie.Title }, newMovie);
        }

        // DELETE: api/movies/{title}
        // DELETE: api/movies/{title}
        [HttpDelete("{title}")]
        public async Task<ActionResult> DeleteMovie(string title)
        {
            try
            {
                // Log the incoming request and title
                Console.WriteLine($"Attempting to delete movie with title: {title}");

                // Use ToLower() for case-insensitive comparison
                var movie = await _context.Movies
                    .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());

                // Debugging message: if movie is not found
                if (movie == null)
                {
                    Console.WriteLine($"Movie with title '{title}' not found in the database.");
                    return NotFound(new { Message = "Movie not found." });
                }

                // Debugging message: if movie is found
                Console.WriteLine($"Movie found: {movie.Title}, {movie.Director}, {movie.ReleaseYear}.");

                // Attempt to delete the movie
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();

                // Debugging message: successful deletion
                Console.WriteLine($"Movie '{title}' deleted successfully from the database.");

                return Ok(new { Message = $"Movie '{title}' deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.WriteLine($"An error occurred while deleting the movie: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while deleting the movie." });
            }
        }


        // PUT: api/movies/{title}
        [HttpPut("{title}")]
        public async Task<ActionResult> UpdateMovie(string title, [FromBody] Movie updatedMovie)
        {
            if (updatedMovie == null || string.IsNullOrEmpty(updatedMovie.Title))
                return BadRequest(new { Message = "Invalid movie data." });

            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());

            if (existingMovie == null)
                return NotFound(new { Message = "Movie not found." });

            // Update the existing movie's details
            existingMovie.Title = updatedMovie.Title;
            existingMovie.Director = updatedMovie.Director;
            existingMovie.Format = updatedMovie.Format;
            existingMovie.ReleaseYear = updatedMovie.ReleaseYear;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = $"Movie '{title}' updated successfully.", Movie = existingMovie });
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error occurred: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while updating the movie." });
            }
        }
    }
}