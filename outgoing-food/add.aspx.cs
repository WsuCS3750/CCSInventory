using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    //***************************** Page_Load **************************************//
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            loadPassedSessionInfo();
        }

    }


    //******************************** loadPassedSessionInfo ********************************//
    private void loadPassedSessionInfo()
    {
        try
        {
                using (CCSEntities db = new CCSEntities())
                {
                    if (!Page.IsPostBack)
                    {

                        List<DistributionType> distributionTypes = (from dt in db.DistributionTypes select dt).ToList();
                        ddlDistributionType.DataSource = distributionTypes;
                        ddlDistributionType.DataBind();

                        List<Agency> a = (from ag in db.Agencies select ag).ToList();
                        ddlAgency.DataSource = a;
                        ddlAgency.DataBind();

                        List<FoodSourceType> donorTypes = db.FoodSourceTypes.ToList();
                        ddlDonorType.DataSource = donorTypes;
                        ddlDonorType.DataBind();

                        txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
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


    //***************************** btnSave_Click **************************************//
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = ""; //empty out the error message field

            checkForNullFields(); //makes sure the required database fields are not null or empty
            formValidation();   //validate the form information

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                insertOutgoingFood();
                Response.Redirect("default.aspx");
            }
            
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    }


    //***************************** checkForNullFields **************************************//
    private void checkForNullFields()
    {
        //checks that 2 required input fields are not empty
        //txtDonorName IE the FoodSource
        //txtWeight IE the weight 
        try
        {
            if (ckbxIsUSDA.Checked == true)
            {
                using (CCSEntities db = new CCSEntities())
                {
                    var usdaFoodSourceType = (from d in db.FoodSourceTypes
                                   where d.FoodSourceType1.Equals("usda", StringComparison.OrdinalIgnoreCase)
                                   select d).FirstOrDefault();


                    if (usdaFoodSourceType != null)
                    {


                        if (txtUSDACases.Text == null || txtUSDACases.Text.Equals(""))
                        {
                            lblMessage.Text += "Number of Cases must be provided for a USDA type donation.<br/>";
                        }

                        if (lookupUSDA() == null)
                            lblMessage.Text += "USDA Category Not Found.<br/>";
                    }
                    else
                    {
                        lblMessage.Text += "A problem occured when linking USDA to the USDA donor.<br/>";
                    }
                }

            }
            else
            {
                if (lookupCategory() == null)
                    lblMessage.Text += "Food Category Not Found.<br/>";
            }
            if (txtWeight.Text.ToString() == null || txtWeight.Text.ToString().Equals(""))
                lblMessage.Text += "The Weight cannot be empty.<br/>";


        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** formValidation method **************************************//
    private void formValidation()
    {
        try
        {
            double num;
            //checks that the specified zipcode field is a number before inserting it into the database
            if (!(double.TryParse(txtWeight.Text.ToString(), out num))) //checks that the weight is a number
            {
                lblMessage.Text += "The Weight must be a number.<br/>";
            }

            if (double.TryParse(txtWeight.Text.ToString(), out num)) //weight is a number
            {
                double weight = double.Parse(txtWeight.Text.ToString());
                // Math.Round(weight, 2);
                if (weight > 9999999.99)
                {
                    lblMessage.Text += "The Weight amount exceeds the database numerical limit to be saved.<br/>";
                }

                if (weight == 0)
                {
                    lblMessage.Text += "The Weight for the donation cannot be zero.<br/>";
                }

                if (weight < 0)
                {
                    lblMessage.Text += "The Weight for the donation cannot be a negative number.<br/>";
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    } //end of validation method


    private void insertOutgoingFood()
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                FoodOut foodOut = new FoodOut();
                if (!string.IsNullOrEmpty(txtWeight.Text))
                      foodOut.Weight = double.Parse(txtWeight.Text);
                if(!string.IsNullOrEmpty(txtUSDACases.Text))
                    foodOut.Count = short.Parse(txtUSDACases.Text);

                if (ckbxIsUSDA.Checked)
                {
                    foodOut.USDACategory = db.USDACategories.First(u => u.USDANumber == txtUSDAItemNo.Text);
                    foodOut.FoodSourceType = (from d in db.FoodSourceTypes
                                              where d.FoodSourceType1.Equals("usda", StringComparison.OrdinalIgnoreCase)
                                              select d).FirstOrDefault();
                }
                else
                {
                    foodOut.FoodCategory = db.FoodCategories.First(f => f.CategoryType == txtCategoryType.Text);
                    foodOut.FoodSourceTypeID = short.Parse(ddlDonorType.SelectedValue);

                }

                DateTime ds = DateTime.Parse(txtDate.Text);
                foodOut.TimeStamp = ds;
                foodOut.DateCreated = ds;

                foodOut.DistributionTypeID = short.Parse(ddlDistributionType.SelectedValue);
                if(ddlAgency.SelectedValue != "0")
                    foodOut.AgencyID = short.Parse(ddlAgency.SelectedValue);

                db.FoodOuts.Add(foodOut);

                db.SaveChanges();

                LogChange.logChange("Added outgoing row " + foodOut.DistributionID, DateTime.Now,
                                        short.Parse(Session["userID"].ToString()));

            }
        }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** lookupDonor method **************************************//
  

    //***************************** lookupCategory method **************************************//
    private FoodCategory lookupCategory()
    {
        FoodCategory fc = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var categoryRslt = (from d in db.FoodCategories
                                    where d.CategoryType.Equals(txtCategoryType.Text, StringComparison.OrdinalIgnoreCase)
                                    select d).FirstOrDefault();
                if (categoryRslt != null)
                    fc = categoryRslt;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return fc;
    }


    //***************************** lookupUSDA method **************************************//
    private USDACategory lookupUSDA()
    {
        USDACategory us = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var usCategoryRslt = (from d in db.USDACategories
                                      where d.USDANumber.Equals(txtUSDAItemNo.Text, StringComparison.OrdinalIgnoreCase)
                                      select d).FirstOrDefault();
                if (usCategoryRslt != null)
                    us = usCategoryRslt;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return us;
    }


    //***************************** btnCancel_Click method **************************************//
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("default.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }




    //***************************** btnFindUSDA_Click method **************************************//
    protected void btnFindUSDA_Click(object sender, EventArgs e)
    {
        try
        {
            USDACategory us = null;

            if (txtUSDAItemNo.Text == null || txtUSDAItemNo.Text.Equals(""))
                lblMessage.Text += "Unable to perform look up on empty USDA Item Number field.<br/>";
            else
            {
                us = lookupUSDA();
            }

            if (us == null)
            {
                lblMessage.Text = "The USDA Item Number:  " + txtUSDAItemNo.Text + "  was not found. Create the new USDA Item Number before adding the donation.";
            
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** btnFindCategory_Click method **************************************//
    protected void btnFindCategory_Click(object sender, EventArgs e)
    {
        try
        {
            FoodCategory fc = null;

            if (txtCategoryType.Text == null || txtCategoryType.Text.Equals(""))
                lblMessage.Text += "Unable to perform look up on empty Category Type field.<br/>";
            else
            {
                fc = lookupCategory();
            }

            if (fc == null)
            {
                lblMessage.Text = "The Category Type:  " + txtCategoryType.Text + "  was not found. Create the new Category Type before adding the donation.";
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