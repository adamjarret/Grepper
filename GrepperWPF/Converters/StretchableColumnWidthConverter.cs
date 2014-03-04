using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Media;

namespace GrepperWPF.Converters
{
    class StretchableColumnWidthConverter: IValueConverter
    {
        public static double GetWidth(ListView l)
        {
            double total = 0;
            var g = l.View as GridView;

            // Get offset for scrollbar
            var source = PresentationSource.FromVisual(l);
            if (source != null)
            {
                // Resolution independence
                double scale = source.CompositionTarget == null ? 1 : source.CompositionTarget.TransformToDevice.M11; // x=M11, y=M22

                // Only offset when scrollbar is visible
                //  Note: I originally only wanted to offset the last column width when the scrollbar was visible.
                //  This worked eveywhere except when resizing the details list column when the selection was changed
                //  in the top list. Unfortunately, the value for ComputedVerticalScrollBarVisibility is not updated
                //  in time when this method is called in ShowMatches resulting in inconsitent behavior.
                //  I'd like to find an event to hook into eventually to do this consistently, but for now I just 
                //  commented it out.
                //ScrollViewer scrollview = FindVisualChild<ScrollViewer>(l);
                //bool scrollbarIsVisible = scrollview.ComputedVerticalScrollBarVisibility == Visibility.Visible;
                //total += scrollbarIsVisible 
                //    ? SystemParameters.VerticalScrollBarWidth * scale
                //    : 5 * scale; // Hard-coded buffer to keep horizontal scroll bar from appearing

                total += SystemParameters.VerticalScrollBarWidth * scale;
            }

            // Calculate combined width of other columns
            if (g != null)
            {
                for (int i = 0; i < g.Columns.Count - 1; i++)
                {
                    total += g.Columns[i].Width;
                }
            }

            return (l.ActualWidth - total);
        }

        public object Convert(object o, Type type, object parameter, CultureInfo culture)
        {
            return GetWidth(o as ListView);
        }

        public object ConvertBack(object o, Type type, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        // Helper for 'Only offset when scrollbar is visible' code
        /*
        private static childItem FindVisualChild<childItem>(DependencyObject obj)
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        */
    }
}

