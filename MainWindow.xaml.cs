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

        int buyerClickCounter = 0;
        int sellerClickCounter = 0;
        int arrayStartValue = 0;
        int arrayEndValue = 1840;
        long[] barGraphValues = new long[20];
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

            } //end final FOR loop

            int[] integerArray = new int[] { 21, 2, 3, 4, 5, -10, 75, -50, 100, -24, 21, 2, 3, 4, 5, -10, 75, -50, 87, -24 };
            
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


            long[] longArray = Array.ConvertAll<int, long>(integerArray,
                delegate (int ie)
                {
                    return (long)ie;
                });

            longerArray = Array.ConvertAll<int, long>(integer10Array,
                delegate (int ie)
                {
                    return (long)ie;
                });

            displayOutput(longArray);
            displayLineGraph(longerArray);




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

        

        private void displayOutput(long[] output)
        {
            barGraphValues = output;
            front_Canvas.Children.Clear();
            front_Canvas.Width = 35 * 20;
            long[] minMaxValue = new long[20];
            int i;
            long minValue=10000;
            long maxValue=0;
            float minMaxDiff;
            long prevBase = 0;
            long carry = 0;
            float unitHeight;
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
            minMaxDiff = (float) Abs(maxValue - minValue);
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
            newZeroPoint = unitHeight * -(minValue);
            

            for (i = 0; i <= 19; i++)
            {
                if(output[i]<0)
                {
                    System.Windows.Shapes.Rectangle rect;
                    rect = new System.Windows.Shapes.Rectangle();
                    rect.Stroke = new SolidColorBrush(Colors.Red);
                    rect.Fill = new SolidColorBrush(Colors.Red);
                    
                    
                    rect.Width = 35;
                    rect.Height = Abs(output[i]) * unitHeight;
                    
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
                    Canvas.SetBottom(rect, newZeroPoint - rect.Height);
                   
                    front_Canvas.Children.Add(rect);
                    Label label = new Label();
                    label.FontSize = 8;
                    label.FontWeight = FontWeights.Bold;
                    label.Content = "$" + output[i].ToString();
                    label.Visibility = Visibility.Hidden;

                  
                    Canvas.SetLeft(label, i * rect.Width);
                    Canvas.SetBottom(label, newZeroPoint - rect.Height - 5);
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
                    rect.Height = Abs(output[i]) * unitHeight;
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
                    Canvas.SetBottom(rect, newZeroPoint);
                    front_Canvas.Children.Add(rect);
                    
                    Label label = new Label();
                    label.Content = "$" + output[i].ToString();
                    label.FontSize = 8;
                    label.FontWeight = FontWeights.Bold;
                    
                  
                    label.Visibility = Visibility.Hidden;

                    Canvas.SetLeft(label, i * rect.Width);
                    Canvas.SetBottom(label, output[i] + 35);
                    
                    front_Canvas.Children.Add(label);
                    if (showHideBarValueBox.IsChecked == true)
                    {
                        label.Visibility = Visibility.Visible;
                    }
                }

                

            }
            Max.Content = "$" + maxValue.ToString();
            Min.Content = "$" + minValue.ToString();
           
        }

        private void showHideBarGraphValues(object sender, RoutedEventArgs e)
        {
            displayOutput(barGraphValues);
        }

        private void displayLineGraph(long[] output)
        {
            lineGraphCanvas.Children.Clear();

            int i;
            long minValue = 10000;
            long maxValue = 0;
            float minMaxAvg = 0;
            long height = 200;
            float minMaxDiff = 0;
            float unitHeight;
            float centerGraph = 0;
            float newZeroPoint = 0;

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
            
            centerGraph = ((float)(maxValue + minValue) / 2);
            maxLineGraph.Content = maxValue;
            minLineGraph.Content = minValue;
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
            displayLineGraph(longerArray);
        }


        private void SellerNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

      
        
    }


}

