using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        Dictionary<string, int> menu = new Dictionary<string, int>
        {
            {"紅茶", 40 },
            {"綠茶", 60 },
            {"可樂", 85 },
            {"紅牛", 95 },
            {"魔爪", 110 },
            {"PRIME", 150 },
        };

        Dictionary<string, int> orders = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OrderBtn_Click(object sender, RoutedEventArgs e)
        {
            orders.Clear();

            foreach (var child in DrinkMenu_StackPanel.Children)
            {
                if (child is StackPanel sp)
                {
                    var cb = sp.Children[0] as CheckBox;
                    var nameLabel = sp.Children[1] as Label;
                    var slider = sp.Children[3] as Slider;

                    string drinkName = nameLabel.Content.ToString();
                    int amount = (int)slider.Value;

                    if (cb.IsChecked == true && amount > 0)
                    {
                        orders[drinkName] = amount;
                    }
                }
            }

            finalOrder.Text = "飲料訂單如下:\n";
            int total = 0, index = 0;

            foreach (var item in orders)
            {
                string drinkName = item.Key;
                int amount = item.Value;
                int price = menu[drinkName];
                int subTotal = price * amount;

                index++;
                finalOrder.Text += $"{index} : {drinkName} {price}元，{amount}個，共{subTotal}元\n";
                total += subTotal;
            }

            finalOrder.Text += $"總計:{total}元\n";

            double discountRate = 1.0;
            string discountMessage = "無折扣";

            if (total >= 500)
            {
                discountRate = 0.8;
                discountMessage = "八折";
            }
            else if (total >= 300)
            {
                discountRate = 0.85;
                discountMessage = "八五折";
            }
            else if (total >= 200)
            {
                discountRate = 0.9;
                discountMessage = "九折";
            }

            int finalPrice = (int)(total * discountRate);
            finalOrder.Text += $"折扣：{discountMessage}\n應付金額：{finalPrice}元\n";

            string diningOption = (bool)DineInRadio.IsChecked ? "內用" :
                                  (bool)TakeOutRadio.IsChecked ? "外帶" : "未選擇";

            finalOrder.Text += $"用餐方式：{diningOption}";
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}
