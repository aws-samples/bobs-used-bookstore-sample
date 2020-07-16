using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobBookstore.Models.ViewModels
{
    public class BookViewModel
    {
        public long BookId { get; set; }
        public string BookName { get; set; }
        public long ISBN { get; set; }
        public string GenreName { get; set; }
        public string TypeName { get; set; }
        public List<Book.Price> Prices { get; set; }
        public string Url { get; set; }


        public static void sortBy(List<BookViewModel> lst, string prop)
        {
            switch (prop)
            {
                case "Name":
                    lst.Sort((elt1, elt2) => elt1.BookName.CompareTo(elt2.BookName));
                    break;
                case "Genre":
                    lst.Sort((elt1, elt2) => elt1.GenreName.CompareTo(elt2.GenreName));
                    break;
                case "Type":
                    lst.Sort((elt1, elt2) => elt1.TypeName.CompareTo(elt2.TypeName));
                    break;
                case "Price":
                    lst.Sort((elt1, elt2) => elt1.Prices.Min().ItemPrice.CompareTo(elt2.Prices.Min().ItemPrice));
                    break;
                default:
                    break;
            }
        }
    }
}
