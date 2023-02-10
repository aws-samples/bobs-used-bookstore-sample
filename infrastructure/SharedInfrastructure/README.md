# Deployment Instructions

The project contained in this folder hierarchy uses the [AWS Cloud Development Kit (CDK)](https://aws.amazon.com/cdk) to create and configure either the minimal cloud resources needed to support the _Integrated_ launch profile of the web application, or full "production deployment" resources, including a Virtual Private Cloud (VPC), SQL Server database in Amazon RDS, application roles, and more.

The CDK project uses Infrastructure as Code (IaC) to define and configure the resources. It's a .NET console project, which you can examine by opening the _src/SharedInfrastructure_ project. During deployment, the CDK creates [AWS CloudFormation](https://aws.amazon.com/cloudformation) Stacks containing the resources defined using C# in the CDK application. The stacks in the sample application are modular and take advantage of [cross-stack references](https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/walkthrough-crossstackref.html) to minimize code duplication and maximize reuse. The following stacks have been created:

* BobsBookstoreCore - Deploys the minimal resources needed to support the _Integrated_ launch profile
* BobsBookstoreNetwork - Deploys a VPC that is used by the application in a "production" deployment
* BobsBookstoreDatabase - Deploys an RDS for SQL Server database that is used by the application in a "production" deployment. Depends on _BobsBookstoreNetwork_
* BobsBookstoreEC2 - Deploys the application to an EC2 Linux instance. Depends on _BobsBookstoreCore_, _BobsBookstoreNetwork_, and _BobsBookstoreDatabase_
* BobsBookstoreAppRunner - Deploys the application to AWS AppRunner. Depends on _BobsBookstoreCore_, _BobsBookstoreNetwork_, and _BobsBookstoreDatabase_

When deploying the minimal resources needed to support the _Integrated_ launch profile the stack is named _BobsBookstoreCore_. Alternatively you can perform a full "production" deployment to EC2 or AppRunner by deploying either the _BobsBookstoreEC2_ or _BobsBookstoreAppRunner_ stacks.

**Note:** some of the resources created by the "production" stacks (_BobsBookstoreEC2_ and _BobsBookstoreAppRunner_) will incur charges to your AWS account if you are not able to take advantage of the [AWS Free Tier](https://aws.amazon.com/free).

## Deploying the resources

1. In a command-line shell, set your current folder to be the _infrastructure/SharedInfrastructure_. This folder is the root of the CDK application. The .NET project and code to create and configure the AWS resources for the respective stacks can be found in the _src/SharedInfrastructure/SharedInfrastructure_ subfolder. You will see that it is a .NET console application.

1. In the command-line shell run the command `cdk deploy <stack-name>`, replacing `<stack-name>` with either _BobsBookstoreCore_ (to deploy the minimal cloud resources to support running the _Integrated_ launch profile on your local machine), _BobsBookstoreEC2_ (to deploy the full set of AWS resources and host the application on EC2), or _BobsBookstoreAppRunner_ (to deploy the full set of AWS resources and host the application on AWS AppRunner).

    **Note:** the sample CDK command above assumes that your AWS credential profile is named `default`. If you do not have a credential profile with that name, or wish to use an alternate credential profile, add the `--profile` parameter followed by the name of the credential profile. For example, `cdk deploy BobsBookstoreCore --profile my-credentials`. Note that the selected profile should be configured to set the target region. See [Configuration and credential file settings](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html) for more details. An example of a credential profile containing region configuration is shown below.

    ```text
    [my-credentials]
    aws_access_key_id=AKIAIOSFODNN7EXAMPLE
    aws_secret_access_key=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
    region=us-west-2
    ```

1. Respond 'y' to prompts to create resources. Once the process starts it will take a few moments to complete.

![Confirming resource creation at deployment](/media/shell_deployment1.png)

![Completed deployment](/media/shell_deployment2.png)

 Once complete, if you deployed the _BobsBookstoreCore_ stack, you can then run and debug the sample web application using the application's _Integrated_ launch profile, as described in the [Running and debugging with the _Integrated_ launch profile](./README.md#running-and-debugging-with-the-integrated-launch-profiles) section of the main README file for the repository. If you deployed the _BobsBookstoreEC2_ stack or the _BobsBookstoreAppRunner_ stack you are now ready to run the application on the respective AWS service.

## Deleting the resources

When you have completed working with the sample applications we recommend deleting the resources to avoid possible charges. To do this, either:

* Navigate to the CloudFormation dashboard in the AWS Management Console and delete all _BobsBookstore_ stacks (see list above).

* Or, in a command-line shell, set the working folder to be the _infrastructure/SharedInfrastructure_ folder of the repository and run the command `cdk destroy <stack-names>`, replacing `<stack-names>` with each stack defined in the list above. If you supplied `--profile` parameter to the CDK when instantiating the stack, be sure to provide the same ones on deletion, otherwise the CDK command will error out complaining that the stack cannot be found.
