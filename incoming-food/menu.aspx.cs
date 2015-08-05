using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class incoming_food_menu : System.Web.UI.Page
{
    List<short> foodInIDs = new List<short>();

    protected void Page_Load(object sender, EventArgs e)
    {
        foodInIDs = (List<short>)Session["newFoodInIDs"];

        if (!Page.IsPostBack && Session["newFoodInIDs"] != null)
        {

            LoadGrid(foodInIDs);
        }
    }

    private void LoadGrid(List<short> ids)
    {
        using(CCSEntities db = new CCSEntities())
        {
            grdFoodIn.DataSource = (from f in db.FoodIns
                                    where  ids.Contains(f.FoodInID)
                                    select new { id = f.FoodInID, category = (f.FoodCategory == null? f.USDACategory.Description : f.FoodCategory.CategoryType), weight = f.Weight}).ToList();

            grdFoodIn.DataBind();
        }
    }

    protected void grdTemplates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            index = Convert.ToInt32(grdFoodIn.DataKeys[index].Value);

            if (e.CommandName == "moveOut")
            {
                MoveOut(index);
            }
            else if (e.CommandName == "addContain")
            {
                AddContainer(index);
            }

        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("~/errorpages/error.aspx");
        }
    }

    private void AddContainer(int index)
    {
        Container container = new Container();
        FoodIn foodInRecord = null;

        using (CCSEntities db = new CCSEntities())
        {
            foodInRecord = db.FoodIns.Find(index);

            
            container.Cases = foodInRecord.Count;
            container.FoodCategoryID = foodInRecord.FoodCategoryID;
            container.FoodSourcesTypeID = foodInRecord.FoodSource.FoodSourceTypeID;
            container.USDAID = foodInRecord.USDAID;
            container.isUSDA = foodInRecord.USDACategory != null;
            container.Weight = foodInRecord.Weight;
        }
        Session["container"] = container;
        Response.Redirect("~/container/add.aspx");
    }

    private void MoveOut(int index)
    {
        List<FoodOut> foodOut = new List<FoodOut>();
        FoodIn foodInRecord = null;

        using (CCSEntities db = new CCSEntities())
        {
            foodInRecord = db.FoodIns.Find(index);

            FoodOut newFoodOut = new FoodOut();
            newFoodOut.TimeStamp = foodInRecord.TimeStamp;
            newFoodOut.FoodCategory = foodInRecord.FoodCategory;
            newFoodOut.USDACategory = foodInRecord.USDACategory;
            newFoodOut.FoodCategoryID = foodInRecord.FoodCategoryID;
            newFoodOut.FoodSourceTypeID = foodInRecord.FoodSource.FoodSourceTypeID;
            newFoodOut.USDAID = foodInRecord.USDAID;
            newFoodOut.Weight = (double)foodInRecord.Weight;
            newFoodOut.Count = foodInRecord.Count ?? 0;
            foodOut.Add(newFoodOut);
        }
        Session["foodOut"] = foodOut;
        Response.Redirect("~/outgoing-food/quickout.aspx");
    }

    protected void btnMoveAllOut_Click(object sender, EventArgs e)
    {

        List<FoodOut> foodOut = new List<FoodOut>();
        IEnumerable<FoodIn> foodInRecords = null;

        using (CCSEntities db = new CCSEntities())
        {
            foodInRecords = db.FoodIns.Where(x => foodInIDs.Contains(x.FoodInID));

            foreach (var foodInRecord in foodInRecords)
            {
                FoodOut newFoodOut = new FoodOut();
                newFoodOut.TimeStamp = foodInRecord.TimeStamp;
            	newFoodOut.FoodCategory = foodInRecord.FoodCategory;
            	newFoodOut.USDACategory = foodInRecord.USDACategory;
            	newFoodOut.FoodCategoryID = foodInRecord.FoodCategoryID;
           	newFoodOut.FoodSourceTypeID = foodInRecord.FoodSource.FoodSourceTypeID;
            	newFoodOut.USDAID = foodInRecord.USDAID;
            	newFoodOut.Weight = (double)foodInRecord.Weight;
            	newFoodOut.Count = foodInRecord.Count ?? 0;
                foodOut.Add(newFoodOut);
            }
        }

        Session["foodOut"] = foodOut;
        Response.Redirect("~/outgoing-food/quickout.aspx");

    }

    protected void btnPrintInKind_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/incoming-food/print.aspx");
    }
    protected void btnNewTransaction_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/incoming-food/add.aspx");
    }


    protected void MoveOutClick(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        int rowIndex = Convert.ToInt32(btn.Attributes["RowIndex"]);
        int index = Convert.ToInt32(grdFoodIn.DataKeys[rowIndex].Value);
        MoveOut(index);
    }

    protected void AddContainerClick(object sender, EventArgs e)
    {
        Button btn = sender as Button;
        int rowIndex = Convert.ToInt32(btn.Attributes["RowIndex"]);
        int index = Convert.ToInt32(grdFoodIn.DataKeys[rowIndex].Value);
        AddContainer(index);
    }
}