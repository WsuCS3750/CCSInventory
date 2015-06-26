using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditDistributionType : System.Web.UI.Page
{
    private int id;
    private DistributionType dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            loadDistributionType();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        updateDistributionType();
    }

    protected void btnRemove_Click(object sender, EventArgs e)
    {
        removeDistributionType();
    }

    private void updateDistributionType()
    {
        try
        {
            lblMessage.Text = "";

            if (txtDistributionType.Text.Length == 0)                       // if distribution type isn't empty
                lblMessage.Text = "The distribution type can't be blank";
            else if (txtDistributionType.Text.Length > 30)                  // if distribution isn't too long
                lblMessage.Text = "The distribution type can't be longer than 30 characters in length";
            else if (isDistributionTypeExisting(txtDistributionType.Text))  // if distribution type doesn't already exist
                lblMessage.Text = "A distribution type of that name already exists";
            else
            {
                id = int.Parse(Request.QueryString["id"]);

                using (CCSEntities db = new CCSEntities())
                {
                    dt = db.DistributionTypes.Where(x => x.DistributionTypeID == id).FirstOrDefault();
                    dt.DistributionType1 = txtDistributionType.Text;

                    db.SaveChanges();
                }
                LogChange.logChange("Edited Distribution Type " + txtDistributionType.Text, DateTime.Now, short.Parse(Session["userID"].ToString()));
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

    private void removeDistributionType()
    {
        try
        {
            lblMessage.Text = "";

            if (Request.QueryString["id"] != null)
                id = int.Parse(Request.QueryString["id"]);
            else
                Response.Redirect("default.aspx");

            if (!isDistributionTypeUsed(id))
            {
                using (CCSEntities db = new CCSEntities())
                {
                    dt = db.DistributionTypes.Where(x => x.DistributionTypeID == id).FirstOrDefault();

                    if (dt != null)
                    {
                        String distributionType = dt.DistributionType1;
                        db.DistributionTypes.Remove(dt);
                        db.SaveChanges();

                        LogChange.logChange("Removed Distribution Type called " + distributionType + ".", DateTime.Now, short.Parse(Session["UserID"].ToString()));
                        Response.Redirect("default.aspx");
                    }
                }
            }
            else
            {
                lblMessage.Text = "This Distribution Type can't be deleted because it is used in a Food Out record.";
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private Boolean isDistributionTypeExisting(String distributionType)
    {
        Boolean result = false;
        DistributionType dt = new DistributionType();
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                dt = db.DistributionTypes.Where(x => x.DistributionType1.Equals(distributionType)).FirstOrDefault();
            }

            if (dt != null)
                result = true;
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }

        return result;
    }

    private Boolean isDistributionTypeUsed(int ID)
    {
        Boolean result = true;
        FoodOut fo;
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                fo = db.FoodOuts.Where(x => x.DistributionTypeID == ID).FirstOrDefault();

                if (fo == null)
                    result = false;
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

    private void loadDistributionType()
    {
        try
        {
            lblMessage.Text = "";

            if (Request.QueryString["id"] != null)
                id = int.Parse(Request.QueryString["id"]);
            else
                Response.Redirect("default.aspx");

            using (CCSEntities db = new CCSEntities())
            {
                dt = db.DistributionTypes.Where(x => x.DistributionTypeID == id).FirstOrDefault();
            }

            if (dt != null)
            {
                txtDistributionType.Text = dt.DistributionType1;
            }
            else
                lblMessage.Text = "The requested distribution type could not be found.";

        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
}