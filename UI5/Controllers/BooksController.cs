using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
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

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_context.Books);
        }

        [EnableQuery]
        public IActionResult Get(int Id)
        {
            return Ok(_context.Books.FirstOrDefault(c => c.Id == Id));
        }
    }
}