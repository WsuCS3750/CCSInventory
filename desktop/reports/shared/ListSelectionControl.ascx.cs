using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

/// <summary>
/// Displays two list boxes that allows the user to either include or not included items.
/// </summary>
public partial class desktop_reports_shared_ListSelectionControl : System.Web.UI.UserControl
{
    /// <summary>
    /// Title of the control to show to the user.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// The type of selection the user made, such as all, none, or some. Pluse
    /// some other options related to food categories
    /// </summary>
    public  ReportTemplate.SelectionType SelectedType
    {
        get
        {
            if (chkAll.Checked)
                return ReportTemplate.SelectionType.ALL;
            else if (chkNone.Checked)
                return ReportTemplate.SelectionType.NONE;
            else if (chkPerishable.Checked)
                return ReportTemplate.SelectionType.PERISHABLE;
            else if (chkRegular.Checked)
                return ReportTemplate.SelectionType.REGULAR;
            else if (chkNonFood.Checked)
                return ReportTemplate.SelectionType.NONFOOD;
            else
                return ReportTemplate.SelectionType.SOME;
        }
        set
        {
            if (value == ReportTemplate.SelectionType.ALL)
                chkAll.Checked = true;
            else if (value == ReportTemplate.SelectionType.NONE)
                chkNone.Checked = true;
            else if (value == ReportTemplate.SelectionType.PERISHABLE)
                chkPerishable.Checked = true;
            else if (value == ReportTemplate.SelectionType.REGULAR)
                chkRegular.Checked = true;
            else if (value == ReportTemplate.SelectionType.NONFOOD)
                chkNonFood.Checked = true;
            else
                tableSelectItems.Visible = true;

        }
    }


    private bool _AllowNone = true;

    /// <summary>
    /// Specifies if the the user is allowed to select non
    /// </summary>
    public bool AllowNone
    { 
        get { return _AllowNone; }
        set
        {
            _AllowNone = value;
            chkNone.Visible = _AllowNone;
        }
    }


    private bool _FoodCategories = false;

    /// <summary>
    /// States if the list box and be chosen by food category type
    /// </summary>
    public bool FoodCategories
    {
        get { return _AllowNone; }
        set
        {   
            _FoodCategories = value;
            pnlFoodTypes.Visible = _FoodCategories;
        }
    }

    /// <summary>
    /// The list the user can choose for
    /// </summary>
    public object AvailableList
    {
        get { return lstAvailableItems.DataSource; }
        set
        {
            lstAvailableItems.DataSource = value;
            lstAvailableItems.DataBind();
        }
    }

    /// <summary>
    /// The object name to show as the text in each list box
    /// </summary>
    public string DataTextField
    {
        get { return lstAvailableItems.DataTextField; }
        set
        {
            lstAvailableItems.DataTextField = value;
            lstChosenItems.DataTextField = value;
        }
    }

    /// <summary>
    /// The object name to hold as the value in each list box
    /// </summary>
    public string DataValueField
    {
        get { return lstAvailableItems.DataValueField; }
        set
        {
            lstAvailableItems.DataValueField = value;
            lstChosenItems.DataValueField = value;
        }
    }

    /// <summary>
    /// The list of IDs of the items selected by the user.
    /// </summary>
    public List<string> SelectedIDs
    {
        get
        {
            List<string> items = new List<string>();
            foreach (ListItem i in lstChosenItems.Items)
            {
                items.Add(i.Value);
            }
            return items;
        }
        set
        {
            List<ListItem> itemsToTransfer = new List<ListItem>();

            foreach (string i in value)
            {
                foreach (ListItem item in lstAvailableItems.Items)
                {
                    if (item.Value == i)
                    {
                        itemsToTransfer.Add(item);
                    }
                }
            }

            foreach (ListItem item in itemsToTransfer)
                lstChosenItems.Items.Add(item);

            
            foreach (ListItem item in itemsToTransfer)
                lstAvailableItems.Items.Remove(item);

        }
    }
   
    /// <summary>
    /// Tells if the user has checked all
    /// </summary>
    public bool SelectAll
    {
        get { return chkAll.Checked; }
        set { chkAll.Checked = value; }
    }

    /// <summary>
    /// Tells if the user has checked none
    /// </summary>
    public bool SelectNone
    {
        get { return chkNone.Checked; }
        set { chkNone.Checked = value; }
    }

    /// <summary>
    /// Loads the previously chosen options picked by the user 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// Adds items selected by the user to the show list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        List<ListItem> itemsToRemove = new List<ListItem>();
        foreach (var i in lstAvailableItems.GetSelectedIndices())
        { 
            ListItem item = lstAvailableItems.Items[i];
            itemsToRemove.Add(item);
            lstChosenItems.Items.Add(item);
        }

        foreach (var i in itemsToRemove)
            lstAvailableItems.Items.Remove(i);
    }

    /// <summary>
    /// Removes items selected by the user from the show list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemove_Click(object sender, EventArgs e)
    {
        List<ListItem> itemsToRemove = new List<ListItem>();
        foreach (var i in lstChosenItems.GetSelectedIndices())
        {
            ListItem item = lstChosenItems.Items[i];
            itemsToRemove.Add(item);
            lstAvailableItems.Items.Add(item);
        }

        foreach (var i in itemsToRemove)
            lstChosenItems.Items.Remove(i);
    }

    /// <summary>
    /// When the check states are change this verifies only one is chosen at a time
    /// and if none are check then it shows the checke dbox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void CheckChanged(object sender, EventArgs e)
    {
        bool wasChecked = (sender as CheckBox).Checked;

        foreach (Control ctl in pnlCheckboxes.Controls)
        {
            if (ctl != sender && ctl is CheckBox)
            {
                (ctl as CheckBox).Checked = false;
            }
        }

        if(wasChecked)
        {
            tableSelectItems.Visible = false;
        }
        else
        {
            tableSelectItems.Visible = true;
        }
    }
}