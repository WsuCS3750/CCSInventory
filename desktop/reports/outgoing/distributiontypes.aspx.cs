using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Allows the user to specify what distribution types they want to display on the report.
/// </summary>
public partial class desktop_reports_incoming_DistributionType : System.Web.UI.Page
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

                OutgoingReportTemplate template = (OutgoingReportTemplate)Session["reportTemplate"];

                using (CCSEntities db = new CCSEntities())
                {
                    lstdistributiontypes.SelectedType = template.DistributionSelection;
                    lstdistributiontypes.AvailableList = (from f in db.DistributionTypes
                                                    select new { ID = f.DistributionTypeID, Display = f.DistributionType1 }).ToList();

                    lstdistributiontypes.SelectedIDs = template.DistributionTypes;
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
        OutgoingReportTemplate template = (OutgoingReportTemplate)Session["reportTemplate"];
        Template row = (Template)Session["reportTemplateRow"];
        template.DistributionTypes = lstdistributiontypes.SelectedIDs;
        template.DistributionSelection = lstdistributiontypes.SelectedType;

        Session["reportTemplate"] = template;
        Response.Redirect(Config.DOMAIN() + "desktop/reports/outgoing/agencies.aspx");
    }
}