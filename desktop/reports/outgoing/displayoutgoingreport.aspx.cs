using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

/// <summary>
/// Allows the user to set the options of the outgoing eport.
/// </summary>
public partial class desktop_reports_outgoing_displayoutgoingreport : System.Web.UI.Page
{
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
                LogChange.logChange("Ran Outgoing Report", DateTime.Now, short.Parse(Session["userID"].ToString()));

                if (Session["startDate"] == null || Session["endDate"] == null || Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                DateTime startDate = (DateTime)Session["startDate"];
                DateTime endDate = (DateTime)Session["endDate"];
                OutgoingReportTemplate template = (OutgoingReportTemplate)Session["reportTemplate"];

                ReportDataSource source = new ReportDataSource("DataSet", (DataTable)(LoadData(startDate, endDate, template).Outgoing));
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", startDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", endDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("reportTitle", template.Title));
                if (template.DisplayType.Equals(OutgoingReportTemplate.Display.ShowCategoryType)) 
                    reportViewer.LocalReport.SetParameters(new ReportParameter("showGrandTotalBins", (!template.ShowGrandTotal).ToString()));
                else 
                    reportViewer.LocalReport.SetParameters(new ReportParameter("showGrandTotal", (!template.ShowGrandTotal).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("display", ((int)template.DisplayType).ToString()));

               
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
    /// <param name="enddate">The dataset with the data loaded into it</param>
    /// <param name="template">The template to load options for the query</param>
    /// <returns>The dataset with the data loaded into it</returns>
    private ReportsDataSet LoadData(DateTime startdate, DateTime enddate, OutgoingReportTemplate template)
    {
        ReportsDataSet ds = new ReportsDataSet();

        using (CCSEntities db = new CCSEntities())
        {
            DateTime dt = enddate.AddDays(1);
            List<FoodOut> data = db.FoodOuts.Where(f => (f.TimeStamp >= startdate.Date && f.TimeStamp < dt.Date)).ToList();

            if (template.FoodSourceTypesSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedTypes = new List<short>();
                foreach (var i in template.FoodSourceTypes)
                    selectedTypes.Add(short.Parse(i));

                data = (from c in data
                        where selectedTypes.Contains((short)c.FoodSourceTypeID)
                        select c).ToList();
            }

            if (template.DistributionSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedDistribution = new List<short>();
                foreach (var i in template.DistributionTypes)
                    selectedDistribution.Add(short.Parse(i));

                data = (from c in data
                        where selectedDistribution.Contains((short)c.DistributionTypeID)
                        select c).ToList();
            }

            if (template.AgenciesSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedAgencies = new List<short>();
                foreach (var i in template.Agencies)
                    selectedAgencies.Add(short.Parse(i));

                data = (from c in data
                        where selectedAgencies.Contains((short)c.AgencyID)
                        select c).ToList();
            }

            List<FoodOut> foodInRegularData = new List<FoodOut>();

            List<FoodOut> foodInUSDAData = new List<FoodOut>();

            if (template.CategoriesSelection != ReportTemplate.SelectionType.NONE)
                foodInRegularData = data.Where(f => f.FoodCategory != null).ToList();

            if (template.USDASelection != ReportTemplate.SelectionType.NONE)
                foodInUSDAData = data.Where(f => f.USDACategory != null).ToList();

            if (template.CategoriesSelection == ReportTemplate.SelectionType.REGULAR)
            {
                foodInRegularData = (from c in foodInRegularData
                                     where c.FoodCategory.Perishable == false && c.FoodCategory.NonFood == false
                                     select c).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.PERISHABLE)
            {
                foodInRegularData = (from c in foodInRegularData
                                     where c.FoodCategory.Perishable == true && c.FoodCategory.NonFood == false
                                     select c).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.NONFOOD)
            {
                foodInRegularData = (from c in foodInRegularData
                                     where c.FoodCategory.Perishable == false && c.FoodCategory.NonFood == true
                                     select c).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedCategories = new List<short>();
                foreach (var i in template.FoodCategories)
                    selectedCategories.Add(short.Parse(i));

                foodInRegularData = (from c in foodInRegularData
                                     where selectedCategories.Contains((short)c.FoodCategoryID)
                                     select c).ToList();
            }


            if (template.USDASelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedUSDA = new List<short>();
                foreach (var i in template.USDACategories)
                    selectedUSDA.Add(short.Parse(i));

                foodInUSDAData = (from u in foodInUSDAData
                                  where selectedUSDA.Contains((short)u.USDAID)
                                  select u).ToList();
            }

            foodInRegularData.InsertRange(0, foodInUSDAData);

            data = foodInRegularData;

            foreach (var i in data)
            {
                if(i.FoodCategory != null || i.USDACategory != null)
                    ds.Outgoing.AddOutgoingRow(i.FoodCategory == null? i.USDACategory.Description: i.FoodCategory.CategoryType, i.BinNumber, i.TimeStamp, (double)(i.Count ?? 0), i.Weight, i.Agency == null ? "No-Agency" : i.Agency.AgencyName, i.DistributionType.DistributionType1, i.FoodSourceType.FoodSourceType1);
            }
        }
        return ds;
    }
}