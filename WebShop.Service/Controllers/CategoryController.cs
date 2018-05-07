using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data;
using WebStore.Models;

namespace WebShop.Service.Controllers
{
    /// <summary>
	/// Kategóriák lekérdezését biztosító vezérlő.
	/// </summary>
	[Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly WebStoreContext _context;

        public CategoryController(WebStoreContext context)
        {
            if(context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
        }

        /// <summary>
        /// Kategóriák lekérdezése.
        /// </summary>
        [HttpGet]
        public IActionResult GetCategories()
        {
            try
            {
                return Ok(_context.Categories.ToList().Select(category => new CategoryDTO
                {
                    Id = category.Id,
                    Name = category.Name
                }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
