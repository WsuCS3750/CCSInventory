<%@ WebHandler Language="C#" Class="USDAItemService" %>

using System;
using System.Web;
using System.Linq;

public class USDAItemService : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/javascript";
        using (CCSEntities db = new CCSEntities())
        {
            var fetchUSDAItem = (from d in db.USDACategories.OrderBy(x => x.USDANumber)
                                 select d).ToList();
          
            String array = "[";
            foreach (USDACategory d in fetchUSDAItem)
            {
                array += "{";
                //USDA Number
                if (d.USDANumber.Contains("'"))
                {
                    array += "label: '" + d.USDANumber.Replace("'", "\\'") + "', ";
                }
                else
                {
                    array += "label: '" + d.USDANumber + "', ";
                }

                //USDA Category Desc
                if (d.Description.Contains("'"))
                {
                    array += "description: '" + d.Description.Replace("'", "\\'") + "', ";
                }
                else
                {
                    array += "description: '" + d.Description + "', ";
                }

                array += "},";
            }
            array = array.Remove(array.Length - 1); //removes the last comma
            array += "];";
            context.Response.Write("usdaItmNumbers = " + array);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}