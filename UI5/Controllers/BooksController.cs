using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI5.Data;
using static UI5.Models.OData;

namespace UI5.Controllers
{
    public class BooksController : ODataController
    {
        private readonly MyDbContext _context;
        public BooksController(MyDbContext context)
        {
            _context = context;
            if (!_context.Books.Any())
            {
                var books = new List<Book>()
                {
                new Book
                {
                     Id = 1,
                     Author = "Jone Doe",
                     Title = "Kaczor Donald"
                },
                new Book
                {
                     Id = 2,
                     Author = "Jone Doe",
                     Title = "Kaczor Donald 2"
                },
                new Book
                {
                     Id = 3,
                     Title = "Essential C#5.0",
                     Author = "Mark Michaelis",
                },
                new Book
                {
                     Id = 4,
                     Title = "Enterprise Games",
                     Author = "Michael Hugos",
                },
                };

                _context.Books.AddRange(books);
                _context.SaveChanges();
            }
        }

        private bool BookExists(int key)
        {
            return _context.Books.Any(p => p.Id == key);
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Books);
        }

        [EnableQuery]
        public SingleResult Get([FromODataUri] int Id)
        {
            IQueryable<Book> result = _context.Books.Where(c => c.Id == Id);
            return SingleResult.Create(result);
        }

        public async Task<IActionResult> Post(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return Created(book);
        }

        public async Task<IActionResult> Patch([FromODataUri] int key, Delta<Book> book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await _context.Books.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            book.Patch(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }

        public async Task<IActionResult> Put([FromODataUri] int key, Book update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            _context.Entry(update).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }


        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var book = await _context.Books.FindAsync(key);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return StatusCode(410);
        }

    }

}