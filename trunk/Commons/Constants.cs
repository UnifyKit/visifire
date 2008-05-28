using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace Visifire.Commons
{
    public class Constants
    {
        public struct Html
        {
            public static String BaseUri = HtmlPage.Document.DocumentUri.ToString();
        }

        public struct ThemeAndAnimation
        {
            public static String DefaultAnimation = "Type1";
        }

        public struct General
        {
            public static String SrtingUndefined = "Undefined";
            public static String SrtingTrue = "True";
            public static String SrtingFalse = "False";
        }
    }

    public class Generic
    {
        public static void SetNameAndTag(FrameworkElement element)
        {
            if (element.Name.Length == 0)
            {
                Int32 i = 0;

                String type = element.GetType().Name;
                String name = type;

                // Check for an available name
                while (element.FindName(name + i.ToString()) != null)
                {
                    i++;
                }

                name += i.ToString();

                element.SetValue(FrameworkElement.NameProperty, name);
            }

            element.Tag = element.Name;
        }
    }
}
