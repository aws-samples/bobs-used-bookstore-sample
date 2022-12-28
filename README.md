# 'Bob's Used Books' Sample Application

_Bob's Used Books_ is a work-in-progress sample .NET application representing a real-world bookstore. It contains a couple of ASP.NET Core MVC web applications.

The first application is the bookstore's 'admin' website which our fictional bookstore staff use to maintain the stocks of books available in the store, process customer orders, and other bookstore-related tasks. The second application is a customer-facing website for the store. In this application, customers can browse the store's stock of used books being offered for sale, select and add them to their cart, and work through a fictional check-out process. Store customers can also offer their own books for resale through the website. Both applications make use of a single Microsoft SQL Server database.

In its current state the sample web applications illustrate an in-progress "lift and shift" from on-premises applications to the AWS cloud, with partial modernization to use some AWS services. As the sample application progresses over time we have plans to add further modernization examples including (but not limited to) changes such as database modernization and refactoring to microservices.

## Running the sample applications

Both web applications, and supporting shared projects, can be found in the _app_ subfolder of the repository. _Bookstore.Admin_ is the ASP.NET Core MVC application representing the stores administrative application, and _Bookstore.Customer_ is the ASP.NET Core MVC application representing the customer-facing web site.

You can run and debug both web applications, at the same time, from the repository codebase without needing to create any AWS resources or deploy the applications.

Optionally, you can also run both applications locally, in a state representing the in-progress lift and shift operation, where a small number of AWS services are used to provide application features such as:

* an object store to hold cover images for books, using an [Amazon S3](https://aws.amazon.com/s3) bucket
* user authentication, for both bookstore staff and customers, using [Amazon Cognito](https://aws.amazon.com/cognito) user pools
* secrets management for the database credentials using [Amazon Secrets Manager](https://aws.amazon.com/secrets-manager)
* application configuration parameters retrieved at runtime from [AWS Systems Manager](https://aws.amazon.com/systems-manager) Parameter Store.

These two states are differentiated using launch profiles, contained in the respective application's launchSettings.json files. The launch profile that represents a fully local run, without using any AWS services, is called the _Local_ profile. The second profile, in which the applications can be run locally but also make use of some AWS services, is called the _integrated_ profile.

To run the applications using the _Integrated_ profile a set of AWS resources need to be provisioned. This is achieved using Infrastructure as Code (IaC) with an [AWS Cloud Development Kit (CDK)](https://aws.amazon.com/cdk) application contained in the _infrastructure_ folder of the repository. The project in the _src_ subfolder can be deployed using the CDK's command-line tooling to instantiate the required resources.

**Note 1:** the CDK application does not create and configure all the resources that would be needed to support the sample web applications when fully deployed to the cloud. We have plans to add further CDK projects to achieve this in future. For now, the applications should be run locally on developer workstations, using Microsoft Windows, making use of minimal cloud resources.

**Note 2:** the sample applications make use of **localdb** for database connectivity. Therefore, Microsoft Windows is currently required to run these samples.

### Running and debugging with the _Local_ launch profiles

Having cloned the repository, the admin and customer web site applications can be run, in parallel, on a Microsoft Windows workstation without further configuration or deployment.

1. To run the admin web application fully local, using no cloud resources, start the **Bookstore.Admin** project using the _AdminSite Local_ profile in your IDE or at the command line. Ports 5000 and 5001 are used for the admin site on localhost.

1. To run the customer web application fully local, using no cloud resources, start the **Bookstore.Customer** project using the _CustomerSite Local_ profile in your IDE or at the command line. Ports 4000 and 4001 are used for the customer site on localhost.

When running under the _local_ profiles, both applications will use a **localdb** instance for database support, with the connection string defined in their respective appSettings.Development.json files. User authentication for both applications is simulated. Clicking the respective _Log in_ options in both applications results in a transition to an authenticated state, without prompting for username or password, or user sign-up. This enables you to test the work processes in the admin application, and customer features such as shopping cart, without needing cloud resources. Facades within the applications abstract away potential use of AWS services such as Amazon S3 to the local file system.
### Running and debugging with the _Integrated_ launch profiles

Prior to using the _Integrated_ launch profiles for the web applications you need to perform a deployment of the single CDK infrastructure stack, to create and configure a minimum set of AWS resources that each application will use. To deploy the minimal cloud infrastructure needed to support running the applications using the _Integrated_ profiles, see the [README](./infrastructure/LocalTest/README.md) file.

Once the CDK deployment has completed:

1. Run the admin application by starting the **Bookstore.Admin** project using the _AdminSite Integrated_ profile in your IDE or at the command line. Ports 5000 and 5001 are used for the admin site on localhost.

1. Run the customer application by starting the **Bookstore.Customer** project using the _CustomerSite Integrated_ profile in your IDE or at the command line. Ports 4000 and 4001 are used for the customer site on localhost.

### User authentication

In integrated mode, both applications use Amazon Cognito user pools for user authentication.

For the admin site you will need to create a user, representing bookstore staff, in the user pool for the admin site, as follows.

1. Sign into the AWS Management Console and proceed to the Amazon Cognito dashboard.

1. Select **User Pools** in the dashboard and in the resulting view, select the (admin) pool.

1. Choose _Users and groups_ from the left-panel navigation.

1. Create a user...

1. Wait for the confirmation email to arrive...

1. In the admin application, select _Log in_ from the application toolbar and sign in using the user details you just created.

You can now access features in the admin application as if you were a staff member of the bookstore, for example to process customer orders, maintain stock, and more.

In the customer application, end-users register themselves so as to be more similar to a real-world shopping experience. The process starts using the _Log in_ option on the customer application's toolbar.

### Tearing down infrastructure

Although minimal resources are created to support the _Integrated_ launch profiles of the web applications, it may still incur some cost. To avoid charges for unused resources, we recommend you delete the stack containing the infrastructure when you have finished exploring the sample.

To delete resources, either:

1. Navigate to the CloudFormation dashboard in the AWS Management Console, select the stack, and delete it.

1. Or, in a command-line shell, set the working folder to be the _infrastructure/IntegratedTest_ folder of the repository and run the command `cdk destroy`. If you supplied `--profile` and/or `--region` parameters to the CDK when instantiating the stack, be sure to provide the same ones on deletion, otherwise the CDK command will error out complaining that the stack cannot be found.
