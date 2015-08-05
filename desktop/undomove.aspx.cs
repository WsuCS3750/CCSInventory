using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class desktop_undomove : System.Web.UI.Page
{

    private List<Location> lstLocation;
    int id;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userID"] == null)
            Response.Redirect("login.aspx");
        else
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["do"].Equals("u"))
                {
                    lblTitle.Text = "Undo Food Out Transaction";

                    using (CCSEntities db = new CCSEntities())
                    {
                        id = short.Parse(Session["editWhat"].ToString());

                        FoodOut f1 = (from f0 in db.FoodOuts
                              where f0.BinNumber == id
                              select f0).First();

                        lblBinNumber.Text = "Bin Number: " + f1.BinNumber.ToString();
                        lblCount.Text = "Count: " + f1.Count.ToString();
                        lblWeight.Text = "Weight: " + f1.Weight.ToString();
                    }

                    try
                    {
                        using (CCSEntities db = new CCSEntities())
                        {
                            lstLocation = db.Locations.OrderBy(x => x.RoomName).ToList();
                            ddlLocation.DataSource = lstLocation;
                            ddlLocation.DataBind();
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

    // Saves data from the bin that is to be moved off of the line and 
    // re-inserts it to a new container.
    
    protected void btnUndoMoveSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlLocation.SelectedValue != "0")
            {

                FoodOut f;
                Container c;
                int count;

                using (CCSEntities db = new CCSEntities())
                {
                    id = short.Parse(Session["editWhat"].ToString());

                    f = (from fo in db.FoodOuts
                         where fo.BinNumber == id
                         select fo).First();

                    count = (from ct in db.Containers
                             where ct.BinNumber == id
                             select ct).Count();

                    if (count == 0)
                    {
                        Container inLog = new Container();

                        short typeVal = short.Parse(ddlLocation.SelectedValue);

                        Location l = (from lo in db.Locations
                                      where lo.LocationID == typeVal
                                      select lo).First();

                        inLog.Location = l;

                        inLog.FoodSourceType = f.FoodSourceType;
                        inLog.Cases = f.Count;

                        if (f.USDACategory != null)
                            inLog.USDACategory = f.USDACategory;
                        else
                            inLog.FoodCategory = f.FoodCategory;
                        
                        if (f.USDACategory != null)
                            inLog.isUSDA = true;
                        else
                            inLog.isUSDA = false;

                        inLog.Weight = (decimal)f.Weight;
                        inLog.BinNumber = f.BinNumber;
                        inLog.FoodSourceType = (from co in db.FoodSourceTypes select co).FirstOrDefault();
                        inLog.DateCreated = f.DateCreated;
                        
                        db.Containers.Add(inLog);
                        db.FoodOuts.Remove(f);
                    }
                    else
                    {
                        c = (from co in db.Containers
                             where co.BinNumber == id
                             select co).First();

                        short typeVal = short.Parse(ddlLocation.SelectedValue);

                        Location l = (from lo in db.Locations
                                      where lo.LocationID == typeVal
                                      select lo).First();

                        c.Location = l;

                        if (f.USDACategory != null)
                            c.USDACategory = f.USDACategory;
                        else
                            c.FoodCategory = f.FoodCategory;

                        c.FoodSourceType = f.FoodSourceType;

                        if (f.USDACategory != null)
                            c.isUSDA = true;
                        else
                            c.isUSDA = false;

                        c.Weight = (decimal)f.Weight;
                        c.Cases = f.Count;

                        db.FoodOuts.Remove(f);
                    }

                    db.SaveChanges();

                    LogChange.logChange("Container " + f.BinNumber + " was moved back into inventory.", DateTime.Now, short.Parse(Session["userID"].ToString()));
                }

                Response.Redirect("~/");

            }
            else
            {
                lblError.Text = "Please select a Location.";
                lblError.Visible = true;
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