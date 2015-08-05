<%@ WebHandler Language="C#" Class="USDAService" %>

using System;
using System.Web;
using System.Linq;

public class USDAService : IHttpHandler 
{
    
    public void ProcessRequest (HttpContext context) 
    {
        try
        {
            context.Response.ContentType = "text/javascript";
            using (CCSEntities db = new CCSEntities())
            {
                var fetchFood = (from c in db.USDACategories.OrderBy(x => x.USDAID)
                                 select c).ToList();
                String USDANames = "[";
                foreach (USDACategory c in fetchFood)
                {
                    USDANames += "'" + c.Description + "', ";
                }
                USDANames = USDANames.Remove(USDANames.Length - 2); //removes the last comma
                USDANames += "]";
                context.Response.Write("USDANames = " + USDANames);
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            LogError.logError(ex);
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