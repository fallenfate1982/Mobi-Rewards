using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

namespace MobileReward.HelperExtensions
{
  public static class HelperExtension
  {

    public static MvcHtmlString PageLinks(this HtmlHelper htmlHelper, int TotalPages, int CurrentPage, string actionName, string controllername, object routeValues)
    {
      var repID = Guid.NewGuid().ToString();
      var lnk = htmlHelper.ActionLink(repID, actionName, controllername, routeValues);

      const int maxPages = 11;
      int pagesAfter = TotalPages - CurrentPage; // Number of pages after current
      int pagesBefore = CurrentPage - 1; // Number of pages before current

      if (pagesAfter <= 4)
      {
        BuildPagging(htmlHelper, 1, TotalPages, null); // Show 1st page

        int pageSubset = TotalPages - maxPages - 1 > 1 ? TotalPages - maxPages - 1 : 2;
        BuildPagging(htmlHelper, pageSubset, pageSubset, ""); // Show page subset (...)

        BuildPagging(htmlHelper, TotalPages - maxPages + 3, TotalPages, null); // Show last pages

        //return ; // Exit
      }

      if (pagesBefore <= 4)
      {
        BuildPagging(htmlHelper, 1, maxPages - 2, null); // Show 1st pages

        int pageSubset = maxPages + 2 < TotalPages ? maxPages + 2 : TotalPages - 1;
        BuildPagging(htmlHelper, pageSubset, pageSubset, ""); // Show page subset (...)

        BuildPagging(htmlHelper, TotalPages, TotalPages, null); // Show last page

        return MvcHtmlString.Create(lnk.ToString().Replace(repID, actionName)); // Exit

      }

      if (pagesAfter > 4)
      {
        BuildPagging(htmlHelper, 1, 1, null); // Show 1st pages

        int pageSubset1 = CurrentPage - 7 > 1 ? CurrentPage - 7 : 2;
        int pageSubset2 = CurrentPage + 7 < TotalPages ? CurrentPage + 7 : TotalPages - 1;

        BuildPagging(htmlHelper, pageSubset1, pageSubset1, pageSubset1 == CurrentPage - 4 ? null : ""); // Show 1st page subset (...)

        BuildPagging(htmlHelper, CurrentPage - 3, CurrentPage + 3, null); // Show middle pages

        // Show 2nd page subset (...)
        // only show ... if page is contigous to the previous one.
        BuildPagging(htmlHelper, pageSubset2, pageSubset2, pageSubset2 == CurrentPage + 4 ? null : "");
        BuildPagging(htmlHelper, TotalPages, TotalPages, null); // Show last page

        return MvcHtmlString.Create(lnk.ToString().Replace(repID, actionName));  // Exit

      }

      return MvcHtmlString.Create(lnk.ToString().Replace(repID, actionName));
    }
    public static MvcHtmlString BuildPagging(this HtmlHelper htmlHelper, int start, int end, string innerContent)
    {
      var repID = Guid.NewGuid().ToString();
      var lnk = htmlHelper.ActionLink(start + "", end + "", innerContent);
      return MvcHtmlString.Create(lnk.ToString().Replace(repID, innerContent));

    }
  }
}