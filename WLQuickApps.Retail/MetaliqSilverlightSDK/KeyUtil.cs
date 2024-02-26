using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MetaliqSilverlightSDK
{
    public class KeyUtil
    {
        public static string GetShiftModifiedValue(Key key)
        {
            string result = null;
            string value = key.ToString();
            switch (value)
            {
                case "D1":
                    result = "!";
                    break;
                case "D2":
                    result = "@";
                    break;
                case "D3":
                    result = "#";
                    break;
                case "D4":
                    result = "$";
                    break;
                case "D5":
                    result = "%";
                    break;
                case "D6":
                    result = "^";
                    break;
                case "D7":
                    result = "&";
                    break;
                case "D8":
                    result = "*";
                    break;
                case "D9":
                    result = "(";
                    break;
                case "D0":
                    result = ")";
                    break;
            }
            return result;
        }
    }
}
