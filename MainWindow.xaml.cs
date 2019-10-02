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
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Seller> sellerWorkingList;
        List<Buyer> buyerWorkingList;

        List<Seller> sellerDataList;
        List<Buyer> buyerDataList;

        List<Seller> sellerSurplusList;
        List<Buyer> buyerDeficitList;

        List<Seller> sellerCurrSurplusList;
        List<Buyer> buyerCurrDeficitList;
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

            int buyFlagWatch;
            int sellFlagWatch;

            


            for (i = 0; i < 3650; i++)   // simulate 10 years each step being a day
            {
                buyEmptyFlag = 0;
                sellEmptyFlag = 0;

                sellerWorkingList = sellerDataList.Concat(sellerSurplusList).ToList();
                buyerWorkingList = buyerDataList.Concat(buyerDeficitList).ToList();


                for (k = buyEmptyFlag; k < buyerWorkingList.Count; k++)
	            {

                for (j = sellEmptyFlag; j < sellerWorkingList.Count; j++)
                {
                    buyFlagWatch = buyEmptyFlag;
                    if (sellerWorkingList[j].CostProduction >= buyerWorkingList[buyEmptyFlag].MaxCost * buyerWorkingList[buyEmptyFlag].NegotiationPercent)
                    {
                        transact(sellerWorkingList[j], buyerWorkingList[buyEmptyFlag]);
                    }
                    if (buyFlagWatch != buyEmptyFlag)
                    {
                        break;
                    }

                    if (j == sellerWorkingList.Count - 1)     // this is where price should iterate upwards
                    {
                        if (buyEmptyFlag >= buyerDataList.Count)
                        {
                                buyerCurrDeficitList[buyEmptyFlag - buyerDataList.Count].TotalDemand += buyerWorkingList[buyEmptyFlag].DailyVol;
            
                        }
                        else
                        {
                                buyerCurrDeficitList[buyEmptyFlag] += buyerWorkingList[buyEmptyFlag].DailyVol;
            
                        }
                        buyerWorkingList[buyEmptyFlag].DailyVol = 0;
                        buyEmptyFlag++;
                    }


                }

            }



        }

       

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

                
                Seller newSeller = new Seller(hold, sellVolume, costProduction, incentivePercent);
                sellerDataList.Add(newSeller);
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
            SellVolTextBox.Text = selectedSeller.SellVolume.ToString();
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
            if (seller.DailyVol > buyer.DailyVol)
            {
                buyEmptyFlag++;


            }
            else if (seller.DailyVol < buyer.DailyVol)
            {
                sellEmptyFlag++;


            }
            else
            {

            }
        }
    }


}

