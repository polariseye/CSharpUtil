namespace Polaris.Utility.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    /// <summary>
    /// 弹窗展示实现
    /// </summary>
    public class AlertUtil : ContentControl
    {
        private string _token;

        private Adorner _container;

        private static readonly Dictionary<string, System.Windows.Window> WindowDic = new Dictionary<string, System.Windows.Window>();

        public static readonly DependencyProperty IsClosedProperty = DependencyProperty.Register(
            "IsClosed", typeof(bool), typeof(AlertUtil), new PropertyMetadata(false));

        public bool IsClosed
        {
            get => (bool)GetValue(IsClosedProperty);
            internal set => SetValue(IsClosedProperty, value);
        }

        public static readonly DependencyProperty TokenProperty = DependencyProperty.RegisterAttached(
            "Token", typeof(string), typeof(AlertUtil), new PropertyMetadata(default(string), OnTokenChanged));

        private static void OnTokenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is System.Windows.Window window)
            {
                if (e.NewValue == null)
                {
                    Unregister(window);
                }
                else
                {
                    Register(e.NewValue.ToString(), window);
                }
            }
        }

        public static void SetToken(DependencyObject element, string value)
            => element.SetValue(TokenProperty, value);

        public static string GetToken(DependencyObject element)
            => (string)element.GetValue(TokenProperty);

        private Boolean isForbidWindow = false;

        private Window attachedWIndow = null;

        public AlertUtil()
        {          
        }

        public static void Register(string token, System.Windows.Window window)
        {
            if (string.IsNullOrEmpty(token) || window == null) return;
            WindowDic[token] = window;
        }

        public static void Unregister(string token, System.Windows.Window window)
        {
            if (string.IsNullOrEmpty(token) || window == null) return;

            if (WindowDic.ContainsKey(token))
            {
                if (ReferenceEquals(WindowDic[token], window))
                {
                    WindowDic.Remove(token);
                }
            }
        }

        public static void Unregister(System.Windows.Window window)
        {
            if (window == null) return;
            var first = WindowDic.FirstOrDefault(item => ReferenceEquals(window, item.Value));
            if (!string.IsNullOrEmpty(first.Key))
            {
                WindowDic.Remove(first.Key);
            }
        }

        public static void Unregister(string token)
        {
            if (string.IsNullOrEmpty(token)) return;

            if (WindowDic.ContainsKey(token))
            {
                WindowDic.Remove(token);
            }
        }

        public static AlertUtil Show<T>(string token = "", Boolean isForbidWindow = false) where T : new() => Show(new T(), token, isForbidWindow);

        public static AlertUtil Show(object content, string token = "", Boolean isForbidWindow = false)
        {
            System.Windows.Window window;

            if (string.IsNullOrEmpty(token))
            {
                window = WindowHelper.GetActiveWindow();
            }
            else
            {
                WindowDic.TryGetValue(token, out window);
            }

            return Show(content, window, isForbidWindow);
        }

        public static AlertUtil Show(object content, Window window, Boolean isForbidWindow = false)
        {
            var dialog = new AlertUtil
            {
                Content = content,
                attachedWIndow = window,
                isForbidWindow = isForbidWindow,
            };

            if (window != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(window);
                if (decorator != null)
                {
                    if (decorator.Child != null && dialog.isForbidWindow)
                    {
                        decorator.Child.IsEnabled = false;
                    }

                    var layer = decorator.AdornerLayer;
                    if (layer != null)
                    {
                        var container = new AdornerContainer(layer)
                        {
                            Child = dialog
                        };
                        Panel.SetZIndex(container, 255);
                        dialog._container = container;
                        dialog.IsClosed = false;
                        layer.Add(container);
                    }
                }
            }

            return dialog;
        }

        public void Close()
        {
            Close(this.attachedWIndow);
        }

        private void Close(System.Windows.Window window)
        {
            if (window != null)
            {
                var decorator = VisualHelper.GetChild<AdornerDecorator>(window);
                if (decorator != null)
                {
                    if (decorator.Child != null && this.isForbidWindow)
                    {
                        decorator.Child.IsEnabled = true;
                    }

                    var layer = decorator.AdornerLayer;
                    layer?.Remove(_container);
                    IsClosed = true;
                }
            }
        }
    }
}