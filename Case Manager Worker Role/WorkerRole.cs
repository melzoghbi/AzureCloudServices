using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus.Messaging;
using CaseManagerData;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.Azure;
using Microsoft.ServiceBus;

namespace Case_Manager_Worker_Role
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

         // The name of your queue
        const string QueueName = "CustomerQueue";
        public static QueueClient Client;
        bool IsStopped;

        public override void Run()
        {
           Trace.TraceInformation("Case Manager Worker Role is running");

            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
            while (!IsStopped)
            {
                try
                {                
                    // Receive the message
                    BrokeredMessage receivedMessage = null;
                    receivedMessage = Client.Receive();

                    if (receivedMessage != null)
                    {
                        // Process the message
                        Trace.WriteLine("Processing",receivedMessage.SequenceNumber.ToString());
                        Customer command = receivedMessage.GetBody<Customer>();
                        // Create the table client.
                        CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                        // Create the CloudTable object for "Customer" table.
                        CloudTable table = tableClient.GetTableReference("Customer");

                        // Create a new customer entity.
                        CustomerTable customer = new CustomerTable
                        {
                            Id = Guid.NewGuid().ToString(),
                            PartitionKey = command.LastName,
                            RowKey = command.FirstName + command.LastName +"_"+DateTime.UtcNow.Second,
                            FirstName = command.FirstName,
                            LastName = command.LastName,
                            Address = command.Address,
                            Email = command.Email,
                            Phone = command.Phone,
                            Claim = command.Claim,
                        };

                        // Create the TableOperation that inserts the customer entity.
                        TableOperation insertOperation = TableOperation.Insert(customer);

                        // Execute the insert operation.
                        table.Execute(insertOperation);
                        receivedMessage.Complete();
                    }
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }

                    Thread.Sleep(10000);
                }
                catch (OperationCanceledException e)
                {
                    if (!IsStopped)
                    {
                        Trace.WriteLine(e.Message);
                        throw;
                    }
                }
                finally
                {
                    this.runCompleteEvent.Set();
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("Case Manager Worker Role has been started");

            // Create the queue if it does not exist already
            string connectionString = CloudConfigurationManager.GetSetting("ServiceBusConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Initialize the connection to Service Bus Queue
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            IsStopped = false;
            return base.OnStart();

            //return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("Case Manager Worker Role is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            // Close the connection to Service Bus Queue
            IsStopped = true;
            Client.Close();

            base.OnStop();

            Trace.TraceInformation("Case Manager Worker Role has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
