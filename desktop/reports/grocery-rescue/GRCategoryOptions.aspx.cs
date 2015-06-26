using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class desktop_reports_grocery_rescue_GRCategoryOptions : System.Web.UI.Page
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
                if (Session["reportTemplateRow"] == null && Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                GroceryRescueReportTemplate template = (GroceryRescueReportTemplate)Session["reportTemplate"];
                txtBakery.Text = template.BakeryUFBID;
                txtDairy.Text = template.DairyUFBID;
                txtProduce.Text = template.ProduceUFBID;
                txtDeli.Text = template.DeliUFBID;
                txtMeat.Text = template.MeatUFBID;
                txtFrozen.Text = template.FrozenUFBID;
                txtDry.Text = template.DryGroceryUFBID;
                txtNonFood.Text = template.NonFoodUFBID;
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
            GroceryRescueReportTemplate template = (GroceryRescueReportTemplate)Session["reportTemplate"];
            template.BakeryUFBID = txtBakery.Text;
            template.DairyUFBID = txtDairy.Text;
            template.ProduceUFBID = txtProduce.Text;
            template.DeliUFBID = txtDeli.Text;
            template.MeatUFBID = txtMeat.Text;
            template.FrozenUFBID = txtFrozen.Text;
            template.DryGroceryUFBID = txtDry.Text;
            template.NonFoodUFBID = txtNonFood.Text;
            Session["reportTemplate"] = template;

            Response.Redirect(Config.DOMAIN() + "desktop/reports/shared/save.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }
}