using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Reflection;

public static class Extensions
{
    #region List extensions
    public static void AddUnique<T>(this List<T> TheList, T Value)
    {
        if(TheList.Contains(Value) == false)
        {
            TheList.Add(Value);
        }
    }
    #endregion

    #region FrameworkElement extensions
    public static Rect GetBounds(this FrameworkElement Element)
    {
        Point TopLeft = new Point(Convert.ToDouble(Element.GetValue(Canvas.LeftProperty)), Convert.ToDouble(Element.GetValue(Canvas.TopProperty)));
        Point BottomRight = new Point(Convert.ToDouble(Element.GetValue(Canvas.LeftProperty)) + Element.Width, Convert.ToDouble(Element.GetValue(Canvas.TopProperty)) + Element.Height);

        return new Rect(TopLeft, BottomRight);
    }
    public static double GetX(this FrameworkElement UIElement)
    {
        return (double)UIElement.GetValue(Canvas.LeftProperty);
    }
    public static void SetX(this FrameworkElement UIElement, double Value)
    {
        UIElement.SetValue(Canvas.LeftProperty, Value);
    }
    public static double GetY(this FrameworkElement UIElement)
    {
        return (double)UIElement.GetValue(Canvas.TopProperty);
    }
    public static void SetY(this FrameworkElement UIElement, double Value)
    {
        UIElement.SetValue(Canvas.TopProperty, Value);
    }
    #endregion
    public static T Clone<T>(this T source)
    {

        T cloned = (T)Activator.CreateInstance(source.GetType());



        foreach (PropertyInfo curPropInfo in source.GetType().GetProperties())
        {

            if (curPropInfo.GetGetMethod() != null

                && (curPropInfo.GetSetMethod() != null))
            {

                // Handle Non-indexer properties

                if (curPropInfo.Name != "Item")
                {

                    // get property from source

                    object getValue = curPropInfo.GetGetMethod().Invoke(source, new object[] { });



                    // clone if needed

                    if (getValue != null && getValue is DependencyObject)

                        getValue = Clone((DependencyObject)getValue);



                    // set property on cloned
                    try
                    {
                        curPropInfo.GetSetMethod().Invoke(cloned, new object[] { getValue });
                    }
                    catch (Exception e)
                    {
                    }

                }

                    // handle indexer

                else
                {

                    // get count for indexer

                    int numberofItemInColleciton =

                        (int)

                        curPropInfo.ReflectedType.GetProperty("Count").GetGetMethod().Invoke(source, new object[] { });



                    // run on indexer

                    for (int i = 0; i < numberofItemInColleciton; i++)
                    {

                        // get item through Indexer

                        object getValue = curPropInfo.GetGetMethod().Invoke(source, new object[] { i });



                        // clone if needed

                        if (getValue != null && getValue is DependencyObject)

                            getValue = Clone((DependencyObject)getValue);

                        // add item to collection

                        curPropInfo.ReflectedType.GetMethod("Add").Invoke(cloned, new object[] { getValue });

                    }

                }

            }

        }



        return cloned;

    }
}
