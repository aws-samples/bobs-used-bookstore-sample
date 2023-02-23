using Amazon.CostExplorer;
using System.Threading.Tasks;

namespace Bookstore.Data
{
    public class CostExplorerService
    {
        private readonly IAmazonCostExplorer costExplorerClient;

        public CostExplorerService(IAmazonCostExplorer costExplorerClient)
        {
            this.costExplorerClient = costExplorerClient;
        }

        public async Task GetBookstoreUsageAsync()
        {

        }
    }
}
