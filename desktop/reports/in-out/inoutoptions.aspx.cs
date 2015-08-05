using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Allows the user to set the options of the in\out report.
/// </summary>
public partial class desktop_reports_incoming_In_OutOptions : System.Web.UI.Page
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
                if (Session["reportTemplateRow"] == null)
                    Response.Redirect("default.aspx");

                InOutReportTemplate reportTemplate = (InOutReportTemplate)Session["reportTemplate"];
                cbGrandTotal.Checked = reportTemplate.ShowGrandTotal;
                cbShowCount.Checked = reportTemplate.ShowCount;
                cbShowCurrentBalance.Checked = reportTemplate.ShowCurrentBalance;
                cbShowWeight.Checked = reportTemplate.ShowWeight;
                txtTitle.Text = reportTemplate.Title;
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
            InOutReportTemplate reportTemplate = (InOutReportTemplate)Session["reportTemplate"];
            reportTemplate.ShowGrandTotal = cbGrandTotal.Checked;
            reportTemplate.ShowCurrentBalance = cbShowCurrentBalance.Checked;
            reportTemplate.ShowCount = cbShowCount.Checked;
            reportTemplate.ShowWeight = cbShowWeight.Checked;
            reportTemplate.Title = txtTitle.Text;

            Session["reportTemplate"] = reportTemplate;
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