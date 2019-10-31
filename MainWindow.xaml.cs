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
    /// Interaction logic for MainWindow.xaml j
    /// </summary>
    public partial class MainWindow : Window
    {
        float runningTotal = 0;
        List<Seller> sellerWorkingList = new List<Seller>();
        List<Buyer> buyerWorkingList = new List<Buyer>();

        List<Seller> sellerDataList = new List<Seller>();
        List<Buyer> buyerDataList = new List<Buyer>();

        List<Seller> sellerCurrDataList = new List<Seller>();
        List<Buyer> buyerCurrDataList = new List<Buyer>();

        List<Seller> sellerSurplusList = new List<Seller>();
        List<Buyer> buyerDeficitList = new List<Buyer>();

        List<Seller> sellerCurrSurplusList = new List<Seller>();
        List<Buyer> buyerCurrDeficitList = new List<Buyer>();

        private int buyEmptyFlag;
        private int sellEmptyFlag;

        public MainWindow()
        {
            InitializeComponent();
            //test
            //test 2
            int i;
            int j;
            int k;
            int count;

            int buyFlagWatch;
            int sellFlagWatch;

            float[] surplusDeficit = new float[20];

            Seller dataSeller = new Seller("sell1", 20, 10, 0);
            Buyer dataBuyer = new Buyer("buy1", 18, 50, 50);

            //Seller data2Seller = new Seller("sell2", 22, 10, 0);
            Buyer data2Buyer = new Buyer("buy2", 18, 50, 50);

            Buyer deficitBuyer = new Buyer(dataBuyer.BuyerName, 0, dataBuyer.NegotiationPercent, dataBuyer.MaxCost);
            Seller surplusSeller = new Seller(dataSeller.SellerName, 0, dataSeller.CostProduction, dataSeller.IncentivePercent);

            Buyer deficit2Buyer = new Buyer(data2Buyer.BuyerName, 0, data2Buyer.NegotiationPercent, data2Buyer.MaxCost);
            //Seller surplus2Seller = new Seller(data2Seller.SellerName, 0, data2Seller.CostProduction, data2Seller.IncentivePercent);

            ListBoxItem stockListBoxItem = new ListBoxItem();
            stockListBoxItem.Content = dataSeller.SellerName;
            SellerDataListBox.Items.Add(stockListBoxItem);

            ListBoxItem buyerListBoxItem = new ListBoxItem();
            buyerListBoxItem.Content = dataBuyer.BuyerName;
            BuyerDataListBox.Items.Add(buyerListBoxItem);

            //ListBoxItem stock2ListBoxItem = new ListBoxItem();
            //stock2ListBoxItem.Content = data2Seller.SellerName;
            //SellerDataListBox.Items.Add(stock2ListBoxItem);

            ListBoxItem buyer2ListBoxItem = new ListBoxItem();
            buyer2ListBoxItem.Content = data2Buyer.BuyerName;
            BuyerDataListBox.Items.Add(buyer2ListBoxItem);

            sellerDataList.Add(dataSeller);
            buyerDataList.Add(dataBuyer);

            //sellerDataList.Add(data2Seller);
            buyerDataList.Add(data2Buyer);

            sellerSurplusList.Add(surplusSeller);
            buyerDeficitList.Add(deficitBuyer);

            //sellerSurplusList.Add(surplus2Seller);
            buyerDeficitList.Add(deficit2Buyer);


            for (i = 0; i < 368; i++)   // simulate 10 years each step being a day surrently modified to simulate only 1 day with total demand for testing purposes
            {
                buyEmptyFlag = 0;
                sellEmptyFlag = 0;



                //filler code to test functionality

                //Seller dataSeller = new Seller("sell1", 20, 10, 0);
                //Buyer dataBuyer = new Buyer("buy1", 18, 50, 50);

                Seller currDataSeller;

                Buyer currDataBuyer;

                Seller currData2Seller;

                Buyer currData2Buyer;


                currDataSeller = new Seller(dataSeller.SellerName, dataSeller.TotalVolume , dataSeller.CostProduction, dataSeller.IncentivePercent);
                currDataBuyer = new Buyer(dataBuyer.BuyerName, dataBuyer.TotalDemand, dataBuyer.NegotiationPercent, dataBuyer.MaxCost);

                //currData2Seller = new Seller(data2Seller.SellerName, data2Seller.TotalVolume, data2Seller.CostProduction, data2Seller.IncentivePercent);
                currData2Buyer = new Buyer(data2Buyer.BuyerName, data2Buyer.TotalDemand, data2Buyer.NegotiationPercent, data2Buyer.MaxCost);





                Buyer currDeficitBuyer = new Buyer(deficitBuyer.BuyerName, 0, deficitBuyer.NegotiationPercent, deficitBuyer.MaxCost);
                Seller currSurplusSeller = new Seller(surplusSeller.SellerName, 0, surplusSeller.CostProduction, surplusSeller.IncentivePercent);

                Buyer currDeficit2Buyer = new Buyer(deficit2Buyer.BuyerName, 0, deficit2Buyer.NegotiationPercent, deficit2Buyer.MaxCost);
                //Seller currSurplus2Seller = new Seller(surplus2Seller.SellerName, 0, surplus2Seller.CostProduction, surplus2Seller.IncentivePercent);


                sellerCurrDataList.Add(currDataSeller);
                buyerCurrDataList.Add(currDataBuyer);

                //sellerCurrDataList.Add(currData2Seller);
                buyerCurrDataList.Add(currData2Buyer);


                sellerCurrSurplusList.Add(currSurplusSeller);
                buyerCurrDeficitList.Add(currDeficitBuyer);

                //sellerCurrSurplusList.Add(currSurplus2Seller);
                buyerCurrDeficitList.Add(currDeficit2Buyer);

                count = 0;

                foreach (Seller index in sellerSurplusList)
                {
                    sellerCurrSurplusList[count].TotalVolume += index.TotalVolume;
                    count++;
                    index.TotalVolume = 0;
                }
                count = 0;
                foreach (Buyer index in buyerDeficitList)
                {
                     buyerCurrDeficitList[count].TotalDemand += index.TotalDemand;
                     count++;
                    index.TotalDemand = 0;
                }


                /*ListBoxItem stockListBoxItem = new ListBoxItem();
                stockListBoxItem.Content = dataSeller.SellerName;
                SellerDataListBox.Items.Add(stockListBoxItem);

                ListBoxItem buyerListBoxItem = new ListBoxItem();
                buyerListBoxItem.Content = dataBuyer.BuyerName;
                BuyerDataListBox.Items.Add(buyerListBoxItem); */

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
                        if (sellFlagWatch == (sellerWorkingList.Count - 1) && sellerWorkingList[sellFlagWatch].TotalVolume == 0 )
                        {
                            break;
                        }

                        if (j == sellerWorkingList.Count - 1)     // this is where price should iterate upwards
                        {
                            if (buyEmptyFlag >= buyerDataList.Count)
                            {
                                buyerDeficitList[buyEmptyFlag - buyerDataList.Count].TotalDemand += buyerWorkingList[buyEmptyFlag].TotalDemand;
            
                            }
                            else
                            {
                                buyerDeficitList[buyEmptyFlag].TotalDemand += buyerWorkingList[buyEmptyFlag].TotalDemand; // need to change this
            
                            }
                            buyerWorkingList[buyEmptyFlag].TotalDemand = 0;
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

                if (sellEmptyFlag < sellerWorkingList.Count )
                {
                    int s;
                    for (s = sellEmptyFlag; s < sellerWorkingList.Count; s++)
                    {
                        if (s >= sellerDataList.Count)
                        {
                            sellerSurplusList[s - sellerDataList.Count].TotalVolume += sellerWorkingList[s].TotalVolume;

                        }
                        else
                        {
                            sellerSurplusList[s].TotalVolume += sellerWorkingList[s].TotalVolume; // need to change this

                        }
                        sellerWorkingList[s].TotalVolume = 0;

                    }
                }
                
                if (buyEmptyFlag < buyerWorkingList.Count )
                {
                    int b;
                    for (b = buyEmptyFlag; b < buyerWorkingList.Count; b++)
                    {
                        if (b >= buyerDataList.Count)
                        {
                            buyerDeficitList[b - buyerDataList.Count].TotalDemand += buyerWorkingList[b].TotalDemand;

                        }
                        else
                        {
                            buyerDeficitList[b].TotalDemand += buyerWorkingList[b].TotalDemand; // need to change this

                        }
                        buyerWorkingList[b].TotalDemand = 0;

                    }
                }
                else
                {

                }

                float total = buyerDeficitList.Sum(item => item.TotalDemand);
                float total2 = sellerSurplusList.Sum(item => item.TotalVolume);

                if (i/368 == 0) //yr 1
                {
                    if (i%368 == 91) //q1
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
                else if(i/368 ==1) //yr 2
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

            } //end final FOR loop

           


        }

 

        private void AddSellerButton_Click(object sender, RoutedEventArgs e)
        {
           

            long sellVolume;
            long costProduction;
            int incentivePercent;
            string hold;
            hold = SellerNameTextBox.Text;
            if (Int64.TryParse(SellVolTextBox.Text, out sellVolume) && Int64.TryParse(CostProductionTextBox.Text, out costProduction) && Int32.TryParse(IncentiveTextBox.Text, out incentivePercent)&& !(hold.Any(char.IsDigit)))
            {

                
                Seller dataSeller = new Seller(hold, sellVolume, costProduction, incentivePercent);
                sellerDataList.Add(dataSeller);
                ListBoxItem stockListBoxItem = new ListBoxItem();
                stockListBoxItem.Content = SellerNameTextBox.Text;
                SellerDataListBox.Items.Add(stockListBoxItem);
            }
            else
            {
    
            }
        }

        private void AddBuyerButton_Click(object sender, RoutedEventArgs e)
        {
 
           
                
           
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
            if (seller.TotalVolume > buyer.TotalDemand)
            {
                buyEmptyFlag++;
                seller.TotalVolume -= buyer.TotalDemand;
                buyer.TotalDemand = 0;


            }
            else if (seller.TotalVolume < buyer.TotalDemand)
            {
                sellEmptyFlag++;
                buyer.TotalDemand -= seller.TotalVolume;
                seller.TotalVolume = 0;


            }
            else
            {
                buyEmptyFlag++;
                sellEmptyFlag++;
                seller.TotalVolume = 0;
                buyer.TotalDemand = 0;
            }
        }
    }


}

