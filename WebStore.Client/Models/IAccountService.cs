using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public interface IAccountService
    {
       
    /// <summary>
    /// Felhasználószám lekérdezése.
    /// </summary>
    Int32 UserCount { get; }

    /// <summary>
    /// Aktuálisan bejelentkezett felhasználó nevének lekérdezése.
    /// </summary>
    String CurrentUserName { get; }

    /// <summary>
    /// vásárló adatok lekérdezése.
    /// </summary>
    /// <param name="userName">A felhasználónév.</param>
    Customer GetCustomer(String userName);

    /// <summary>
    /// Felhasználó bejelentkeztetése.
    /// </summary>
    /// <param name="user">A felhasználó nézetmodellje.</param>
    Boolean Login(LoginViewModel user);

    /// <summary>
    /// Felhasználó kijelentkeztetése.
    /// </summary>
    Boolean Logout();

    /// <summary>
    /// vásárló regisztrációja.
    /// </summary>
    /// <param name="customer">A vásárló nézetmodellje.</param>
    Boolean Register(RegistrationViewModel customer);

    /// <summary>
    /// Vásárló létrehozása (regisztráció nélkül).
    /// </summary>
    /// <param name="customer">A vásárló nézetmodellje.</param>
    /// <param name="userName">A felhasznalónév.</param>
    Boolean Create(CustomerViewModel customer, out String userName);
}
}
