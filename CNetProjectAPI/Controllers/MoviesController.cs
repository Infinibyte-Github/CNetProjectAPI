using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CNetProjectAPI
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        // In-memory list to simulate a data source
        private static readonly List<Movie> Movies = new List<Movie>
        {
            new Movie { Title = "Inception", Format = "Blu-ray", Director = "Christopher Nolan", ReleaseYear = 2010 },
            new Movie { Title = "The Matrix", Format = "DVD", Director = "The Wachowskis", ReleaseYear = 1999 },
            new Movie { Title = "Parasite", Format = "Digital", Director = "Bong Joon-ho", ReleaseYear = 2019 }
        };

        // GET: api/movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetAllMovies()
        {
            return Ok(Movies);
        }

        // GET: api/movies/{title}
        [HttpGet("{title}")]
        public ActionResult<Movie> GetMovieByTitle(string title)
        {
            var movie = Movies.Find(m => m.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (movie == null)
            {
                return NotFound(new { Message = "Movie not found." });
            }
            return Ok(movie);
        }

        // POST: api/movies
        [HttpPost]
        public ActionResult AddMovie([FromBody] Movie newMovie)
        {
            if (newMovie == null || string.IsNullOrEmpty(newMovie.Title))
            {
                return BadRequest(new { Message = "Invalid movie data." });
            }

            Movies.Add(newMovie);
            return CreatedAtAction(nameof(GetMovieByTitle), new { title = newMovie.Title }, newMovie);
        }

        // DELETE: api/movies/{title}
        [HttpDelete("{title}")]
        public ActionResult DeleteMovie(string title)
        {
            var movie = Movies.Find(m => m.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (movie == null)
            {
                return NotFound(new { Message = "Movie not found." });
            }

            Movies.Remove(movie);
            return Ok(new { Message = $"Movie '{title}' deleted successfully." });
        }
        
        // PUT: api/movies/{title}
        [HttpPut("{title}")]
        public ActionResult UpdateMovie(string title, [FromBody] Movie updatedMovie)
        {
            if (updatedMovie == null || string.IsNullOrEmpty(updatedMovie.Title))
                return BadRequest(new { Message = "Invalid movie data." });

            var existingMovie = Movies.Find(m => m.Title.Equals(title, System.StringComparison.OrdinalIgnoreCase));
            if (existingMovie == null)
                return NotFound(new { Message = "Movie not found." });

            // Update the existing movie details
            existingMovie.Title = updatedMovie.Title;
            existingMovie.Format = updatedMovie.Format;
            existingMovie.Director = updatedMovie.Director;
            existingMovie.ReleaseYear = updatedMovie.ReleaseYear;

            return Ok(new { Message = $"Movie '{title}' updated successfully.", Movie = existingMovie });
        }
    }
}
