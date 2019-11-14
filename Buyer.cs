using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    public class Buyer
    {
        private string buyerName;
        private float totalDemand;
        private float negotiationPercent;
        private float maxCost;

        private float realCost;
        private float dailyDemand;

        public Buyer( string buyerName, float totalDemand, float negotiationPercent, float maxCost )
        {
            this.buyerName = buyerName;
            this.totalDemand = totalDemand;
            this.negotiationPercent = negotiationPercent;
            this.maxCost = maxCost;

            this.dailyDemand = this.totalDemand / 368;
            this.realCost = this.maxCost * (float)this.negotiationPercent/(float)100;






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
        public float DailyDemand
        {
            get
            {
                return dailyDemand;
            }
            set
            {
                dailyDemand = value;
            }
        }
        public float RealCost
        {
            get
            {
                return realCost;
            }
            set
            {
                realCost = value;
            }
        }
        public float NegotiationPercent
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

                public void Update()
        {
            this.dailyDemand = this.totalDemand/(float)368;
            this.realCost = this.maxCost * ((float)this.negotiationPercent/(float)100);
        }



    }
}
