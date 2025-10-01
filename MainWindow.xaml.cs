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
            finalOrder.Text = "飲料訂單如下:\n";
            int total = 0;
            int index = 0;

            foreach (var item in menu)
            {
                string drinkItem = item.Key;
                int price = item.Value;

                if (orders.TryGetValue(drinkItem, out int amount) && amount > 0)
                {
                    index++;
                    int subTotal = price * amount;
                    finalOrder.Text += $"{index} : {drinkItem} {price}元，{amount}個，共{subTotal}元\n";
                    total += subTotal;
                }
            }


            finalOrder.Text += $"總計:{total}元";

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

            finalOrder.Text += $"\n折扣：{discountMessage}";
            finalOrder.Text += $"\n應付金額：{finalPrice}元";

            // 取得用餐方式
            string diningOption = (bool)DineInRadio.IsChecked ? "內用" :
                                  (bool)TakeOutRadio.IsChecked ? "外帶" : "未選擇";

            finalOrder.Text += $"\n用餐方式：{diningOption}";
        }

        private void TextBox_Changed(object sender, TextChangedEventArgs e)
        {
            var TargetTextBox = sender as TextBox;
            if (TargetTextBox == null) return;

            int amount;
            bool success = int.TryParse(TargetTextBox.Text, out amount);

            var targetStackPanel = TargetTextBox.Parent as StackPanel;
            var targetNameLabel = targetStackPanel?.Children[0] as Label;

            if (targetNameLabel == null) return;

            string drinkName = targetNameLabel.Content.ToString();

            if (!success || amount < 0)
            {
                MessageBox.Show("請輸入正整數");
                return;
            }

            if (menu.ContainsKey(drinkName))
            {
                orders[drinkName] = amount;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var targetSlider = sender as Slider;
            if (targetSlider == null) return;

            int amount = (int)targetSlider.Value;

            var targetStackPanel = targetSlider.Parent as StackPanel;
            var targetNameLabel = targetStackPanel?.Children[0] as Label;
            if (targetNameLabel == null) return;

            string drinkName = targetNameLabel.Content.ToString();

            if (menu.ContainsKey(drinkName))
            {
                orders[drinkName] = amount;
            }
        }
    }
}
