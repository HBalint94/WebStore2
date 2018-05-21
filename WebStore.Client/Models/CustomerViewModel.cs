using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace WebStore.Models
{
    public class CustomerViewModel
    {
        /// <summary>
        /// Vendég neve.
        /// </summary>
        [Required(ErrorMessage = "A név megadása kötelező.")] // feltételek a validáláshoz
        [StringLength(60, ErrorMessage = "A foglaló neve maximum 60 karakter lehet.")]
        public String CustomerName { get; set; }

        /// <summary>
        /// Vendég e-mail címe.
        /// </summary>
        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.EmailAddress)] // pontosítjuk az adatok típusát
        public String CustomerEmail { get; set; }

        /// <summary>
        /// Vendég címe.
        /// </summary>
        [Required(ErrorMessage = "A cím megadása kötelező.")]
        public String CustomerAddress { get; set; }

        /// <summary>
        /// Vendég telefonszáma.
        /// </summary>
        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        public String CustomerPhoneNumber { get; set; }
    }
}
