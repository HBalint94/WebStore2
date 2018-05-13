using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    [Route("api/[controller]")]
    public class RentController : Controller
    {
        private readonly WebStoreContext _context;
        /// <summary>
        /// Authentikációs szolgáltatás.
        /// </summary>
        private readonly SignInManager<Customer> _signInManager;


        /// <summary>
        /// Vezérlő példányosítása.
        /// </summary>
        /// <param name="context">Entitásmodell.</param>
        public RentController(WebStoreContext context, SignInManager<Customer> signInManager)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            _context = context;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult GetRents()
        {
            try
            {
                List<Customer> customers = _signInManager.UserManager.Users.ToList();

                return Ok(_context.Rents.Where(p => !p.Performed).ToList().Select(rent => new RentDTO
                {
                    Id = rent.Id,
                    UserName = SelectCustomer(customers, rent.CustomerId).UserName,
                    Address = SelectCustomer(customers, rent.CustomerId).Address,
                    PhoneNumber = SelectCustomer(customers, rent.CustomerId).PhoneNumber,
                    Email = SelectCustomer(customers, rent.CustomerId).Email,
                    TotalPrice = rent.TotalPrice,
                    Performed = rent.Performed
                }));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public Customer SelectCustomer(List<Customer> customers, int customer_id)
        {
            for(int i=0; i< customers.Count; i++)
            {
                if (customers[i].Id == customer_id) {
                    return customers[i];
                }
            }
            return null;
        }

        [HttpPut]
        [Authorize(Roles = "administrator")]
        public IActionResult PutProduct([FromBody] RentDTO rentDTO)
        {
            try
            {
                Rent selectedRent = _context.Rents.FirstOrDefault(b => b.Id == rentDTO.Id);
                List<RentProductConnection> rentedProductsForSelectedRent = _context.RentProductConnections.Where(rp => rp.RentId == selectedRent.Id).ToList();
                List<Product> products = _context.Products.ToList(); // Ha kell módosítjuk az raktárkészletet az egyes termékeknél

                if (selectedRent == null) // ha nincs ilyen azonosító, akkor hibajelzést küldünk
                    return NotFound();

                selectedRent.Performed = rentDTO.Performed;

                if (selectedRent.Performed)
                {
                    foreach(RentProductConnection rp in rentedProductsForSelectedRent)
                    {
                        if(selectedRent.Id == rp.RentId)
                        {
                            foreach (Product product in products)
                            {
                                if(rp.ProductModellNumber == product.ModellNumber)
                                {
                                    product.Inventory = product.Inventory - rp.CountProduct;
                                    if(product.Inventory <= 0)
                                    {
                                        product.Available = false;
                                    }
                                }
                            }
                        }
                    }
                }

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
