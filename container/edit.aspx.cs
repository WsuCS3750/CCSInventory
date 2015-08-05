using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    public int id;
    private Container container;
    private List<Location> lstLocations;
    private List<FoodCategory> lstFoodCategories;
    private List<USDACategory> lstUSDACategories;
    private List<FoodSourceType> foodSourceList;
    public bool isUSDA;
    public bool isNew;
    private string callback;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] == null)
            {
                Response.Redirect("default.aspx");
            }
            if (Request.QueryString["type"] != null)
            {
                isNew = Request.QueryString["type"] == "new";
            }
            callback = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "";

            id = int.Parse(Request.QueryString["id"]);

            using (CCSEntities db = new CCSEntities())
            {
                container = (from c in db.Containers
                             where c.BinNumber == (Int16) id
                             select c).FirstOrDefault();

                if (!Page.IsPostBack)
                {
                    hdnCases.Value = container.Cases == null ? "0" : container.Cases.ToString();
                    hdnWeight.Value = container.Weight == null ? "0" : container.Weight.ToString() ;

                    lstLocations = db.Locations.OrderBy(x => x.RoomName).ToList();
                    ddlLocation.DataSource = lstLocations;
                    ddlLocation.DataBind();

                    lstFoodCategories = db.FoodCategories.OrderBy(x => x.CategoryType).ToList();
                    ddlType.DataSource = lstFoodCategories;
                    ddlType.DataBind();

                    lstUSDACategories = db.USDACategories.OrderBy(x => x.Description).ToList();
                    ddlUSDAType.DataSource = lstUSDACategories;
                    ddlUSDAType.DataBind();

                    foodSourceList = db.FoodSourceTypes.OrderBy(x => x.FoodSourceType1).ToList();
                    ddlFoodSourceType.DataSource = foodSourceList;
                    ddlFoodSourceType.DataBind();

                    isUSDA = (bool) container.isUSDA;
                    chkIsUSDA.Checked = isUSDA;

                    txtNumberOfCases.Text = container.Cases.ToString();

                    if (container != null)
                    {
                        lblID.Text = container.BinNumber.ToString();
                        txtWeight.Text = container.Weight.ToString();
                        if (container.FoodCategoryID != null)
                            ddlType.Items.FindByValue(container.FoodCategoryID.ToString()).Selected = true;


                        if (container.USDAID != null)
                            ddlUSDAType.Items.FindByValue(container.USDAID.ToString()).Selected = true;

                        if (container.Location != null)
                            ddlLocation.Items.FindByValue(container.LocationID.ToString()).Selected = true;

                        if (container.FoodSourcesTypeID != null)
                            ddlFoodSourceType.Items.FindByValue(container.FoodSourcesTypeID.ToString()).Selected = true;

                    }
                }
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
        }
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
            double num;
            int iNum;
            
            if (!(double.TryParse(txtWeight.Text, out num))) //checks that it is a number
            {
                lblWeightError.Text = "Invalid Weight Number!";
            }
            else if ((double.Parse(txtWeight.Text)) < 0) //checks that number is not less than 0
            {
                lblWeightError.Text = "Invalid Weight Number!";
            }
            else if (chkIsUSDA.Checked && !int.TryParse(txtNumberOfCases.Text, out iNum)) //checks that it is a number OR it isn't used
            {
                lblWeightError.Text = "Invalid number of cases!";
            }
            else if (chkIsUSDA.Checked && int.Parse(txtNumberOfCases.Text) < 0)  //checks that number is not less than 0 OR it isn't used
            {
                lblWeightError.Text = "Invalid number of cases!";
            }
            else
            {
                using (CCSEntities db = new CCSEntities())
                {
                    Container cont = (from c in db.Containers
                                      where c.ContainerID == container.ContainerID
                                      select c).FirstOrDefault();

                    cont.Weight = decimal.Parse(txtWeight.Text);
                    cont.LocationID = short.Parse(ddlLocation.SelectedValue);
                    cont.FoodSourcesTypeID = short.Parse(ddlFoodSourceType.SelectedValue);

                    if (!chkIsUSDA.Checked)
                    {
                        short foodtype = short.Parse(ddlType.SelectedValue);
                        FoodCategory fc = (from f in db.FoodCategories
                                           where f.FoodCategoryID == foodtype
                                           select f).First();

                        cont.FoodCategory = fc;
                        cont.USDAID = null;

                        if (txtNumberOfCases.Text != "" && fc.CaseWeight != null) // if case weight is used and if there is a case count, calculate the weight
                        {
                            cont.Cases = short.Parse(txtNumberOfCases.Text);
                            cont.Weight = (Decimal)fc.CaseWeight * int.Parse(txtNumberOfCases.Text);
                        }
                        else
                        {
                            cont.Cases = null;
                        }
                    }

                    if (chkIsUSDA.Checked)
                    {
                        short usdafoodtype = short.Parse(ddlUSDAType.SelectedValue);
                        USDACategory fc = (from f in db.USDACategories
                                           where f.USDAID == usdafoodtype
                                           select f).First();

                        cont.USDACategory = fc;
                        cont.FoodCategoryID = null;
                        cont.Cases = short.Parse(txtNumberOfCases.Text == "" ? "0" : txtNumberOfCases.Text);

                        cont.Weight = (Decimal) fc.CaseWeight*int.Parse(txtNumberOfCases.Text);
                        txtWeight.Text = cont.Weight.ToString();

                        
                    }

                    cont.isUSDA = chkIsUSDA.Checked;

                    db.SaveChanges();
                    AddFoodOutRecord(cont);
                    LogChange.logChange("Container " + cont.BinNumber + " Edited.", DateTime.Now,
                                        short.Parse(Session["userID"].ToString()));
                }
                //Redirect back to menu
                Response.Redirect("menu.aspx?id=" + id);
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void AddFoodOutRecord(Container container)
    {
        short cases = 0;
        decimal weight = 0;

        if (decimal.TryParse(hdnWeight.Value, out weight) && short.TryParse(hdnCases.Value, out cases))
        {
            if (container.Weight < weight || container.Cases < cases)
            {
                List<FoodOut> foodOut = new List<FoodOut>();

                using (CCSEntities db = new CCSEntities())
                {
                    FoodOut newFoodOut = new FoodOut();
                    newFoodOut.TimeStamp = DateTime.Now;
                    newFoodOut.FoodCategory = container.FoodCategory;
                    newFoodOut.FoodSourceType = container.FoodSourceType;
                    newFoodOut.USDACategory = container.USDACategory;
                    newFoodOut.FoodCategoryID = container.FoodCategoryID;
                    newFoodOut.FoodSourceType = container.FoodSourceType;
                    newFoodOut.FoodSourceTypeID = (short)(container.FoodSourcesTypeID ?? 0);
                    newFoodOut.USDAID = container.USDAID;
                    newFoodOut.Weight = (double)(weight - container.Weight);
                    newFoodOut.Count = container.Cases == null? (short)0 : (short)(cases - container.Cases);
                    foodOut.Add(newFoodOut);
                }

                Session["foodOut"] = foodOut;
                Response.Redirect("~/outgoing-food/quickout.aspx?redirect=~/container/menu.aspx?id=" + id);
            }
        }

    }
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                Container c = (from co in db.Containers
                               where co.BinNumber == (short) id
                               select co).First();

                String binNumber = c.BinNumber.ToString(); // saved for logging purposes
                db.Containers.Remove(c);
                db.SaveChanges();

                LogChange.logChange("Container " + binNumber + " Removed.", DateTime.Now,
                                    short.Parse(Session["userID"].ToString()));
            }
            Response.Redirect("default.aspx");
        }
        catch (System.Threading.ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (isNew)
        {
            //This was a new container that is being cancelled -- delete it.
            using (CCSEntities db = new CCSEntities())
            {
                Container c = (from co in db.Containers
                               where co.BinNumber == (short)id
                               select co).First();
                db.Containers.Remove(c);
                db.SaveChanges();
            }
        }
        Response.Redirect(callback);
    }

}