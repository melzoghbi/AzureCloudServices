Azure Cloud Services Demo


This demo is a demonstration for using Cloud Services, Web & Worker roles, Azure Storage (Tables) and Azure Service Bus communications.
        
        
        

The demo structure:

1) Cloud Service: A service that contains web and worker role that communicate to each other using azure service bus.

2) Web Role: A web app to collect user's claims, the app send these claims into azure service bus.

3) Worker Role: Receive submitted messages by web roles into azure service bus.

4) Azure Service Bus: A communication channel for captured claims from web role. The messages are being submitted to the service bus and picked up by the worker role.

5) Azure Table Storage: This is used to store processed data from the worker role. The case manager worker role capture user's claim and store it in Azure tables.
