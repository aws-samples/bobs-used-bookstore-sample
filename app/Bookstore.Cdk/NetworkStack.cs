namespace Bookstore.Cdk;

using Amazon.CDK;
using Amazon.CDK.AWS.EC2;

using Constructs;

public class NetworkStack : Stack
{
    public Vpc Vpc { get; private set; }

    internal NetworkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // Create a new vpc spanning two AZs and with public and private subnets
        // to host the application resources
        this.Vpc = new Vpc(this, $"{Constants.AppName}Vpc", new VpcProps
        {
            IpAddresses = IpAddresses.Cidr("10.0.0.0/16"),
            // Cap at 2 AZs in case we are deployed to a region with only 2, RDS deployment requires VPC with minimum 2 AZ
            MaxAzs = 2, 
            // Cap at 1 NAT Gateway in order to optimize for costs
            NatGateways = 0,
            SubnetConfiguration = new ISubnetConfiguration[]
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