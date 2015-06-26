using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

/// <summary>
/// Displays the Grocery Rescue Report
/// </summary>
public partial class desktop_reports_displayGroceryRescueReport : System.Web.UI.Page
{
    /// <summary>
    /// The Data used to display on the report
    /// </summary>
    public DataTable source { get; set; }

    /// <summary>
    /// Loads the Data into the report and sets the paramters
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.IsPostBack)
            {
                LogChange.logChange("Ran Grocery Rescue Report", DateTime.Now, short.Parse(Session["userID"].ToString()));

                if (Session["startDate"] == null || Session["endDate"] == null || Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                DateTime startDate = (DateTime)Session["startDate"];
                DateTime endDate = (DateTime)Session["endDate"];
                GroceryRescueReportTemplate template = (GroceryRescueReportTemplate)Session["reportTemplate"];

                ReportDataSource source = new ReportDataSource("dataSet", (DataTable)(LoadData(startDate, endDate).GroceryRescue));
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", startDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", endDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Agency", template.Agency));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Phone", template.Phone));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Contact", template.Contact));
                reportViewer.LocalReport.SetParameters(new ReportParameter("Email", template.Email));
                reportViewer.LocalReport.SetParameters(new ReportParameter("mcNum", template.MC));
                reportViewer.LocalReport.SetParameters(new ReportParameter("reNum", template.RE));
                reportViewer.LocalReport.SetParameters(new ReportParameter("comments", template.AdditonalComments));
                reportViewer.LocalReport.SetParameters(new ReportParameter("bakery", template.BakeryUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("dairy", template.DairyUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("produce", template.ProduceUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("deli", template.DeliUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("meat", template.MeatUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("frozen", template.FrozenUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("dry", template.DryGroceryUFBID));
                reportViewer.LocalReport.SetParameters(new ReportParameter("nonfood", template.NonFoodUFBID));

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
    /// <param name="startDate">The date to start from</param>
    /// <param name="endDate">The date to end grabbing transactions</param>
    /// <returns>The dataset with the data loaded into it</returns>
    private ReportsDataSet LoadData(DateTime startDate, DateTime endDate)
    {

        List<FoodSource> data;
        ReportsDataSet ds = new ReportsDataSet();

 

        using (CCSEntities db = new CCSEntities())
        {
            data = (from f in db.FoodSources
                    where f.FoodSourceType.FoodSourceType1 == "Grocery Rescue"
                    orderby f.Source
                    select f).ToList();

           
            foreach (FoodSource row in data)
            {
                string storeName = row.Source;
                Address add = row.Address;
                string storeAddress = string.Format("{0} {1} {2}", add.StreetAddress1, add.StreetAddress2, add.City.CityName);
                string storeID = row.StoreID;
                DateTime dtEnd = endDate.AddDays(1);

                var foodIn = from f in row.FoodIns
                             where (f.TimeStamp >= startDate.Date && f.TimeStamp < dtEnd.Date)
                             select f;
                decimal bakeryWeight = (from f in foodIn
                                       where f.FoodCategory.CategoryType == "Bakery"
                                       select f.Weight).Sum();
                decimal dairyWeight = (from f in foodIn
                                      where f.FoodCategory.CategoryType == "Dairy"
                                      select f.Weight).Sum();
                decimal meatWeight = (from f in foodIn
                                     where f.FoodCategory.CategoryType == "Meat"
                                     select f.Weight).Sum();
                decimal deliWeight = (from f in foodIn
                                         where f.FoodCategory.CategoryType == "Deli"
                                         select f.Weight).Sum();
                decimal produceWeight = (from f in foodIn
                                        where f.FoodCategory.CategoryType == "Produce"
                                        select f.Weight).Sum();
                decimal dryWeight = (from f in foodIn
                                     where f.FoodCategory.CategoryType == "Dry"
                                     select f.Weight).Sum();
                decimal frozenWeight = (from f in foodIn
                                        where f.FoodCategory.CategoryType == "Frozen"
                                        select f.Weight).Sum();
                decimal nonFoodWeight = (from f in foodIn
                                         where f.FoodCategory.CategoryType == "Non-food"
                                         select f.Weight).Sum();

                if(bakeryWeight + dairyWeight + meatWeight + deliWeight + produceWeight + dryWeight + frozenWeight + nonFoodWeight > 0)
                    ds.GroceryRescue.AddGroceryRescueRow(storeAddress, storeName, storeID, bakeryWeight.ToString(), dairyWeight.ToString(), meatWeight.ToString(), produceWeight.ToString(), dryWeight.ToString(), frozenWeight.ToString(), nonFoodWeight.ToString(), deliWeight.ToString());

            }
        }
        return ds;
    }
}