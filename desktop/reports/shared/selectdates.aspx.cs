using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Allows the user to specify dates to run the report for.
/// </summary>
public partial class desktop_reports_incoming_SelectDates : System.Web.UI.Page
{
    /// <summary>
    /// Passes the template in and shows the dates for specifying
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Session["reportTemplateRow"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                calStart.SelectedDate = DateTime.Now;
                calEnd.SelectedDate = DateTime.Now;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }

    /// <summary>
    /// Displays the report with the dates specified
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            if (calStart.SelectedDate != null && calEnd.SelectedDate != null)
            {

                if (calStart.SelectedDate > calEnd.SelectedDate)
                    lblError.Text = "The end date must be later than the start date.";
                else
                {
                    Session["startDate"] = calStart.SelectedDate;
                    Session["endDate"] = calEnd.SelectedDate;

                    Template reportTemplate = (Template)Session["reportTemplateRow"];
                    string redirectURL = "default.aspx";

                    switch (reportTemplate.TemplateType)
                    {
                        case ((int)ReportTemplate.ReportType.GroceryRescue):
                            redirectURL = Config.DOMAIN() + "desktop/reports/grocery-rescue/displaygroceryrescuereport.aspx";
                            break;
                        case ((int)ReportTemplate.ReportType.Incoming):
                            redirectURL = Config.DOMAIN() + "desktop/reports/incoming/displayincomingreport.aspx";
                            break;
                        case ((int)ReportTemplate.ReportType.Outgoing):
                            redirectURL = Config.DOMAIN() + "desktop/reports/outgoing/displayoutgoingreport.aspx";
                            break;
                        case ((int)ReportTemplate.ReportType.InOut):
                            redirectURL = Config.DOMAIN() + "desktop/reports/in-out/displayinoutreport.aspx";
                            break;
                        case ((int)ReportTemplate.ReportType.Inventory):
                            redirectURL = Config.DOMAIN() + "desktop/reports/inventory/displayinventoryreport.aspx";
                            break;
                    }

                    Response.Redirect(redirectURL);
                }

            }
            else
            {
                lblError.Text = "Please select two dates";
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }

    /// <summary>
    /// Moves to the previous page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["previousPage"] == null)
                Response.Redirect("default.aspx");

            string previousPage = Request.QueryString["previousPage"];
            Response.Redirect(previousPage);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }
}