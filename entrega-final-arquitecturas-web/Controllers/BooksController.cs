using entrega_final_arquitecturas_web.DAL.Models;
using entrega_final_arquitecturas_web.Domain.DTO;
using entrega_final_arquitecturas_web.Domain.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace entrega_final_arquitecturas_web.Controllers
{
    [Route("api/books")]
    [ApiController]
    [Authorize]
    public class BooksController(DbCtx dbCtx, ILogger<BooksController> logger) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<Book>> Get()
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var books = await dbCtx.Books.Where(book => book.UserId == userId).ToArrayAsync();

            return books;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookDTO dto)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var book = new Book
            {
                Description = dto.Descripcion,
                Name = dto.Nombre,
                UserId = userId,
            };

            await dbCtx.Books.AddAsync(book);
            await dbCtx.SaveChangesAsync();

            var success = book.Id != 0;
            var statusCode = success ? StatusCodes.Status201Created : StatusCodes.Status422UnprocessableEntity;

            return new ObjectResult(book)
            {
                StatusCode = statusCode
            };
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] BookDTO dto)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var book = await dbCtx.Books
                .Where(book => book.Id == id && book.UserId == userId)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            book.Name = dto.Nombre;
            book.Description = dto.Descripcion;

            await dbCtx.SaveChangesAsync();

            return Ok(book);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var book = await dbCtx.Books
                .Where(book => book.Id == id && book.UserId == userId)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                return NotFound();
            }

            dbCtx.Books.Remove(book);
            await dbCtx.SaveChangesAsync();

            return NoContent();
        }
    }
}
