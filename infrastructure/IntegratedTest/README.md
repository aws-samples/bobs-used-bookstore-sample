# Deployment Instructions

The project contained in this folder hierarchy uses the [AWS Cloud Development Kit (CDK)](https://aws.amazon.com/cdk) to create and configure the minimal cloud resources needed to support the respective _Integrated_ launch profiles of the admin and customer web applications. The CDK project should be deployed before running these launch profiles.

The CDK project uses Infrastructure as Code (IaC) to define and configure the resources. It's a .NET console project, which you can examine by opening the _src/IntegratedTestInfrastructure_ project. On deployment, the CDK creates an [AWS CloudFormation](https://aws.amazon.com/cloudformation) Stack containing the resources defined using C# in the CDK application. This stack is named _BobsUsedBooksIntegratedTest_.

## Deploying the resources

1. In a command-line shell, set your current folder to be the _infrastructure/IntegratedTest_. This folder is the root of the CDK application. The code to create and configure the AWS resources can be found in the _src/IntegratedTestInfrastructure_ subfolder. You will see that it is a .NET-based CDK application.

1. In the command-line shell run the command `cdk deploy --region <region-code>`, replacing `<region-code>` with the system code of an AWS region, for example `us-east-1`, `us-west-2`, etc. If you omit the `--region` parameter in its entirety, the CDK stack will deploy by default to the US West (Oregon), or us-west-2, region.

    **Note:** the sample CDK command above assumes that your AWS credential profile is named `default`. If you do not have a credential profile with that name, or wish to use an alternate credential profile, add the `--profile` parameter followed by the name of the credential profile. For example, `cdk deploy --profile steve-demo --region us-east-1`.

1. Respond 'y' to prompts to create resources. Once the process starts it will take a few moments to complete. Once complete, you can then run and debug the sample web applications using each application's _Integrated_ launch profile, as described in the [Running and debugging with the _Integrated_ launch profiles](./README.md#running-and-debugging-with-the-integrated-launch-profiles) section of the main README file for the repository.

![Confirming resource creation at deployment](/media/shell_deployment1.png)

![Completed deployment](/media/shell_deployment2.png)

On completion you will have a CloudFormation stack named _BobsUsedBooksIntegratedTest_ containing the minimal resources needed to support running and debugging the applications using their respective _Integrated_ launch profiles.

## Deleting the resources

When you have completed working with the sample applications we recommend deleting the resources to avoid possible charges. To do this, either:

1. Navigate to the CloudFormation dashboard in the AWS Management Console, select the _BobsUsedBooksIntegratedTest_ stack, and delete it.

1. Or, in a command-line shell, set the working folder to be the _infrastructure/IntegratedTest_ folder of the repository and run the command `cdk destroy`. If you supplied `--profile` and/or `--region` parameters to the CDK when instantiating the stack, be sure to provide the same ones on deletion, otherwise the CDK command will error out complaining that the stack cannot be found.
