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
        private float totalDemand;
        private int negotiationPercent;
        private float maxCost;

        private float realCost;
        private float dailyDemand;

        public Buyer( string buyerName, float totalDemand, int negotiationPercent, float maxCost )
        {
            this.buyerName = buyerName;
            this.totalDemand = totalDemand;
            this.negotiationPercent = negotiationPercent;
            this.maxCost = maxCost;

            this.dailyDemand = this.totalDemand / 368;
            this.realCost = this.maxCost * (float)this.negotiationPercent;






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
        public float TotalDemand
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
