# 'Bob's Used Books' Sample Application

## Overview
_Bob's Used Books_ is a sample application built on ASP.NET Core 6.0 that aims to represent a simple, real-world application. It is a monolithic n-tier application with an ASP.NET Core MVC front end and a Microsoft SQL Server database backend.

The MVC application contains a customer portal and an administration portal. The customer portal enables customers to search for books, select and add them to a shopping cart, and work through a simulated check-out process. Customers can also offer their own books for resale through the website.

![Customer portal home page](./media/customer_app_home.png)

The administration portal is used by site administrators to maintain inventory, process customer orders, and perform other administrative tasks.

![Admin web app home page](./media/admin_app_home.png)

In its current state the application illustrates an in-progress "lift and shift" from on-premises to AWS, with partial modernization to use some AWS services. As the sample application progresses over time we have plans to add further modernization examples including (but not limited to) database modernization and refactoring to microservices.

<img src="./media/aws-architecture.png" width="884" height="715" title="AWS Architecture Diagram">


## Prerequisites

To run and debug the application locally you need the following:
* A Windows-based development environment
* An IDE that supports .NET 6.0: [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

To deploy the application to AWS you need the following:
* An AWS IAM User with an attached _AdministratorAccess_ policy
* The [AWS Cloud Development Kit (CDK)](https://docs.aws.amazon.com/cdk/v2/guide/getting_started.html)

## Getting started

Clone the repository or download the source code and open the solution in your preferred IDE. Set _Bookstore.Web_ as the startup project.

![Projects in Visual Studio's Solution Explorer](./media/vs_solution_projects.png)


Make sure the _Local_ profile is selected, then press **F5** to debug the application.

![Projects in Visual Studio's Solution Explorer](./media/local_profile.png)

When running under the _local_ profile the application will use a SQL Server Express LocalDB instance for database support with the connection string defined in _appSettings.Development.json_. User authentication is simulated. Clicking the _Log in_ option results in a transition to an authenticated state, without prompting for username or password, or user sign-up. This enables you to test the work processes in the admin portal, and customer features such as shopping cart, without needing to provision any cloud resources. Facades within the application abstract away potential use of AWS services such as Amazon S3 to the local file system.

Launch profiles, contained in the application's launchSettings.json file, are used to determine whether you are running the application fully local (no AWS resources) or local with AWS resources. The launch profile that represents a fully local run, without using any AWS services, is called the _Local_ profile. The second profile, in which the application can be run locally but also make use of some AWS services, is called the _Integrated_ profile. See the [Deployment](#deployment) section for details on launching the application with the _Integrated_ profile.


## Deployment

You can run and debug the web application from the repository codebase without needing to create any AWS resources or deploy the application. Optionally, you can also run the web application locally, in a state representing the in-progress lift and shift operation, where a small number of AWS services are used to provide application features such as:

* An object store holding cover images for the store's books, using a private [Amazon S3](https://aws.amazon.com/s3) bucket.
* An [Amazon CloudFront](https://aws.amazon.com/cloudfront) distribution, through which the objects (images) in the bucket are accessed, enabling the bucket to remain private.
* User authentication, for both bookstore staff and customers, using [Amazon Cognito](https://aws.amazon.com/cognito) user pools.
* Secrets management for the database credentials using [Amazon Secrets Manager](https://aws.amazon.com/secrets-manager).
* Application configuration parameters retrieved at runtime from [AWS Systems Manager](https://aws.amazon.com/systems-manager) Parameter Store.

![Launch profiles for the web app](./media/vs_launch_profiles.png)

To run the application using the _Integrated_ profile a minimal set of AWS resources need to be provisioned. This is achieved using Infrastructure as Code (IaC) with an [AWS Cloud Development Kit (CDK)](https://aws.amazon.com/cdk) application contained in the _Bookstore.Cdk_ project. The Bookstore application can be deployed to AWS via the CDK's command-line tooling.

### Running and debugging with the _Integrated_ launch profile

> **NOTE:** Ensure your development environment meets the [Prerequisites](#prerequisites) before attempting to deploy the application

1. Open a command-line shell and navigate to the application solution folder (the folder that contains _BobsBookstore.sln_).

1. [Bootstrap](https://docs.aws.amazon.com/cdk/v2/guide/bootstrapping.html) your AWS environment for the AWS CDK by running the command `cdk bootstrap`

1. In the command-line shell run the command `cdk deploy BobsBookstoreCore --require-approval "never"`. This will take about 5 minutes to complete.

    > **NOTE:** the sample CDK command above assumes that your AWS credential profile is named `default`. If you do not have a credential profile with that name, or wish to use an alternate credential profile, add the `--profile` parameter followed by the name of the credential profile. For example, `cdk deploy BobsBookstoreCore --profile my-profile`. Note that the selected profile should be configured to set the target region. See [Configuration and credential file settings](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html) for more details. An example of a credential profile containing region configuration is shown below.

    ```text
    [my-profile]
    aws_access_key_id=AKIAIOSFODNN7EXAMPLE
    aws_secret_access_key=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
    region=us-west-2
    ```

    ![Completed deployment](./media/shell_core_deployment.png)

1. Once the CDK deployment has completed open the _appsettings.Test.json_ file in the _Bookstore.Web_ project and set a credential profile and region. An example of the required settings is shown below.

    ![AppSettings update](./media/appsettings_integrated_debug.png)

    > **NOTE:** These settings are needed as the application will be calling AWS service APIs to access the resources you just provisioned using the CDK. The `Region` property should match the region you told the CDK to deploy to, or `us-west-2` if you accepted the default built into the CDK application. The `Profile` property value should point to credentials that belong to the same account used with the CDK deployment.

1. In your preferred IDE select the _Integrated_ launch profile and press **F5**.

    ![Select Integrated launch profile](./media/integrated_launch_profile.png)

You are now debugging the Bookstore application locally, however the application is interacting with the following AWS services:
* An object store holding cover images for the store's books, using a private [Amazon S3](https://aws.amazon.com/s3) bucket.
* An [Amazon CloudFront](https://aws.amazon.com/cloudfront) distribution, through which the objects (images) in the bucket are accessed, enabling the bucket to remain private.
* User authentication, for both bookstore staff and customers, using [Amazon Cognito](https://aws.amazon.com/cognito) user pools.
* Secrets management for the database credentials using [Amazon Secrets Manager](https://aws.amazon.com/secrets-manager).
* Application configuration parameters retrieved at runtime from [AWS Systems Manager](https://aws.amazon.com/systems-manager) Parameter Store.

> **NOTE:** The first time you launch the application with the _Integrated_ profile or via a full deployment (see below for a full deployment) you will be prompted to sign in and update the admin user password. See the [User Authentication](#user-authentication) section for details.

### Perform a full deployment

In addition to the AWS resources that are created and used by the [_Integrated_ launch](#running-and-debugging-with-the-integrated-launch-profile), a full deployment deploys the application to an [Amazon EC2](https://aws.amazon.com/ec2/) instance and provisions an [Amazon RDS for SQL Server](https://aws.amazon.com/rds/sqlserver/) database.

> **NOTE:** Ensure your development environment meets the [Prerequisites](#prerequisites) before attempting to deploy the application

1. Open a command-line shell and navigate to the application solution folder (the folder that contains _BobsBookstore.sln_).

1. If you haven't done so already, [Bootstrap](https://docs.aws.amazon.com/cdk/v2/guide/bootstrapping.html) your AWS environment for the AWS CDK by running the command `cdk bootstrap`

1. In the command-line shell run the command `dotnet publish app/Bookstore.Web/Bookstore.Web.csproj -p:PublishProfile=FolderProfile -c Release`
![Publish the application](./media/publish.png)

1. In the command-line shell run the command `cdk deploy BobsBookstoreEC2 --require-approval "never"`. This will take about 25 minutes to complete.

1. To run your deployed application, copy the _BobsBookstoreEC2.EC2Url output and paste it into a browser.
![Copy the application URL](./media/shell_full_deployment.png)

>**NOTE:** The application uses a self-signed certificate when deployed to EC2. The first time you navigate to the application your browser will warn you about a potential security risk. This is expected behavior and is due to the self-signed certificate not being recognized by your browser.

![Browser warning](./media/browser_warning.png)

> **NOTE:** The first time you launch the application with the _Integrated_ profile or via a full deployment (see below for a full deployment) you will be prompted to sign in and update the admin user password. See the [User Authentication](#user-authentication) section for details.

## User authentication

When the application is launched with the _Integrated_ profile or as a full deployment it uses Amazon Cognito user pools for user authentication and Amazon Cognito user groups for role-based authorization. The first time you login to the application you will be asked to update the admin password.

1. In the application, select _Log in_ from the application toolbar.

    ![Login](./media/login.png)

1. Sign in using **admin** for the username and **P@ssword1** for the password.

    ![Initial sign-in](./media/app_sign-in.png)

1. Because you signed in with a temporary password you are prompted to create a new password and to provide some additional details.

    ![Change password](./media/app_sign-in_password_update.png)

You are then signed into the application and placed at the customer portal home page. The admin user belongs to the _Administrators_ group  which means you can navigate between the admin portal and the customer portal by following the _Admin Portal_ and _Customer Portal_ links in the top-right corner of the page.

![Order view in Admin app](./media/app_post_sign-in.png)

> **NOTE:** The application supports self sign up. When users sign up to Bob's Used Books they are granted access to the customer portal, but not to the administrator portal. Administrators cannot be added via self sign up, they must be added directly in the application's Cognito user pool.

## Amazon Cloud Development Kit (CDK)

The application uses the [AWS Cloud Development Kit (CDK)](https://aws.amazon.com/cdk) to provision resources in AWS. The CDK project can create and configure either the minimal cloud resources needed to support the _Integrated_ launch profile of the web application, or a full set of resources that simulate a "production deployment" that includes a Virtual Private Cloud (VPC), a SQL Server database in Amazon RDS, application roles, and more.

The CDK project uses Infrastructure as Code (IaC) to define and configure the application's resources. It is a .NET console project, which you can examine by opening the _Bookstore.Cdk_ project. During deployment, the CDK creates [AWS CloudFormation](https://aws.amazon.com/cloudformation) Stacks containing the resources defined using C# in the _Bookstore.Cdk_ application. The stacks in the sample application are modular and take advantage of [cross-stack references](https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/walkthrough-crossstackref.html) to minimize code duplication and maximize reuse. The following stacks have been defined:

* BobsBookstoreCore - Deploys the minimal resources needed to support the _Integrated_ launch profile
* BobsBookstoreNetwork - Deploys a VPC that is used by the application in a "production" deployment
* BobsBookstoreDatabase - Deploys an RDS for SQL Server database that is used by the application in a "production" deployment. This stack depends on resources created in the _BobsBookstoreNetwork_ stack
* BobsBookstoreEC2 - Deploys the application to an EC2 Linux instance. This stack depends on resources created in the _BobsBookstoreCore_, _BobsBookstoreNetwork_, and _BobsBookstoreDatabase_ stacks

The _BobsBookstoreCore_ stack is used to deploy the minimal resources needed to support the _Integrated_ launch profile. Use the _BobsBookstoreEC2_ stack to deploy the resources required to simulate a full "production" deployment to EC2.

> **NOTE:** some of the resources created by the _BobsBookstoreCore_ and _BobsBookstoreEC2_ stacks will incur charges to your AWS account if you are not able to take advantage of the [AWS Free Tier](https://aws.amazon.com/free). These resources should be deleted when they are no longer required. See the [Deleting the resources](#deleting-the-resources) section for details.

## Deleting the resources

When you have completed working with the sample applications we recommend deleting the resources to avoid possible charges. To do this, either:

* Navigate to the CloudFormation dashboard in the AWS Management Console and delete all _BobsBookstore_ stacks (see list above).

* Or, in a command-line shell, navigate to the solution folder and run the command `cdk destroy BobsBookstore*`. 
> **NOTE:** If you supplied `--profile` parameter to the CDK when instantiating the stack, be sure to provide the same ones on deletion, otherwise the CDK command will error out complaining that the stack cannot be found.
