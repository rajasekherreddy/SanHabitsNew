using System;
using System.Text;
using BuildHappiness.Core.Models;

namespace BuildHappiness.Core.Helpers
{
    public static class ExtensionMethods
    {
        public static bool CompareDate(this DateTime left, DateTime right)
        {
            return left.Date == right.Date;
        }

        public static bool IsInside(this DateTime date, DateTime start, DateTime end)
        {
            return date.Date >= start.Date && date.Date <= end.Date;
        }

        public static string GetCountyCode(this string countryName)
        {
            if (countryName == "India")
            {
                return "in";
            }
            else if (countryName == "Brazil")
            {
                return "br";
            }
            else if (countryName == "France")
            {
                return "fr";
            }
            else if (countryName == "United States")
            {
                return "us";
            }

            return string.Empty;
        }

        public static void CopyTo(this ServiceProvider from, ServiceProvider to)
        {
            to.Country = from.Country;
            to.MobileNumber = from.MobileNumber;
            to.Fax = from.Fax;
            to.Name = from.Name;
            to.Address = from.Address;
            to.BusinessNumber = from.BusinessNumber;
            to.City = from.City.ToString();
            to.Email = from.Email;
            to.PostalCode = from.PostalCode;
            to.ProfilePhotoStream = from.ProfilePhotoStream;
            to.State = from.State;
            to.Website = from.Website;
            to.Type = from.Type;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}