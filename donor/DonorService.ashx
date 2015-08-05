<%@ WebHandler Language="C#" Class="DonorService" %>

using System;
using System.Web;
using System.Linq;

public class DonorService : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/javascript";
        using (CCSEntities db = new CCSEntities())
        {
            var fetchDonor = (from d in db.FoodSources.OrderBy(x => x.Source)
                              select d).ToList();

            /*
             * var projects = [
                  {
                    label: "Michael Jasper",
                    address: "789 Reading Rainbow"
                  },
             */

            String array = "[";
            foreach (FoodSource d in fetchDonor)
            {
                array += "{";
                //donor name
                if (d.Source.Contains("'"))
                {
                    array += "label: '" + d.Source.Replace("'", "\\'") + "', ";
                }
                else
                {
                    array += "label: '" + d.Source + "', ";
                }

                //donor address
                try
                {
                    {
                        if (d.Address.StreetAddress1.Contains("'"))
                        {
                            array += "address: '" + d.Address.StreetAddress1.Replace("'", "\\'") + "', ";
                        }
                        else
                        {
                            array += "address: '" + d.Address.StreetAddress1 + "', ";
                        }
                    }
                }
                catch (NullReferenceException e) { }
                
                //donor address
                if (d.FoodSourceType.FoodSourceType1.Contains("'"))
                {
                    array += "type: '" + d.FoodSourceType.FoodSourceType1.Replace("'", "\\'") + "'";
                }
                else
                {
                    array += "type: '" + d.FoodSourceType.FoodSourceType1 + "' ";
                }
                
                
                array += "},";
            }
            array = array.Remove(array.Length - 1); //removes the last comma
            array += "];";
            context.Response.Write("donorNames = " + array);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}