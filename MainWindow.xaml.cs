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

        public float[] surplusDeficit = new float[20];

        public List<Seller> sellerCurrSurplusList = new List<Seller>();
        public List<Buyer> buyerCurrDeficitList = new List<Buyer>();

        private int buyEmptyFlag;
        private int sellEmptyFlag;

        public int i;
        public int j;
        public int k;
        public int count = 0;

        public int buyFlagWatch;
        public int sellFlagWatch;

        public MainWindow()
        {
            InitializeComponent();

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

        private void SellerDataListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Seller selectedSeller;
            selectedSeller = sellerDataList[SellerDataListBox.SelectedIndex];
            SellerNameTextBox.Text = selectedSeller.SellerName;
            SellVolTextBox.Text = selectedSeller.TotalVolume.ToString();
            CostProductionTextBox.Text = selectedSeller.CostProduction.ToString();
            IncentiveTextBox.Text = selectedSeller.IncentivePercent.ToString();
            
        }



        private void displayOutput(long[] output)
        {
            front_Canvas.Children.Clear();
            long[] minMaxValue = new long[20];
            int i;
            long minValue=0;
            long maxValue=0;
            long minMaxDiff;
            long height = 200;
            long prevBase = 0;
            long carry = 0;
            long unitHeight;
            minMaxValue[0] = output[0];
            for (i = 1; i <= 19; i++)
            {
                minMaxValue[i] = minMaxValue[i-1] + output[i];

            }
            for (i = 0; i <= 19; i++)
            {
                if (minMaxValue[i]>maxValue)
                {
                    maxValue = minMaxValue[i];
                }
                if (minMaxValue[i] < minValue)
                {
                    minValue = minMaxValue[i];
                }

            }
            minMaxDiff = Abs(maxValue - minValue);
            unitHeight = minMaxDiff/height;
            prevBase = maxValue / unitHeight;
            for (i = 0; i <= 19; i++)
            {
                if(output[i]<0)
                {
                    System.Windows.Shapes.Rectangle rect;
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    rect.Width = 35;
                    if (unitHeight != 0)
                    {
                        rect.Height = Abs((carry + output[i]) / unitHeight);
                        carry = ((carry + output[i]) % unitHeight);
                    }
                    else
                    {
                        rect.Height = height;
                    }
                    Canvas.SetLeft(rect, i * rect.Width);
                    prevBase = (long)rect.Height + prevBase;
                    Canvas.SetTop(rect, prevBase-rect.Height);
                    front_Canvas.Children.Add(rect);
                    Label label = new Label();
                    label.FontSize = 5;
                    label.Content = "$" + output[i].ToString();
                    Canvas.SetLeft(label, i * rect.Width);
                    Canvas.SetTop(label, prevBase-rect.Height-2);
                    front_Canvas.Children.Add(label);
                }
                else
                {
                    System.Windows.Shapes.Rectangle rect;
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Green);
                    rect.Fill = new SolidColorBrush(Colors.Green);
                    rect.Width = 35;
                    if (unitHeight != 0)
                    {
                        rect.Height = (float)((carry + output[i]) / unitHeight);
                        carry = (carry + output[i]) % unitHeight;
                    }
                    else
                    {
                        rect.Height = height/20;
                        carry = (carry + output[i]);
                    }
                    Canvas.SetLeft(rect, i * rect.Width);
                    prevBase = -(long)rect.Height + prevBase;
                    Canvas.SetTop(rect, prevBase);
                    front_Canvas.Children.Add(rect);
                    Label label = new Label();
                    label.Content = "$" + output[i].ToString();
                    Canvas.SetLeft(label, i * rect.Width);
                    Canvas.SetTop(label, prevBase);
                    label.FontSize = 5;
                    front_Canvas.Children.Add(label);
                }


               
                



            }
            Max.Content = "$" + maxValue.ToString();
            Min.Content = "$" + minValue.ToString();
            







        }

        private void SellerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        void transact(Seller seller, Buyer buyer)

        {
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
            for (i = 0; i < 368; i++)   // simulate 5 years 
            {
                buyEmptyFlag = 0;
                sellEmptyFlag = 0;


                foreach (Seller index in sellerDataList)
                {
                    sellerCurrDataList.Add(new Seller(index.SellerName, index.TotalVolume, index.CostProduction, index.IncentivePercent));
                }

                foreach (Buyer index in buyerDataList)
                {
                    buyerCurrDataList.Add(new Buyer(index.BuyerName, index.TotalDemand, index.NegotiationPercent, index.MaxCost));
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
                        if (sellerWorkingList[j].CostProduction <= buyerWorkingList[buyEmptyFlag].MaxCost * (float)buyerWorkingList[buyEmptyFlag].NegotiationPercent)
                        {
                            transact(sellerWorkingList[j], buyerWorkingList[buyEmptyFlag]);
                            sellFlagWatch = sellEmptyFlag;

                        }
                        if (buyFlagWatch != buyEmptyFlag)
                        {
                            break;
                        }
                        if (sellFlagWatch == (sellerWorkingList.Count - 1) && sellerWorkingList[sellFlagWatch].DailyVolume == 0)
                        {
                            break;
                        }

                        if (j == sellerWorkingList.Count - 1)     // this is where price should iterate upwards
                        {
                            if (buyEmptyFlag >= buyerDataList.Count)
                            {
                                buyerDeficitList[buyEmptyFlag - buyerDataList.Count].DailyDemand += buyerWorkingList[buyEmptyFlag].DailyDemand;

                            }
                            else
                            {
                                buyerDeficitList[buyEmptyFlag].DailyDemand += buyerWorkingList[buyEmptyFlag].DailyDemand; // need to change this

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

            }
        }
    }


}

