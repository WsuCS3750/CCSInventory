using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DistributionTypeManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bindGridView();
    }

    private void bindGridView()
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                var distributionTypes = db.DistributionTypes.ToList();
                distributionTypes.Sort((x, y) => String.Compare(x.DistributionType1, y.DistributionType1));

                gvDistributionTypes.DataSource = distributionTypes;
                gvDistributionTypes.DataBind();
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // Paging the gridView
    protected void gvDistributionTypes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvDistributionTypes.PageIndex = e.NewPageIndex;
        bindGridView();
    }
}