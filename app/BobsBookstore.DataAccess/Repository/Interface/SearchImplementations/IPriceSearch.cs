using System.Collections.Generic;
using BobsBookstore.Models.Books;

namespace BobsBookstore.DataAccess.Repository.Interface.SearchImplementations
{
    public interface IPriceSearch : IGenericRepository<Price>
    {
        public IEnumerable<Price> GetPricebySearch(string searchString);
    }
}