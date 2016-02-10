Azure Cloud Services Demo


This demo is a demonstration for using Cloud Services, Web & Worker roles, Azure Storage (Tables) and Azure Service Bus communications.
        
        
        

The demo structure:

1) Cloud Service: A service that contains web and worker roles that communicate to each other using azure service bus and store processed messages into Azure table storage.

2) Web Role: A web app to collect user's claims, the app submits these claims into azure service bus.

3) Worker Role: Receive submitted messages by web roles from azure service bus. The worker role capture the message content and store the details into Azure table sorage.

4) Azure Service Bus: A communication channel for captured claims from a web role. The messages are being submitted to the service bus and picked up by a worker role.

5) Azure Table Storage: This is used to store processed messages from a service bus. The case manager worker role capture users' claims and store it in Azure tables.
