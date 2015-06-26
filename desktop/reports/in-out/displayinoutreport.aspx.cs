using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;


/// <summary>
/// Allows the user to set the options of the in\out report.
/// </summary>
public partial class desktop_reports_in_out_displayinoutreport : System.Web.UI.Page
{
    /// <summary>
    /// Loads the Data into the report and sets the paramters
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try{
            if (!this.IsPostBack)
            {
                LogChange.logChange("Ran In\\Out Report", DateTime.Now, short.Parse(Session["userID"].ToString()));

                if (Session["startDate"] == null || Session["endDate"] == null || Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                DateTime startDate = (DateTime)Session["startDate"];
                DateTime endDate = (DateTime)Session["endDate"];
                InOutReportTemplate template = (InOutReportTemplate)Session["reportTemplate"];

                ReportDataSource source = new ReportDataSource("DataSet", (DataTable)(LoadData(startDate, endDate, template).InOut));
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", startDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", endDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("reportTitle", template.Title));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showGrandTotal", (!template.ShowGrandTotal).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showCount", (!template.ShowCount).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showWeight", (!template.ShowWeight).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showCurrentBalance", (!template.ShowCurrentBalance).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showRegular", (!(template.CategoriesSelection != ReportTemplate.SelectionType.NONE)).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showUSDA", (!(template.USDASelection != ReportTemplate.SelectionType.NONE)).ToString()));


                reportViewer.DataBind();
                reportViewer.LocalReport.Refresh();

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
    /// Loads the data into a dataset depending on the options set by the user.
    /// </summary>
    /// <param name="startdate">The date to start from</param>
    /// <param name="enddate">The date to end grabbing transactions</param>
    /// <param name="template">The template to load options for the query</param>
    /// <returns>The dataset with the data loaded into it</returns>
    private ReportsDataSet LoadData(DateTime startdate, DateTime enddate, InOutReportTemplate template)
    {
        ReportsDataSet ds = new ReportsDataSet();

        using (CCSEntities db = new CCSEntities())
        {
            DateTime dt = enddate.AddDays(1);   
            List<FoodCategory> foodCategoriesData = null;
            List<USDACategory> usdaCategoriesData = null;


            foodCategoriesData = GenerateDatasetHelper.GetFoodCategoriesBySelection(template, db);

            if (template.USDASelection == ReportTemplate.SelectionType.ALL)
            {
                usdaCategoriesData = db.USDACategories.OrderBy(f => f.Description).ToList();
            }
            else if (template.USDASelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedUSDA = new List<short>();
                foreach (var i in template.USDACategories)
                    selectedUSDA.Add(short.Parse(i));

                usdaCategoriesData = (from u in db.USDACategories
                                      where  selectedUSDA.Contains(u.USDAID)
                                      orderby u.Description
                                      select u).ToList();
            }


            if (template.CategoriesSelection != ReportTemplate.SelectionType.NONE)
            {
                foreach (var f in foodCategoriesData)
                {

                    double weightIn = (from w in f.FoodIns
                                       where (w.TimeStamp >= startdate.Date && w.TimeStamp < dt.Date)
                                        select (double)w.Weight).Sum();

                    double weightOut = (from w in f.FoodOuts
                                        where (w.TimeStamp >= startdate.Date && w.TimeStamp < dt.Date)
                                      
                                        select w.Weight).Sum();

                    double countIn = (from w in f.FoodIns
                                      where (w.TimeStamp >= startdate.Date && w.TimeStamp < dt.Date)
                                          && w.Count != null
                                      select (double)w.Count).Sum();

                    double countOut = (from w in f.FoodOuts
                                       where (w.TimeStamp >= startdate.Date && w.TimeStamp < dt.Date)
                                           && w.Count != null
                                       select (double)w.Count).Sum();

                    double currentBalanceWeight = 0;
                    double currentBalanceCount = 0;

                    if (template.ShowCurrentBalance)
                    {
                        currentBalanceWeight = (from c in f.Containers
                                          select (double)c.Weight).Sum();
                        currentBalanceCount = (from c in f.Containers
                                                where c.Cases != null
                                                select (double)c.Cases).Sum();
                    }
                    ds.InOut.AddInOutRow(f.CategoryType, false, currentBalanceWeight, currentBalanceCount, weightIn, weightOut, countIn, countOut);
                }
            }

            if (template.USDASelection != ReportTemplate.SelectionType.NONE)
            {
                foreach (var f in usdaCategoriesData)
                {

                    double weightIn = (from w in f.FoodIns
                                       where w.TimeStamp < dt.Date && w.TimeStamp >= startdate
                                       select (double)w.Weight).Sum();

                    double weightOut = (from w in f.FoodOuts
                                        where w.TimeStamp < dt.Date && w.TimeStamp >= startdate
                                        select w.Weight).Sum();

                    double countIn = (from w in f.FoodIns
                                      where w.TimeStamp < dt.Date && w.TimeStamp >= startdate && w.Count != null
                                      select (double)w.Count).Sum();

                    double countOut = (from w in f.FoodOuts
                                       where w.TimeStamp < dt.Date && w.TimeStamp >= startdate && w.Count != null
                                       select (double)w.Count).Sum();

                    double currentBalanceWeight = 0;
                    double currentBalanceCount = 0;

                    if (template.ShowCurrentBalance)
                    {
                        currentBalanceWeight = (from c in f.Containers
                                                select (double)c.Weight).Sum();
                        currentBalanceCount = (from c in f.Containers
                                               where c.Cases != null
                                               select (double)c.Cases).Sum();
                    }
                    ds.InOut.AddInOutRow("(" + f.USDANumber + ")" + f.Description, true, currentBalanceWeight, currentBalanceCount, weightIn, weightOut, countIn, countOut);
                }
            }
        }
        return ds;
    }
}