using BookstoreAPI.Models;
using BookstoreAPI.Repositories;
using BookstoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetAllBooks()
        {
            return Ok(_bookService.GetAllBooks());
        }

        [HttpGet("{id}")]
        public ActionResult<Book> GetBookById(int id)
        {
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        [HttpPost]
        public ActionResult AddBook(Book book)
        {
            _bookService.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateBook(int id, Book book)
        {
            try
            {
                var existingBook = _bookService.GetBookById(id);
                if (existingBook == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (BookNotFoundException)
            {
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteBook(int id)
        {
            try
            {
                _bookService.DeleteBook(id);
                return NoContent();
            }
            catch (BookNotFoundException)
            {
                return NotFound();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("external/{isbn}")]
        public async Task<ActionResult<Book>> GetBookDetailsFromExternalApiAsync(string isbn)
        {
            var book = await _bookService.GetBookDetailsFromExternalApiAsync(isbn);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
    }
}
