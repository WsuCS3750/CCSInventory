using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    public List<short> passedFoodIDS;
    public List<int> submitDonationRows;

    //***************************** Page_Load **************************************//
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            //loads any passed session information
            loadPassedSessionInfo();
        }

    }


    //******************************** loadPassedSessionInfo ********************************//
    private void loadPassedSessionInfo()
    {
        try
        {
            if (Session["PassedFoodInInfo"] != null)
            {
                String[] passedfoodInInfo = Session["PassedFoodInInfo"] as String[];
                txtDonorName.Text = passedfoodInInfo[0];
                txtCategoryType1.Text = passedfoodInInfo[1];
                txtWeight1.Text = passedfoodInInfo[2];
                txtCases1.Text = passedfoodInInfo[3];
                txtUSDAItemNo1.Text = passedfoodInInfo[4];

                txtCategoryType2.Text = passedfoodInInfo[5];
                txtWeight2.Text = passedfoodInInfo[6];
                txtCases2.Text = passedfoodInInfo[7];
                txtUSDAItemNo2.Text = passedfoodInInfo[8];

                txtCategoryType3.Text = passedfoodInInfo[9];
                txtWeight3.Text = passedfoodInInfo[10];
                txtCases3.Text = passedfoodInInfo[11];
                txtUSDAItemNo3.Text = passedfoodInInfo[12];

                txtCategoryType4.Text = passedfoodInInfo[13];
                txtWeight4.Text = passedfoodInInfo[14];
                txtCases4.Text = passedfoodInInfo[15];
                txtUSDAItemNo4.Text = passedfoodInInfo[16];

                txtCategoryType5.Text = passedfoodInInfo[17];
                txtWeight5.Text = passedfoodInInfo[18];
                txtCases5.Text = passedfoodInInfo[19];
                txtUSDAItemNo5.Text = passedfoodInInfo[20];

                ckbxIsUSDA.Checked = passedfoodInInfo[21].Equals("true") ? true : false;
                txtDate.Text = passedfoodInInfo[22];
            }
            else
            {
                txtDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
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

            completePageValidation(); //validates the submitted info

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                if (ckbxIsUSDA.Checked == true)
                {
                    insertUSDADonations();
                }

                if (ckbxIsUSDA.Checked == false)
                {
                    insertNonUSDADonations();
                }

            }

            if (lblMessage.Text.ToString().Length == 0)
            {
                Session["PassedFoodInInfo"] = null;
                Session["PassedFoodInURL"] = null;
                Session["newFoodInIDs"] = passedFoodIDS;
                Response.Redirect("menu.aspx");
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    }


    //***************************** btnCancel_Click method **************************************//
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Session["PassedFoodInInfo"] = null;
            Session["PassedFoodInURL"] = null;
            Session["newFoodInIDs"] = null;
            Response.Redirect("default.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** btnAddDonor_Click method **************************************//
    protected void btnAddDonor_Click(object sender, EventArgs e)
    {
        try
        {
            String[] foodIn = new String[23];
            foodIn[0] = txtDonorName.Text.ToString().Length == 0 ? "" : txtDonorName.Text;
            foodIn[1] = txtCategoryType1.Text.ToString().Length == 0 ? "" : txtCategoryType1.Text;
            foodIn[2] = txtWeight1.Text.ToString().Length == 0 ? "" : txtWeight1.Text;
            foodIn[3] = txtCases1.Text.ToString().Length == 0 ? "" : txtCases1.Text;
            foodIn[4] = txtUSDAItemNo1.Text.ToString().Length == 0 ? "" : txtUSDAItemNo1.Text;

            foodIn[5] = txtCategoryType2.Text.ToString().Length == 0 ? "" : txtCategoryType2.Text;
            foodIn[6] = txtWeight2.Text.ToString().Length == 0 ? "" : txtWeight2.Text;
            foodIn[7] = txtCases2.Text.ToString().Length == 0 ? "" : txtCases2.Text;
            foodIn[8] = txtUSDAItemNo2.Text.ToString().Length == 0 ? "" : txtUSDAItemNo2.Text;

            foodIn[9] = txtCategoryType3.Text.ToString().Length == 0 ? "" : txtCategoryType3.Text;
            foodIn[10] = txtWeight3.Text.ToString().Length == 0 ? "" : txtWeight3.Text;
            foodIn[11] = txtCases3.Text.ToString().Length == 0 ? "" : txtCases3.Text;
            foodIn[12] = txtUSDAItemNo3.Text.ToString().Length == 0 ? "" : txtUSDAItemNo3.Text;

            foodIn[13] = txtCategoryType4.Text.ToString().Length == 0 ? "" : txtCategoryType4.Text;
            foodIn[14] = txtWeight4.Text.ToString().Length == 0 ? "" : txtWeight4.Text;
            foodIn[15] = txtCases4.Text.ToString().Length == 0 ? "" : txtCases4.Text;
            foodIn[16] = txtUSDAItemNo4.Text.ToString().Length == 0 ? "" : txtUSDAItemNo4.Text;

            foodIn[17] = txtCategoryType5.Text.ToString().Length == 0 ? "" : txtCategoryType5.Text;
            foodIn[18] = txtWeight5.Text.ToString().Length == 0 ? "" : txtWeight5.Text;
            foodIn[19] = txtCases5.Text.ToString().Length == 0 ? "" : txtCases5.Text;
            foodIn[20] = txtUSDAItemNo5.Text.ToString().Length == 0 ? "" : txtUSDAItemNo5.Text;
            foodIn[21] = ckbxIsUSDA.Checked == true ? "true" : "false";
            foodIn[22] = txtDate.Text.ToString().Length == 0 ? "" : txtDate.Text;

            Session["PassedFoodInInfo"] = foodIn;
            Session["PassedFoodInURL"] = "../incoming-food/add.aspx";
            Session["newFoodInIDs"] = null;
            Response.Redirect("../donor/add.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** checkForEmptyFields **************************************//
    private void completePageValidation()
    {
        //checks that 2 required input fields are not empty
        //txtDonorName IE the FoodSource
        //txtWeight IE the weight 
        try
        {
            checkEmptyDonorField(); //validationg on donor field for both USDA and regular
            checkDonor(); //checks if the donor is found

            if (ckbxIsUSDA.Checked == true)
            {
                formValdationUSDA(); //cass validation on USDA fields
            }

            if (ckbxIsUSDA.Checked == false)
            {
                formValidationNonUSDA(); //calls validation for Non-USDA fields
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** checkEmptyDonorField **************************************//
    private void checkEmptyDonorField()
    {
        try
        {
            if (ckbxIsUSDA.Checked == true)
            {
                using (CCSEntities db = new CCSEntities())
                {
                    var dnrRslt = (from d in db.FoodSources
                                   where d.Source.Equals("usda", StringComparison.OrdinalIgnoreCase)
                                   && d.FoodSourceType.FoodSourceType1.Equals("usda", StringComparison.OrdinalIgnoreCase)
                                   select d).FirstOrDefault();

                    if (dnrRslt != null)
                    {
                        txtDonorName.Text = dnrRslt.Source;
                    }
                    else
                        lblMessage.Text += "A problem occured when linking USDA to the USDA donor.<br/>";
                }
            }

            if (txtDonorName.Text == null || txtDonorName.Text.Equals(""))
                lblMessage.Text += "The Donor field cannot be empty.<br/>";
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** checkDonor **************************************//
    private bool checkDonor()
    {
        bool passed = true;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var dnrRslt = (from d in db.FoodSources
                               where d.Source.Equals(txtDonorName.Text, StringComparison.OrdinalIgnoreCase)
                               select d).FirstOrDefault();

                if (dnrRslt == null)
                {
                    passed = false;
                    lblMessage.Text += "The Donor does not exist.<br/> Create the new Donor: " + txtDonorName.Text + " first.<br/>";
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return passed;
    }

    //***************************** formValidationNonUSDA method **************************************//
    private void formValidationNonUSDA()
    {
        try
        {
            checkNonUSDAEmptyRowData();

            submitDonationRows = new List<int>(); //creates the list to hold rows that have data to submit to database

            //make sure no previous errors
            if (lblMessage.Text.ToString().Length == 0)
            {
                //ROW 1
                if (!txtCategoryType1.Text.Equals("") && !txtWeight1.Text.Equals(""))
                {
                    bool passWeight = checkWeight(txtWeight1.Text.ToString(), "1");
                    bool passCat = checkCategory(txtCategoryType1.Text.ToString(), "1");
                    bool passCases = true;
                    if (!txtCases1.Text.Equals(""))
                        passCases = checkCases(txtCases1.Text.ToString(), "1");
                    if (passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(1);// add row 1 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "added 1<br>";
                    }
                }

                //ROW 2 
                if (!txtCategoryType2.Text.Equals("") && !txtWeight2.Text.Equals(""))
                {
                    bool passWeight = checkWeight(txtWeight2.Text.ToString(), "2");
                    bool passCat = checkCategory(txtCategoryType2.Text.ToString(), "2");
                    bool passCases = true;
                    if (!txtCases2.Text.Equals(""))
                        passCases = checkCases(txtCases2.Text.ToString(), "2");
                    if (passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(2);// add row 2 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "added 2<br>";
                    }
                }

                //ROW 3 
                if (!txtCategoryType3.Text.Equals("") && !txtWeight3.Text.Equals(""))
                {
                    bool passWeight = checkWeight(txtWeight3.Text.ToString(), "3");
                    bool passCat = checkCategory(txtCategoryType3.Text.ToString(), "3");
                    bool passCases = true;
                    if (!txtCases3.Text.Equals(""))
                        passCases = checkCases(txtCases3.Text.ToString(), "3");
                    if (passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(3);// add row 3 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "added 3<br>";
                    }
                }

                //ROW 4 
                if (!txtCategoryType4.Text.Equals("") && !txtWeight4.Text.Equals(""))
                {
                    bool passWeight = checkWeight(txtWeight4.Text.ToString(), "4");
                    bool passCat = checkCategory(txtCategoryType4.Text.ToString(), "4");
                    bool passCases = true;
                    if (!txtCases4.Text.Equals(""))
                        passCases = checkCases(txtCases4.Text.ToString(), "4");
                    if (passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(4);// add row 4 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "added 4<br>";
                    }
                }

                //ROW 5 
                if (!txtCategoryType5.Text.Equals("") && !txtWeight5.Text.Equals(""))
                {
                    bool passWeight = checkWeight(txtWeight5.Text.ToString(), "5");
                    bool passCat = checkCategory(txtCategoryType5.Text.ToString(), "5");
                    bool passCases = true;
                    if (!txtCases5.Text.Equals(""))
                        passCases = checkCases(txtCases5.Text.ToString(), "5");
                    if (passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(5);// add row 5 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "added 5<br>";
                    }
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


    //***************************** checkNonUSDAEmptyRowData method **************************************//
    private void checkNonUSDAEmptyRowData()
    {
        try
        {
            //checks that not all fields for Catergory and Weight are empty
            if (txtCategoryType1.Text.Equals("") && txtCategoryType2.Text.Equals("") && txtCategoryType3.Text.Equals("")
                && txtCategoryType4.Text.Equals("") && txtCategoryType5.Text.Equals("") && txtWeight1.Text.Equals("") &&
                txtWeight2.Text.Equals("") && txtWeight3.Text.Equals("") && txtWeight4.Text.Equals("") && txtWeight5.Text.Equals(""))
            {
                lblMessage.Text += "Must have category and weight data for at least 1 donation to save.<br/>";
            }
            else
            {
                //ON ROW 1: check if category empty and weight has a value or if category has a value and weight is empty
                if ((txtCategoryType1.Text.Equals("") && !txtWeight1.Text.Equals(""))
                    || (!txtCategoryType1.Text.Equals("") && txtWeight1.Text.Equals(""))
                    || (txtCategoryType1.Text.Equals("") && txtWeight1.Text.Equals("") && !txtCases1.Text.Equals("")))
                {
                    lblMessage.Text += "On row 1, both category and weight must have data to save.<br/>";
                }

                //ON ROW 2: check if category empty and weight has a value or if category has a value and weight is empty
                if ((txtCategoryType2.Text.Equals("") && !txtWeight2.Text.Equals(""))
                    || (!txtCategoryType2.Text.Equals("") && txtWeight2.Text.Equals(""))
                    || (txtCategoryType2.Text.Equals("") && txtWeight2.Text.Equals("") && !txtCases2.Text.Equals("")))
                {
                    lblMessage.Text += "On row 2, both category and weight must have data to save.<br/>";
                }

                //ON ROW 3: check if category empty and weight has a value or if category has a value and weight is empty
                if ((txtCategoryType3.Text.Equals("") && !txtWeight3.Text.Equals(""))
                    || (!txtCategoryType3.Text.Equals("") && txtWeight3.Text.Equals(""))
                    || (txtCategoryType3.Text.Equals("") && txtWeight3.Text.Equals("") && !txtCases3.Text.Equals("")))
                {
                    lblMessage.Text += "On row 3, both category and weight must have data to save.<br/>";
                }

                //ON ROW 4: check if category empty and weight has a value or if category has a value and weight is empty
                if ((txtCategoryType4.Text.Equals("") && !txtWeight4.Text.Equals(""))
                    || (!txtCategoryType4.Text.Equals("") && txtWeight4.Text.Equals(""))
                    || (txtCategoryType4.Text.Equals("") && txtWeight4.Text.Equals("") && !txtCases4.Text.Equals("")))
                {
                    lblMessage.Text += "On row 4, both category and weight must have data to save.<br/>";
                }

                //ON ROW 5: check if category empty and weight has a value or if category has a value and weight is empty
                if ((txtCategoryType5.Text.Equals("") && !txtWeight5.Text.Equals(""))
                    || (!txtCategoryType5.Text.Equals("") && txtWeight5.Text.Equals(""))
                    || (txtCategoryType5.Text.Equals("") && txtWeight5.Text.Equals("") && !txtCases5.Text.Equals("")))
                {
                    lblMessage.Text += "On row 5, both category and weight must have data to save.<br/>";
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

    //***************************** formValdationUSDA method **************************************//
    private void formValdationUSDA()
    {
        try
        {
            checkUSDAEmptyRow();

            submitDonationRows = new List<int>(); //creates the list to hold rows that have data to submit to database

            //make sure no previous errors
            if (lblMessage.Text.ToString().Length == 0)
            {
                //ROW 1
                if (!txtUSDAItemNo1.Text.Equals("") && !txtWeight1.Text.Equals("") && !txtCases1.Text.Equals(""))
                {
                    bool passItemNo = checkUSDA(txtUSDAItemNo1.Text.ToString(), "1");
                    bool passWeight = checkWeight(txtWeight1.Text.ToString(), "1");
                    bool passCases = checkUSDACases(txtCases1.Text.ToString(), "1");
                    bool passCat = true;
                    if (!txtCategoryType1.Text.Equals(""))
                        passCat = checkCategory(txtCategoryType1.Text.ToString(), "1");
                    if (passItemNo && passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(1);// add row 1 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "USDA added 1<br>";
                    }
                }

                //ROW 2 
                if (!txtUSDAItemNo2.Text.Equals("") && !txtWeight2.Text.Equals("") && !txtCases2.Text.Equals(""))
                {
                    bool passItemNo = checkUSDA(txtUSDAItemNo2.Text.ToString(), "2");
                    bool passWeight = checkWeight(txtWeight2.Text.ToString(), "2");
                    bool passCases = checkUSDACases(txtCases2.Text.ToString(), "2");
                    bool passCat = true;
                    if (!txtCategoryType2.Text.Equals(""))
                        passCat = checkCategory(txtCategoryType2.Text.ToString(), "2");
                    if (passItemNo && passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(2);// add row 2 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "USDA added 2<br>";
                    }
                }

                //ROW 3 
                if (!txtUSDAItemNo3.Text.Equals("") && !txtWeight3.Text.Equals("") && !txtCases3.Text.Equals(""))
                {
                    bool passItemNo = checkUSDA(txtUSDAItemNo3.Text.ToString(), "3");
                    bool passWeight = checkWeight(txtWeight3.Text.ToString(), "3");
                    bool passCases = checkUSDACases(txtCases3.Text.ToString(), "3");
                    bool passCat = true;
                    if (!txtCategoryType3.Text.Equals(""))
                        passCat = checkCategory(txtCategoryType3.Text.ToString(), "3");
                    if (passItemNo && passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(3);// add row 3 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "USDA added 3<br>";
                    }
                }

                //ROW 4 
                if (!txtUSDAItemNo4.Text.Equals("") && !txtWeight4.Text.Equals("") && !txtCases4.Text.Equals(""))
                {
                    bool passItemNo = checkUSDA(txtUSDAItemNo4.Text.ToString(), "4");
                    bool passWeight = checkWeight(txtWeight4.Text.ToString(), "4");
                    bool passCases = checkUSDACases(txtCases4.Text.ToString(), "4");
                    bool passCat = true;
                    if (!txtCategoryType4.Text.Equals(""))
                        passCat = checkCategory(txtCategoryType4.Text.ToString(), "4");
                    if (passItemNo && passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(4);// add row 4 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "USDA added 4<br>";
                    }
                }

                //ROW 5 
                if (!txtUSDAItemNo5.Text.Equals("") && !txtWeight5.Text.Equals("") && !txtCases5.Text.Equals(""))
                {
                    bool passItemNo = checkUSDA(txtUSDAItemNo5.Text.ToString(), "5");
                    bool passWeight = checkWeight(txtWeight5.Text.ToString(), "5");
                    bool passCases = checkUSDACases(txtCases5.Text.ToString(), "5");
                    bool passCat = true;
                    if (!txtCategoryType5.Text.Equals(""))
                        passCat = checkCategory(txtCategoryType5.Text.ToString(), "5");
                    if (passItemNo && passWeight == true && passCat == true && passCases == true)
                    {
                        submitDonationRows.Add(5);// add row 5 to the row number to keep track of which row's data to save to the database
                        //lblMessage.Text += "USDA added 5<br>";
                    }
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


    private void checkUSDAEmptyRow()
    {
        try
        {
            //checks that not all fields for USDA item#, Weight, and Cases are empty
            if (txtUSDAItemNo1.Text.Equals("") && txtUSDAItemNo2.Text.Equals("") && txtUSDAItemNo3.Text.Equals("")
                && txtUSDAItemNo4.Text.Equals("") && txtUSDAItemNo5.Text.Equals("") && txtWeight1.Text.Equals("")
                && txtWeight2.Text.Equals("") && txtWeight3.Text.Equals("") && txtWeight4.Text.Equals("") && txtWeight5.Text.Equals("")
                && txtCases1.Text.Equals("") && txtCases2.Text.Equals("") && txtCases3.Text.Equals("") && txtCases4.Text.Equals("")
                && txtCases5.Text.Equals(""))
            {
                lblMessage.Text += "Must have USDA Item#, Weight, and Cases data for at least 1 donation to save.<br/>";
            }
            else
            {
                //ON ROW 1: check if empty USDA item#, Weight, and Cases has a value
                if ((!txtUSDAItemNo1.Text.Equals("") && txtWeight1.Text.Equals("") && txtCases1.Text.Equals(""))
                  || (!txtUSDAItemNo1.Text.Equals("") && !txtWeight1.Text.Equals("") && txtCases1.Text.Equals(""))
                  || (!txtUSDAItemNo1.Text.Equals("") && txtWeight1.Text.Equals("") && !txtCases1.Text.Equals(""))
                  || (txtUSDAItemNo1.Text.Equals("") && !txtWeight1.Text.Equals("") && txtCases1.Text.Equals(""))
                  || (txtUSDAItemNo1.Text.Equals("") && !txtWeight1.Text.Equals("") && !txtCases1.Text.Equals(""))
                  || (txtUSDAItemNo1.Text.Equals("") && txtWeight1.Text.Equals("") && !txtCases1.Text.Equals("")))
                {
                    lblMessage.Text += "On row 1, USDA Item#, Weight, and Cases must have data to save.<br/>";
                }

                //ON ROW 2: check if empty USDA item#, Weight, and Cases has a value
                if ((!txtUSDAItemNo2.Text.Equals("") && txtWeight2.Text.Equals("") && txtCases2.Text.Equals(""))
                 || (!txtUSDAItemNo2.Text.Equals("") && !txtWeight2.Text.Equals("") && txtCases2.Text.Equals(""))
                 || (!txtUSDAItemNo2.Text.Equals("") && txtWeight2.Text.Equals("") && !txtCases2.Text.Equals(""))
                 || (txtUSDAItemNo2.Text.Equals("") && !txtWeight2.Text.Equals("") && txtCases2.Text.Equals(""))
                 || (txtUSDAItemNo2.Text.Equals("") && !txtWeight2.Text.Equals("") && !txtCases2.Text.Equals(""))
                 || (txtUSDAItemNo2.Text.Equals("") && txtWeight2.Text.Equals("") && !txtCases2.Text.Equals("")))
                {
                    lblMessage.Text += "On row 2, USDA Item#, Weight, and Cases must have data to save.<br/>";
                }

                //ON ROW 3: check if empty USDA item#, Weight, and Cases has a value
                if ((!txtUSDAItemNo3.Text.Equals("") && txtWeight3.Text.Equals("") && txtCases3.Text.Equals(""))
                 || (!txtUSDAItemNo3.Text.Equals("") && !txtWeight3.Text.Equals("") && txtCases3.Text.Equals(""))
                 || (!txtUSDAItemNo3.Text.Equals("") && txtWeight3.Text.Equals("") && !txtCases3.Text.Equals(""))
                 || (txtUSDAItemNo3.Text.Equals("") && !txtWeight3.Text.Equals("") && txtCases3.Text.Equals(""))
                 || (txtUSDAItemNo3.Text.Equals("") && !txtWeight3.Text.Equals("") && !txtCases3.Text.Equals(""))
                 || (txtUSDAItemNo3.Text.Equals("") && txtWeight3.Text.Equals("") && !txtCases3.Text.Equals("")))
                {
                    lblMessage.Text += "On row 3, USDA Item#, Weight, and Cases must have data to save.<br/>";
                }

                //ON ROW 4: check if empty USDA item#, Weight, and Cases has a value
                if ((!txtUSDAItemNo4.Text.Equals("") && txtWeight4.Text.Equals("") && txtCases4.Text.Equals(""))
                 || (!txtUSDAItemNo4.Text.Equals("") && !txtWeight4.Text.Equals("") && txtCases4.Text.Equals(""))
                 || (!txtUSDAItemNo4.Text.Equals("") && txtWeight4.Text.Equals("") && !txtCases4.Text.Equals(""))
                 || (txtUSDAItemNo4.Text.Equals("") && !txtWeight4.Text.Equals("") && txtCases4.Text.Equals(""))
                 || (txtUSDAItemNo4.Text.Equals("") && !txtWeight4.Text.Equals("") && !txtCases4.Text.Equals(""))
                 || (txtUSDAItemNo4.Text.Equals("") && txtWeight4.Text.Equals("") && !txtCases4.Text.Equals("")))
                {
                    lblMessage.Text += "On row 4, USDA Item#, Weight, and Cases must have data to save.<br/>";
                }

                //ON ROW 5: check if empty USDA item#, Weight, and Cases has a value
                if ((!txtUSDAItemNo5.Text.Equals("") && txtWeight5.Text.Equals("") && txtCases5.Text.Equals(""))
                 || (!txtUSDAItemNo5.Text.Equals("") && !txtWeight5.Text.Equals("") && txtCases5.Text.Equals(""))
                 || (!txtUSDAItemNo5.Text.Equals("") && txtWeight5.Text.Equals("") && !txtCases5.Text.Equals(""))
                 || (txtUSDAItemNo5.Text.Equals("") && !txtWeight5.Text.Equals("") && txtCases5.Text.Equals(""))
                 || (txtUSDAItemNo5.Text.Equals("") && !txtWeight5.Text.Equals("") && !txtCases5.Text.Equals(""))
                 || (txtUSDAItemNo5.Text.Equals("") && txtWeight5.Text.Equals("") && !txtCases5.Text.Equals("")))
                {
                    lblMessage.Text += "On row 5, USDA Item#, Weight, and Cases must have data to save.<br/>";
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

    //***************************** checkWeight **************************************//
    private bool checkWeight(String weightField, String row)
    {
        bool passed = true;
        try
        {
            double num;
            //checks that the specified zipcode field is a number before inserting it into the database
            if (!(double.TryParse(weightField, out num))) //checks that the weight is a number
            {
                lblMessage.Text += "The Weight on row " + row + " must be a number.<br/>";
                passed = false;
            }

            if (double.TryParse(weightField, out num)) //weight is a number
            {
                double weight = double.Parse(weightField);
                // Math.Round(weight, 2);
                if (weight > 9999999.99)
                {
                    lblMessage.Text += "The Weight on row " + row + " exceeds the database numerical limit to be saved.<br/>";
                    passed = false;
                }

                if (weight == 0)
                {
                    lblMessage.Text += "The Weight on row " + row + " cannot be zero.<br/>";
                    passed = false;
                }

                if (weight < 0)
                {
                    lblMessage.Text += "The Weight on row " + row + " cannot be a negative number.<br/>";
                    passed = false;
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return passed;
    }


    //***************************** checkCategory **************************************//
    private bool checkCategory(String catField, String row)
    {
        bool passed = true;

        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var categoryRslt = (from d in db.FoodCategories
                                    where d.CategoryType.Equals(catField, StringComparison.OrdinalIgnoreCase)
                                    select d).FirstOrDefault();

                if (categoryRslt == null)
                {
                    passed = false;
                    lblMessage.Text += "Category Type on row " + row + " does not exist.<br/> Create the new Category Type: " + catField + "first.<br/>";
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return passed;
    }


    //***************************** checkUSDA **************************************//
    private bool checkUSDA(String usdaItemField, String row)
    {
        bool passed = true;

        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var usCategoryRslt = (from d in db.USDACategories
                                      where d.USDANumber.Equals(usdaItemField, StringComparison.OrdinalIgnoreCase)
                                      select d).FirstOrDefault();

                if (usCategoryRslt == null)
                {
                    passed = false;
                    lblMessage.Text += "USDA Item# on row " + row + " does not exist.<br/> Create the new USDA Item#: " + usdaItemField + "first.<br/>";
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return passed;
    }

    //***************************** checkCases **************************************//
    private bool checkCases(String casesField, String row)
    {
        bool passed = true;

        try
        {
            if (casesField.Length != 0)
            {
                short cases;
                //check if not a number 
                if (!(short.TryParse(casesField, out cases)))
                {
                    lblMessage.Text += "The Number of Cases on row " + row + " must be an integer.<br/>";
                    passed = false;
                }

                if (short.TryParse(casesField, out cases))
                {
                    short numCases = short.Parse(casesField);

                    if (numCases < 0)
                    {
                        lblMessage.Text += "The Number of Cases on row " + row + " cannot be a negative number.<br/>";
                        passed = false;
                    }
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return passed;
    }


    //***************************** checkUSDACases **************************************//
    private bool checkUSDACases(String casesField, String row)
    {
        bool passed = true;

        try
        {
            if (casesField.Length != 0)
            {
                short cases;
                //check if not a number 
                if (!(short.TryParse(casesField, out cases)))
                {
                    lblMessage.Text += "The Number of Cases on row " + row + " must be an integer.<br/>";
                    passed = false;
                }

                if (short.TryParse(casesField, out cases))
                {
                    short numCases = short.Parse(casesField);

                    if (numCases < 0)
                    {
                        lblMessage.Text += "The Number of Cases on row " + row + " cannot be a negative number.<br/>";
                        passed = false;
                    }

                    if (numCases == 0)
                    {
                        lblMessage.Text += "The Number of Cases on row " + row + " cannot be zero.<br/>";
                    }
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return passed;
    }


    //***************************** insertNonUSDADonations method **************************************//
    private void insertNonUSDADonations()
    {
        try
        {
            passedFoodIDS = new List<short>(); //create a list to hold the id's of the inserted donations

            if (submitDonationRows != null && submitDonationRows.Count != 0)
            {
                foreach (int val in submitDonationRows) // Loop through List with foreach
                {
                    switch (val)
                    {
                        case 1:
                            insertNonUSDARow(txtCategoryType1.Text.ToString(), txtWeight1.Text.ToString(), txtCases1.Text.ToString());
                            //lblMessage.Text += "row 1 in inserted NONUSDA <br>";
                            break;
                        case 2:
                            insertNonUSDARow(txtCategoryType2.Text.ToString(), txtWeight2.Text.ToString(), txtCases2.Text.ToString());
                            //lblMessage.Text += "row 2 in inserted NONUSDA<br>";
                            break;
                        case 3:
                            insertNonUSDARow(txtCategoryType3.Text.ToString(), txtWeight3.Text.ToString(), txtCases3.Text.ToString());
                            //lblMessage.Text += "row 3 in inserted NONUSDA<br>";
                            break;
                        case 4:
                            insertNonUSDARow(txtCategoryType4.Text.ToString(), txtWeight4.Text.ToString(), txtCases4.Text.ToString());
                            //lblMessage.Text += "row 4 in inserted NONUSDA<br>";
                            break;
                        case 5:
                            insertNonUSDARow(txtCategoryType5.Text.ToString(), txtWeight5.Text.ToString(), txtCases5.Text.ToString());
                            //lblMessage.Text += "row 5 in inserted NONUSDA<br>";
                            break;
                        default:
                            break;
                    }

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


    //***************************** insertNonUSDARow method **************************************//
    private void insertNonUSDARow(String categoryField, String weightField, String casesField)
    {
        try
        {
            FoodSource fs = lookupDonor();
            FoodCategory fc = null;
            if (categoryField.Length != 0)
                fc = lookupCategory(categoryField);

            if (fs == null)
            {
                lblMessage.Text += "The donation cannot be added yet.<br/>Please create the new Donor:  " + txtDonorName.Text + "  first.<br/>";
            }

            if (fs != null && lblMessage.Text.Length == 0)
            {
                FoodIn addDonation = new FoodIn();

                using (CCSEntities db = new CCSEntities())
                {
                    //insert the following nonUSDA fields
                    //TimeStamp (not null)
                    //Weight (not null)
                    //FoodSourceID(not null)
                    //CategoryID (nullable)
                    //Cases (nullable)

                    //adds a timestamp to the donation record
                    DateTime ds = DateTime.Parse(txtDate.Text);
                    addDonation.TimeStamp = ds;

                    //adds the weight
                    decimal donationWeight = decimal.Parse(weightField);
                    donationWeight = Math.Round(donationWeight, 2);
                    addDonation.Weight = donationWeight;

                    //adds the FoodSourceID of the txtDonorName
                    addDonation.FoodSourceID = fs.FoodSourceID;

                    if (fc == null && categoryField.Length != 0)
                    {
                        lblMessage.Text += "The donation cannot be added yet.<br/> Please create the new Category Type:  " + categoryField + "  first.<br/>";
                    }

                    if (fc != null)
                    {
                        //adds the FoodCategoryID of the txtCategoryType
                        addDonation.FoodCategoryID = fc.FoodCategoryID;
                    }

                    if (casesField.Length != 0)
                    {
                        short numCases = short.Parse(casesField);

                        if (numCases != 0)
                        {
                            //adds the number of cases IE Units to the donation record
                            addDonation.Count = numCases;
                        }
                    }


                    if (lblMessage.Text.Length == 0)
                    {
                        db.FoodIns.Add(addDonation); // add the new food category record
                        db.SaveChanges();

                        passedFoodIDS.Add(addDonation.FoodInID); //adds the newly created donation's incoming-food id to the list 

                        LogChange.logChange("Incoming food from " + fs.Source + " was added.", DateTime.Now, short.Parse(Session["userID"].ToString()));
                    }
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }//end of insertNonUSDARow method



    //***************************** insertUSDADonations method **************************************//
    private void insertUSDADonations()
    {
        try
        {
            passedFoodIDS = new List<short>(); //create a list to hold the id's of the inserted donations

            if (submitDonationRows != null && submitDonationRows.Count != 0)
            {
                foreach (int val in submitDonationRows) // Loop through List with foreach
                {
                    switch (val)
                    {
                        case 1:
                            insertUSDARow(txtUSDAItemNo1.Text.ToString(), txtCategoryType1.Text.ToString(), txtWeight1.Text.ToString(), txtCases1.Text.ToString());
                            //lblMessage.Text += "row 1 in inserted USDA <br>";
                            break;
                        case 2:
                            insertUSDARow(txtUSDAItemNo2.Text.ToString(), txtCategoryType2.Text.ToString(), txtWeight2.Text.ToString(), txtCases2.Text.ToString());
                            //lblMessage.Text += "row 2 in inserted USDA<br>";
                            break;
                        case 3:
                            insertUSDARow(txtUSDAItemNo3.Text.ToString(), txtCategoryType3.Text.ToString(), txtWeight3.Text.ToString(), txtCases3.Text.ToString());
                            //lblMessage.Text += "row 3 in inserted USDA<br>";
                            break;
                        case 4:
                            insertUSDARow(txtUSDAItemNo4.Text.ToString(), txtCategoryType4.Text.ToString(), txtWeight4.Text.ToString(), txtCases4.Text.ToString());
                            //lblMessage.Text += "row 4 in inserted USDA<br>";
                            break;
                        case 5:
                            insertUSDARow(txtUSDAItemNo5.Text.ToString(), txtCategoryType5.Text.ToString(), txtWeight5.Text.ToString(), txtCases5.Text.ToString());
                            //lblMessage.Text += "row 5 in inserted USDA<br>";
                            break;
                        default:
                            break;
                    }

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


    //***************************** insertUSDARow method **************************************//
    private void insertUSDARow(String usdaItemField, String categoryField, String weightField, String casesField)
    {
        try
        {
            FoodSource fs = lookupDonor();
            USDACategory us = null;

            if (usdaItemField.Length != 0)
                us = lookupUSDA(usdaItemField);

            if (fs == null)
            {
                lblMessage.Text += "Problem occured when linking the donation with the USDA donor: " + txtDonorName.Text + ".<br/>";
            }

            if (fs != null && lblMessage.Text.Length == 0)
            {

                FoodIn addDonation = new FoodIn();

                using (CCSEntities db = new CCSEntities())
                {
                    //insert the following USDA fields
                    //TimeStamp (not null)
                    //Weight (not null)
                    //FoodSourceID(not null)
                    //CategoryID (nullable)
                    //USDAID (not)
                    //Units (not)


                    //adds a timestamp to the donation record
                    DateTime ds = DateTime.Parse(txtDate.Text);
                    addDonation.TimeStamp = ds;

                    //adds the weight
                    decimal donationWeight = decimal.Parse(weightField);
                    donationWeight = Math.Round(donationWeight, 2);
                    addDonation.Weight = donationWeight;

                    //adds the FoodSourceID of the txtDonorName
                    addDonation.FoodSourceID = fs.FoodSourceID;

                    if (us == null && usdaItemField.Length != 0)
                    {
                        lblMessage.Text += "The donation cannot be added yet. Please create the new USDA Item Number:  " + usdaItemField + "  first.<br/>";
                    }

                    if (us != null)
                    {
                        //adds the USDAID of the txtUSDAItemNo
                        addDonation.USDAID = us.USDAID;
                    }

                    if (casesField.Length != 0)
                    {
                        short numCases = short.Parse(casesField);

                        //adds the number of cases IE Units to the donation record
                        addDonation.Count = numCases;
                    }

                    if (lblMessage.Text.Length == 0)
                    {
                        db.FoodIns.Add(addDonation); // add the new food category record
                        db.SaveChanges();
                        passedFoodIDS.Add(addDonation.FoodInID); //adds the newly created donation's incoming-food id to the list 

                        LogChange.logChange("Incoming USDA food from " + fs.Source + " was added.", DateTime.Now, short.Parse(Session["userID"].ToString()));
                    }
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }//end of insertUSDARow method


    //***************************** lookupDonor method **************************************//
    private FoodSource lookupDonor()
    {
        FoodSource fs = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var dnrRslt = (from d in db.FoodSources
                               where d.Source.Equals(txtDonorName.Text, StringComparison.OrdinalIgnoreCase)
                               select d).FirstOrDefault();

                if (dnrRslt != null)
                    fs = dnrRslt;
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        return fs;
    }


    //***************************** lookupCategory method **************************************//
    private FoodCategory lookupCategory(String catField)
    {
        FoodCategory fc = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var categoryRslt = (from d in db.FoodCategories
                                    where d.CategoryType.Equals(catField, StringComparison.OrdinalIgnoreCase)
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
    private USDACategory lookupUSDA(String usdaItemField)
    {
        USDACategory us = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var usCategoryRslt = (from d in db.USDACategories
                                      where d.USDANumber.Equals(usdaItemField, StringComparison.OrdinalIgnoreCase)
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


    protected void checkbox_changed(object sender, EventArgs e)
    {
        txtDonorName.Text = "";
        txtCategoryType1.Text = "";
        txtWeight1.Text = "";
        txtCases1.Text = "";
        txtUSDAItemNo1.Text = "";

        txtCategoryType2.Text = "";
        txtWeight2.Text = "";
        txtCases2.Text = "";
        txtUSDAItemNo2.Text = "";

        txtCategoryType3.Text = "";
        txtWeight3.Text = "";
        txtCases3.Text = "";
        txtUSDAItemNo3.Text = "";

        txtCategoryType4.Text = "";
        txtWeight4.Text = "";
        txtCases4.Text = "";
        txtUSDAItemNo4.Text = "";

        txtCategoryType5.Text = "";
        txtWeight5.Text = "";
        txtCases5.Text = "";
        txtUSDAItemNo5.Text = "";

    }
}