using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vintage.Rabbit.Interfaces.Cache;

namespace Vintage.Rabbit.Admin.Web.Controllers
{
    public class CacheController : Controller
    {
        private ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            this._cacheService = cacheService;
        }

        public ActionResult Index()
        {
            IList<string> keys = this._cacheService.Keys();

            return View("Index", keys);
        }

        public ActionResult RemoveKeys(string[] key)
        {
            foreach (var k in key)
            {
                this._cacheService.Remove(k);
            }

            return RedirectToRoute(Routes.Cache.Index);
        }
	}
}