using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UI5.Data;
using UI5.Helper;
using static UI5.Models.OData;

namespace UI5.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //[Authorize]
    [Route("Books")]
    [ODataRoutePrefix("Books")]
    public class BooksController : ODataController
    {
        private readonly MyDbContext _context;
        public BooksController(MyDbContext context)
        {
            _context = context;
            if (_context.Books.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    _context.Books.Add(b);
                    _context.Presses.Add(b.Press);
                }
                context.SaveChanges();
            }
        }

        private bool BookExists(int key)
        {
            return _context.Books.Any(p => p.Id == key);
        }

        [HttpGet]
        [ODataRoute]
        [EnableQuery]
        public IActionResult Get()
        {
            var model = _context.Books.Include(x=>x.Press);

            return Ok(model); 
        }

        [HttpGet("{Id}")]
        [ODataRoute("({Id})")]
        [EnableQuery]
        public IActionResult Get([FromODataUri] int Id)
        {
            var result = _context.Books.Where(c => c.Id == Id).Include(x=>x.Press).FirstOrDefault();
            return Ok(result);
        }

        [HttpPost]
        [EnableQuery]
        [ODataRoute]
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

        [EnableQuery]
        [ODataRoute]
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

        [EnableQuery]
        [ODataRoute]
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

        [EnableQuery]
        [ODataRoute]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var book = await _context.Books.FindAsync(key);
            if (book == null)
            {
                return NotFound();
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return Ok("Deleted");
        }

    }

}