using System;
using System.Collections.Generic;
using HappinessIndex.Models;
using System.Linq;
using System.Text;

namespace HappinessIndex.Helpers
{
    public static class ExtensionMethods
    {
        public static List<Factor> GetSelectedFactors(this User user, List<Factor> allFactors)
        {
            List<Factor> selectedFactors = new List<Factor>();

            var selectedFactorsArray = user.SelectedFactors.Split(',');

            foreach (var item in selectedFactorsArray)
            {
                int id = 0;

                int.TryParse(item, out id);

                if (id != 0)
                {
                    var factor = allFactors.Where(item => item.ID == id).FirstOrDefault();

                    if (factor != null)
                    {
                        selectedFactors.Add(factor);
                    }
                }
            }

            return selectedFactors;
        }

        public static void UpdateSelectedFactors(this IList<Factor> allFactors, User user)
        {
            //Reset IsSelected property
            foreach (var item in allFactors)
            {
                item.IsSelected = false;
            }

            var selectedFactorsArray = user.SelectedFactors.Split(',');

            foreach (var item in selectedFactorsArray)
            {

                int.TryParse(item, out int id);

                if (id != 0)
                {
                    var factor = allFactors.Where(item => item.ID == id).FirstOrDefault();

                    if (factor != null)
                    {
                        factor.IsSelected = true;
                    }
                }
            }
        }

        //public static void UpdateSelectedFactorsMicro(this IList<FactorWorkout> allFactors, User user)
        //{
        //    //Reset IsSelected property
        //    foreach (var item in allFactors)
        //    {
        //        item.IsSelected = false;
        //    }

        //    var selectedFactorsArray = user.SelectedFactors.Split(',');

        //    foreach (var item in selectedFactorsArray)
        //    {

        //        int.TryParse(item, out int id);

        //        if (id != 0)
        //        {
        //            var factor = allFactors.Where(item => item.ID == id).FirstOrDefault();

        //            if (factor != null)
        //            {
        //                factor.IsSelected = true;
        //            }
        //        }
        //    }
        //}
    }
}