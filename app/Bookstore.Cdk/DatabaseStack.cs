using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.SSM;
using Bookstore.Common;
using Constructs;

namespace Bookstore.Cdk;

public class DatabaseStackProps : StackProps
{
    public Vpc Vpc { get; set; }
}

public class DatabaseStack : Stack
{
    private const int DatabasePort = 1433;

    public DatabaseInstance Database { get; set; }

    internal DatabaseStack(Construct scope, string id, DatabaseStackProps props) : base(scope, id, props)
    {
        var securityGroup = CreateSecurityGroup(props);

        CreateDatabase(props, securityGroup);

        CreateConnectionString();
    }

    private SecurityGroup CreateSecurityGroup(DatabaseStackProps props)
    {
        return new SecurityGroup(this, "DatabaseSecurityGroup", new SecurityGroupProps
        {
            Vpc = props.Vpc,
            Description = "Allow access to the SQL Server instance from the website",
        });
    }

    private void CreateDatabase(DatabaseStackProps props, SecurityGroup securityGroup)
    {
        Database = new DatabaseInstance(this, "RDSDatabase", new DatabaseInstanceProps
        {
            Vpc = props.Vpc,
            VpcSubnets = new SubnetSelection
            {
                SubnetType = SubnetType.PRIVATE_WITH_EGRESS
            },
            // SQL Server 2017 Express Edition, in conjunction with a db.t2.micro instance type,
            // fits inside the free tier for new accounts
            Engine = DatabaseInstanceEngine.SqlServerEx(new SqlServerExInstanceEngineProps
            {
                Version = SqlServerEngineVersion.VER_14
            }),
            StorageType = StorageType.GP3,
            AllocatedStorage = 20,
            Port = DatabasePort,
            SecurityGroups = new[]
                    {
                securityGroup
            },
            InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.MICRO),
            InstanceIdentifier = $"{Constants.AppName}Database",
            // As this is a sample app, turn off automated backups to avoid any storage costs
            // of automated backup snapshots. It also helps the stack launch a little faster by
            // avoiding an initial backup.
            BackupRetention = Duration.Seconds(0)
        });
    }

    private void CreateConnectionString()
    {
        var password = Database.Secret.SecretValueFromJson("password");
        var server = Database.Secret.SecretValueFromJson("host");
        var userId = Database.Secret.SecretValueFromJson("username");
        var database = Database.Secret.SecretValueFromJson("dbInstanceIdentifier");

        _ = new StringParameter(this, "RDSDatabaseConnectionStringSSMParameter", new StringParameterProps
        {
            ParameterName = $"/{Constants.AppName}/Database/ConnectionStrings/BookstoreDatabaseConnection",
            StringValue = $"Server={server};Database={database};TrustServerCertificate=True;User Id={userId};Password=\"{password}\";"
        });
    }
}