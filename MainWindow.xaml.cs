using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Math;

namespace Capstone
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml 
    /// 
    /// </summary>
   
       
    public partial class MainWindow : Window
    {
        float runningTotal = 0;
        public List<Seller> sellerWorkingList = new List<Seller>();
        public List<Buyer> buyerWorkingList = new List<Buyer>();

        public List<Seller> sellerDataList = new List<Seller>();
        public List<Buyer> buyerDataList = new List<Buyer>();

        public List<Seller> sellerCurrDataList = new List<Seller>();
        public List<Buyer> buyerCurrDataList = new List<Buyer>();

        public List<Seller> sellerSurplusList = new List<Seller>();
        public List<Buyer> buyerDeficitList = new List<Buyer>();

        public float sellMult = 1;
        public float buyMult = 1;

        public float buyStand;
        public float sellStand;

        public float[] buyList =  { 92.51f, 92.84f, 93.96f, 94.47f, 94.34f, 95.16f, 96.84f, 96.25f, 96.01f, 96.48f, 97.44f, 97.50f, 97.18f, 98.93f, 99.39f, 99.77f, 99.50f, 99.83f, 100.73f, 100.53f, 99.92f, 100.19f, 101.61f, 101.87f };
        public float[] sellList = { 92.71f, 93.36f, 94.70f, 96.29f, 95.79f, 96.86f, 97.79f, 97.94f, 97.33f, 96.67f, 97.28f, 98.66f, 97.23f, 97.54f, 98.59f, 99.07f, 99.40f, 99.88f, 101.55f, 102.47f, 100.47f, 100.45f, 100.46f, 102.10f };


        public float[] surplusDeficit = new float[20];
        public float[] lineGraph = new float[1840];

        public List<Seller> sellerCurrSurplusList = new List<Seller>();
        public List<Buyer> buyerCurrDeficitList = new List<Buyer>();

        private int buyEmptyFlag;
        private int sellEmptyFlag;

        int buyerClickCounter = 0;
        int sellerClickCounter = 0;
        int arrayStartValue = 0;
        int arrayEndValue = 1840;
        float[] barGraphValues = new float[20];
        int[] integer10Array = new int[1840];
        long[] longerArray = new long[1840];

        public int i;
        public int j;
        public int k;
        public int count = 0;

        public int buyFlagWatch;
        public int sellFlagWatch;

        public MainWindow()
        {
            InitializeComponent();
            InitializeAutoBuyerSeller();
            //test
            //test 2

        }
 

        private void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
           

            float sellVolume;
            long costProduction;
            int incentivePercent;
            string hold;
            hold = SellerNameTextBox.Text;
            if (float.TryParse(SellVolTextBox.Text, out sellVolume) && Int64.TryParse(CostProductionTextBox.Text, out costProduction) && Int32.TryParse(IncentiveTextBox.Text, out incentivePercent))
            {

                
                Seller dataSeller = new Seller(hold, sellVolume, costProduction, incentivePercent);
                sellerDataList.Add(dataSeller);
                ListBoxItem stockListBoxItem = new ListBoxItem();
                stockListBoxItem.Content = SellerNameTextBox.Text;
                SellerDataListBox.Items.Add(stockListBoxItem);

               


                Seller surplusSeller = new Seller(dataSeller.SellerName, 0, dataSeller.CostProduction, dataSeller.IncentivePercent);
                stockListBoxItem.Content = dataSeller.SellerName;
                sellerSurplusList.Add(surplusSeller);

            }
            else
            {
    
            }
        }

        private void AddBuyerButton_Click(object sender, RoutedEventArgs e)
        {

            float buyVolume;
            float maxCost;
            float negotiationPercent;
            string hold;
            hold = BuyerNameTextBox.Text;
            if (float.TryParse(TotalDemandTextbox.Text, out buyVolume) && float.TryParse(MaxCostTextBox.Text, out maxCost) && float.TryParse(NegotiationTextBox.Text, out negotiationPercent))
            {


                Buyer dataBuyer = new Buyer(hold, buyVolume, negotiationPercent,maxCost);
                buyerDataList.Add(dataBuyer);
                ListBoxItem buyerListBoxItem = new ListBoxItem();
                buyerListBoxItem.Content = BuyerNameTextBox.Text;
                BuyerDataListBox.Items.Add(buyerListBoxItem);




                Buyer deficitBuyer = new Buyer(dataBuyer.BuyerName, 0, dataBuyer.NegotiationPercent, dataBuyer.MaxCost);
                buyerListBoxItem.Content = dataBuyer.BuyerName;
                buyerDeficitList.Add(deficitBuyer);

            }
            else
            {

            }


        }





        


        void transact(Seller seller, Buyer buyer)

        {
            if (buyEmptyFlag >= buyerDataList.Count)
            {
                if (buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent > 0)
                {
                    if (sellEmptyFlag >= sellerDataList.Count)
                    {
                        buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent -= sellerCurrSurplusList[sellEmptyFlag - sellerDataList.Count].DailyVolume / sellerCurrDataList[sellEmptyFlag - sellerDataList.Count].TotalVolume;
                    }
                    else
                    {
                        buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent -= sellerCurrSurplusList[sellEmptyFlag].DailyVolume / sellerCurrDataList[sellEmptyFlag].TotalVolume;
                    }
                }

                if (buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent < 100)
                    buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent += buyerCurrDeficitList[buyEmptyFlag - buyerDataList.Count].DailyDemand / buyerCurrDataList[buyEmptyFlag - buyerDataList.Count].TotalDemand;
            }
            else
            {

                if (buyerDataList[buyEmptyFlag].NegotiationPercent > 0)
                {
                    if (sellEmptyFlag >= sellerDataList.Count)
                    {
                        buyerDataList[buyEmptyFlag].NegotiationPercent -= sellerCurrSurplusList[sellEmptyFlag - sellerDataList.Count].DailyVolume / sellerDataList[sellEmptyFlag - sellerDataList.Count].TotalVolume;
                    }
                    else
                    {
                        buyerDataList[buyEmptyFlag].NegotiationPercent -= sellerCurrSurplusList[sellEmptyFlag].DailyVolume / sellerDataList[sellEmptyFlag].TotalVolume;
                    }
                }
                if (buyerDataList[buyEmptyFlag].NegotiationPercent < 100)
                    buyerDataList[buyEmptyFlag].NegotiationPercent += buyerCurrDeficitList[buyEmptyFlag].DailyDemand / buyerCurrDataList[buyEmptyFlag].TotalDemand;

            }
            if (seller.DailyVolume > buyer.DailyDemand)
            {
                buyEmptyFlag++;
                seller.DailyVolume -= buyer.DailyDemand;
                buyer.DailyDemand = 0;


            }
            else if (seller.DailyVolume < buyer.DailyDemand)
            {
                sellEmptyFlag++;
                buyer.DailyDemand -= seller.DailyVolume;
                seller.DailyVolume = 0;


            }
            else
            {
                buyEmptyFlag++;
                sellEmptyFlag++;
                seller.DailyVolume = 0;
                buyer.DailyDemand = 0;
            }
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int count = 0;
            buyStand = buyerDataList.Sum(item => item.TotalDemand);
            sellStand = sellerDataList.Sum(item => item.TotalVolume);

            for (i = 0; i < 1840; i++)   // simulate 5 years 
            {
                buyEmptyFlag = 0;
                sellEmptyFlag = 0;


                if (i / 368 == 0) //yr 1
                {
                    if (i % 368 == 0) //q1
                    {
                        buyMult =  buyList[0] / buyStand;
                        sellMult = sellList[0] / sellStand;
                    }
                    else if (i % 368 == 92) //q2
                    {
                        buyMult = buyList[1] / buyStand;
                        sellMult = sellList[1] / sellStand;
                    }
                    else if (i % 368 == 184) //q3
                    {
                        buyMult = buyList[2] / buyStand;
                        sellMult = sellList[2] / sellStand;
                    }
                    else if (i % 368 == 276) //q4
                    {
                        buyMult = buyList[3] / buyStand;
                        sellMult = sellList[3] / sellStand;
                    }
                }
                else if (i / 368 == 1) //yr 2
                {
                    if (i % 368 == 0) //q1
                    {
                        buyMult = buyList[4] / buyStand;
                        sellMult = sellList[4] / sellStand;
                    }
                    else if (i % 368 == 92) //q2
                    {
                        buyMult = buyList[5] / buyStand;
                        sellMult = sellList[5] / sellStand;
                    }
                    else if (i % 368 == 184) //q3
                    {
                        buyMult = buyList[6] / buyStand;
                        sellMult = sellList[6] / sellStand;
                    }
                    else if (i % 368 == 276) //q4
                    {
                        buyMult = buyList[7] / buyStand;
                        sellMult = sellList[7] / sellStand;
                    }
                }
                else if (i / 368 == 2) //yr 3
                {
                    if (i % 368 == 0) //q1
                    {
                        buyMult = buyList[8] / buyStand;
                        sellMult = sellList[8] / sellStand;
                    }
                    else if (i % 368 == 92) //q2
                    {
                        buyMult = buyList[9] / buyStand;
                        sellMult = sellList[9] / sellStand;
                    }
                    else if (i % 368 == 184) //q3
                    {
                        buyMult = buyList[10] / buyStand;
                        sellMult = sellList[10] / sellStand;
                    }
                    else if (i % 368 == 276) //q4
                    {
                        buyMult = buyList[11] / buyStand;
                        sellMult = sellList[11] / sellStand;
                    }
                }
                else if (i / 368 == 3) //yr 4
                {
                    if (i % 368 == 0) //q1
                    {
                        buyMult = buyList[12] / buyStand;
                        sellMult = sellList[12] / sellStand;
                    }
                    else if (i % 368 == 92) //q2
                    {
                        buyMult = buyList[13] / buyStand;
                        sellMult = sellList[13] / sellStand;
                    }
                    else if (i % 368 == 184) //q3
                    {
                        buyMult = buyList[14] / buyStand;
                        sellMult = sellList[14] / sellStand;
                    }
                    else if (i % 368 == 276) //q4
                    {
                        buyMult = buyList[15] / buyStand;
                        sellMult = sellList[15] / sellStand;
                    }
                }
                else if (i / 368 == 4) //yr 5
                {
                    if (i % 368 == 0) //q1
                    {
                        buyMult = buyList[16] / buyStand;
                        sellMult = sellList[16] / sellStand;
                    }
                    else if (i % 368 == 92) //q2
                    {
                        buyMult = buyList[17] / buyStand;
                        sellMult = sellList[17] / sellStand;
                    }
                    else if (i % 368 == 184) //q3
                    {
                        buyMult = buyList[18] / buyStand;
                        sellMult = sellList[18] / sellStand;
                    }
                    else if (i % 368 == 276) //q4
                    {
                        buyMult = buyList[19] / buyStand;
                        sellMult = sellList[19] / sellStand;
                    }
                }
                else { }

                foreach (Seller index in sellerDataList)
                {
                    sellerCurrDataList.Add(new Seller(index.SellerName, index.TotalVolume*sellMult, index.CostProduction, index.IncentivePercent));
                }

                foreach (Buyer index in buyerDataList)
                {
                    buyerCurrDataList.Add(new Buyer(index.BuyerName, index.TotalDemand*buyMult, index.NegotiationPercent, index.MaxCost));
                }

                int counter = 0;

                foreach (Buyer index in buyerDeficitList)
                {
                    buyerCurrDeficitList.Add(new Buyer(index.BuyerName, 0, index.NegotiationPercent, index.MaxCost));
                }




                counter = 0;

                foreach (Seller index in sellerSurplusList)
                {
                    sellerCurrSurplusList.Add(new Seller(index.SellerName, 0, index.CostProduction, index.IncentivePercent));
                }

                count = 0;

                foreach (Seller index in sellerSurplusList)
                {
                    sellerCurrSurplusList[count].DailyVolume += index.DailyVolume;
                    count++;
                    index.DailyVolume = 0;
                }
                count = 0;
                foreach (Buyer index in buyerDeficitList)
                {
                    buyerCurrDeficitList[count].DailyDemand += index.DailyDemand;
                    count++;
                    index.DailyDemand = 0;
                }




                sellerWorkingList = sellerCurrDataList.Concat(sellerCurrSurplusList).ToList();
                buyerWorkingList = buyerCurrDataList.Concat(buyerCurrDeficitList).ToList();


                for (k = buyEmptyFlag; k < buyerWorkingList.Count; k++)
                {

                    for (j = sellEmptyFlag; j < sellerWorkingList.Count; j++)
                    {
                        buyFlagWatch = buyEmptyFlag;
                        sellFlagWatch = sellEmptyFlag;
                        if (sellerWorkingList[j].MinSellPrice <= buyerWorkingList[buyEmptyFlag].MaxCost * (float)buyerWorkingList[buyEmptyFlag].NegotiationPercent)
                        {
                            transact(sellerWorkingList[j], buyerWorkingList[buyEmptyFlag]);
                            sellFlagWatch = sellEmptyFlag;

                        }
                        if (buyFlagWatch != buyEmptyFlag)
                        {
                            break;
                        }
                        if (sellFlagWatch == (sellerWorkingList.Count - 1))// && sellerWorkingList[sellFlagWatch].DailyVolume <= 0)
                        {
                            break;
                        }

                        if (j == sellerWorkingList.Count - 1)    
                        {
                            if (buyEmptyFlag >= buyerDataList.Count)
                            {
                                buyerDeficitList[buyEmptyFlag - buyerDataList.Count].DailyDemand += buyerWorkingList[buyEmptyFlag].DailyDemand;

                            }
                            else
                            {
                                buyerDeficitList[buyEmptyFlag].DailyDemand += buyerWorkingList[buyEmptyFlag].DailyDemand; 

                            }
                            buyerWorkingList[buyEmptyFlag].DailyDemand = 0;
                            buyEmptyFlag++;
                        }


                    } //end first FOR

                    if (buyEmptyFlag == buyerWorkingList.Count - 1)
                    {
                        break;
                    }

                    if (sellEmptyFlag == sellerWorkingList.Count - 1)
                    {
                        break;
                    }

                }   // end second FOR loop

                if (sellEmptyFlag < sellerWorkingList.Count)
                {
                    int s;
                    for (s = sellEmptyFlag; s < sellerWorkingList.Count; s++)
                    { 
                        if (s >= sellerDataList.Count)
                        {
                            sellerSurplusList[s - sellerDataList.Count].DailyVolume += sellerWorkingList[s].DailyVolume;

                        }
                        else
                        {
                            sellerSurplusList[s].DailyVolume += sellerWorkingList[s].DailyVolume; // need to change this

                        }

                        if (buyEmptyFlag >= buyerDataList.Count)
                        {
                            if (s >= sellerDataList.Count)
                            {
                                if (buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent > 0)
                                    buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent -= sellerSurplusList[s - sellerDataList.Count].DailyVolume / sellerCurrDataList[s - sellerDataList.Count].TotalVolume;
                            }
                            else
                            {
                                if (buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent > 0)
                                    buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent -= sellerSurplusList[s].DailyVolume / sellerCurrDataList[s].TotalVolume;
                            }
                            if (buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent < 100)
                                buyerDataList[buyEmptyFlag - buyerDataList.Count].NegotiationPercent += buyerCurrDeficitList[buyEmptyFlag - buyerDataList.Count].DailyDemand / buyerCurrDataList[buyEmptyFlag - buyerDataList.Count].TotalDemand;
                        }
                        else
                        {
                            if (s >= sellerDataList.Count)
                            {
                                if (buyerDataList[buyEmptyFlag].NegotiationPercent > 0)
                                    buyerDataList[buyEmptyFlag].NegotiationPercent -= sellerSurplusList[s - sellerDataList.Count].DailyVolume / sellerDataList[s - sellerDataList.Count].TotalVolume;
                            }
                            else
                            {
                                if (buyerDataList[buyEmptyFlag].NegotiationPercent > 0)
                                    buyerDataList[buyEmptyFlag].NegotiationPercent -= sellerSurplusList[s].DailyVolume / sellerDataList[s].TotalVolume;
                            }
                            if (buyerDataList[buyEmptyFlag ].NegotiationPercent < 100)
                                buyerDataList[buyEmptyFlag].NegotiationPercent += buyerCurrDeficitList[buyEmptyFlag].DailyDemand / buyerCurrDataList[buyEmptyFlag].TotalDemand;
                        }
                            sellerWorkingList[s].DailyVolume = 0;


                    }
                }


                if (buyEmptyFlag < buyerWorkingList.Count)
                {
                    int b;
                    for (b = buyEmptyFlag; b < buyerWorkingList.Count; b++)
                    {

                        if (b >= buyerDataList.Count)
                        {

                            buyerDeficitList[b - buyerDataList.Count].DailyDemand += buyerWorkingList[b].DailyDemand;

                        }
                        else
                        {
                            buyerDeficitList[b].DailyDemand += buyerWorkingList[b].DailyDemand; // need to change this

                        }
                        buyerWorkingList[b].DailyDemand = 0;

                        if (b >= buyerDataList.Count)
                        {
                            if (sellEmptyFlag >= sellerDataList.Count)
                            {
                                if (buyerDataList[b - buyerDataList.Count].NegotiationPercent > 0)
                                    buyerDataList[b - buyerDataList.Count].NegotiationPercent -= sellerSurplusList[sellEmptyFlag - sellerDataList.Count].DailyVolume / sellerCurrDataList[sellEmptyFlag - sellerDataList.Count].TotalVolume;
                            }
                            else
                            {
                                if (buyerDataList[b - buyerDataList.Count].NegotiationPercent > 0)
                                    buyerDataList[b - buyerDataList.Count].NegotiationPercent -= sellerSurplusList[sellEmptyFlag].DailyVolume / sellerCurrDataList[sellEmptyFlag].TotalVolume;
                            }
                            if (buyerDataList[b - buyerDataList.Count].NegotiationPercent < 100)
                                buyerDataList[b - buyerDataList.Count].NegotiationPercent += buyerDeficitList[b - buyerDataList.Count].DailyDemand / buyerCurrDataList[b - buyerDataList.Count].TotalDemand;
                        }
                        else
                        {
                            if (sellEmptyFlag >= sellerDataList.Count)
                            {
                                if (buyerDataList[b].NegotiationPercent > 0)
                                    buyerDataList[b].NegotiationPercent -= sellerSurplusList[sellEmptyFlag - sellerDataList.Count].DailyVolume / sellerDataList[sellEmptyFlag - sellerDataList.Count].TotalVolume;
                            }
                            else
                            {
                                if (buyerDataList[b].NegotiationPercent > 0)
                                    buyerDataList[b].NegotiationPercent -= sellerSurplusList[sellEmptyFlag].DailyVolume / sellerDataList[sellEmptyFlag].TotalVolume;
                            }
                            if (buyerDataList[b].NegotiationPercent < 100)
                                buyerDataList[b].NegotiationPercent += buyerDeficitList[b].DailyDemand / buyerCurrDataList[b].TotalDemand;
                           
                        }
                        buyerWorkingList[b].DailyDemand = 0;
                    }
                }
                else
                {

                }

                float total = buyerDeficitList.Sum(item => item.DailyDemand);
                float total2 = sellerSurplusList.Sum(item => item.DailyVolume);

                if (i / 368 == 0) //yr 1
                {
                    if (i % 368 == 91) //q1
                    {
                        surplusDeficit[0] = total2 - total;
                        runningTotal += surplusDeficit[0];
                    }
                    else if (i % 368 == 183) //q2
                    {
                        surplusDeficit[1] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[1];
                    }
                    else if (i % 368 == 275) //q3
                    {
                        surplusDeficit[2] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[2];
                    }
                    else if (i % 368 == 367) //q4
                    {
                        surplusDeficit[3] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[3];
                    }
                }
                else if (i / 368 == 1) //yr 2
                {
                    if (i % 368 == 91) //q1
                    {
                        surplusDeficit[4] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[4];
                    }
                    else if (i % 368 == 183) //q2
                    {
                        surplusDeficit[5] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[5];
                    }
                    else if (i % 368 == 275) //q3
                    {
                        surplusDeficit[6] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[6];
                    }
                    else if (i % 368 == 367) //q4
                    {
                        surplusDeficit[7] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[7];
                    }
                }
                else if (i / 368 == 2) //yr 3
                {
                    if (i % 368 == 91) //q1
                    {
                        surplusDeficit[8] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[8];
                    }
                    else if (i % 368 == 183) //q2
                    {
                        surplusDeficit[9] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[9];
                    }
                    else if (i % 368 == 275) //q3
                    {
                        surplusDeficit[10] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[10];
                    }
                    else if (i % 368 == 367) //q4
                    {
                        surplusDeficit[11] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[11];
                    }
                }
                else if (i / 368 == 3) //yr 4
                {
                    if (i % 368 == 91) //q1
                    {
                        surplusDeficit[12] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[12];
                    }
                    else if (i % 368 == 183) //q2
                    {
                        surplusDeficit[13] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[13];
                    }
                    else if (i % 368 == 275) //q3
                    {
                        surplusDeficit[14] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[14];
                    }
                    else if (i % 368 == 367) //q4
                    {
                        surplusDeficit[15] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[15];
                    }
                }
                else if (i / 368 == 4) //yr 5
                {
                    if (i % 368 == 91) //q1
                    {
                        surplusDeficit[16] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[16];
                    }
                    else if (i % 368 == 183) //q2
                    {
                        surplusDeficit[17] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[17];
                    }
                    else if (i % 368 == 275) //q3
                    {
                        surplusDeficit[18] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[18];
                    }
                    else if (i % 368 == 367) //q4
                    {
                        surplusDeficit[19] = total2 - total - runningTotal;
                        runningTotal += surplusDeficit[19];
                    }
                }

                debugBuyer.Text = total.ToString();
                debugSeller.Text = total2.ToString();
                sellerCurrDataList.Clear();
                buyerCurrDataList.Clear();
                buyerCurrDeficitList.Clear();
                sellerCurrSurplusList.Clear();
                buyerWorkingList.Clear();
                sellerWorkingList.Clear();
                
                //START SORTING

                int n = buyerDataList.Count();

                for (int ii = 1; ii < n; ii++)  //Insertion sort for buyers because the lists are mostly sorted, average case O(n) time; is sorted based on cost.
                {

                    Buyer deficitKey = buyerDeficitList[ii];
                    Buyer key = buyerDataList[ii];
                    int jj = ii - 1;

                    while ((jj > -1) && (buyerDataList[jj].RealCost < key.RealCost))
                    {

                        buyerDataList[jj + 1] = buyerDataList[jj];
                        buyerDeficitList[jj + 1] = buyerDeficitList[jj];
                        jj--;
                    }
                    buyerDeficitList[jj + 1] = deficitKey;
                    buyerDataList[jj + 1] = key;
                }

                n = sellerDataList.Count();

                for (int ii = 1; ii < n; ii++)  //Insertion sort for sellers because the lists are mostly sorted, average case O(n) time; is sorted based on volume.
                {

                    Seller surplusKey = sellerSurplusList[ii];
                    Seller key = sellerDataList[ii];
                    int jj = ii - 1;

                    while ((jj > -1) && (sellerSurplusList[jj].DailyVolume < surplusKey.DailyVolume))
                    {

                        sellerDataList[jj + 1] = sellerDataList[jj];
                        sellerSurplusList[jj + 1] = sellerSurplusList[jj];
                        jj--;
                    }
                    sellerSurplusList[jj + 1] = surplusKey;
                    sellerDataList[jj + 1] = key;
                }
                float sum = 0;
                foreach (Buyer index in buyerDataList)
                {
                    sum += index.RealCost;
                }
                int kk = 0;
                foreach (Buyer index in buyerDataList)
                {
                    index.Update();
                    buyerDeficitList[kk].NegotiationPercent = index.NegotiationPercent;
                    //buyerDeficitList[kk].Update();
                    buyerDeficitList[kk].RealCost = index.RealCost;
                    kk++; 
                }
                lineGraph[i] = sum / (float)buyerDataList.Count;

                sellEmptyFlag = 0;
                buyEmptyFlag = 0;

            } //end final FOR loop

            float[] integerArray = new float[] { -21, -2, -3, -4, 125, -10, -250, 500, -100, -24, 250, -2, -3, -4, -5, -10, 75, -50, -87, -24 };
            
            Random rnd = new Random();
            for(int a = 0; a < 1840; a++)
            {
                if(a < 368)
                {
                    if(a < 150)
                    {
                       // integer10Array[a] = a % 550;
                        integer10Array[a] = 325;
                    }
                    else
                    {
                        integer10Array[a] = -100;
                    }
                    //integer10Array[a] = 100;
                }else if(a < 735){
                    integer10Array[a] = a % 550;
                }else if(a < 1103){
                    integer10Array[a] = a % 150;
                }
                else if(a < 1471)
                {
                    integer10Array[a] = a % 150;
                }
                else
                {
                    integer10Array[a] = a % 550;
                }
                
                
                
                //int range = 100;
               
               //integer10Array[a] = rnd.Next(1, 400);

                //integer100Array[a] = a % 200;
                /*int rand = rnd.Next(0, 2);
                if(a == 0){
                    integer10Array[a] = 100;
                }
                else if(rand % 2 == 0)
                {
                    integer10Array[a] = integer10Array[a-1] + 2;
                }
                else
                {
                    integer10Array[a] = integer10Array[a-1] - 2;
                }
                */

            }


            /*long[] longArray = Array.ConvertAll<int, long>(integerArray,
                delegate (int ie)
                {
                    return (long)ie;
                });
                */
            longerArray = Array.ConvertAll<int, long>(integer10Array,
                delegate (int ie)
                {
                    return (long)ie;
                });

            displayOutput(surplusDeficit);
            //displayOutput(integerArray);
            displayLineGraph(lineGraph);




        } // end MAIN 

 

     



        private void AddMultipleBuyers_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is a test");
        
        }

        private void AddMultipleSellers_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("This is a test");
        }


        private void AutoBuyerButton_Click(object sender, RoutedEventArgs e)
        {
            
            if(buyerClickCounter % 2 == 0)
            {
                AddBuyerButton.Content = "Add Buyers";
                //SellerNameLabel.Content = "Seller Names";
                AddBuyerButton.Click -= AddBuyerButton_Click;
                AddBuyerButton.Click += AddMultipleBuyers_Click;
                 
               
                numberOfBuyersTextBox.Visibility = Visibility.Visible;
                numberOfBuyersLabel.Visibility = Visibility.Visible;
                RangeBuyCostTextBox.Visibility = Visibility.Visible;
                RangeBuyCostLabel.Visibility = Visibility.Visible;


            }
            else
            {
                AddBuyerButton.Click += AddBuyerButton_Click;
                AddBuyerButton.Click -= AddMultipleBuyers_Click;
                AddBuyerButton.Content = "Add Buyer";
                
                numberOfBuyersTextBox.Visibility = Visibility.Hidden;
                numberOfBuyersLabel.Visibility = Visibility.Hidden;
                RangeBuyCostTextBox.Visibility = Visibility.Hidden;
                RangeBuyCostLabel.Visibility = Visibility.Hidden;
            }

            buyerClickCounter++;
        }

        private void AutoSellerButton_Click(object sender, RoutedEventArgs e)
        {
            if (sellerClickCounter % 2 == 0)
            {
                AddSellerButton.Content = "Add Sellers";
                // SellerNameLabel.Content = "Seller Names";
                AddSellerButton.Click -= AddSellerButton_Click;
                AddSellerButton.Click += AddMultipleSellers_Click;
              //  AddSellerButton.Visibility = Visibility.Hidden;
                //AddMultipleSellersButton.Visibility = Visibility.Visible;
                numberOfSellersTextBox.Visibility = Visibility.Visible;
                numberOfSellersLabel.Visibility = Visibility.Visible;
                RangeCostProductionTextBox.Visibility = Visibility.Visible;
                RangeCostProductionLabel.Visibility = Visibility.Visible;
            }
            else
            {
                AddSellerButton.Content = "Add Seller";
                // SellerNameLabel.Content = "Seller Name";
                AddSellerButton.Click += AddSellerButton_Click;
                AddSellerButton.Click -= AddMultipleSellers_Click;

                numberOfSellersTextBox.Visibility = Visibility.Hidden;
                numberOfSellersLabel.Visibility = Visibility.Hidden;
                RangeCostProductionTextBox.Visibility = Visibility.Hidden;
                RangeCostProductionLabel.Visibility = Visibility.Hidden;
            }

            sellerClickCounter++;

        }

        private void InitializeAutoBuyerSeller()
        {
            
            numberOfSellersTextBox.Visibility = Visibility.Hidden;
            numberOfSellersLabel.Visibility = Visibility.Hidden;
            RangeCostProductionTextBox.Visibility = Visibility.Hidden;
            RangeCostProductionLabel.Visibility = Visibility.Hidden;

            numberOfBuyersTextBox.Visibility = Visibility.Hidden;
            numberOfBuyersLabel.Visibility = Visibility.Hidden;
            RangeBuyCostTextBox.Visibility = Visibility.Hidden;
            RangeBuyCostLabel.Visibility = Visibility.Hidden;
        }

        private void SellerDataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Seller selectedSeller;
            selectedSeller = sellerDataList[SellerDataListBox.SelectedIndex];
            SellerNameTextBox.Text = selectedSeller.SellerName;
            SellVolTextBox.Text = selectedSeller.TotalVolume.ToString();
            CostProductionTextBox.Text = selectedSeller.CostProduction.ToString();
            IncentiveTextBox.Text = selectedSeller.IncentivePercent.ToString();
            
        }

        

        private void displayOutput(float[] output)
        {
            barGraphValues = output;
            front_Canvas.Children.Clear();
            front_Canvas.Width = 35 * 20;
            float[] minMaxValue = new float[20];
            int i;
            float minValue=10000;
            float maxValue=-10000;
            float totalUnits = 0;
            float maxValPercent = 0;
            float barGraphAvgVal = 0;

            float minMaxDiff;
            long height = 200;
            long prevBase = 0;
            long carry = 0;
            float unitHeight;
            float startingPoint;
            float zeroPoint = 0;
            minMaxValue[0] = output[0];

            float centerGraph = 0;
            float newZeroPoint = 0;

            for (i = 0; i <= 19; i++)
            {
                if (output[i]>maxValue)
                {
                    maxValue = output[i];
                }
                if (output[i] < minValue)
                {
                    minValue = output[i];
                }

            }

            if(minValue < 0 && maxValue > 0)
            {
                totalUnits = maxValue + Abs(minValue);
                maxValPercent = maxValue / totalUnits;
                zeroPoint = (float) front_Canvas.Height - (maxValPercent * (float) front_Canvas.Height);
            }
            else if(minValue > 0 && maxValue > 0)
            {
                totalUnits = maxValue + minValue;
                maxValPercent = maxValue / totalUnits;
                zeroPoint = 0;

            }else if(minValue < 0 && maxValue < 0)
            {
                //totalUnits = Abs(maxValue) + Abs(minValue);
                totalUnits = Abs(maxValue + minValue);
                //Console.WriteLine("Total" + totalUnits);
                //Console.WriteLine(maxValue +"max");
                maxValPercent = maxValue / totalUnits;
                zeroPoint = (float) front_Canvas.Height;
            }
            minMaxDiff = (float) Abs(maxValue + minValue);
            //Console.WriteLine("BarDiff" + minMaxDiff);
            if(minMaxDiff != 0)
            {
                unitHeight = (float)front_Canvas.Height / minMaxDiff;
            }
            else
            {
                unitHeight = 1;
            }


            // prevBase = maxValue / unitHeight;
            // centerGraph = 200 / (maxValue + Abs(minValue));
            centerGraph =  ((float) front_Canvas.Height / 2 ) - ((float) front_Canvas.Height * maxValPercent);
            //centerGraph = ((float)(maxValue + minValue) / 2);
            // newZeroPoint = unitHeight * -(minValue);
            //Console.WriteLine("CenterGraph:" + centerGraph);

            barGraphAvgVal = (minValue + maxValue) / 2;
            avgBarGraph.Content = Decimal.Round( (decimal) barGraphAvgVal, 2).ToString();
            lowerQuartileBarGraph.Content = Decimal.Round( (decimal) ((barGraphAvgVal + minValue) / 2), 2).ToString();
            upperQuartileBarGraph.Content = Decimal.Round( (decimal) ((barGraphAvgVal + maxValue) / 2), 2).ToString();
            graphGridLines(front_Canvas);

            for (i = 0; i <= 19; i++)
            {
                if(output[i]<0)
                {
                    System.Windows.Shapes.Rectangle rect;
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    
                    
                    rect.Width = 35;
                    rect.Height = Abs(output[i]) * Abs(zeroPoint / minValue);
                    //Console.WriteLine("negRect" + rect.Height);
                    //rect.Height = (Abs(output[i]) / totalUnits) * front_Canvas.Height;
                    //Console.WriteLine("Value" + output[i]);


                    /* if (unitHeight != 0)
                     {
                         rect.Height = Abs((carry + output[i]) / unitHeight);
                         carry = ((carry + output[i]) % unitHeight);
                     }
                     else
                     {
                         rect.Height = height;
                     }
                     */
                    Canvas.SetLeft(rect, i * rect.Width);
                    prevBase = (long)rect.Height + prevBase;
                    // Canvas.SetTop(rect, front_Canvas.Height / 2 - centerGraph);
                    Canvas.SetTop(rect, front_Canvas.Height - zeroPoint);
                    //((output[i] - centerGraph) * unitHeight) + lineGraphCanvas.Height / 2);

                    front_Canvas.Children.Add(rect);
                    Label label = new Label();
                    label.FontSize = 8;
                    label.FontWeight = FontWeights.Bold;
                    label.Content = Decimal.Round( (decimal) output[i], 2).ToString();
                    //label.Content = "$" + output[i].ToString();
                    label.Visibility = Visibility.Hidden;

                  
                    Canvas.SetLeft(label, i * rect.Width);
                    Canvas.SetBottom(label, zeroPoint);
                    front_Canvas.Children.Add(label);
                    if (showHideBarValueBox.IsChecked == true)
                    {
                        label.Visibility = Visibility.Visible;
                    }

                }
                else
                {
                    System.Windows.Shapes.Rectangle rect;
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Green);
                    rect.Fill = new SolidColorBrush(Colors.Green);
                    rect.Width = 35;
                    rect.Height = Abs(output[i]) * Abs((front_Canvas.Height - zeroPoint) / maxValue);
                   // Console.WriteLine("PosRect" + rect.Height);
                    //rect.Height = (output[i] / totalUnits) * front_Canvas.Height;
                    
                   
                    //rect.Height = Abs(output[i]) * unitHeight;
                    // Console.WriteLine(rect.Height);
                    /* if (unitHeight != 0)
                     {
                         rect.Height = (float)((carry + output[i]) / unitHeight);
                         carry = (carry + output[i]) % unitHeight;
                     }
                     else
                     {
                         rect.Height = height/20;
                         carry = (carry + output[i]);
                     }

                    */
                    Canvas.SetLeft(rect, i * rect.Width);
                    // prevBase = -(long)rect.Height + prevBase;
                    Canvas.SetBottom(rect, zeroPoint);
                    front_Canvas.Children.Add(rect);
                    
                    Label label = new Label();
                    label.Content = Decimal.Round( (decimal) output[i], 2).ToString();
                    label.FontSize = 8;
                    label.FontWeight = FontWeights.Bold;
                    
                  
                    label.Visibility = Visibility.Hidden;

                    Canvas.SetLeft(label, i * rect.Width);
                    Canvas.SetBottom(label, zeroPoint - 20);
                    
                    front_Canvas.Children.Add(label);
                    if (showHideBarValueBox.IsChecked == true)
                    {
                        label.Visibility = Visibility.Visible;
                    }
                }

                

            }
            Max.Content = Decimal.Round((decimal) maxValue, 2).ToString();
            Min.Content = Decimal.Round((decimal) minValue, 2).ToString();
            

        }

        private void graphGridLines(Canvas canvas)
        {

            for(int i = 0; i <= 2; i++)
            {
                System.Windows.Shapes.Line line;
                line = new System.Windows.Shapes.Line();
                line.Stroke = new SolidColorBrush(Colors.Black);
                line.StrokeThickness = 1;

                if(i == 0) //upper quartile
                {
                    line.X1 = 0;
                    line.Y1 = canvas.Height * .75;
                    line.X2 = canvas.Width;
                    line.Y2 = canvas.Height * .75;
                }
                else if(i == 1) //middle
                {
                    line.X1 = 0;
                    line.Y1 = canvas.Height / 2;
                    line.X2 = canvas.Width;
                    line.Y2 = canvas.Height / 2;
                }
                else if(i == 2) // lower quartile
                {
                    line.X1 = 0;
                    line.Y1 = canvas.Height * .25;
                    line.X2 = canvas.Width;
                    line.Y2 = canvas.Height * .25;
                }

                canvas.Children.Add(line);

            }
            
            
        }

        private void showHideBarGraphValues(object sender, RoutedEventArgs e)
        {
            displayOutput(barGraphValues);
        }


        private void displayLineGraph (float[] output)
        {
            lineGraphCanvas.Children.Clear();

            int i;
            float minValue = 10000;
            float maxValue = 0;
            float minMaxAvg = 0;
            long height = 200;
            float minMaxDiff = 0;
            float unitHeight;
            float centerGraph = 0;
            float newZeroPoint = 0;
            float lineGraphAvgVal = 0;

            for (i = arrayStartValue; i < arrayEndValue; i++)
            {
                if (output[i] > maxValue)
                {
                    maxValue = output[i];
                }
                if (output[i] < minValue)
                {
                    minValue = output[i];
                }

            }
            //minMaxDiff = (float)Abs(maxValue - minValue);
            // Console.WriteLine("Max -> Min" + maxValue + minValue);
            minMaxDiff = (float)Abs(maxValue) + Abs(minValue);
            // minMaxDiff = (float)Abs(maxValue + minValue);
            centerGraph = ((float)(maxValue + minValue) / 2);
            maxLineGraph.Content = Decimal.Round( (decimal) maxValue, 2);
            minLineGraph.Content = Decimal.Round( (decimal) minValue, 2);
           // Console.WriteLine("Diffes" + minMaxDiff);
           // Console.WriteLine("center" + centerGraph);
            // centerGraph = (float)(maxValue + minValue) / 2;

            if (minMaxDiff != 0)
            {
                unitHeight = (float)lineGraphCanvas.Height / minMaxDiff;
            }
            else
            {
                unitHeight = 1;
            }
            Console.WriteLine("unit" + unitHeight);

            lineGraphAvgVal = (minValue + maxValue) / 2;
            avgLineGraph.Content = Decimal.Round((decimal) lineGraphAvgVal, 2).ToString();
            lowerQuartileLineGraph.Content = Decimal.Round( (decimal) ((lineGraphAvgVal + minValue) / 2), 2).ToString();
            upperQuartileLineGraph.Content = Decimal.Round( (decimal) ((lineGraphAvgVal + maxValue) / 2), 2).ToString();

            graphGridLines(lineGraphCanvas);

            // prevBase = maxValue / unitHeight;
            // centerGraph = 200 / (maxValue + Abs(minValue));
            newZeroPoint = unitHeight * -(minValue);

            //minMaxDiff = maxValue + minValue;
            //lineGraphCanvas.Height = 200;

            //makes line graph for years 1-5
            if (arrayStartValue == 0 && arrayEndValue == 1840)
            {
                
                for (i = 0; i < 1840; i++)
                {
                    if (i % 10 == 0)
                    {

                        System.Windows.Shapes.Ellipse circle;
                        circle = new System.Windows.Shapes.Ellipse();
                        circle.Stroke = new SolidColorBrush(Colors.Red);
                        circle.Fill = new SolidColorBrush(Colors.Red);
                        circle.Height = (lineGraphCanvas.Height / 184) * 3.5;
                        circle.Width = (lineGraphCanvas.Height / 184) * 3.5;

                        Canvas.SetLeft(circle, (i * circle.Width) / 10);
                        Canvas.SetBottom(circle, ((output[i] - centerGraph) * unitHeight) + lineGraphCanvas.Height/2);
                       // ((output[i] - centerGraph) * unitHeight) + (lineGraphCanvas.Height / 2)
                        lineGraphCanvas.Children.Add(circle);


                        if (i < 1840 - 10)
                        {
                            System.Windows.Shapes.Line line;
                            line = new System.Windows.Shapes.Line();
                            line.Stroke = new SolidColorBrush(Colors.Black);
                            line.StrokeThickness = 0.5;

                            line.X1 = ((i * circle.Width) / 10) + circle.Width;
                            line.Y1 = (-(output[i] - centerGraph) * unitHeight) + lineGraphCanvas.Height /2;
                            line.X2 = ((i + 10) * circle.Width) / 10;
                            line.Y2 = (-(output[i + 10] - centerGraph) * unitHeight) + lineGraphCanvas.Height / 2;
                            /*
                            line.X1 = (i % 368) * circle.Width / 2 + circle.Width;
                            line.Y1 = (-(output[i] - centerGraph) * unitHeight) + lineGraphCanvas.Height / 2;
                            line.X2 = (i % 368 + 2) * circle.Width / 2;
                            line.Y2 = (-(output[i + 2] - centerGraph) * unitHeight) + lineGraphCanvas.Height / 2;
                            */
                            lineGraphCanvas.Children.Add(line);
                        }

                    }
                }
            }
            else //makes line graph for year 1, 2, 3, 4, or 5
            {
               // Console.WriteLine(minMaxDiff + "diff");
                //Console.WriteLine(unitHeight + "unit");
                for (i = arrayStartValue; i < arrayEndValue; i++)
                {
                    
                    if (i % 2 == 0)
                    {
                        
                        System.Windows.Shapes.Ellipse circle;
                        circle = new System.Windows.Shapes.Ellipse();
                        circle.Stroke = new SolidColorBrush(Colors.Red);
                        circle.Fill = new SolidColorBrush(Colors.Red);
                        circle.Height = (lineGraphCanvas.Width / 184);
                        circle.Width = (lineGraphCanvas.Width / 184);


                        //  Canvas.SetLeft(circle, -((i % 368 * circle.Width) / 2) + 700) ;
                        Canvas.SetLeft(circle, ((i % 368) * circle.Width) / 2);
                        if(unitHeight == 1)
                        {
                            Canvas.SetBottom(circle, lineGraphCanvas.Height/ 2);
                        }
                        else
                        {
                            Canvas.SetBottom(circle, ((output[i] - centerGraph) * unitHeight) + (lineGraphCanvas.Height / 2));
                        }
                        
                        lineGraphCanvas.Children.Add(circle);


                        if (i < arrayEndValue - 2)
                        {
                            System.Windows.Shapes.Line line;
                            line = new System.Windows.Shapes.Line();
                            line.Stroke = new SolidColorBrush(Colors.Black);
                            line.StrokeThickness = 0.5;


                            if(unitHeight == 1)
                            {
                                line.X1 = (i % 368) * circle.Width / 2 + circle.Width;
                                line.Y1 = lineGraphCanvas.Height / 2;
                                line.X2 = (i % 368 + 2) * circle.Width / 2;
                                line.Y2 = lineGraphCanvas.Height / 2;
                            }
                            else
                            {
                                line.X1 = (i % 368) * circle.Width / 2 + circle.Width;
                                line.Y1 = (-(output[i] - centerGraph) * unitHeight) + lineGraphCanvas.Height / 2;
                                line.X2 = (i % 368 + 2) * circle.Width / 2;
                                line.Y2 = (-(output[i + 2] - centerGraph) * unitHeight) + lineGraphCanvas.Height / 2;

            }
                            lineGraphCanvas.Children.Add(line);


                        }
                    }

                }
            }

        }

        private void UpdateLineGraph_Click(object sender, RoutedEventArgs e)
        {


            if(Year1Radio.IsChecked == true)
            {
                arrayStartValue = 0;
                arrayEndValue = 368;
                Console.WriteLine("Test print");
            }
            else if(Year2Radio.IsChecked == true)
            {
                arrayStartValue = 368;
                arrayEndValue = 736;
                Console.WriteLine("Year 2");
            }
            else if (Year3Radio.IsChecked == true)
            {
                arrayStartValue = 736;
                arrayEndValue = 1104;
            }
            else if (Year4Radio.IsChecked == true)
            {
                arrayStartValue = 1104;
                arrayEndValue = 1472;
            }
            else if (Year5Radio.IsChecked == true)
            {
                arrayStartValue = 1472;
                arrayEndValue = 1840;
            }
            else if (AllYearsRadio.IsChecked == true)
            {
                arrayStartValue = 0;
                arrayEndValue = 1840;
            }
            lineGraphCanvas.Children.Clear();
            displayLineGraph(lineGraph);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            //clears bar graph
            front_Canvas.Children.Clear();
            Max.Content = "";
            Min.Content = "";
            avgBarGraph.Content = "";
            lowerQuartileBarGraph.Content = "";
            upperQuartileBarGraph.Content = "";

            //clears line graph
            lineGraphCanvas.Children.Clear();
            maxLineGraph.Content = "";
            minLineGraph.Content = "";
            avgLineGraph.Content = "";
            lowerQuartileLineGraph.Content = "";
            upperQuartileLineGraph.Content = "";
        }




    }


}

