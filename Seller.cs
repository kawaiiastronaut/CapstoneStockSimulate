using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Seller
    {
        private string sellerName;
        private float totalVolume;
        private long costProduction;
        private int incentivePercent;
        private long[] expenseArray = new long[20];
        private float[] outputArray = new float[4000];

        private float dailyVolume;
        private float minSellPrice;


        public Seller(string sellerName, float totalVolume, long costProduction, int IncentivePercent)
        {
            this.SellerName = sellerName;
            this.TotalVolume = totalVolume;
            this.CostProduction = costProduction;
            this.IncentivePercent = incentivePercent;

            this.dailyVolume = this.totalVolume / 368;
            this.minSellPrice = this.costProduction * ((float)this.incentivePercent/(float)100 + 1);
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
        public float TotalVolume
        {
            get
            {
                return totalVolume;
            }
            set
            {
                totalVolume = value;
            }
        }
        public float DailyVolume
        {
            get
            {
                return dailyVolume;
            }
            set
            {
                dailyVolume = value;
            }
        }
        public float MinSellPrice
        {
            get
            {
                return minSellPrice;
            }
            set
            {
                minSellPrice = value;
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

        public void Update()
        {
            this.DailyVolume = this.TotalVolume/(float)368;
            this.minSellPrice = this.costProduction * ((float)this.incentivePercent/(float)100 + 1);
        }



    }
}
