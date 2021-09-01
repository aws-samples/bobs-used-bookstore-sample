# Steps to run the CDK app and Bob's Used Bookstore application
## Modules
1. Run the CDK application
1. Configure the security group
1. Run migrations
1. Create Admin user for Admin application
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
## Module 2: Configure the security group
1) Open the Amazon RDS Console from AWS Management Console
1) Go to Databases and select the database instance created by the CDK application.
1) Under Connectivity & security, go to Security and select the VPC Security group.
1) Edit the inbound rules and add your ip to allow the database instance port 1433

## Module 3: Run migrations
1) First, you'll have to install the [EF Core command-line tools](https://docs.microsoft.com/en-us/ef/core/cli/)
1) Go to AWS Secrets Manager in AWS Management Console and retrieve the secret values - host, username, password and port from the secret created by the CDK application.
1) Run the migrations to create the tables.
Go to `BobsUsedBookstore\app\BobsBookstore.Migrations` and run 
    ```
    dotnet ef database update --connection "Server=<host>,<port>; Initial Catalog=BobsBookStore; User Id=<username>; Password=<password>"
    ```
## Module 4: Create Admin user for Admin application
1) Go to Amazon Cognito in in AWS Management Console.
1) Select Manage User Pools and click on the User pool created for the admin application.
1) Go to Users and groups -> Create User
1) Enter the Username, password, email and select the option to Send an invitation to this new user via Email
1) Select Create User

## Module 5: Deploy the Customer and Admin application
1) Host the sample application on Elastic Beanstalk using the VPC, security group, subnet and the IAM Instance role created by the CDK application




