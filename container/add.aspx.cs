using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity.Validation;

public partial class container_add : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool autoGenerate = chkAutoGen.Checked;
        int binNumber = 0;
        using (CCSEntities db = new CCSEntities())
        {
            if (autoGenerate)
            {
                Random rnd = new Random();

                while (binNumber == 0)
                {
                    int BinNumberCandidate = rnd.Next(1000, 9999);

                    int count = (from c in db.Containers
                                 where c.BinNumber == BinNumberCandidate
                                 select c).Count();

                    if (count == 0)
                    {
                        binNumber = BinNumberCandidate;
                    }
                }

            }
            else
            {
                if (txtBinNumber.Text != "")
                {
                    short BinNumberCandidate = short.Parse((txtBinNumber.Text));

                    int count = (from c in db.Containers
                                 where c.BinNumber == BinNumberCandidate
                                 select c).Count();

                    if (count == 0)
                    {
                        binNumber = BinNumberCandidate;
                    }
                }
                else
                {
                    lblError.Text = "Whoops! an error occured";
                    lblError.Visible = true;
                }
            }

            if (binNumber != 0)
            {
                Container c = new Container();

                if (Session["container"] != null)
              	{
		            Container copyFromContainer = (Container)Session["container"];
                    c.USDAID = copyFromContainer.USDAID;
		            c.FoodCategoryID = copyFromContainer.FoodCategoryID;
		            c.isUSDA = copyFromContainer.isUSDA;
		            c.Weight = copyFromContainer.Weight;
		            c.Cases = copyFromContainer.Cases;
                    c.FoodSourcesTypeID = copyFromContainer.FoodSourcesTypeID;
	        
		        }
                c.BinNumber = (short) binNumber;

                Location l = (from lo in db.Locations
                              where lo.RoomName == "(NONE)"
                              select lo).First();
                c.Location = l;

                if(c.isUSDA == null)
                    c.isUSDA = false;

                c.DateCreated = DateTime.Now;

                db.Containers.Add(c);

                try
                {
                    db.SaveChanges();
                    LogChange.logChange("New Container " + binNumber + " Created.", DateTime.Now,
                                        short.Parse(Session["userID"].ToString()));
                    Response.Redirect("edit.aspx?id=" + binNumber + "&type=new");
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " +
                                           validationError.ErrorMessage);
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
            else
            {
                lblError.Text = "That bin number is already in use.";
                lblError.Visible = true;
            }
        }
    }
}