using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;


public partial class container_print : System.Web.UI.Page
{
    /// <summary>
    /// Ids of the food in records passed in the session
    /// </summary>
    public List<short> ids;

    /// <summary>
    /// Loads the records passed in the session into the interface
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["newFoodInIDs"] != null)
            {
                ids = (List<short>)Session["newFoodInIDs"];

                using (CCSEntities db = new CCSEntities())
                {
                    var foodInRecords = (from c in db.FoodIns
                                         where ids.Contains(c.FoodInID)
                                         select c);


                    lblAddress.Text = foodInRecords.First().FoodSource.Address.StreetAddress1 + " " + foodInRecords.First().FoodSource.Address.StreetAddress2;
                    lblCity.Text = foodInRecords.First().FoodSource.Address.City.CityName;
                    lblDateRecived.Text = foodInRecords.First().TimeStamp.ToString("d");
                    lblDonor.Text = foodInRecords.First().FoodSource.Source;
                    lblState.Text = foodInRecords.First().FoodSource.Address.State.StateShortName;
                    lblZip.Text = foodInRecords.First().FoodSource.Address.Zipcode.ZipCode1;

                    foreach (var foodIn in foodInRecords)
                    {
                        System.Web.UI.HtmlControls.HtmlTableRow row = new System.Web.UI.HtmlControls.HtmlTableRow();
                        System.Web.UI.HtmlControls.HtmlTableCell cat = new System.Web.UI.HtmlControls.HtmlTableCell();
                        System.Web.UI.HtmlControls.HtmlTableCell weight = new System.Web.UI.HtmlControls.HtmlTableCell();

                        cat.InnerText = foodIn.FoodCategory == null ? foodIn.USDACategory.Description : foodIn.FoodCategory.CategoryType;
                        weight.InnerText = foodIn.Weight.ToString();
                        row.Cells.Add(weight);
                        row.Cells.Add(cat);
                        row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell());
                        tableItems.Rows.Add(row);
                    }

                    if (foodInRecords.Count() < 5)
                        AddBlankRows(5 - foodInRecords.Count());
                }
            }
            else
            {
                AddBlankRows(6);
            }

        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    /// <summary>
    /// Adds a number of blank rows to fill in empty space
    /// </summary>
    /// <param name="count"></param>
    private void AddBlankRows(int count)
    {
        for (int i = 0; i < count; i++)
        {
            System.Web.UI.HtmlControls.HtmlTableRow row = new System.Web.UI.HtmlControls.HtmlTableRow();
            row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell());
            row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell());
            row.Cells.Add(new System.Web.UI.HtmlControls.HtmlTableCell());
            tableItems.Rows.Add(row);

        }
    }

}