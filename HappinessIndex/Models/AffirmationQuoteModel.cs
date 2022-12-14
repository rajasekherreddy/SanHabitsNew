using System;
using System.Collections.Generic;
using System.Text;

namespace HappinessIndex.Models
{
    public class AffirmationQuoteModel
    {
        public String Statement { get; set; }
        public String Author { get; set; }

        public AffirmationQuoteModel(String statement, String author)
        {
            this.Statement = statement;
            this.Author = author;
        }
    }
}
