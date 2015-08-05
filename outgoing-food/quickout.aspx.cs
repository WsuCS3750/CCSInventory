using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class outgoing_food_quickout : System.Web.UI.Page
{
    List<FoodOut> foodOutRecords;

    protected void Page_Load(object sender, EventArgs e)
    {
        foodOutRecords = (List<FoodOut>)Session["foodOut"];

        if (!Page.IsPostBack)
        {
            
                if (Session["foodOut"] != null)
                {
                    LoadDropDowns();
                    LoadGrid();
                }
            
        }
    }

    private void LoadDropDowns()
    {
            using (CCSEntities db = new CCSEntities())
            {
                ddlDistributionType.DataSource = db.DistributionTypes.ToList().OrderBy(x => x.DistributionType1);
                ddlDistributionType.DataBind();

                ddlAgency.DataSource = db.Agencies.ToList().OrderBy(x => x.AgencyName);
                ddlAgency.DataBind();


                ddlDonorType.DataSource = db.FoodSourceTypes.ToList().OrderBy(x => x.FoodSourceType1);
                ddlDonorType.DataBind();

                foreach(var f in foodOutRecords)
                {
                    if (f.FoodSourceTypeID != null)
                        ddlDonorType.SelectedValue = f.FoodSourceTypeID.ToString();
                }
            }
    }

    private void LoadGrid()
    {
        using (CCSEntities db = new CCSEntities())
        {
            grdFoodOut.DataSource = (from f in foodOutRecords
                                    select new { id = f.DistributionID, category = (f.FoodCategory == null ? f.USDACategory.Description : f.FoodCategory.CategoryType), weight = f.Weight, cases = f.Count, binNumber = f.BinNumber }).ToList();

            grdFoodOut.DataBind();
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        using (CCSEntities db = new CCSEntities())
        {
            short Agency = 0;
            short FoodSourceType = short.Parse(ddlDonorType.SelectedValue);
            short DistributionType = short.Parse(ddlDistributionType.SelectedValue);

            if (ddlAgency.SelectedIndex != 0)
                Agency = short.Parse(ddlAgency.SelectedValue);
            foreach (var foodOut in foodOutRecords)
            {
                FoodOut newFoodOut = new FoodOut();
                newFoodOut.DistributionTypeID = DistributionType;
                newFoodOut.FoodSourceTypeID = FoodSourceType;
                newFoodOut.FoodCategoryID = foodOut.FoodCategoryID;
                newFoodOut.USDAID = foodOut.USDAID;
                newFoodOut.TimeStamp = foodOut.TimeStamp;
                newFoodOut.Count = foodOut.Count;
                newFoodOut.Weight = foodOut.Weight;
                newFoodOut.BinNumber = foodOut.BinNumber;
                newFoodOut.DateCreated = DateTime.Now;
        
                if(ddlAgency.SelectedIndex != 0)
                    newFoodOut.AgencyID = Agency;

                db.FoodOuts.Add(newFoodOut);
            }

            db.SaveChanges();
        }

        if (Request.QueryString["redirect"] != null)
            Response.Redirect(Request.QueryString["redirect"]);

        pnlInput.Visible = false;
        pnlSuccess.Visible = true;
    }
}