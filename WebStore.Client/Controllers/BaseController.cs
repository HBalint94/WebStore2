using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class BaseController : Controller
    {
        // a logikát szolgáltatás osztály mögé rejtjük
        protected readonly IStoreService storeService;
    
        // alkalmazás szintű állapot
        protected readonly ApplicationState applicationState;

        public BaseController(IStoreService storeService, ApplicationState applicationState)
        {
            this.storeService = storeService;
            this.applicationState = applicationState;
        }

        /// <summary>
        /// Egy akció meghívása után végrehajtandó metódus.
        /// </summary>
        /// <param name="context">Az akció kontextus argumentuma.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            // a minden oldalról elérhető információkat össze gyűjtjük
            ViewBag.Categories = storeService.Categories.ToArray();
            ViewBag.UserCount = applicationState.UserCount;
            ViewBag.CurrentCustomerName = String.IsNullOrEmpty(User.Identity.Name) ? null : User.Identity.Name;
        }
    }
}
