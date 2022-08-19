using System.Collections.Generic;
using DataModels.Books;

namespace DataAccess.Repository.Interface.SearchImplementations
{
    public interface IPriceSearch : IGenericRepository<Price>
    {
        public IEnumerable<Price> GetPricebySearch(string searchString);
    }
}