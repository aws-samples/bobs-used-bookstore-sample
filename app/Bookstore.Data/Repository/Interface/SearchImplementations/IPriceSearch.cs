using System.Collections.Generic;
using Bookstore.Domain.Books;

namespace Bookstore.Data.Repository.Interface.SearchImplementations
{
    public interface IPriceSearch : IGenericRepository<Price>
    {
        public IEnumerable<Price> GetPricebySearch(string searchString);
    }
}