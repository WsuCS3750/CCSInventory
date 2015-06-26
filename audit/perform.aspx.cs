using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Perform : System.Web.UI.Page
{
    private List<Container> lstUnverifiedContainers;
    private DataTable dtChanges;
    private String sortingColumn;
    private Boolean sortAscending;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            txtContainerNumber.Focus();
            lblMessage.Text = "";

            if (!IsPostBack)
            {
                sortingColumn = "ContainerNumber"; // default sort
                sortAscending = true;
                ViewState["sortingColumn"] = sortingColumn;
                ViewState["sortAscending"] = sortAscending;
            }
            else if (ViewState["sortingColumn"] != null)
            {
                sortingColumn = (String)ViewState["sortingColumn"];
                sortAscending = (Boolean)ViewState["sortAscending"];
            }

            if (Session["unverified"] == null)
            {
                loadUnverified();
                loadChanges();
            }
            else
            {
                lstUnverifiedContainers = (List<Container>) Session["unverified"];
                dtChanges = (DataTable)Session["changes"];
                loadUnverified(lstUnverifiedContainers);
                loadChanges(dtChanges);
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void loadUnverified()
    {
        try
        {
            using (CCSEntities db = new CCSEntities())
            {
                lstUnverifiedContainers = db.Containers.Include("FoodCategory").Include("Location").Include("USDACategory").ToList();
            }

            loadUnverified(lstUnverifiedContainers);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void loadUnverified(List<Container> lstUnverifiedContainers)
    {
        try
        {
            if (lstUnverifiedContainers != null && lstUnverifiedContainers.Count > 0)
            {
                lblUnverified.Text = "Unverified Containers:";

                // sort list according to user choice
                if (sortingColumn != null)
                {
                    switch (sortingColumn)
                    {
                        case "ContainerNumber": // if user wants to sort by CategoryType
                            if (sortAscending)
                                lstUnverifiedContainers.Sort((x, y) => x.BinNumber.CompareTo(y.BinNumber)); // ascending CategoryType
                            else
                                lstUnverifiedContainers.Sort((x, y) => y.BinNumber.CompareTo(x.BinNumber)); // descending CategoryType
                            break;
                        case "Location": // if user wants to sort by Location
                            if (sortAscending)
                                lstUnverifiedContainers.Sort((x, y) => String.Compare(x.Location.RoomName, y.Location.RoomName)); // ascending location
                            else
                                lstUnverifiedContainers.Sort((x, y) => String.Compare(y.Location.RoomName, x.Location.RoomName)); // descending location

                            break;
                    }
                }

                gvUnverified.DataSource = lstUnverifiedContainers;
                gvUnverified.DataBind();
                Session["unverified"] = lstUnverifiedContainers;
            }
            else
            {
                Session["unverified"] = lstUnverifiedContainers;
                lblUnverified.Text = "All Containers Verified";
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void loadChanges()
    {
        try
        {
            dtChanges = new DataTable();

            dtChanges.Columns.Add("Container Number");
            dtChanges.Columns.Add("Weight");
            dtChanges.Columns.Add("Food Category");
            dtChanges.Columns.Add("Location");
            dtChanges.Columns.Add("USDA");
            dtChanges.Columns.Add("USDA Number");
            dtChanges.Columns.Add("Units");

            loadChanges(dtChanges);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void loadChanges(DataTable dtChanges)
    {
        try
        {
            if (dtChanges != null && dtChanges.Rows.Count > 0)
            {
                lblAdjustments.Text = "Container Adjustments:";
                gvChanges.DataSource = dtChanges;
                gvChanges.DataBind();
                Session["changes"] = dtChanges;
            }
            else
            {
                Session["changes"] = dtChanges;
                lblAdjustments.Text = "No Adjustments Made";
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void pushSession()
    {
        try
        {
            Session["unverified"] = lstUnverifiedContainers;
            Session["changes"] = dtChanges;
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void pullSession()
    {
        try
        {
            if (Session["unverified"] == null || Session["changes"] == null)
                lblMessage.Text = "An unexpected problem occurred. Please try again later.";
            else
            {
                lstUnverifiedContainers = (List<Container>)Session["unverified"];
                dtChanges = (DataTable)Session["changes"];
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    protected void btnLookupContainer_Click(object sender, EventArgs e)
    {
        try
        {
            int containerID;
            if (txtContainerNumber.Text == "" || !int.TryParse(txtContainerNumber.Text, out containerID))
                lblMessage.Text = "You must enter a valid Container number";
            else
            {
                String containerNumber;

                using (CCSEntities db = new CCSEntities())
                {
                    containerNumber = (from c in db.Containers            // lookup container with containerNumber
                          where c.BinNumber == containerID
                          select c.ContainerID).FirstOrDefault().ToString();
                }

                if (containerNumber.Equals("0")) // default value
                    lblMessage.Text = "You must enter a valid container number.";
                else if (lstUnverifiedContainers.Find(c => c.ContainerID == containerID) == null)
                    lblAlreadyVerified.Visible = true;
                else
                    Response.Redirect("verify.aspx?id=" + containerNumber, false);
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
    protected void btnFinish_Click(object sender, EventArgs e)
    {
        try
        {
            pushChangeLog();

            Session["unverified"] = null;
            Session["changes"] = null;

            Response.Redirect("default.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    private void pushChangeLog()
    {
        try
        {
            // connect
            // make audit record, retain pk
            // for each row in datatable
            //  add record to Adjustment
            //  add record to AuditAdjustment
            dtChanges = (DataTable)Session["changes"];

            using (CCSEntities db = new CCSEntities())
            {
                Audit audit = new Audit();
                audit.Date = DateTime.Now;
                audit.UserID = short.Parse(Session["userID"].ToString());

                db.Audits.Add(audit);

                Adjustment adjustment;

                for (int i = 0; i < dtChanges.Rows.Count; i++)
                {
                    adjustment = new Adjustment();

                    if (!dtChanges.Rows[i][1].ToString().Equals(""))
                        adjustment.Weight = Decimal.Parse(dtChanges.Rows[i][1].ToString());

                    adjustment.FoodCategory = dtChanges.Rows[i][2].ToString();
                    adjustment.Location = dtChanges.Rows[i][3].ToString();

                    if (!dtChanges.Rows[i][4].ToString().Equals(""))
                        adjustment.isUSDA = Boolean.Parse(dtChanges.Rows[i][4].ToString());

                    adjustment.USDANumber = dtChanges.Rows[i][5].ToString();

                    if (!dtChanges.Rows[i][6].ToString().Equals(""))
                        adjustment.Cases = short.Parse(dtChanges.Rows[i][6].ToString());

                    adjustment.AuditID = audit.AuditID;

                    db.Adjustments.Add(adjustment); // add record
                }
                db.SaveChanges();               // update db table
            }

            LogChange.logChange("Performed an audit.", DateTime.Now, short.Parse(Session["userID"].ToString()));
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            Session["changes"] = null;
            Session["unverified"] = null;
            Response.Redirect("default.aspx");
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // Paging the gridView
    protected void gvUnverified_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvUnverified.PageIndex = e.NewPageIndex;
            if (lstUnverifiedContainers == null)
                pullSession();
            loadUnverified(lstUnverifiedContainers);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }

    // Paging the gridView
    protected void gvChanges_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvChanges.PageIndex = e.NewPageIndex;
            if (dtChanges == null)
                pullSession();
            loadChanges(dtChanges);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
    
    // sorting the GridView
    protected void TaskGridView_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            if (sortingColumn != null && sortingColumn.Equals(e.SortExpression))    // if the list is already sorted by this column,                                                                  
            {                                                                       // toggle ascending/descending
                sortAscending = !sortAscending;
            }
            else                                                                    // else, set new column and set sorting to ascending
            {
                sortingColumn = e.SortExpression;
                sortAscending = true;
            }

            ViewState["sortingColumn"] = sortingColumn;
            ViewState["sortAscending"] = sortAscending;

            if (lstUnverifiedContainers == null)
                pullSession();

            loadUnverified(lstUnverifiedContainers);
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
            Response.Redirect("../errorpages/error.aspx");
        }
    }
}