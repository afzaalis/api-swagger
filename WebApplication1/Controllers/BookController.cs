using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication1.Models;

namespace BookApiSwagger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController
    {
        private static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "C# Programming", Author = "John Doe", Price = 49.99 },
            new Book { Id = 2, Title = "Learn ASP.NET Core", Author = "Jane Smith", Price = 59.99 }
        };

        // Get all books
        [HttpGet]
        public Task GetBooks(HttpContext context)
        {
            var response = JsonSerializer.Serialize(Books);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status200OK;
            return context.Response.WriteAsync(response);
        }

        // Get a book by ID
        [HttpGet("{id}")]
        public Task GetBook(HttpContext context, int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return context.Response.WriteAsync("Book not found");
            }

            var response = JsonSerializer.Serialize(book);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status200OK;
            return context.Response.WriteAsync(response);
        }

        // Add a new book
        [HttpPost]
        public async Task AddBook(HttpContext context)
        {
            try
            {
                var newBook = await JsonSerializer.DeserializeAsync<Book>(context.Request.Body);
                if (newBook == null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid book data");
                    return;
                }

                Books.Add(newBook);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status201Created;
                await context.Response.WriteAsync(JsonSerializer.Serialize(newBook));
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Failed to add book");
            }
        }

        // Update a book
        [HttpPut("{id}")]
        public async Task UpdateBook(HttpContext context, int id)
        {
            try
            {
                var updatedBook = await JsonSerializer.DeserializeAsync<Book>(context.Request.Body);
                if (updatedBook == null)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Invalid book data");
                    return;
                }

                var book = Books.FirstOrDefault(b => b.Id == id);
                if (book == null)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    await context.Response.WriteAsync("Book not found");
                    return;
                }

                book.Title = updatedBook.Title;
                book.Author = updatedBook.Author;
                book.Price = updatedBook.Price;

                context.Response.StatusCode = StatusCodes.Status204NoContent;
            }
            catch
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Failed to update book");
            }
        }

        // Delete a book
        [HttpDelete("{id}")]
        public Task DeleteBook(HttpContext context, int id)
        {
            var book = Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return context.Response.WriteAsync("Book not found");
            }

            Books.Remove(book);
            context.Response.StatusCode = StatusCodes.Status204NoContent;
            return Task.CompletedTask;
        }
    }
}
