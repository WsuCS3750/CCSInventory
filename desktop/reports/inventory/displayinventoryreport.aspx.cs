using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

/// <summary>
/// Displays the Inventory Report
/// </summary>
public partial class desktop_reports_inventory_displayinventoryreport : System.Web.UI.Page
{
    /// <summary>
    /// The report diretory for displaying by Category
    /// </summary>
    string ReportByCategory = @"desktop\reports\inventory\InventoryByCategory.rdlc";

    /// <summary>
    /// The report diretory for displaying by Loacation
    /// </summary>
    string ReportByLocation = @"desktop\reports\inventory\InventoryByLocation.rdlc";

    /// <summary>
    /// The report directory for displaying Bin Number
    /// </summary>
    string ReportByBinNumber = @"desktop\reports\inventory\InventoryByBinNumber.rdlc";


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
                LogChange.logChange("Ran Inventory Report", DateTime.Now, short.Parse(Session["userID"].ToString()));

                if (Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");


                InventoryReportTemplate template = (InventoryReportTemplate)Session["reportTemplate"];

                if (template.SortByChoice == InventoryReportTemplate.SortBy.Category)
                    reportViewer.LocalReport.ReportPath = ReportByCategory;
                else if (template.SortByChoice == InventoryReportTemplate.SortBy.Location)
                    reportViewer.LocalReport.ReportPath = ReportByLocation;
                else
                    reportViewer.LocalReport.ReportPath = ReportByBinNumber;


                ReportDataSource source = new ReportDataSource("Inventory", (DataTable)(LoadData(template).Inventory));
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.LocalReport.SetParameters(new ReportParameter("reportTitle", template.Title));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showBins", (template.ShowOnlyCategoryTotals).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showGrandTotal", (!template.ShowGrandTotal).ToString()));


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
    /// Loads the data specified by the template into a dataset
    /// </summary>
    /// <param name="template">The template to base the queries on</param>
    /// <returns>The dataset with the data laoded into it.</returns>
    private ReportsDataSet LoadData(InventoryReportTemplate template)
    {
        ReportsDataSet ds = new ReportsDataSet();
        using (CCSEntities db = new CCSEntities())
        {
            List<Container> regularContainers = new List<Container>();
            List<Container> usdaContainers = new List<Container>();

            if (template.USDASelection == ReportTemplate.SelectionType.ALL)
            {
                usdaContainers = db.Containers.Where(c => c.isUSDA == true).ToList();
            }
            else if (template.USDASelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> usdaCategories = new List<short>();
                foreach (var i in template.FoodCategories)
                    usdaCategories.Add(short.Parse(i));

                usdaContainers = (from c in db.Containers
                                     where c.isUSDA == true && usdaCategories.Contains((short)c.USDAID)
                                     select c).ToList();
            }


            if (template.CategoriesSelection == ReportTemplate.SelectionType.ALL)
            {
                regularContainers = db.Containers.Where( c => c.isUSDA == false).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.REGULAR)
            {
                regularContainers = (from c in db.Containers
                              where  c.isUSDA == false && c.FoodCategory.Perishable == false && c.FoodCategory.NonFood == false
                              select c).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.PERISHABLE)
            {
                regularContainers = (from c in db.Containers
                                     where c.isUSDA == false && c.FoodCategory.Perishable == true && c.FoodCategory.NonFood == false
                              select c).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.NONFOOD)
            {
                regularContainers = (from c in db.Containers
                              where c.isUSDA == false && c.FoodCategory.Perishable == false && c.FoodCategory.NonFood == true
                              select c).ToList();
            }
            else if (template.CategoriesSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedCategories = new List<short>();
                foreach (var i in template.FoodCategories)
                    selectedCategories.Add(short.Parse(i));

                regularContainers = (from c in db.Containers
                                     where c.isUSDA == false &&  selectedCategories.Contains((short)c.FoodCategoryID)
                                       select c).ToList();
            }


       


            regularContainers.AddRange(usdaContainers);


            List<short> selectedLocations = new List<short>();

            if (template.LocationsSelection == ReportTemplate.SelectionType.SOME)
            {

                foreach (var i in template.Locations)
                    selectedLocations.Add(short.Parse(i));

                regularContainers = (from c in regularContainers
                                     where selectedLocations.Contains(c.Location.LocationID)
                                     select c).ToList();


            }

            List<int> foundLocations = new List<int>();

            foreach (var c in regularContainers)
            {
                if(c.FoodCategory != null || c.USDACategory != null)
                    ds.Inventory.AddInventoryRow(c.Location.RoomName, c.USDACategory != null ? c.USDACategory.Description : c.FoodCategory.CategoryType, c.BinNumber.ToString(), c.Cases == null ? 0 : (double)c.Cases, (double)c.Weight);



                if (!foundLocations.Contains(c.LocationID))
                    foundLocations.Add(c.LocationID);

            }

            if (template.LocationsSelection != ReportTemplate.SelectionType.SOME)
                selectedLocations = db.Locations.Select(x => x.LocationID).ToList();

            foreach(var l in selectedLocations.Where( x => !(foundLocations.Contains(x))))
            {
                var location = db.Locations.FirstOrDefault( x => x.LocationID == l);
                ds.Inventory.AddInventoryRow(location.RoomName, "EMPTY", "", 0, 0);
            }

        }
        return ds;
    }
}