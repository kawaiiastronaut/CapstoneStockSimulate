using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Buyer
    {
        private string buyerName;
        private long totalDemand;
        private int negotiationPercent;
        private float maxCost;

        public Buyer( string buyerName, long totalDemand, int negotiationPercent, float maxCost )
        {
            this.buyerName = buyerName;
            this.totalDemand = totalDemand;
            this.negotiationPercent = negotiationPercent;
            this.maxCost = maxCost;


 



        }


        public string BuyerName
        {
            get
            {
                return buyerName;
            }
            set
            {
                buyerName = value;
            }
        }
        public long TotalDemand
        {
            get
            {
                return totalDemand;
            }
            set
            {
                totalDemand = value;
            }
        }
        public int NegotiationPercent
        {
            get
            {
                return negotiationPercent;
            }
            set
            {
                negotiationPercent = value;
            }
        }
       
        


        



        public float MaxCost
        {
            get
            {
                return maxCost;
            }
            set
            {
                maxCost = value;
            }
        }




    }
}
