using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ScientificCalculator
{
    public partial class CalculatorForm : Form
    {
        private TextBox textBox_Result;
        private string input = "";

        public CalculatorForm()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Casio Calculator";
            this.Size = new Size(290, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.WhiteSmoke;

            textBox_Result = new TextBox
            {
                Location = new Point(10, 10),
                Width = 250,
                Height = 40,
                ReadOnly = true,
                TextAlign = HorizontalAlignment.Right,
                Font = new Font("Arial", 18, FontStyle.Bold),
                BackColor = Color.White
            };
            Controls.Add(textBox_Result);

            string[,] buttonTexts = {
                { "7", "8", "9", "/" },
                { "4", "5", "6", "*" },
                { "1", "2", "3", "-" },
                { "0", "00", "=", "+" },
                { "sqrt", "!", "log", "^" },
                { "sin", "cos", "tan", "C" }
            };

            int x = 10, y = 60;
            int buttonWidth = 60, buttonHeight = 50;

            for (int row = 0; row < buttonTexts.GetLength(0); row++)
            {
                for (int col = 0; col < buttonTexts.GetLength(1); col++)
                {
                    string text = buttonTexts[row, col];

                    Button btn = new Button
                    {
                        Text = text,
                        Width = buttonWidth,
                        Height = buttonHeight,
                        Location = new Point(x, y),
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        FlatStyle = FlatStyle.Flat,
                        BackColor = (text == "=") ? Color.Orange :
                                    (text == "C") ? Color.Red :
                                    (text == "/" || text == "*" || text == "-" || text == "+" || text == "^") ? Color.LightBlue :
                                    Color.LightGray
                    };

                    btn.Click += Button_Click;
                    Controls.Add(btn);

                    x += buttonWidth + 5;
                }
                x = 10;
                y += buttonHeight + 5;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string buttonText = btn.Text;

            if (buttonText == "C")
            {
                input = "";
            }
            else if (buttonText == "=")
            {
                try
                {
                    input = EvaluateExpression(input).ToString();
                }
                catch
                {
                    input = "Error";
                }
            }
            else
            {
                input += buttonText;
            }

            textBox_Result.Text = input;
        }

        private double EvaluateExpression(string expr)
        {
            if (expr.StartsWith("sqrt"))
            {
                double num = double.Parse(expr.Substring(4));
                return Math.Sqrt(num);
            }

            if (expr.StartsWith("log"))
            {
                double num = double.Parse(expr.Substring(3));
                return Math.Log10(num);
            }

            if (expr.EndsWith("!"))
            {
                int num = int.Parse(expr.TrimEnd('!'));
                return Factorial(num);
            }

            if (expr.Contains("^"))
            {
                string[] parts = expr.Split('^');
                if (parts.Length == 2)
                {
                    double baseNum = double.Parse(parts[0]);
                    double exponent = double.Parse(parts[1]);
                    return Math.Pow(baseNum, exponent);
                }
            }

            if (expr.StartsWith("sin"))
            {
                double num = double.Parse(expr.Substring(3));
                return Math.Sin(num * (Math.PI / 180));
            }
            if (expr.StartsWith("cos"))
            {
                double num = double.Parse(expr.Substring(3));
                return Math.Cos(num * (Math.PI / 180));
            }
            if (expr.StartsWith("tan"))
            {
                double num = double.Parse(expr.Substring(3));
                return Math.Tan(num * (Math.PI / 180));
            }

            return Convert.ToDouble(new DataTable().Compute(expr, null));
        }

        private int Factorial(int num)
        {
            if (num < 0) throw new ArgumentException("Factorial of negative numbers is not defined.");
            if (num == 0 || num == 1) return 1;
            int result = 1;
            for (int i = 2; i <= num; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
