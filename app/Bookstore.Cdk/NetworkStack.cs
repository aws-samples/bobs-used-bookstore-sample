using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Constructs;

namespace Bookstore.Cdk;

public class NetworkStack : Stack
{
    public Vpc Vpc { get; private set; }

    internal NetworkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // Create a new vpc spanning two AZs and with public and private subnets
        // to host the application resources
        Vpc = new Vpc(this, "Vpc", new VpcProps
        {
            IpAddresses = IpAddresses.Cidr("10.0.0.0/16"),
            // Cap at 2 AZs in case we are deployed to a region with only 2, RDS deployment requires VPC with minimum 2 AZ
            MaxAzs = 2, 
            // Cap at 1 NAT Gateway in order to optimize for costs
            NatGateways = 1,
            SubnetConfiguration = new[]
            {
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PUBLIC,
                    Name = "Public"
                },
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS,
                    Name = "Private"
                }
            }
        });
    }
}