using Case_Manager_Web.BLL;
using Case_Manager_Web.Models;
using CaseManagerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Case_Manager_Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Create Azure Table
            AzureStorageHelper.InitializeAzureTable("Customer");
            //Initialize Service Bus Queue
            ServiceBusQueueHelper.Initialize();
            // Configure the auto mapper
            AutoMapper.Mapper.CreateMap<CustomerViewModel, Customer>();
        }
    }
}
