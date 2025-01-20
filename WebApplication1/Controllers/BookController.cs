using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace BookApiSwagger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "C# Programming", Author = "John Doe", Price = 49.99 },
            new Book { Id = 2, Title = "Learn ASP.NET Core", Author = "Jane Smith", Price = 59.99 }
        };

        // Get all books
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            return Ok(Books);
        }

        // Get a book by ID
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // Add a new book
        [HttpPost]
        public ActionResult AddBook(Book newBook)
        {
            Books.Add(newBook);
            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        // Update a book
        [HttpPut("{id}")]
        public ActionResult UpdateBook(int id, Book updatedBook)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.Price = updatedBook.Price;

            return NoContent();
        }

        // Delete a book
        [HttpDelete("{id}")]
        public ActionResult DeleteBook(int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null) return NotFound();

            Books.Remove(book);
            return NoContent();
        }
    }
}
