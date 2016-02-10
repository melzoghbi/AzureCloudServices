using AutoMapper;
using Case_Manager_Web.BLL;
using Case_Manager_Web.Models;
using CaseManagerData;
using Microsoft.ServiceBus.Messaging;
using System.Web.Mvc;

namespace Case_Manager_Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var viewModel = new CustomerViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(CustomerViewModel customerForm)
        {
            //maps to domain object from view model
            var command = Mapper.Map(customerForm, typeof(CustomerViewModel), typeof(Customer));
            var message = new BrokeredMessage(command);
            // Send customer message to Service Bus Queue          
            ServiceBusQueueHelper.CustomersQueueClient.Send(message);
            ViewBag.Status = "Customer claim has been submitted to be processed!";
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}