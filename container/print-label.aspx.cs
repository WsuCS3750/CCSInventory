using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Drawing;


public partial class container_print : System.Web.UI.Page
{
    public int id;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            id = int.Parse(Request.QueryString["id"]);

            using (CCSEntities db = new CCSEntities())
            {
                Container container = (from c in db.Containers
                             where c.BinNumber == (Int16)id
                             select c).FirstOrDefault();

                String type = (bool)container.isUSDA ?
                    container.USDACategory.Description + " (" + container.USDACategory.USDANumber + ") " + container.Cases + " Cases" : 
                    container.FoodCategory.CategoryType + " - " + container.Weight + " Lbs";

                decimal weight = container.Weight;
           

                string date = DateTime.Today.ToString("d");
                string sId = id.ToString();
                int len = sId.Length;
                for (int i = len-1; i > 0; i--)
                {
                    sId = sId.Insert(i, " ");
                }


                lblBinNumber.Text = sId;
                lblCategory.Text = type;
                image.Src = "barcode.ashx?data=" + id;
                lblDate.Text = date;

                lblBinNumber2.Text = sId;
                lblCategory2.Text = type;
                image2.Src = "barcode.ashx?data=" + id;
                lblDate2.Text = date;
            }
        }
        catch (Exception ex)
        {
            lblBinNumber.Text = "Error: invalid id";
            image.Visible = false;
        }
    }

}