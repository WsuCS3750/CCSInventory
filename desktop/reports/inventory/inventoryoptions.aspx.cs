using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Allows the user to set the options of the Inventory report.
/// </summary>
public partial class desktop_reports_incoming_InventoryOptions : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                if (Session["reportTemplateRow"] == null || Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                InventoryReportTemplate template = (InventoryReportTemplate)Session["reportTemplate"];

                if (template.SortByChoice == InventoryReportTemplate.SortBy.Category)
                    rbCategory.Checked = true;
                else if (template.SortByChoice == InventoryReportTemplate.SortBy.Location)
                    rbLocation.Checked = true;
                else 
                    rbBinNumber.Checked = true;

                if (template.ShowOnlyCategoryTotals)
                    rbTotal.Checked = true;
                else
                    rbFoodBin.Checked = true;

                cbGrandTotal.Checked = template.ShowGrandTotal;
                txtTitle.Text = template.Title;
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

            if (rbCategory.Checked)
                template.SortByChoice = InventoryReportTemplate.SortBy.Category;
            else if (rbLocation.Checked)
                template.SortByChoice = InventoryReportTemplate.SortBy.Location;
            else
                template.SortByChoice = InventoryReportTemplate.SortBy.BinNumber;

            template.ShowOnlyCategoryTotals = rbTotal.Checked;
            template.ShowGrandTotal = cbGrandTotal.Checked;
            template.Title = txtTitle.Text;

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

    /// <summary>
    /// Disables "Total of Category" radio button when Sort by Bin Number is selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void RadioButton_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rbBinNumber.Checked == true)
            {
                rbFoodBin.Checked = true;
                rbTotal.Checked = false;
                rbTotal.Enabled = false;
            }
            else
            {
                rbTotal.Enabled = true;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }
}