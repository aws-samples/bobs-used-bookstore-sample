# Steps to run the CDK app and Bob's Used Bookstore application
## Modules
1. Run the CDK application
1. Create Admin user for Admin application
1. Select the appropriate db connection string (local db or RDS) before deployment
1. Deploy the Customer and Admin application

## Module 1: Run the CDK application
1) Clone the repository
    ```
    git clone <repo_url>
    ```
1) To work with the AWS CDK, you must have an AWS account and credentials and have installed Node.js and the AWS CDK Toolkit. [See AWS CDK Prerequisites](https://docs.aws.amazon.com/cdk/latest/guide/work-with.html#work-with-prerequisites)

1) Deploy CDK Project

    > The CDK CLI requires you to be in the same directory as your cdk.json file.


1) Go to `\BobsUsedBookstore\infra\bookstoreCdkStack` and run the following command(This will take several minutes). If the profile is not mentioned, it will pick the default profile.
    ```
    cdk deploy -profile <your_aws_credentials_profile>
    ```


## Module 2: Create Admin user for Admin application
1) Go to Amazon Cognito in in AWS Management Console.
1) Select Manage User Pools and click on the User pool created for the admin application - *BobsBookstoreAdminUserPool*.
1) Go to Users and groups -> Create User
1) Enter the Username, password, email and select the option to Send an invitation to this new user via Email (Phone Number is not required)
1) Select Create User


## Module 3: Select the appropriate db connection string (local db or RDS) before deployment
1) Customer Application
    - Go to *PATH_TO_SLN\bobs-used-bookstore-sample\app\frontend\BookstoreFrontend\Startup.cs* and change the value of the variable *connectionString* to *localConnectionString* for local database and *rdsConnectionString* for RDS Database
2) Admin Application
     - Go to *PATH_TO_SLN\bobs-used-bookstore-sample\app\backend\BookstoreBackend\Startup.cs*

    ```C#
    var connectionString = localConnectionString // for local db connection
    ```
     ```C#
    var connectionString = rdsConnectionString // for rds db connection
    ```

## Module 4: Deploy the Customer and Admin application
1) Host the sample application on Elastic Beanstalk using the VPC, security group, subnet and the IAM Instance role created by the CDK application
2) Select *Create a new application environment*
3) Select default Environment and URL. Click *Next*
4) Check *Use non-default VPC*
5) In the *Relational Database Access* select the security group corresponding to the **bookstoredb** Database Instance. Click *Next*
6) Select the following 
    - VPC - Select the **BobsBookstoreCoreInfra/BobsBookstoreVpc...**
    - Security Group - Select the **default** security group for the selected VPC
    - Instances Subnet - Select the 2 public subnets available (Hover over the options to view the complete string and select the ones which are PublicSubnet)
7) Under *Deployed Application Permissions*, select **BobsBookstoreCoreInfra-AdminBobsBookstoreInstanceProfile-...** for the admin application deployment and **BobsBookstoreCoreInfra-CustomerBobsBookstoreInstanceProfile-...** for the customer application deployment
8) Keep the *Services Permission* field unchanged. Click *Next*
9) Select the *Reverse Proxy* as **none**. Click *Next*
10) Click *Deploy*

## Note: User Registration for Customer Application
> Create different sets of users in Customer application while connecting to local and RDS database. 




