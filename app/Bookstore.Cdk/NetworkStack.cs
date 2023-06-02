using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.EC2;

namespace SharedInfrastructure.Production;

public class NetworkStack : Stack
{
    public Vpc Vpc { get; private set; }

    internal NetworkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // Create a new vpc spanning two AZs and with public and private subnets
        // to host the application resources
        Vpc = new Vpc(this, $"{Constants.AppName}Vpc", new VpcProps
        {
            IpAddresses = IpAddresses.Cidr("10.0.0.0/16"),
            // Cap at 2 AZs in case we are deployed to a region with only 2
            MaxAzs = 2, 
            SubnetConfiguration = new[]
            {
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PUBLIC,
                    Name = $"{Constants.AppName}PublicSubnet"
                },
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS,
                    Name = $"{Constants.AppName}PrivateSubnet"
                }
            }
        });
    }
}