using HappinessIndex.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HappinessIndex.ViewModels
{
    public class AffirmationQuoteViewModel
    {
        public List<AffirmationQuoteModel> Quotes { get; set; }

        public List<String> ImageTemplates { get; set; }

        public String SelectedQuote { get; set; }

        public String SelectedQuoteColor { get; set; }

        public String SelectedQuoteAuthor { get; set; }

        public String SelectedQuoteAuthorColor { get; set; }

        public String SelectedImageTemplate { get; set; }

        public AffirmationQuoteViewModel()
        {
            var rnd = new Random();

            Quotes = new List<AffirmationQuoteModel>
            {
                new AffirmationQuoteModel("All happiness or unhappiness solely depends upon the quality of the object to which we are attached by love.", " Baruch Spinoza"),
                new AffirmationQuoteModel("The true secret of happiness lies in the taking a genuine interest in all the details of daily life.", "William Morris"),
                new AffirmationQuoteModel("The essence of philosophy is that a man should so live that his happiness shall depend as little as possible on external things.","Epictetus"),
                new AffirmationQuoteModel("How simple it is to see that we can only be happy now, and there will never be a time when it is not now.","Gerald Jampolsky"),
                new AffirmationQuoteModel("Happiness and sadness run parallel to each other. When one takes a rest, the other one tends to take up the slack.","Hazelmarie Elliott"),
            };

            ImageTemplates = new List<string>
            {
                new string("AffirmationPopupWhiteBg.jpeg"),
                new string("AffirmationPopupGreenBg.jpeg"),
            };

            int rndIndex = rnd.Next(0,Quotes.Count - 1);
            SelectedQuote = Quotes[rndIndex].Statement;
            SelectedQuoteAuthor = Quotes[rndIndex].Author;
            SelectedImageTemplate = ImageTemplates[rnd.Next(0, ImageTemplates.Count)];
            SelectedQuoteColor = SelectedImageTemplate == "AffirmationPopupGreenBg.jpeg" ? "White" : "Green";
            SelectedQuoteAuthorColor = SelectedQuoteColor == "Green" ? "White" : "Green";
        }
    }
}
