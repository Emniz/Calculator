using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using CalcLibrary;

namespace Calculator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool PerfOperation;
        bool inField;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonNumber(object sender, RoutedEventArgs e)
        {
            MatchCollection nums = new Regex(@"(?:(?<=\D)-|(?<=^)-)?\d+(?:[,\.]\d+)?").Matches(textBlock.Text);
            string num = (string)((Button)e.OriginalSource).Content;
            if (inField)
            {
                textBlock.Text = "";
                inField = false;
            }
            if (nums.Count > 0 && nums[nums.Count - 1].ToString() == "0" && textBlock.Text.EndsWith(nums[nums.Count - 1].ToString()))
                textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
            textBlock.Text += num;
        }

        private void Operation(object sender, RoutedEventArgs e)
        {
            string op = (string)((Button)e.OriginalSource).Content;
            if (!PerfOperation && (textBlock.Text.Last() == 'π' || textBlock.Text.Last() == 'e' || char.IsDigit(textBlock.Text.Last())))
            {
                textBlock.Text += op;
                PerfOperation = true;
                inField = false;
            }
        }

        private void ButtonFunction(object sender, RoutedEventArgs e)
        {
            if (!PerfOperation)
            {
                string op = (string)((Button)e.OriginalSource).Content;
                if (op == "1/" || op == "e^") textBlock.Text = textBlock.Text.Insert(0, op);
                else
                    textBlock.Text += op;
                PerfOperation = true;
                ButtonResult(sender, e);
            }
        }
        private void ButtonClear(object sender, RoutedEventArgs e)//очистка поля
        {
            textBlock.Text = "0";
            PerfOperation = false;
            inField = false;
        }
        private void ButtonRemove(object sender, RoutedEventArgs e)//удаление последнего символа
        {
            MatchCollection opers = new Regex(@"(?:[^\d\.,\-eπ]|(?<=[\deπ])-)+").Matches(textBlock.Text);
            if (inField)
                ButtonClear(sender, e);
            if (opers.Count > 0 && textBlock.Text.EndsWith(opers[opers.Count - 1].ToString()))
            {
                textBlock.Text = textBlock.Text.Remove(opers[opers.Count - 1].Index);
                PerfOperation = false;
            }
            else
                textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
            if (textBlock.Text == "")
                textBlock.Text = "0";
        }

        private void ButtonResult(object sender, RoutedEventArgs e)//вывод результата в поле
        {
            try
            {
                textBlock.Text = Calc.DoOperation(textBlock.Text); ;
                inField = true;
                PerfOperation = false;
            }
            catch
            {
                MessageBox.Show("Неверные данные!");
                ButtonClear(sender, e);
            }
        }

        private void ButtonConstant(object sender, RoutedEventArgs e)//константное выражение pi и exp
        {
            string cons = (string)((Button)e.OriginalSource).Content;
            if (inField)
                textBlock.Text = "";
            MatchCollection numOrConst = new Regex(@"\d+(?:[,\.]\d+)?|π|e").Matches(textBlock.Text);
            if (textBlock.Text.Length > 0 && numOrConst[numOrConst.Count - 1].ToString() == "0")
                textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
            if (textBlock.Text == "" || numOrConst.Count > 0 && !textBlock.Text.EndsWith(numOrConst[numOrConst.Count - 1].ToString()))
                textBlock.Text += cons;
        }

        private void ButtonDot(object sender, RoutedEventArgs e)//число с запятой
        {
            if (char.IsDigit(textBlock.Text.Last()))
            {
                MatchCollection mc = new Regex(@"\d+(,\d+)?").Matches(textBlock.Text);
                if (!mc[mc.Count - 1].ToString().Contains(","))
                    textBlock.Text += ",";
            }
        }

        private void ButtonSign(object sender, RoutedEventArgs e)//свап знака числа
        {
            MatchCollection mc = new Regex(@"(?:[^\d\.,\-eπ]|(?<=[\deπ])-)+").Matches(textBlock.Text);
            int index = (mc.Count > 0) ? mc[mc.Count - 1].Index : -1;
            if (index == textBlock.Text.Length - 1)
                textBlock.Text += '-';
            else if (index < textBlock.Text.Length - 1 && new Regex(@"[\deπ]").IsMatch(textBlock.Text[index + 1].ToString()) && textBlock.Text[index + 1] != '0')
                textBlock.Text = textBlock.Text.Insert(index + 1, "-");
            else if (index == textBlock.Text.Length - 2 && textBlock.Text.Last() == '-')
                textBlock.Text = textBlock.Text.Remove(textBlock.Text.Length - 1);
            else if (index < textBlock.Text.Length - 1 && textBlock.Text[index + 1] == '-')
                textBlock.Text = textBlock.Text.Remove(index + 1, 1);
        }
    }
}
