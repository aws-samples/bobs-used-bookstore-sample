# Deployment Instructions

The project contained in this folder hierarchy uses the [AWS Cloud Development Kit (CDK)](https://aws.amazon.com/cdk) to create and configure either the minimal cloud resources needed to support the _Integrated_ launch profiles of the admin and customer web applications, or full "production deployment" resources, including a Virtual Private Cloud (VPC), SQL Server database in Amazon RDS, application roles, and more.

The CDK project uses Infrastructure as Code (IaC) to define and configure the resources. It's a .NET console project, which you can examine by opening the _src/SharedInfrastructure_ project. During deployment, the CDK creates [AWS CloudFormation](https://aws.amazon.com/cloudformation) Stacks containing the resources defined using C# in the CDK application. When deploying the minimal resources needed to support the _Integrated_ launch profiles the stack is named _BookstoreIntegratedTest_. If you deploy the full resource stack equating to a "production" deployment the stack is named _BookstoreProduction_.

Before using the _Integrated_ launch profiles, you need to deploy the _BookstoreIntegratedTest_ stack contained in the CDK application. Alternatively, before deploying the admin and/or customer apps to compute services in AWS, you need to deploy the _BookstoreProduction_ stack.

**Note:** some of the resources created by the Production stack will incur charges to your AWS account if you are not able to take advantage of the [AWS Free Tier](https://aws.amazon.com/free).

## Deploying the resources

1. In a command-line shell, set your current folder to be the _infrastructure/SharedInfrastructure_. This folder is the root of the CDK application. The .NET project and code to create and configure the AWS resources for the respective stacks can be found in the _src/SharedInfrastructure/SharedInfrastructure_ subfolder. You will see that it is a .NET console application.

1. In the command-line shell run the command `cdk deploy <stack-name>`, replacing `<stack-name>` with either _BookstoreIntegratedTest_ (to deploy the minimal cloud resources to support running the _Integrated_ launch profiles on your local machine) or _BookstoreProduction_ (to deploy the full resources needed when you intend to host the admin and customer web applications in the cloud).

    **Note:** the sample CDK command above assumes that your AWS credential profile is named `default`. If you do not have a credential profile with that name, or wish to use an alternate credential profile, add the `--profile` parameter followed by the name of the credential profile. For example, `cdk deploy BookstoreIntegratedTest --profile my-credentials`. Note that the selected profile should be configured to set the target region. See [Configuration and credential file settings](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html) for more details. An example of a credential profile containing region configuration is shown below.

    ```text
    [my-credentials]
    aws_access_key_id=AKIAIOSFODNN7EXAMPLE
    aws_secret_access_key=wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY
    region=us-west-2
    ```

1. Respond 'y' to prompts to create resources. Once the process starts it will take a few moments to complete.

![Confirming resource creation at deployment](/media/shell_deployment1.png)

![Completed deployment](/media/shell_deployment2.png)

 Once complete, if you deployed the _BookstoreIntegratedTest_ stack, you can then run and debug the sample web applications using each application's_Integrated_launch profile, as described in the [Running and debugging with the _Integrated_ launch profiles](./README.md#running-and-debugging-with-the-integrated-launch-profiles) section of the main README file for the repository. If you deployed the _BookstoreProduction_ stack you are now ready to deploy the admin and customer web applications to compute service on AWS.

## Deleting the resources

When you have completed working with the sample applications we recommend deleting the resources to avoid possible charges. To do this, either:

* Navigate to the CloudFormation dashboard in the AWS Management Console, select the stack (_BookstoreIntegratedTest_ or _BookstoreProduction_) and delete it.

* Or, in a command-line shell, set the working folder to be the _infrastructure/SharedInfrastructure_ folder of the repository and run the command `cdk destroy <stack-name>`, replacing `<stack-name>` with either _BookstoreIntegratedTest_ or _BookstoreProduction_. If you supplied `--profile` parameter to the CDK when instantiating the stack, be sure to provide the same ones on deletion, otherwise the CDK command will error out complaining that the stack cannot be found.
