using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Case_Manager_Web.BLL
{
    public static class ServiceBusQueueHelper
    {
    //// Recommended that you cache QueueClient       
    //// rather than recreating it on every request.
    public static  QueueClient CustomersQueueClient;
    // The name of the queue
    public const string QueueName = "CustomerQueue";
     
    public static NamespaceManager CreateNamespaceManager()
    {
        string connectionString =
        CloudConfigurationManager.GetSetting(
        "ServiceBusConnectionString");
        var namespaceManager = NamespaceManager.
          CreateFromConnectionString(connectionString);
        return namespaceManager;
    }
    public static void Initialize()
    {
        // Using Http to be friendly with outbound firewalls
        ServiceBusEnvironment.SystemConnectivity.Mode =
            ConnectivityMode.Http;
        string connectionString =  CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
        NamespaceManager namespaceManager = CreateNamespaceManager();
        if (!(namespaceManager.QueueExists(QueueName)))
        {
            namespaceManager.CreateQueue(QueueName);
        }
     
        // Initialize the connection to Service Bus Queue
        CustomersQueueClient = QueueClient.CreateFromConnectionString(connectionString, QueueName);
    }
    }
}
