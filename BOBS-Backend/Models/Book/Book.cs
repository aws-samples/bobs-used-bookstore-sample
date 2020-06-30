using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Models.Book
{
    public class Book
    {

        /*
         * Book Model  
         * Object Reference indicate relationship with other tables
         */

        [Key]
        public long Book_Id { get; set; }

        // Many to One Relationship with Publisher Table
        public Publisher Publisher { get; set; }

        public long ISBN { get; set; }
        
        // Many to One Relationship with Type Table 
        public Type Type { get; set; }

        public string Name { get; set; }

        // Many to One Relationship with Genre Table
        public Genre Genre { get; set; }

        public string Front_Url { get; set; }
        
        public string Back_Url { get; set; }

        public string Left_Url { get; set; }
        public string Right_Url { get; set; }

        public string AudioBook_Url { get; set; }

        public string Summary { get; set; } 



    }

}
