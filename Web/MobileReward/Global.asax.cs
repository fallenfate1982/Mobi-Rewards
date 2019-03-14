using System;
using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MobileReward.Controllers;
using WebMatrix.WebData;

namespace MobileReward
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      //var migrator = new DbMigrator(new Migrations.Configuration());
      //migrator.Update();

      DataBaseConfig.RegisterDatabaseConnection();
      AreaRegistration.RegisterAllAreas();

      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      AuthConfig.RegisterAuth();
      
    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
      HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
      if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
      {
        HttpContext.Current.Response.AddHeader("Cache-Control", "no-cache");
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
        HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, x-requested-with");
        HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
        HttpContext.Current.Response.End();
      }
    }

    protected void Application_Error(object sender, EventArgs e)
    {
      Exception exception = Server.GetLastError();
      Server.ClearError();

      RouteData routeData = new RouteData();
      routeData.Values.Add("controller", "Error");
      routeData.Values.Add("action", "Index");
      routeData.Values.Add("exception", exception);

      if (exception.GetType() == typeof(HttpException))
      {
        routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
      }
      else
      {
        routeData.Values.Add("statusCode", 500);
      }

      IController controller = new ErrorController();
      controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
      Response.End();
    }
  }
}