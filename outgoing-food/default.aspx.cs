using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Default2 : System.Web.UI.Page
{
    private String sortingColumn;
    private Boolean sortAscending;

    //******************************** Page_Load ********************************//
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                sortingColumn = "TimeStamp"; // default sort
                sortAscending = false;
                ViewState["sortingColumn"] = sortingColumn;
                ViewState["sortAscending"] = sortAscending;
            }

            if (ViewState["sortingColumn"] != null)
            {
                sortingColumn = (String)ViewState["sortingColumn"];
                sortAscending = (Boolean)ViewState["sortAscending"];
            }

            updateGridView();   //makes call to update the gridview that is displayed
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    }


    //******************************** updateGridView method ********************************//
    private void updateGridView()
    {
        try
        {
            lblMessage.Text = "";   //empty out the error message field

            //using statement calls the dispose method on the object 
            // and causes the object to go out of scope at the end of the statement
            using (CCSEntities db = new CCSEntities())
            {
                var listFoodOut = (from d in db.FoodOuts
                                       select new
                                       {
                                           d.DistributionID,
                                           d.FoodCategory.CategoryType,
                                           d.Weight,
                                           d.FoodSourceType.FoodSourceType1,
                                           d.TimeStamp,
                                           d.Agency.AgencyName
                                       }).ToList();
                // sort list according to user choice
                switch (sortingColumn)
                {
                    case "Agency":
                        if (sortAscending)
                            listFoodOut.Sort((x, y) => String.Compare(x.AgencyName, y.AgencyName)); // ascending Donor
                        else
                            listFoodOut.Sort((x, y) => String.Compare(y.AgencyName, x.AgencyName)); // descending Donor
                        break;
                    case "Category":
                        if (sortAscending)
                            listFoodOut.Sort((x, y) => String.Compare(x.CategoryType, y.CategoryType)); // ascending Category
                        else
                            listFoodOut.Sort((x, y) => String.Compare(y.CategoryType, x.CategoryType)); // descending Category
                        break;
                    case "Donor":
                        if (sortAscending)
                            listFoodOut.Sort((x, y) => String.Compare(x.FoodSourceType1, y.FoodSourceType1)); // ascending Category
                        else
                            listFoodOut.Sort((x, y) => String.Compare(y.FoodSourceType1, x.FoodSourceType1)); // descending Category
                        break;
                    case "Weight":
                        if (sortAscending)
                            listFoodOut.Sort((x, y) => x.Weight.CompareTo(y.Weight)); // ascending Weight
                        else
                            listFoodOut.Sort((x, y) => y.Weight.CompareTo(x.Weight)); // descending Weight
                        break;
                    case "TimeStamp":
                        if (sortAscending)
                            listFoodOut.Sort((x, y) => DateTime.Compare(x.TimeStamp, y.TimeStamp)); // ascending TimeStamp
                        else
                            listFoodOut.Sort((x, y) => DateTime.Compare(y.TimeStamp, x.TimeStamp)); // descending TimeStamp
                        break;
                }

                DataTable dtFoodOut = new DataTable();     //creates a new data table object

                dtFoodOut.Columns.Add("Id");
                dtFoodOut.Columns.Add("Category"); 
                dtFoodOut.Columns.Add("Weight");
                dtFoodOut.Columns.Add("FoodSourceType1");
                dtFoodOut.Columns.Add("TimeStamp");
                dtFoodOut.Columns.Add("AgencyName");


                for (int i = 0; i < listFoodOut.Count; i++)     //loops through the list of FoodSource results 
                {
                    dtFoodOut.Rows.Add(listFoodOut.ElementAt(i).DistributionID, listFoodOut.ElementAt(i).CategoryType,
                        listFoodOut.ElementAt(i).Weight, listFoodOut.ElementAt(i).FoodSourceType1,
                        listFoodOut.ElementAt(i).TimeStamp, listFoodOut.ElementAt(i).AgencyName);

                } //end of for loop


                //adds the datatable results to the gridview on the default.aspx page if row count more than 0
                if (dtFoodOut.Rows.Count > 0)
                {
                    grdFoodOut.DataSource = dtFoodOut;  //assigns the dataTable object results to the gridview
                    grdFoodOut.DataBind();       //binds the datasource to the gridview
                }

            }//end of the using statement
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    } //end of updateGridView method


    //******************************** btnAddIncomingFood method ********************************//
    protected void btnAddIncomingFood_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("add.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //******************************** paging the gridview ********************************//
    protected void grdFoodIn_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdFoodOut.PageIndex = e.NewPageIndex;
            updateGridView();
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }





    protected void TaskGridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (sortingColumn != null && sortingColumn.Equals(e.SortExpression))    // if the list is already sorted by this column,                                                                  
            {                                                                       // toggle ascending/descending
                sortAscending = !sortAscending;
            }
            else                                                                    // else, set new column and set sorting to ascending
            {
                sortingColumn = e.SortExpression;
                sortAscending = true;
            }

            ViewState["sortingColumn"] = sortingColumn;
            ViewState["sortAscending"] = sortAscending;

            updateGridView();
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
}