﻿using System.Web;
using System.Web.Mvc;

namespace IVA.FindExpert
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
