using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Allows the user to specify what food sources they want to display on the report.
/// </summary>
public partial class desktop_reports_incoming_FoodSources : System.Web.UI.Page
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

                IncomingReportTemplate template = (IncomingReportTemplate)Session["reportTemplate"];


                using (CCSEntities db = new CCSEntities())
                {
                    List<FoodSource> foodSources = null;

                    if (template.FoodSourceTypesSelection == ReportTemplate.SelectionType.SOME)
                    {
                        List<short> selectedTypes = new List<short>();
                        foreach (var i in template.FoodSourceTypes)
                            selectedTypes.Add(short.Parse(i));

                        foodSources = (from f in db.FoodSources
                                        where selectedTypes.Contains((short)f.FoodSourceTypeID)
                                        select f).ToList();
                    }
                    else
                    {
                        foodSources = (from f in db.FoodSources
                                       select f).ToList();
                    }
                    lstFoodSources.SelectedType = template.FoodSourcesSelection;
                    lstFoodSources.AvailableList = (from f in foodSources
                                                    select new { ID = f.FoodSourceID, Display = f.Source }).ToList();

                    lstFoodSources.SelectedIDs = template.FoodSources;
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
            IncomingReportTemplate template = (IncomingReportTemplate)Session["reportTemplate"];
            Template row = (Template)Session["reportTemplateRow"];
            template.FoodSources = lstFoodSources.SelectedIDs;
            template.FoodSourcesSelection = lstFoodSources.SelectedType;

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