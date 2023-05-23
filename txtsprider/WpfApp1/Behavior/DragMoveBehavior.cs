using System.Windows.Input;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace WpfApp1.Behaviors
{
    internal class DragMoveBehavior : Behavior<Border>
    {
        public Window Target
        {
            get { return (Window)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(Window), typeof(DragMoveBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonDown += HeaderLeftDown;
        }

        private void HeaderLeftDown(object sender, MouseButtonEventArgs e)
        {
            Target.DragMove();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.MouseLeftButtonDown -= HeaderLeftDown;
        }
    }
}
