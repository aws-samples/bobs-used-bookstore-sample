using BobsBookstore.Models.Books;
using System.Collections.Generic;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface IPriceSearch : IGenericRepository<Price>
    {
        public IEnumerable<Price> GetPricebySearch(string searchString);

    }
}
