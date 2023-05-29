using System;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;

namespace weight2
{
    public partial class ViewController : NSViewController
    {
        private float weightInKg;

        [Outlet]
        private NSTextField textlineMyWeight { get; set; }

        [Outlet]
        private NSButton ButtonNum { get; set; }

        [Outlet]
        private NSTextField textlineMyHeight { get; set; }

        [Outlet]
        private NSTextField bmiIdentificator { get; set; }

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ButtonNum.Activated += CalculateBMI;

            NSNotificationCenter.DefaultCenter.AddObserver(
                NSControl.TextDidChangeNotification,
                HandleWeightInput,
                textlineMyWeight);
        }

        private void HandleWeightInput(NSNotification notification)
        {
            if (float.TryParse(textlineMyWeight.StringValue, out float result))
            {
                weightInKg = result;
            }
            else
            {
                textlineMyWeight.StringValue = RemoveNonNumericCharacters(textlineMyWeight.StringValue);
            }
        }

        private string RemoveNonNumericCharacters(string input)
        {
            string numericOnly = new string(input.ToCharArray().Where(c => char.IsDigit(c)).ToArray());
            return numericOnly;
        }

        private void CalculateBMI(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textlineMyWeight.StringValue) || string.IsNullOrWhiteSpace(textlineMyHeight.StringValue))
            {
                bmiIdentificator.StringValue = "Заповніть всі поля.";
                return;
            }

            if (float.TryParse(textlineMyWeight.StringValue, out float weightInKg) && float.TryParse(textlineMyHeight.StringValue, out float heightInCm))
            {
                if (weightInKg <= 0 || heightInCm <= 0)
                {
                    bmiIdentificator.StringValue = "Помилка.";
                }
                else
                {
                    float heightInM = heightInCm / 100f;
                    float bmi = weightInKg / (heightInM * heightInM);
                    bmi = (float)Math.Round(bmi, 1);

                    if (bmi < 18.5)
                    {
                        bmiIdentificator.TextColor = NSColor.Orange;
                    }
                    else if (bmi >= 18.5 && bmi < 25)
                    {
                        bmiIdentificator.TextColor = NSColor.Green;
                    }
                    else if (bmi >= 25 && bmi < 30)
                    {
                        bmiIdentificator.TextColor = NSColor.Red;
                    }
                    else if (bmi >= 30 && bmi < 35)
                    {
                        bmiIdentificator.TextColor = NSColor.Red;
                    }
                    else if (bmi >= 35 && bmi < 40)
                    {
                        bmiIdentificator.TextColor = NSColor.Red;
                    }
                    else
                    {
                        bmiIdentificator.TextColor = NSColor.Black;
                    }

                    bmiIdentificator.StringValue = $"{bmi:F2}";
                }
            }
            else
            {
                bmiIdentificator.StringValue = "Помилка.";
            }
        }

        public override NSObject RepresentedObject
        {
            get => base.RepresentedObject;
            set => base.RepresentedObject = value;
        }
    }
}
