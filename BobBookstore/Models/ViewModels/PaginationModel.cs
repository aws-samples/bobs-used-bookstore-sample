using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.ViewModels
{
    public class PaginationModel
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 1;
        public string CurrentFilter { get; set; }
        public int Count { get; set; }
        public IEnumerable<BookViewModel> Data { get; set; }
    }
}
