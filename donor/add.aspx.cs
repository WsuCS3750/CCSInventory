using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Default2 : System.Web.UI.Page
{
    private List<FoodSourceType> listFoodSourcesTypes;
    private List<State> listUSStates;
    private String pageTarget;
    private String[] passedfoodInInfo;
    private String[] passedDonorInfo;

    //***************************** Page Load **************************************//
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            loadPassedFoodInInfo();

            using (CCSEntities db = new CCSEntities())
            {
                if (!Page.IsPostBack)
                {
                    //adds the Donor types to the drop down list
                    //listFoodSourcesTypes = db.FoodSourceTypes.ToList(); //gets the foodSourceType results from database
                    listFoodSourcesTypes = (from t in db.FoodSourceTypes
                                            where t.FoodSourceType1.ToLower() != "usda"
                                            select t).ToList();

                    ddlDonorType.DataSource = listFoodSourcesTypes; //assigns FoodSourceType list to drop down list
                    ddlDonorType.DataBind();    //binds the source to the drop down list

                    //adds the States to the drop down list
                    listUSStates = db.States.ToList();
                    ddlState.DataSource = listUSStates;
                    ddlState.SelectedIndex = getUtahIndex();
                    ddlState.DataBind();

                    loadPassedDonorInfo();
                }
            }//end of database connection

        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    } //end of PageLoad

    private int getUtahIndex()
    {
        int result = 0;

        for (int i = 0; i < listUSStates.Count; i++)
            if (listUSStates.ElementAt(i).StateFullName.Equals("Utah") || listUSStates.ElementAt(i).StateFullName.Equals("UT"))
            {
                result = i;
                break;
            }

        return result + 1;
    }

    //******************************** loadPassedFoodInInfo ********************************//
    private void loadPassedFoodInInfo()
    {
        try
        {
            if (Session["PassedFoodInInfo"] != null)
            {
                passedfoodInInfo = Session["PassedFoodInInfo"] as String[];
            }

            if (Session["PassedFoodInURL"] != null)
            {
                pageTarget = (String)Session["PassedFoodInURL"];
            }
            
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //******************************** loadPassedDonorInfo ********************************//
    private void loadPassedDonorInfo()
    {
        try
        {
            if (Session["PassedDonorInfo"] != null)
            {
                passedDonorInfo = Session["PassedDonorInfo"] as String[];

                txtDonorName.Text = passedDonorInfo[0];
                txtStoreID.Text = passedDonorInfo[1];
                ddlDonorType.Items.FindByValue(passedDonorInfo[2]).Selected = true;
                txtStreet1.Text = passedDonorInfo[3];
                txtStreet2.Text = passedDonorInfo[4];
                txtCity.Text = passedDonorInfo[5];
                ddlState.Items.FindByValue(passedDonorInfo[6]).Selected = true;
                txtZip.Text = passedDonorInfo[7];
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** btnSave_Click listener **************************************//
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            lblMessage.Text = ""; //empty out the error message field

            checkForNullFields(); //makes sure the required database fields are not null or empty

            
            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                formValidation();   //validate the form information
            }

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                insertCityIfNotExist(); 
            }

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                insertZipIfNotExist();    
            }

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {                
                insertAddrIfNotExist();
            }

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                insertDonorNmIfNotExist(); 
            }

            if (lblMessage.Text.Length == 0 || lblMessage.Text == null)
            {
                if (pageTarget != null)
                {
                    if (passedfoodInInfo != null)
                    {
                        Session["PassedFoodInInfo"] = passedfoodInInfo;
                    }

                    Response.Redirect(pageTarget);
                }
                else
                {
                    Session["PreviousPage"] = null;
                    Session["PassedDonorInfo"] = null;
                    Session["PassedDonorURL"] = null;
                    Response.Redirect("default.aspx"); //goes back to the donor default page
                }
            }

        } //end of try
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

    } // end of btnSave_Click listener


    //***************************** checkForNullFields **************************************//
    private void checkForNullFields() 
    {
        try
        {
            if (txtDonorName.Text == null || txtDonorName.Text.Equals(""))
                lblMessage.Text += "The Donor Name cannot be empty.<br/>";

            if (ddlDonorType.SelectedValue.ToString().Equals("0") || ddlDonorType.SelectedItem.ToString().Equals("< Select Donor Type >"))
                lblMessage.Text += "A Donor Type must be selected.<br/>";
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** lookupZipcode **************************************//
    private Zipcode lookupZipcode()
    {
        Zipcode result = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                //must check if exists
                result = (from t in db.Zipcodes
                          where t.ZipCode1.Equals(txtZip.Text, StringComparison.OrdinalIgnoreCase)
                          select t).FirstOrDefault();
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


    //***************************** lookupCity **************************************//
    private City lookupCity()
    {
        City result = null;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                //must check if exists
                result = (from t in db.Cities
                               where t.CityName.Equals(txtCity.Text, StringComparison.OrdinalIgnoreCase)
                               select t).FirstOrDefault();
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


    //***************************** lookupAddrRecord **************************************//
    private short findAddrRecord()
    {
        short recordID = -1;

        try 
        {
            if (chkAddress.Checked == true)
            {
                short selectedStateID = short.Parse(ddlState.SelectedValue.ToString());

                //get values if not null, otherwise set value to null
                string strAddr1 = null;
                if (txtStreet1.Text != "")
                    strAddr1 = txtStreet1.Text;

                string strAddr2 = null;
                if (txtStreet2.Text != "")
                    strAddr2 = txtStreet1.Text;

                string city = null;
                if (txtCity.Text != "")
                    city = txtCity.Text;

                string zip = null;
                if (txtZip.Text != "")
                    zip = txtZip.Text;

                if (selectedStateID == 0)
                {
                    using (CCSEntities db = new CCSEntities())
                    {
                        var addrWithoutState = (from a in db.Addresses
                                               where a.Zipcode.ZipCode1.Equals(txtZip.Text, StringComparison.OrdinalIgnoreCase)
                                               && a.City.CityName.Equals(txtCity.Text, StringComparison.OrdinalIgnoreCase)
                                               && a.StateID == null
                                               && a.StreetAddress1.Equals(txtStreet1.Text, StringComparison.OrdinalIgnoreCase)
                                               && a.StreetAddress2.Equals(txtStreet2.Text, StringComparison.OrdinalIgnoreCase)
                                               select new { a.AddressID, a.StreetAddress1, a.StreetAddress2, a.City.CityName, a.Zipcode.ZipCode1 }).FirstOrDefault();

                        if (addrWithoutState != null)
                            recordID = addrWithoutState.AddressID;

                    }

                }
                else
                {
                    using (CCSEntities db = new CCSEntities())
                    {
                        var addrWithState = (from a in db.Addresses
                                            where a.Zipcode.ZipCode1.Equals(txtZip.Text, StringComparison.OrdinalIgnoreCase)
                                            && a.City.CityName.Equals(txtCity.Text, StringComparison.OrdinalIgnoreCase)
                                            && a.State.StateID == selectedStateID
                                            && a.StreetAddress1.Equals(txtStreet1.Text, StringComparison.OrdinalIgnoreCase)
                                            && a.StreetAddress2.Equals(txtStreet2.Text, StringComparison.OrdinalIgnoreCase)
                                            select new { a.AddressID, a.StreetAddress1, a.StreetAddress2, a.City.CityName, a.State.StateFullName, a.Zipcode.ZipCode1 }).FirstOrDefault();

                        if (addrWithState != null)
                            recordID = addrWithState.AddressID;
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

        return recordID;
    }


    //***************************** lookupDonorRecord **************************************//
    private short findDonorRecord()
    {
        short recordID = -1;
        try
        {
            short selectedDonorTypeID = short.Parse(ddlDonorType.SelectedValue.ToString());

            short selectedAddrID = findAddrRecord();

            if (txtStoreID.Text.Length == 0 || txtStoreID.Text == null)
            {
                //look for donor record in the FoodSource table that has the same fields as the input provided
                using (CCSEntities db = new CCSEntities())
                {
                    var dnrRsltNoStoreID = (from d in db.FoodSources
                                            where d.Source.Equals(txtDonorName.Text, StringComparison.OrdinalIgnoreCase)
                                            && d.AddressID == selectedAddrID
                                            && d.FoodSourceTypeID == selectedDonorTypeID
                                            select d).FirstOrDefault();

                    if (dnrRsltNoStoreID != null)
                        recordID = dnrRsltNoStoreID.FoodSourceID;
                }
            }
            else //lookup record that has a store id
            {
                //look for donor record in the FoodSource table that has the same fields as the input provided
                using (CCSEntities db = new CCSEntities())
                {
                    var dnrRsltWthStoreID = (from d in db.FoodSources
                                             where d.Source.Equals(txtDonorName.Text, StringComparison.OrdinalIgnoreCase)
                                             && d.StoreID.Equals(txtStoreID.Text, StringComparison.OrdinalIgnoreCase)
                                             && d.FoodSourceTypeID == selectedDonorTypeID
                                             && d.AddressID == selectedAddrID
                                             select d).FirstOrDefault();

                    if (dnrRsltWthStoreID != null)
                        recordID = dnrRsltWthStoreID.FoodSourceID;

                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
        
        return recordID;
    }


    //***************************** insertCityIfNotExist **************************************//
    private void insertCityIfNotExist()
    {
        try
        {
            if (chkAddress.Checked == true)
            {
                City ct = new City();
                City result = lookupCity();

                //the city doesn't exist already. Add it to the database.
                if (result == null)
                {
                    using (CCSEntities db = new CCSEntities())
                    {
                        if (!txtCity.Text.ToString().Equals(""))
                            ct.CityName = txtCity.Text;
                        else
                            ct.CityName = "";
                        
                        db.Cities.Add(ct);
                        db.SaveChanges();

                    }   //end of db connection
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


    //***************************** insertZipIfNotExist **************************************//
    private void insertZipIfNotExist()
    {
        try
        {
            if (chkAddress.Checked == true)
            {
                Zipcode zp = new Zipcode();
                Zipcode result = lookupZipcode();

                //the zipcode doesn't exist already. Add it to the database.
                if (result == null)
                {
                    using (CCSEntities db = new CCSEntities())
                    {
                        if (!txtZip.Text.ToString().Equals(""))
                            zp.ZipCode1 = txtZip.Text.ToString();
                        else
                            zp.ZipCode1 = "";
                        
                        db.Zipcodes.Add(zp);
                        db.SaveChanges();

                    } //end of db connection
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


    //***************************** insertAddrIfNotExist **************************************//
    private void insertAddrIfNotExist()
    {
        try
        {
            if (chkAddress.Checked == true)
            {
                Address addr = new Address();
                short addrRecordID = findAddrRecord();

                //an Address record with street addr 1, street addr2, and zipcode do not already exist. Add it to the database.
                if (addrRecordID == -1)
                {
                    using (CCSEntities db = new CCSEntities())
                    {
                        if (!txtStreet1.Text.ToString().Equals(""))
                            addr.StreetAddress1 = txtStreet1.Text.ToString();
                        else
                            addr.StreetAddress1 = "";

                        if (!txtStreet2.Text.ToString().Equals(""))
                            addr.StreetAddress2 = txtStreet2.Text.ToString();
                        else
                            addr.StreetAddress2 = "";

                        City cityResult = lookupCity();
                        short statesResult = short.Parse(ddlState.SelectedValue.ToString());
                        Zipcode zipResult = lookupZipcode();

                        if (lblMessage.Text.Length == 0)
                        {
                            addr.CityID = cityResult.CityID;

                            if (statesResult != 0)
                                addr.StateID = statesResult;    //adds the stateID
                            else
                                addr.StateID = null;
                            
                            addr.ZipID = zipResult.ZipID;
                            db.Addresses.Add(addr);
                            db.SaveChanges();

                        } //end of if

                    } //end of using db connection

                } //end of outer else           
            } // end of checkBoxAddress checked if
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


    //***************************** insertDonorNmIfNotExist **************************************//
    private void insertDonorNmIfNotExist()
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                FoodSource fs = new FoodSource(); // AKA donor
                short donorRecordID = findDonorRecord(); //checks if donor the exists already


                //add the new donor to the database, since it doesn't exist already
                if (donorRecordID == -1)
                {
                    //FoodSource Table fields: Source, StoreID(Nullable), FoodSourceTypeID, and AddressID(Nullable)
                    fs.Source = txtDonorName.Text; //adds the Source

                    if (!txtStoreID.Text.Equals(""))
                    {
                        fs.StoreID = txtStoreID.Text.ToString(); //adds the storeID in not empty
                    }

                    short donorType = short.Parse(ddlDonorType.SelectedValue.ToString());
                    fs.FoodSourceTypeID = donorType;    //adds the FoodSourceTypeID

                    if (chkAddress.Checked == true)
                    {
                        short addrRecordID = findAddrRecord();

                        //add the donor to the database if the able to lookup the address after the address was just inserted in 
                        if (addrRecordID != -1 && lblMessage.Text.Length == 0)
                        {
                            fs.AddressID = addrRecordID; //adds the AddressID to the FoodSource
                            db.FoodSources.Add(fs); //add the foodSource to the database
                            db.SaveChanges();

                            LogChange.logChange("Donor " + fs.Source + " was added.", DateTime.Now, short.Parse(Session["userID"].ToString()));
                        }
                        else
                        {
                            lblMessage.Text += "The donor was unable to be added.<br/>";
                        }
                    }
                    else
                    {
                        fs.AddressID = null; //set AddressID to null 
                        db.FoodSources.Add(fs); //add the foodSource to the database
                        db.SaveChanges();
                    }
                }
                else        
                {
                    lblMessage.Text += txtDonorName.Text + " donor with that address exists already!<br/>";
                }
     
            } //end of using statement
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    } //end of insertDonorNmIfNotExist


    //***************************** formValidation method **************************************//
    private void formValidation()
    {
        try
        {
            //txtDonorName
            //txtStoreID
            //txtStreet1 *
            //txtStreet2 *
            //txtCity *
            //txtState
            //txtZip  (limit to 5)
            //ddlDonorType

            int num;
        
            //check that the values don't exceed database limits
            if (txtDonorName.Text.Length > 30) //checks that the street addr1 is not longer than 50 characters
            {
                lblMessage.Text += "The donor's name cannot exceed 30 characters.<br/>";
            }

            if (txtStoreID.Text.Length > 10) //checks that the street addr1 is not longer than 50 characters
            {
                lblMessage.Text += "The Store ID cannot exceed 10 characters.<br/>";

            }

            if (txtStreet1.Text.Length > 50) //checks that the street addr1 is not longer than 50 characters
            {
                lblMessage.Text += "The street address 1 field cannot exceed 50 characters.<br/>";
            }

            if (txtStreet2.Text.Length > 50) //checks that the street addr2 is not longer than 50 characters
            {
                lblMessage.Text += "The street address 2 field cannot exceed 50 characters.<br/>";
            }
        
            if (txtCity.Text.Length > 30) //checks that the city is not longer than 30 characters
            {
                lblMessage.Text += "The city cannot exceed 30 characters.<br/>";
            }

            if (txtZip.Text.Length != 5 && txtZip.Text.Length != 0) //checks that the zipcode is only 5 digits
            {
                lblMessage.Text += "The zipcode must be a 5 digits.<br/>";
            }

        
            //check if user input just spaces in all fields
            if (txtDonorName.Text.Length > 0 && txtDonorName.Text.Trim().Length == 0)
            {
                lblMessage.Text += "Spaces are not valid input for the Donor's name.<br/>";
            }

            if (txtCity.Text.Length > 0 && txtCity.Text.Trim().Length == 0)
            {
                lblMessage.Text += "Spaces are not valid input for the city.<br/>";
            }

            //checks that the specified zipcode field is a number before inserting it into the database
            if (!(int.TryParse(txtZip.Text, out num)) && txtZip.Text.Length > 0) //checks that the zipcode is a number
            {
                lblMessage.Text += "The zipcode must be a number.<br/>";
            }
        
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    } //end of validation method


    //***************************** btnCancel_Click method **************************************//
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try 
        {
            if (pageTarget != null)
            {
                if (passedfoodInInfo != null)
                {
                    Session["PassedFoodInInfo"] = passedfoodInInfo;
                }

                Response.Redirect(pageTarget);
            }
            else
            {
                Session["PreviousPage"] = null;
                Session["PassedDonorInfo"] = null;
                Session["PassedDonorURL"] = null;
                Response.Redirect("default.aspx"); //goes back to the donor default page
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    //***************************** toggleAddress method **************************************//
    // @author: Nittaya Phonharath - toggleAddress()
    // Toggles and either hides/shows the address gridview.
    protected void toggleAddress(object sender, EventArgs e)
    {
        // show addres
        if (chkAddress.Checked == true)
            addressDiv.Visible = true;

        // hide address div
        if (chkAddress.Checked == false)
            addressDiv.Visible = false;
    }

    //***************************** btnAddDonorType_Click method **************************************//
    protected void btnAddDonorType_Click(object sender, EventArgs e)
    {
        try
        {
            String[] fs = new String[8];
            fs[0] = txtDonorName.Text.ToString().Length == 0 ? "" : txtDonorName.Text;
            fs[1] = txtStoreID.Text.ToString().Length == 0 ? "" : txtStoreID.Text;
            fs[2] = ddlDonorType.SelectedValue.ToString();
            fs[3] = txtStreet1.Text.ToString().Length == 0 ? "" : txtStreet1.Text;
            fs[4] = txtStreet2.Text.ToString().Length == 0 ? "" : txtStreet2.Text;
            fs[5] = txtCity.Text.ToString().Length == 0 ? "" : txtCity.Text;
            fs[6] = ddlState.SelectedValue.ToString();
            fs[7] = txtZip.Text.ToString().Length == 0 ? "" : txtZip.Text;
            
            Session["PassedDonorInfo"] = fs;
            Session["PassedDonorURL"] = "../donor/add.aspx";
            Response.Redirect("../donor-type/add.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }


}