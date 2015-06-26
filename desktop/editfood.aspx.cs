using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class desktop_editfood : System.Web.UI.Page
{

    private List<FoodCategory> lstFoodCategory;
    private List<FoodSource> lstFoodSource;
    private List<DistributionType> lstDistributionType;
    private List<USDACategory> lstUSDA;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userID"] == null)
            Response.Redirect("login.aspx");
        else
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["do"].Equals("i"))
                {
                    editFoodOutDiv.Visible = false;
                    editHistoryDiv.Visible = false;
                    lblTitle.Text = "Edit Food In Data";

                    try
                    {
                        using (CCSEntities db = new CCSEntities())
                        {

                            short shFoodInID = short.Parse(Session["editWhat"].ToString());

                            var editFoodInQuery = (from c in db.FoodIns
                                                   where c.FoodInID.Equals(shFoodInID)
                                                   select new 
                                                   { 
                                                       c.FoodInID, 
                                                       c.FoodCategoryID,
                                                       c.USDAID,
                                                       c.FoodSourceID,
                                                       c.Weight, 
                                                       c.TimeStamp 
                                                   });

                            lstFoodCategory = db.FoodCategories.OrderBy(x => x.CategoryType).ToList();
                            ddlFoodInCategoryType.DataSource = lstFoodCategory;
                            ddlFoodInCategoryType.DataBind();

                            lstUSDA = db.USDACategories.OrderBy(x => x.USDAID).ToList();
                            ddlFoodInUSDA.DataSource = lstUSDA;
                            ddlFoodInUSDA.DataBind();

                            lstFoodSource = db.FoodSources.OrderBy(x => x.Source).ToList();
                            ddlFoodInSource.DataSource = lstFoodSource;
                            ddlFoodInSource.DataBind();


                            if (editFoodInQuery.ToList().ElementAt(0).FoodInID.ToString() != null)
                                txtEditFoodInID.Text = editFoodInQuery.ToList().ElementAt(0).FoodInID.ToString();

                            if (editFoodInQuery.ToList().ElementAt(0).FoodCategoryID != null)
                            {
                                ddlFoodInCategoryType.SelectedValue = editFoodInQuery.ToList().ElementAt(0).FoodCategoryID.ToString();
                                lblFoodInCatType.Visible = true;
                                ddlFoodInCategoryType.Visible = true;
                                lblFoodInUSDA.Visible = false;
                                ddlFoodInUSDA.Visible = false;
                                chkUSDAin.Checked = false;
                            }
                            else
                            {
                                lblFoodInCatType.Visible = false;
                                ddlFoodInCategoryType.Visible = false;
                                lblFoodInUSDA.Visible = true;
                                ddlFoodInUSDA.Visible = true;
                                chkUSDAin.Checked = true;
                            }

                            if (editFoodInQuery.ToList().ElementAt(0).USDAID != null)
                            {
                                ddlFoodInUSDA.SelectedValue = editFoodInQuery.ToList().ElementAt(0).USDAID.ToString();
                                lblFoodInCatType.Visible = false;
                                ddlFoodInCategoryType.Visible = false;
                                lblFoodInUSDA.Visible = true;
                                ddlFoodInUSDA.Visible = true;
                                chkUSDAin.Checked = true;
                            }
                            else
                            {
                                lblFoodInCatType.Visible = true;
                                ddlFoodInCategoryType.Visible = true;
                                lblFoodInUSDA.Visible = false;
                                ddlFoodInUSDA.Visible = false;
                                chkUSDAin.Checked = false;
                            }

                            txtFoodInWeight.Text = editFoodInQuery.ToList().ElementAt(0).Weight.ToString();
                            ddlFoodInSource.SelectedValue = editFoodInQuery.ToList().ElementAt(0).FoodSourceID.ToString();
                            txtFoodInTime.Text = editFoodInQuery.ToList().ElementAt(0).TimeStamp.ToString();

                        }
                    }
                    catch (System.Threading.ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        LogError.logError(ex);
                        Response.Redirect("../errorpages/error.aspx");
                    }
                }
                else if (Request.QueryString["do"].Equals("o"))
                {
                    editFoodInDiv.Visible = false;
                    editHistoryDiv.Visible = false;
                    lblTitle.Text = "Edit Food Out Data";

                    try
                    {
                        using (CCSEntities db = new CCSEntities())
                        {

                            short shDistID = short.Parse(Session["editWhat"].ToString());

                            var editFoodOutQuery = (from c in db.FoodOuts
                                                 where c.DistributionID.Equals(shDistID)
                                                 select new 
                                                 { 
                                                     c.DistributionID, 
                                                     c.FoodCategoryID, 
                                                     c.USDAID,
                                                     c.Weight, 
                                                     c.Count, 
                                                     c.TimeStamp,
                                                 });

                            lstFoodCategory = db.FoodCategories.OrderBy(x => x.CategoryType).ToList();
                            ddlFoodOutCategoryType.DataSource = lstFoodCategory;
                            ddlFoodOutCategoryType.DataBind();

                            lstUSDA = db.USDACategories.OrderBy(x => x.USDAID).ToList();
                            ddlFoodOutUSDA.DataSource = lstUSDA;
                            ddlFoodOutUSDA.DataBind();

                            txtFoodOutDistID.Text = editFoodOutQuery.ToList().ElementAt(0).DistributionID.ToString();

                            if (editFoodOutQuery.ToList().ElementAt(0).FoodCategoryID != null)
                            {
                                ddlFoodOutCategoryType.SelectedValue = editFoodOutQuery.ToList().ElementAt(0).FoodCategoryID.ToString();
                                lblFoodOutCatType.Visible = true;
                                ddlFoodOutCategoryType.Visible = true;
                                lblFoodOutUSDA.Visible = false;
                                ddlFoodOutUSDA.Visible = false;
                                chkUSDA.Checked = false;
                            }
                            else
                            {
                                lblFoodOutCatType.Visible = false;
                                ddlFoodOutCategoryType.Visible = false;
                                lblFoodOutUSDA.Visible = true;
                                ddlFoodOutUSDA.Visible = true;
                                chkUSDA.Checked = true;
                            }

                            if (editFoodOutQuery.ToList().ElementAt(0).USDAID != null)
                            {
                                ddlFoodOutUSDA.SelectedValue = editFoodOutQuery.ToList().ElementAt(0).USDAID.ToString();
                                lblFoodOutCatType.Visible = false;
                                ddlFoodOutCategoryType.Visible = false;
                                lblFoodOutUSDA.Visible = true;
                                ddlFoodOutUSDA.Visible = true;
                                chkUSDA.Checked = true;
                            }
                            else
                            {
                                lblFoodOutCatType.Visible = true;
                                ddlFoodOutCategoryType.Visible = true;
                                lblFoodOutUSDA.Visible = false;
                                ddlFoodOutUSDA.Visible = false;
                                chkUSDA.Checked = false;
                            }

                            txtFoodOutWeight.Text = editFoodOutQuery.ToList().ElementAt(0).Weight.ToString();
                            txtFoodOutCount.Text = editFoodOutQuery.ToList().ElementAt(0).Count.ToString();
                            txtFoodOutTime.Text = editFoodOutQuery.ToList().ElementAt(0).TimeStamp.ToString();
                        }
                    }
                    catch (System.Threading.ThreadAbortException) { }
                    catch (Exception ex)
                    {
                        LogError.logError(ex);
                        Response.Redirect("../errorpages/error.aspx");
                    }
                }
                else if (Request.QueryString["do"].Equals("h"))
                {
                    editFoodInDiv.Visible = false;
                    editFoodOutDiv.Visible = false;
                    lblTitle.Text = "Edit History Data";

                    try
                    {
                        using (CCSEntities db = new CCSEntities())
                        {

                            short shDistID = short.Parse(Session["editWhat"].ToString());

                            var editHistoryQuery = (from c in db.FoodOuts
                                                    where c.DistributionID.Equals(shDistID)
                                                    select new
                                                    {
                                                        c.DistributionID,
                                                        c.BinNumber,
                                                        c.FoodCategoryID,
                                                        c.USDAID,
                                                        c.DistributionType.DistributionTypeID,
                                                        c.DateCreated,
                                                        c.TimeStamp,
                                                    });

                            lstFoodCategory = db.FoodCategories.OrderBy(x => x.CategoryType).ToList();
                            ddlHistoryCategoryType.DataSource = lstFoodCategory;
                            ddlHistoryCategoryType.DataBind();

                            lstDistributionType = db.DistributionTypes.OrderBy(x => x.DistributionTypeID).ToList();
                            ddlHistoryDistributionType.DataSource = lstDistributionType;
                            ddlHistoryDistributionType.DataBind();

                            ddlHistoryDistributionType.SelectedValue = editHistoryQuery.ToList().ElementAt(0).DistributionTypeID.ToString();

                            lstUSDA = db.USDACategories.OrderBy(x => x.USDAID).ToList();
                            ddlHistoryUSDA.DataSource = lstUSDA;
                            ddlHistoryUSDA.DataBind();

                            txtHistoryDistID.Text = editHistoryQuery.ToList().ElementAt(0).DistributionID.ToString();

                            if (editHistoryQuery.ToList().ElementAt(0).FoodCategoryID != null)
                            {
                                ddlHistoryCategoryType.SelectedValue = editHistoryQuery.ToList().ElementAt(0).FoodCategoryID.ToString();                                
                                lblHistoryCatType.Visible = true;
                                ddlHistoryCategoryType.Visible = true;
                                lblHistoryUSDA.Visible = false;
                                ddlHistoryUSDA.Visible = false;
                                chkUSDAhistory.Checked = false;
                            }
                            else
                            {
                                lblHistoryCatType.Visible = false;
                                ddlHistoryCategoryType.Visible = false;
                                ddlHistoryDistributionType.Visible = false;
                                lblHistoryUSDA.Visible = true;
                                ddlHistoryUSDA.Visible = true;
                                chkUSDAhistory.Checked = true;
                            }

                            if (editHistoryQuery.ToList().ElementAt(0).USDAID != null)
                            {
                                ddlHistoryUSDA.SelectedValue = editHistoryQuery.ToList().ElementAt(0).USDAID.ToString();
                                lblHistoryCatType.Visible = false;
                                ddlHistoryCategoryType.Visible = false;
                                ddlHistoryDistributionType.Visible = true;
                                lblHistoryUSDA.Visible = true;
                                ddlHistoryUSDA.Visible = true;
                                chkUSDAhistory.Checked = true;
                            }
                            else
                            {
                                lblHistoryCatType.Visible = true;
                                ddlHistoryCategoryType.Visible = true;
                                ddlHistoryDistributionType.Visible = true;
                                lblHistoryUSDA.Visible = false;
                                ddlHistoryUSDA.Visible = false;
                                chkUSDAhistory.Checked = false;
                            }
                            
                            txtHistoryTimeCreated.Text = editHistoryQuery.ToList().ElementAt(0).DateCreated.ToString();
                            txtHistoryTime.Text = editHistoryQuery.ToList().ElementAt(0).TimeStamp.ToString();
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
        }
    }


    // @author: Anthony Dietrich - Save Food In Edit Data
    // Saves Edit Data from the Food In version of the editfood page.
    protected void btnEditFoodInSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                short shFoodInID = short.Parse(Session["editWhat"].ToString());

                FoodIn editFoodIn = (from c in db.FoodIns
                                     where c.FoodInID.Equals(shFoodInID)
                                     select c).FirstOrDefault();

                if (chkUSDAin.Checked == true)
                {
                    //save ddlUSDA info, set FoodCatID to null
                    int ID = int.Parse(ddlFoodInUSDA.SelectedValue);
                    USDACategory uc = db.USDACategories.Single(x => x.USDAID == ID);

                    editFoodIn.FoodCategoryID = null;
                    editFoodIn.USDACategory = uc;
                    editFoodIn.FoodSourceID = short.Parse(ddlFoodInSource.SelectedValue);
                    editFoodIn.Weight = Convert.ToDecimal(txtFoodInWeight.Text);
                    editFoodIn.TimeStamp = Convert.ToDateTime(txtFoodInTime.Text);

                    db.SaveChanges();
                    lblResponse.Visible = true;
                    lblResponse.Text = "Changes to Food In ID: " + txtEditFoodInID.Text + ", successfully saved!";

                }
                if (chkUSDAin.Checked == false)
                {
                    //save ddlFoodCat info, set USDAID to null
                    editFoodIn.FoodCategoryID = short.Parse(ddlFoodInCategoryType.SelectedValue);
                    editFoodIn.USDAID = null;
                    editFoodIn.Weight = Convert.ToDecimal(txtFoodInWeight.Text);
                    editFoodIn.FoodSourceID = short.Parse(ddlFoodInSource.SelectedValue);
                    editFoodIn.TimeStamp = Convert.ToDateTime(txtFoodInTime.Text);

                    db.SaveChanges();
                    lblResponse.Visible = true;
                    lblResponse.Text = "Changes to Food In ID: " + txtEditFoodInID.Text + ", successfully saved!";

                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    }

    // @author: Anthony Dietrich - Save Food Out Edit Data
    // Saves Edit Data from the Food Out version of the editfood page.
    protected void btnEditFoodOutSave_Click(object sender, EventArgs e)
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {

                short shFoodOutID = short.Parse(Session["editWhat"].ToString());

                FoodOut editFoodOut = (from c in db.FoodOuts
                                       where c.DistributionID.Equals(shFoodOutID)
                                       select c).FirstOrDefault();

                if (chkUSDA.Checked == true)
                {
                    //save ddlUSDA info, set FoodCatID to null
                    int ID = int.Parse(ddlFoodOutUSDA.SelectedValue);
                    USDACategory uc = db.USDACategories.Single(x => x.USDAID == ID);

                    editFoodOut.FoodCategoryID = null;
                    editFoodOut.USDACategory = uc;
                    editFoodOut.Weight = Convert.ToDouble(txtFoodOutWeight.Text);
                    editFoodOut.Count = short.Parse(txtFoodOutCount.Text);
                    editFoodOut.TimeStamp = Convert.ToDateTime(txtFoodOutTime.Text);

                    db.SaveChanges();
                    lblResponse.Visible = true;
                    lblResponse.Text = "Changes to Distribution ID: " + txtFoodOutDistID.Text + ", successfully saved!";

                }
                if (chkUSDA.Checked == false) 
                { 
                    //save ddlFoodCat info, set USDAID to null
                    editFoodOut.DistributionID = short.Parse(txtFoodOutDistID.Text);
                    editFoodOut.FoodCategoryID = short.Parse(ddlFoodOutCategoryType.SelectedValue);
                    editFoodOut.USDAID = null;
                    editFoodOut.Weight = Convert.ToDouble(txtFoodOutWeight.Text);
                    editFoodOut.TimeStamp = Convert.ToDateTime(txtFoodOutTime.Text);
                    if (txtFoodOutCount.Text.Length != 0)
                    {
                        editFoodOut.Count = short.Parse(txtFoodOutCount.Text);
                    }
                    else editFoodOut.Count = null; 

                    db.SaveChanges();
                    lblResponse.Visible = true;
                    lblResponse.Text = "Changes to Distribution ID: " + txtFoodOutDistID.Text + ", successfully saved!";

                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // @author: Nittaya Phonharath - Save Food Out Edit Data
    // Saves Edit Data from the Food Out version of the editfood page.
    protected void btnEditHistorySave_Click(object sender, EventArgs e)
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {

                short shHistoryID = short.Parse(Session["editWhat"].ToString());

                FoodOut editHistory = (from c in db.FoodOuts
                                       where c.DistributionID.Equals(shHistoryID)
                                       select c).FirstOrDefault();

                if (chkUSDAhistory.Checked == true)
                {
                    //save ddlUSDA info, set FoodCatID to null
                    int ID = int.Parse(ddlHistoryUSDA.SelectedValue);
                    USDACategory uc = db.USDACategories.Single(x => x.USDAID == ID);

                    editHistory.FoodCategoryID = null;
                    editHistory.USDACategory = uc;
                    editHistory.DistributionTypeID = short.Parse(ddlHistoryDistributionType.SelectedValue);
                    //editHistory.Weight = Convert.ToDouble(txtFoodOutWeight.Text);
                    //editHistory.Count = short.Parse(txtFoodOutCount.Text);
                    editHistory.DateCreated = Convert.ToDateTime(txtHistoryTimeCreated.Text);
                    editHistory.TimeStamp = Convert.ToDateTime(txtHistoryTime.Text);

                    db.SaveChanges();
                    lblResponse.Visible = true;
                    lblResponse.Text = "Changes to Distribution ID: " + txtHistoryDistID.Text + ", successfully saved!";

                }
                if (chkUSDAhistory.Checked == false)
                {
                    //save ddlFoodCat info, set USDAID to null
                    editHistory.DistributionID = short.Parse(txtHistoryDistID.Text);
                    editHistory.FoodCategoryID = short.Parse(ddlHistoryCategoryType.SelectedValue);
                    editHistory.DistributionTypeID = short.Parse(ddlHistoryDistributionType.SelectedValue);
                    editHistory.USDAID = null;
                    //editHistory.Weight = Convert.ToDouble(txtFoodOutWeight.Text);
                    //editHistory.Count = short.Parse(txtFoodOutCount.Text);
                    editHistory.DateCreated = Convert.ToDateTime(txtHistoryTimeCreated.Text);
                    editHistory.TimeStamp = Convert.ToDateTime(txtHistoryTime.Text);

                    db.SaveChanges();
                    lblResponse.Visible = true;
                    lblResponse.Text = "Changes to Distribution ID: " + txtHistoryDistID.Text + ", successfully saved!";

                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // @author: Anthony Dietrich - toggleEditUSDA()
    // Toggles an item from USDA to non-USDA
    protected void toggleEditUSDAout(object sender, EventArgs e)
    {
        if (chkUSDA.Checked == true)
        {
            lblFoodOutCatType.Visible = false;
            ddlFoodOutCategoryType.Visible = false;
            lblFoodOutUSDA.Visible = true;
            ddlFoodOutUSDA.Visible = true;
        }
        if (chkUSDA.Checked == false)
        {
            lblFoodOutCatType.Visible = true;
            ddlFoodOutCategoryType.Visible = true;
            lblFoodOutUSDA.Visible = false;
            ddlFoodOutUSDA.Visible = false;
        }
    }

    // @author: Nittaya Phonharath - toggleEditUSDAhistory()
    // Toggles an item from USDA to non-USDA
    protected void toggleEditUSDAhistory(object sender, EventArgs e)
    {
        if (chkUSDAhistory.Checked == true)
        {
            lblHistoryCatType.Visible = false;
            ddlHistoryCategoryType.Visible = false;
            lblHistoryUSDA.Visible = true;
            ddlHistoryUSDA.Visible = true;
        }
        if (chkUSDAhistory.Checked == false)
        {
            lblHistoryCatType.Visible = true;
            ddlHistoryCategoryType.Visible = true;
            lblHistoryUSDA.Visible = false;
            ddlHistoryUSDA.Visible = false;
        }
    }

    // @author: Anthony Dietrich - toggleEditUSDA()
    // Toggles an item from USDA to non-USDA
    protected void toggleEditUSDAin(object sender, EventArgs e)
    {
        if (chkUSDAin.Checked == true)
        {
            lblFoodInCatType.Visible = false;
            ddlFoodInCategoryType.Visible = false;
            lblFoodInUSDA.Visible = true;
            ddlFoodInUSDA.Visible = true;
        }
        if (chkUSDAin.Checked == false)
        {
            lblFoodInCatType.Visible = true;
            ddlFoodInCategoryType.Visible = true;
            lblFoodInUSDA.Visible = false;
            ddlFoodInUSDA.Visible = false;
        }
    }
}