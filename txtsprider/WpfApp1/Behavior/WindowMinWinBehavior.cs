using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Microsoft.Web.WebView2.Core.DevToolsProtocolExtension.CacheStorage;

namespace WpfApp1.Behaviors
{
    internal class WindowMinWinBehavior : Behavior<UIElement>
    {
        public Window Target
        {
            get { return (Window)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(Window), typeof(WindowMinWinBehavior), new PropertyMetadata(null));

        public int ModelType { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is Button)
            {
                switch (ModelType)
                {
                    case 0:
                        ((Button)AssociatedObject).Click += MinWinEnvent;
                        break;
                    case 1:
                        ((Button)AssociatedObject).Click += MaxWinEnvent;
                        break;

                    case 2:
                        ((Button)AssociatedObject).Click += CloseWinEnvent;
                        break;
                    case 3:
                        ((Button)AssociatedObject).Click += HideEnvent;
                        break;
                }
            }
            if (AssociatedObject is Border)
            {
                AssociatedObject.MouseLeftButtonDown += (o, e) =>
                {
                    if (e.ClickCount >= 2)
                    {
                        if (Target.WindowState == WindowState.Maximized)
                        {
                            Target.WindowState = WindowState.Normal;
                        }
                        else
                        {
                            Target.WindowState = WindowState.Maximized;
                        }
                    }
                };
            }
        }

        private void HideEnvent(object sender, RoutedEventArgs e)
        {
            Target?.Hide();
        }

        private void CloseWinEnvent(object sender, RoutedEventArgs e)
        {
            Target?.Close();
        }

        private void MaxWinEnvent(object sender, RoutedEventArgs e)
        {
            if (Target.WindowState == WindowState.Maximized)
            {
                Target.WindowState = WindowState.Normal;
            }
            else
            {
                Target.WindowState = WindowState.Maximized;
            }
        }

        private void MinWinEnvent(object sender, RoutedEventArgs e)
        {
            Target.WindowState = WindowState.Minimized;
        }
    }
}
