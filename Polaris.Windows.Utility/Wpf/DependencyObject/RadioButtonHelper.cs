namespace Polaris.Utility.Wpf
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    public class RadioButtonHelper : DependencyObject
    {
        public static Object GetSelfValue(DependencyObject obj)
        {
            return obj.GetValue(SelfValueProperty);
        }

        public static void SetSelfValue(DependencyObject obj, Object value)
        {
            obj.SetValue(SelfValueProperty, value);
        }

        public static readonly DependencyProperty SelfValueProperty =
            DependencyProperty.Register("SelfValue", typeof(Object), typeof(RadioButtonHelper), new PropertyMetadata(null, SelfValueChangedCallback));

        public static Object GetConditionValue(DependencyObject obj)
        {
            return obj.GetValue(ConditionValueProperty);
        }

        public static void SetConditionValue(DependencyObject obj, Object value)
        {
            obj.SetValue(ConditionValueProperty, value);
        }

        public static readonly DependencyProperty ConditionValueProperty =
            DependencyProperty.Register("ConditionValue", typeof(Object), typeof(RadioButtonHelper), new PropertyMetadata(null, ConditionValueChangedCallback));

        private static void SelfValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radioButton = d as RadioButton;
            if (radioButton == null)
            {
                return;
            }

            var targetVal = GetConditionValue(d);
            if (targetVal == null)
            {
                radioButton.IsChecked = false;
                return;
            }
            if (e.NewValue == null)
            {
                radioButton.IsChecked = false;
                return;
            }

            if (targetVal == e.NewValue)
            {
                radioButton.IsChecked = true;
            }
            else
            {
                radioButton.IsChecked = false;
            }
        }

        private static void ConditionValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var radioButton = d as RadioButton;
            if (radioButton == null)
            {
                return;
            }

            var selfValue = GetSelfValue(d);
            if (selfValue == null)
            {
                radioButton.IsChecked = false;
                return;
            }
            if (e.NewValue == null)
            {
                radioButton.IsChecked = false;
                return;
            }

            if (selfValue == e.NewValue)
            {
                radioButton.IsChecked = true;
            }
            else
            {
                radioButton.IsChecked = false;
            }
        }
    }
}
