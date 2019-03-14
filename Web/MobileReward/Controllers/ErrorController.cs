using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MobileReward.Models;

namespace MobileReward.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

      public ActionResult Index(int statusCode, Exception exception)
      {
        var model = new ErrorModel { HttpStatusCode = statusCode, Exception = exception };
        Response.StatusCode = statusCode;

        return View(model);
      }

    }
}
