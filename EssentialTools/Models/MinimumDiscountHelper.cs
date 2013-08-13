using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentialTools.Models
{
    public class MinimumDiscountHelper : IDiscountHelper
    {
        public decimal ApplyDiscount(decimal totalParam)
        {
            if (totalParam < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            else if (100 < totalParam)
            {
                return totalParam * 0.9M;
            }
            else if (10 < totalParam && totalParam <= 100)
            {
                return totalParam - 5;
            }
            else {
                return totalParam;
            }
        }
    }
}