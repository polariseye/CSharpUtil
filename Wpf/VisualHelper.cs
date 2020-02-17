namespace Polaris.Utility.Wpf
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    public static class VisualHelper
    {
        internal static VisualStateGroup TryGetVisualStateGroup(DependencyObject d, string groupName)
        {
            var root = GetImplementationRoot(d);
            if (root == null)
            {
                return null;
            }

            return VisualStateManager
                .GetVisualStateGroups(root)?
                .OfType<VisualStateGroup>()
                .FirstOrDefault(group => string.CompareOrdinal(groupName, group.Name) == 0);
        }

        internal static FrameworkElement GetImplementationRoot(DependencyObject d)
        {
            return 1 == VisualTreeHelper.GetChildrenCount(d)
                ? VisualTreeHelper.GetChild(d, 0) as FrameworkElement
                : null;
        }

        public static T GetChild<T>(DependencyObject d) where T : DependencyObject
        {
            if (d is T t)
            {
                return t;
            }

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(d); i++)
            {
                var child = VisualTreeHelper.GetChild(d, i);

                var result = GetChild<T>(child);
                if (result != null) return result;
            }

            return default;
        }

        public static IntPtr GetHandle(this Visual visual) =>
            (PresentationSource.FromVisual(visual) as HwndSource)?.Handle ?? IntPtr.Zero;

        public static T GetChild<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            if (null != obj)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                    if (child != null && child is T && (child as T).Name.Equals(name))
                    {
                        return (T)child;
                    }
                    else
                    {
                        T childOfChild = GetChild<T>(child, name);
                        if (childOfChild != null && childOfChild is T && (childOfChild as T).Name.Equals(name))
                        {
                            return childOfChild;
                        }
                    }
                }
            }
            return null;
        }

        public static T GetParent<T>(DependencyObject obj) where T : FrameworkElement
        {
            while (null != obj)
            {
                obj = VisualTreeHelper.GetParent(obj);
                if (obj == null)
                {
                    return null;
                }

                if (obj is T)
                {
                    return (T)obj;
                }
            }

            return null;
        }
    }
}