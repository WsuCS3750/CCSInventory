using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Verify : System.Web.UI.Page
{
    private int id;
    private List<Location> lstLocations;
    private List<FoodCategory> lstFoodCategories;
    private List<USDACategory> lstUSDACategories;
    private List<FoodSourceType> foodSourceList;
    private DataTable dtChanges;
    private Container container;
    
    private List<Container> lstUnverifiedContainers;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                lblMessage.Text = "";

                if (Session["unverified"] == null ||
                    Session["changes"] == null ||
                    Request.QueryString["id"] == null ||
                    !int.TryParse(Request.QueryString["id"], out id))
                {
                    lblMessage.Text = "The page failed to load. Please try again later.";
                }

                else
                {
                    pullSession();
                    lookupContainer();
                }

            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                LogError.logError(ex);
                Response.Redirect("../errorpages/error.aspx");
            }
        }
    }

    private void lookupContainer()
    {
        try
        {
            lblMessage.Text = "";
            String weight, category, location;
            weight = category = location = "";
            container = lstUnverifiedContainers.Find(c => c.ContainerID == id);

            using (CCSEntities db = new CCSEntities())
            {
                db.Containers.Attach(container);

                lstLocations = db.Locations.OrderBy(x => x.RoomName).ToList();
                ddlLocation.DataSource = lstLocations;
                ddlLocation.DataBind();

                lstFoodCategories = db.FoodCategories.OrderBy(x => x.CategoryType).ToList();
                ddlFoodCategory.DataSource = lstFoodCategories;
                ddlFoodCategory.DataBind();

                lstUSDACategories = db.USDACategories.OrderBy(x => x.Description).ToList();
                ddlUSDACategory.DataSource = lstUSDACategories;
                ddlUSDACategory.DataBind();

                foodSourceList = db.FoodSourceTypes.OrderBy(x => x.FoodSourceType1).ToList();
                ddlFoodSourceType.DataSource = foodSourceList;
                ddlFoodSourceType.DataBind();

                //load container values
                lblNumber.Text = container.BinNumber.ToString();
                txtWeight.Text = container.Weight.ToString();
                if (container.FoodCategory != null)
                    ddlFoodCategory.Items.FindByValue(container.FoodCategoryID.ToString()).Selected = true;
                if (container.Location != null)
                    ddlLocation.Items.FindByValue(container.LocationID.ToString()).Selected = true;
                if (container.isUSDA != null)
                    chkIsUSDA.Checked = (Boolean)container.isUSDA;
                if (container.USDACategory != null)
                    ddlUSDACategory.Items.FindByValue(container.USDAID.ToString()).Selected = true;
                if (container.Cases != null)
                    txtNumberOfCases.Text = container.Cases.ToString();
                if (container.FoodSourceType != null)
                    ddlFoodSourceType.Items.FindByValue(container.FoodSourcesTypeID.ToString()).Selected = true;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            removeUnverified();
            pushSession();
            Response.Redirect("perform.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            addChanges();
            removeUnverified();
            pushSession();
            Response.Redirect("perform.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // removes the container from the list of unverified containers
    public void removeUnverified()
    {
        try
        {
            lblMessage.Text = "";
            pullSession();

            container = lstUnverifiedContainers.Find(c => c.ContainerID == int.Parse(Request.QueryString["id"]));
            if (container != null)
                lstUnverifiedContainers.Remove(container);
            else
                lblMessage.Text = "An unexpected problem occurred. Please try again later.";
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // if changes were made to the container, those changes are added to the change log
    public void addChanges()
    {
        try
        {
            String weight, foodCategory, location, usdaCategory, isUSDA, units;
            weight = foodCategory = location = usdaCategory = isUSDA = units = "";
            pullSession();
            container = lstUnverifiedContainers.Find(c => c.ContainerID == int.Parse(Request.QueryString["id"]));

            // check if a field is different from the container. If so, store the change in the string.
            if (!container.Weight.ToString().Equals(txtWeight.Text))
                weight = (Decimal.Parse(txtWeight.Text) - container.Weight).ToString();

            if (!container.Location.RoomName.Equals(ddlLocation.SelectedItem.Text))
                location = ddlLocation.SelectedItem.ToString();


            if (container.isUSDA == false && chkIsUSDA.Checked == true)  // if isUSDA status was changed from false to true
            {
                isUSDA = "True";
                usdaCategory = ddlUSDACategory.SelectedItem.ToString();
                units = txtNumberOfCases.Text;
                
            }
            else if (container.isUSDA == true && chkIsUSDA.Checked == false) // if isUSDA status was changed from true to false
            {
                isUSDA = "False";

                foodCategory = ddlFoodCategory.SelectedItem.Text;

                units = (container.Cases * -1).ToString(); // adjustment to units is equal to the number of previous units
            }
            else if (container.isUSDA == true && chkIsUSDA.Checked == true)  // if the isUSDA status remained true, check if the fields changed
            {
                if (container.USDACategory == null ||
                    !container.USDACategory.Description.Equals(ddlUSDACategory.SelectedItem.Text))
                    usdaCategory = ddlUSDACategory.SelectedItem.Text;

                if (container.Cases == null ||
                    !container.Cases.ToString().Equals(txtNumberOfCases.Text))
                    units = (int.Parse(txtNumberOfCases.Text) - container.Cases).ToString();
            }
            else // else it wasn't changed from false, so only worry about FoodCategory field
            {
                if (!container.FoodCategory.CategoryType.Equals(ddlFoodCategory.SelectedItem.Text))
                    foodCategory = ddlFoodCategory.SelectedItem.Text;

                // if the selected food cateogory uses case weight, then update the number of cases.
                if (isCaseWeightUsed())
                    units = (int.Parse((txtNumberOfCases.Text == "" ? "0" : txtNumberOfCases.Text)) - container.Cases).ToString();
            }

            // if any changes were made, add a record of it to the dtChanges log
            if (!weight.Equals("") || !foodCategory.Equals("") || !location.Equals("") ||
                !usdaCategory.Equals("") || !isUSDA.Equals("") || !units.Equals(""))
                dtChanges.Rows.Add(lblNumber.Text, weight, foodCategory, location, isUSDA, usdaCategory, units);

            updateContainer(container);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // Update Container
    private void updateContainer(Container c)
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                db.Containers.Attach(c);

                Location l = db.Locations.Where(x => x.RoomName.Equals(ddlLocation.SelectedItem.Text)).FirstOrDefault();
                if (l != null)
                    c.LocationID = l.LocationID;

                FoodSourceType f = db.FoodSourceTypes.Where(x => x.FoodSourceType1.Equals(ddlFoodSourceType.SelectedItem.Text)).FirstOrDefault();
                if (f != null)
                    c.FoodSourcesTypeID = f.FoodSourceTypeID;

                // if USDA item
                if (chkIsUSDA.Checked)
                {
                    c.isUSDA = true;

                    // usda id
                    USDACategory uc = db.USDACategories.Where(x => x.Description.Equals(ddlUSDACategory.SelectedItem.Text)).FirstOrDefault();
                    if (uc != null)
                    {
                        c.USDAID = uc.USDAID;

                        c.Cases = short.Parse(txtNumberOfCases.Text == "" ? "0" : txtNumberOfCases.Text);

                        //c.Weight = Decimal.Parse(uc.CaseWeight.ToString());
                        c.Weight = (Decimal)uc.CaseWeight * int.Parse(c.Cases.ToString());
                        c.FoodCategoryID = null;
                    }
                }
                // not USDA item
                else
                {
                    c.Weight = Decimal.Parse(txtWeight.Text);

                    // food category id
                    FoodCategory fc = db.FoodCategories.Where(x => x.CategoryType.Equals(ddlFoodCategory.SelectedItem.Text)).FirstOrDefault();
                    if (fc != null)
                        c.FoodCategoryID = fc.FoodCategoryID;

                    c.isUSDA = false;
                    c.USDAID = null;

                    // if the selected food cateogory uses case weight, then update the number of cases.
                    if(isCaseWeightUsed())
                        c.Cases = short.Parse(txtNumberOfCases.Text == "" ? "0" : txtNumberOfCases.Text);
                    else
                        c.Cases = null;
                }

                db.Entry(c).State = System.Data.EntityState.Modified;
                db.SaveChanges();
            }

            LogChange.logChange("Container " + c.BinNumber + " edited during audit.", DateTime.Now, short.Parse(Session["userID"].ToString()));
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void pushSession()
    {
        try
        {
            Session["unverified"] = lstUnverifiedContainers;
            Session["changes"] = dtChanges;
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void pullSession()
    {
        try
        {
            if (Session["unverified"] == null || Session["changes"] == null)
                lblMessage.Text = "An unexpected problem occurred. Please try again later.";
            else
            {
                lstUnverifiedContainers = (List<Container>)Session["unverified"];
                dtChanges = (DataTable)Session["changes"];
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // returns true if the selected food cateogry uses the CaseWeight field
    private Boolean isCaseWeightUsed()
    {
        Boolean result = false;
        try
        {
            FoodCategory fc = null;
            using (CCSEntities db = new CCSEntities())
            {
                fc = db.FoodCategories.Where(x => x.CategoryType.Equals(ddlFoodCategory.SelectedItem.Text)).FirstOrDefault(); // get matching food category
            }

            if (fc.CaseWeight != null)  // if the food category uses case weight
            {
                result = true;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

        return result;
    }
}