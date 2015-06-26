using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddDistributionType : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSave_Click(object sender, EventArgs e)
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
                DistributionType dt = new DistributionType();
                dt.DistributionType1 = txtDistributionType.Text;

                using (CCSEntities db = new CCSEntities())
                {
                    db.DistributionTypes.Add(dt);
                    db.SaveChanges();
                }
                LogChange.logChange("Added Distribution Type " + txtDistributionType.Text, DateTime.Now, short.Parse(Session["userID"].ToString()));
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

    Boolean isDistributionTypeExisting(String distributionType)
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
}