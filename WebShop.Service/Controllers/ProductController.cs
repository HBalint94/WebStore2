using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data;
using WebStore.Models;

namespace WebShop.Service.Controllers
{
    /// <summary>
	/// Termékek lekérdezését és módosítását biztosító vezérlő.
	/// </summary>
	[Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly WebStoreContext _context;

        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        /// <param name="context">Entitásmodell.</param>
        public ProductController(WebStoreContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
        }

        /// <summary>
        /// Termékek lekérdezése
        /// </summary>
        [HttpGet]
        public IActionResult GetProducts()
        {
            try
            {
             
                return Ok(_context.Products.Where(p => p.Available && p.Inventory > 0).ToList().Select(product => new ProductDTO
                {
                    Id = product.Id,
                    ModellNumber = product.ModellNumber,
                    Producer = product.Producer,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Inventory = product.Inventory,
                    Available = product.Available,
                    Description = product.Description
                }));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProductForCategory([FromRoute]int categoryId)
        {
            try
            {
                return Ok(_context.Products.Include(b => b.Available).Where(b => b.CategoryId == categoryId).ToList().Select(product => new ProductDTO
                {
                    Id = product.Id,
                    ModellNumber = product.ModellNumber,
                    Producer = product.Producer,
                    CategoryId = product.CategoryId,
                    Price = product.Price,
                    Inventory = product.Inventory,
                    Available = product.Available,
                    Description = product.Description
                }));
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Új termék létrehozása.
        /// </summary>
        /// <param name="productDTO">Termék.</param>
        [HttpPost]
       // [Authorize(Roles = "administrator")]
        public IActionResult PostProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                var addedProduct = _context.Products.Add(new Product
                {
                    Id = productDTO.Id,
                    ModellNumber = productDTO.ModellNumber,
                    Producer = productDTO.Producer,
                    CategoryId = productDTO.CategoryId,
                    Price = productDTO.Price,
                    Inventory = productDTO.Inventory,
                    Available = productDTO.Available,
                    Description = productDTO.Description
                });

                _context.SaveChanges(); // elmentjük az új terméket

                productDTO.Id = addedProduct.Entity.Id;

                // visszaküldjük a létrehozott épületet
                return Created(Request.GetUri() + addedProduct.Entity.Id.ToString(), productDTO);
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// Termék módosítása.
        /// </summary>
        /// <param name="productDTO">Termék.</param>
        [HttpPut]
        [Authorize(Roles = "administrator")]
        public IActionResult PutProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _context.Products.FirstOrDefault(b => b.Id == productDTO.Id);

                if (product == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                product.Id = productDTO.Id;
                product.ModellNumber = productDTO.ModellNumber;
                product.Producer = productDTO.Producer;
                product.CategoryId = productDTO.CategoryId;
                product.Price = productDTO.Price;
                product.Inventory = productDTO.Inventory;
                product.Available = productDTO.Available;
                product.Description = productDTO.Description;

                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
		/// Termék törlése.
		/// </summary>
		/// <param name="id">Termék azonosító.</param>
		[HttpDelete("{id}")]
        [Authorize(Roles = "administrator")]
        public IActionResult DeleteProduct(Int32 id)
        {
            try
            {
                Product product = _context.Products.FirstOrDefault(b => b.Id == id);

                if (product == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                _context.Products.Remove(product);

                _context.SaveChanges(); // elmentjük a módosított épületet

                return Ok();
            }
            catch
            {
                // Internal Server Error
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }





    }
}
