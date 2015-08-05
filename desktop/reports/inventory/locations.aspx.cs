using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Allows the user to specify what locations they want to display on the report.
/// </summary>
public partial class desktop_reports_incoming_Locations : System.Web.UI.Page
{
    /// <summary>
    /// Loads any of the previously chosen options with the template into the interface
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                if (Session["reportTemplateRow"] == null || Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                InventoryReportTemplate template = (InventoryReportTemplate)Session["reportTemplate"];

                using (CCSEntities db = new CCSEntities())
                {
                    lstlocations.SelectedType = template.LocationsSelection;
                    lstlocations.AvailableList = (from f in db.Locations
                                                    select new { ID = f.LocationID, Display = f.RoomName }).ToList();

                    lstlocations.SelectedIDs = template.Locations;
                }

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
    /// Saves the options chosen by the user
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            InventoryReportTemplate template = (InventoryReportTemplate)Session["reportTemplate"];
            Template row = (Template)Session["reportTemplateRow"];
            template.Locations = lstlocations.SelectedIDs;
            template.LocationsSelection = lstlocations.SelectedType;

            Session["reportTemplate"] = template;
            Response.Redirect(Config.DOMAIN() + "desktop/reports/shared/food_usda.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }
}