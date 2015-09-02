using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;

/// <summary>
/// Allows the user to set the options of the incoming eport.
/// </summary>
public partial class desktop_reports_incoming_DisplayIncomingReport : System.Web.UI.Page
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
                LogChange.logChange("Ran Incoming Report", DateTime.Now, short.Parse(Session["userID"].ToString()));

                if (Session["startDate"] == null || Session["endDate"] == null || Session["reportTemplate"] == null)
                    Response.Redirect(Config.DOMAIN() + "desktop/reports");

                DateTime startDate = (DateTime)Session["startDate"];
                DateTime endDate = (DateTime)Session["endDate"];
                IncomingReportTemplate template = (IncomingReportTemplate)Session["reportTemplate"];

                ReportDataSource source = new ReportDataSource("DataSet", (DataTable)(LoadData(startDate, endDate, template).Incoming));
                reportViewer.LocalReport.DataSources.Clear();
                reportViewer.LocalReport.DataSources.Add(source);
                reportViewer.LocalReport.SetParameters(new ReportParameter("startDate", startDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("endDate", endDate.ToString("d")));
                reportViewer.LocalReport.SetParameters(new ReportParameter("reportTitle", template.Title));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showGrandTotal", (!template.ShowGrandTotal).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("display", ((int)template.DisplayType).ToString()));
                reportViewer.LocalReport.SetParameters(new ReportParameter("showDonorAddress", (!template.ShowDonorAddress).ToString()));


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
    private ReportsDataSet LoadData(DateTime startdate, DateTime enddate, IncomingReportTemplate template)
    {
        ReportsDataSet ds = new ReportsDataSet();

        

        using (CCSEntities db = new CCSEntities())
        {
            DateTime dt = enddate.AddDays(1);
            List<FoodIn> data = db.FoodIns.Where(f => (f.TimeStamp >= startdate.Date && f.TimeStamp < dt.Date)).ToList();

            if (template.FoodSourceTypesSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedTypes = new List<short>();
                foreach (var i in template.FoodSourceTypes)
                    selectedTypes.Add(short.Parse(i));

                data = (from c in data
                        where selectedTypes.Contains((short)c.FoodSource.FoodSourceTypeID)
                        select c).ToList();
            }

            if (template.FoodSourcesSelection == ReportTemplate.SelectionType.SOME)
            {
                List<short> selectedSources = new List<short>();
                foreach (var i in template.FoodSources)
                    selectedSources.Add(short.Parse(i));

                data = (from c in data
                        where selectedSources.Contains((short)c.FoodSourceID)
                        select c).ToList();
            }


            List<FoodIn> foodInRegularData = new List<FoodIn>();

            if(template.CategoriesSelection != ReportTemplate.SelectionType.NONE)
                foodInRegularData = data.Where(f => f.FoodCategory != null && f.USDACategory == null).ToList();

            List<FoodIn> foodInUSDAData = data.Where(f => f.USDACategory != null && f.FoodCategory == null).ToList();


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

            if (template.USDASelection != ReportTemplate.SelectionType.NONE)
            {
                foodInRegularData.InsertRange(0, foodInUSDAData);
            }

            data = foodInRegularData;


            // Static variables of what they want to come first.    @Author Jake Abel
            const string taxable = "In-Kind (Taxable)";
            const string nonTaxable = "In-Kind (Non-Tax)";
            const string noAgency = "No-Agency";



            /**
                    Before fixing the date sort too.
            */

//            
//                        // ds.Incoming.AddIncomingRow(categoryType, timeStamp, count, weight, foodSource, address, foodSourceType1);
//                        // Sort based on in-Kind (taxable and non-tax) and then 
//                        data.Sort(delegate (FoodIn dis, FoodIn otr)
//                        {
//            
//                            //                dis.FoodSource.FoodSourceType.FoodSourceType1         // 
//                            // Put the taxable first, and then the non taxable, and then whatever
//                            if (dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) || dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable) ||
//                                otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) || otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable))
//                            {
//                                if (dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable))
//                                {
//                                    return -1;
//                                }
//            
//                                if (otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) && !dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable))
//                                {
//                                    return 1;
//                                }
//            
//                                if (dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable))
//                                {
//                                    return -1;
//                                }
//            
//                                if (otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable) && !dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable))
//                                {
//                                    return 1;
//                                }
//            
//                            }
//            
//                            if (dis.FoodSource.FoodSourceType.FoodSourceType1.Contains(taxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Contains(taxable))
//                            {
//                                return 1;
//                            }
//                            if (dis.FoodSource.FoodSourceType.FoodSourceType1.Contains(nonTaxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Contains(nonTaxable))
//                            {
//                                return 1;
//                            }
//            
//                            return dis.FoodSource.Source.CompareTo(otr.FoodSource.Source);
//            
//            
//                            //                return 0;
//                        });




            /**
                Sort the foodIn similar to the food out
                ADDED by
                @Author Jake Abel


                        Modified to include the date sorting. Sept 1, 2015
                        Modified to include the row id sorting Sept 1, 2015
            */

            //////// ds.Incoming.AddIncomingRow(categoryType, timeStamp, count, weight, foodSource, address, foodSourceType1);
            // Sort based on in-Kind (taxable and non-tax) and then 
            data.Sort(delegate (FoodIn dis, FoodIn otr)
            {
                
//                dis.FoodSource.FoodSourceType.FoodSourceType1         // 
                // Put the taxable first, and then the non taxable, and then whatever
                if (dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) || dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable) ||
                    otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) || otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable))
                {
                    if (dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable))
                    {
                        return -1;
                    }

                    if (otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable) && !dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(taxable))
                    {
                        return 1;
                    }

                    if (dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable))
                    {
                        return -1;
                    }

                    if (otr.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable) && !dis.FoodSource.FoodSourceType.FoodSourceType1.Equals(nonTaxable))
                    {
                        return 1;
                    }

                }

                if (dis.FoodSource.FoodSourceType.FoodSourceType1.Contains(taxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Contains(taxable))
                {
                    return 1;
                }
                if (dis.FoodSource.FoodSourceType.FoodSourceType1.Contains(nonTaxable) && !otr.FoodSource.FoodSourceType.FoodSourceType1.Contains(nonTaxable))
                {
                    return 1;
                }

//                return dis.FoodSource.Source.CompareTo(otr.FoodSource.Source);
                if (dis.FoodSource.Source.CompareTo(otr.FoodSource.Source) == 1 ||
                    dis.FoodSource.Source.CompareTo(otr.FoodSource.Source) == -1)
                {
                    return dis.FoodSource.Source.CompareTo(otr.FoodSource.Source);
                }

//                return dis.TimeStamp.CompareTo(otr.TimeStamp);
                int timeSort = dis.TimeStamp.CompareTo(otr.TimeStamp);
                if (timeSort == 1 || timeSort == -1)
                {
                    return timeSort;
                }

                // Sort by the food in ID last of all.
                return dis.FoodInID.CompareTo(otr.FoodInID);


            });





            //ORIGINAL VERSION, @Author Nittaya P.
            //            foreach (var i in data)
            //            {
            //                if (i.FoodCategory != null || i.USDACategory != null)
            //                {
            //                    string address = string.Format("{0} {1} {2}, {3} {4}", i.FoodSource.Address.StreetAddress1 ?? "",
            //                        i.FoodSource.Address.StreetAddress2 ?? "", i.FoodSource.Address.City.CityName ?? "",
            //                        i.FoodSource.Address.State == null ? "" : i.FoodSource.Address.State.StateShortName, 
            //                        i.FoodSource.Address.Zipcode.ZipCode1 ?? "");
            //                    ds.Incoming.AddIncomingRow(i.FoodCategory == null ? i.USDACategory.Description : i.FoodCategory.CategoryType, i.TimeStamp, i.Count == null ? 0 : (double)i.Count, i.Weight == null ? 0 : (double)i.Weight, i.FoodSource.Source, address, i.FoodSource.FoodSourceType.FoodSourceType1);
            //                }
            //            }


            /**
                    Expanded version for simplicities sake
                    @Author Jake Abel            
            */

            
            foreach (var i in data)
            {
                if (i.FoodCategory != null || i.USDACategory != null)
                {
                    string address = string.Format("{0} {1} {2}, {3} {4}", i.FoodSource.Address.StreetAddress1 ?? "",
                        i.FoodSource.Address.StreetAddress2 ?? "", i.FoodSource.Address.City.CityName ?? "",
                        i.FoodSource.Address.State == null ? "" : i.FoodSource.Address.State.StateShortName,
                        i.FoodSource.Address.Zipcode.ZipCode1 ?? "");
                    
                    string categoryType;
                    if (i.FoodCategory == null)
                    {
                        categoryType = i.USDACategory.Description;
                    }
                    else
                    {
                        categoryType = i.FoodCategory.CategoryType;
                    }

                    DateTime timeStamp = i.TimeStamp;

                    double count;                                               
                    if (i.Count == null)
                    {
                        count = 0;
                    }
                    else
                    {
                        count = (double)i.Count;
                    }
              
                    double weight;
                    if (i.Weight == null)
                    {
                        weight = 0;
                    }
                    else
                    {
                        weight = (double) i.Weight;
                    }
                    
                    string foodSource = i.FoodSource.Source;
                    // address
                    string foodSourceType1 = i.FoodSource.FoodSourceType.FoodSourceType1;


                    // Original version         @Author Nittaya
                    //ds.Incoming.AddIncomingRow(i.FoodCategory == null ? i.USDACategory.Description : i.FoodCategory.CategoryType, i.TimeStamp, 
                    //    i.Count == null ? 0 : (double)i.Count, i.Weight == null ? 0 : (double)i.Weight, i.FoodSource.Source, address, 
                    //    i.FoodSource.FoodSourceType.FoodSourceType1


                    // Condensed version @Author Jake Abel
                    ds.Incoming.AddIncomingRow(categoryType, timeStamp, count, weight, foodSource, address, foodSourceType1);


                }
            }

        }
        return ds;
    }
}