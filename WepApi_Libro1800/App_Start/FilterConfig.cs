using System.Web;
using System.Web.Mvc;

namespace WepApi_Libro1800
{
    /// <summary>
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// </summary>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
