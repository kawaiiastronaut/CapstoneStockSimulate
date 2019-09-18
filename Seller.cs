using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Seller
    {
        private string sellerName;
        private long sellVolume;
        private long costProduction;
        private int incentivePercent;
        private long[] expenseArray = new long[20];
        private float[] outputArray = new float[4000];


        public Seller(string sellerName, long sellVolume, long costProduction, int IncentivePercent)
        {
            this.SellerName = sellerName;
            this.SellVolume = sellVolume;
            this.CostProduction = costProduction;
            this.IncentivePercent = incentivePercent;
        }

        public string SellerName
        {
            get
            {
                return sellerName;
            }
            set
            {
                sellerName = value;
            }
        }
        public long SellVolume
        {
            get
            {
                return sellVolume;
            }
            set
            {
                sellVolume = value;
            }
        }
        public long CostProduction
        {
            get
            {
                return costProduction;
            }
            set
            {
                costProduction = value;
            }
        }


        public int IncentivePercent
        {
            get
            {
                return incentivePercent;
            }
            set
            {
                incentivePercent = value;
            }
        }


        public long[] ExpenseArray
        {
            get { return expenseArray; }
            set { expenseArray = value; }
        }
        public float[] OutputArray
        {
            get { return outputArray; }
            set { outputArray = value; }
        }




    }
}
