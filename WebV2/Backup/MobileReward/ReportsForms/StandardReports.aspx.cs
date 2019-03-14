using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using MobileReward.Models;

namespace MobileReward.ReportsForms
{
  public partial class StandardReports : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void Page_Init(object sender, EventArgs e)
    {
      ConfigureCrystalReports();
    }

    private void ConfigureCrystalReports()
    {
      try
      {
        bool isValid = true;

        string strReportName = System.Web.HttpContext.Current.Session["ReportName"].ToString();
        MobileReward.Models.MobiRewardDb db = new Models.MobiRewardDb();
        var listRiskDetails = db.Promotions.Where(x => x.Status == true).ToList();

        var rptSource = System.Web.HttpContext.Current.Session["rptSource"];

        if (string.IsNullOrEmpty(strReportName))
        {
          isValid = false;
        }


        if (isValid)
        {
          ReportDocument rd = new ReportDocument();
          string strRptPath = Server.MapPath("~/") + "Reports\\" + strReportName;
          //~/ReportForms/StandardReport.aspx
          rd.Load(strRptPath);

          if (rptSource != null && rptSource.GetType().ToString() != "System.String")
            rd.SetDataSource(rptSource);

          CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
          CrystalReportViewer1.ReportSource = rd;
          CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

        }
        else
        {
          Response.Write("<H2>Nothing Found; No Report name found</H2>");
        }
      }
      catch (Exception ex)
      {
        Response.Write(ex.ToString());
      }
    }
  }
}