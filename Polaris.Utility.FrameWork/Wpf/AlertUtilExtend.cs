namespace Polaris.Utility.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public static class AlertUtilExtend
    {
        public interface IDialogResultable<T>
        {
            T Result { get; set; }

            Action CloseAction { get; set; }
        }

        public static Task<TResult> GetResultAsync<TResult>(this AlertUtil dialog)
        {
            var tcs = new TaskCompletionSource<TResult>();

            try
            {
                if (dialog.IsClosed)
                {
                    SetResult();
                }
                else
                {
                    dialog.Unloaded += OnUnloaded;
                    dialog.GetViewModel<IDialogResultable<TResult>>().CloseAction = dialog.Close;
                }
            }
            catch (Exception e)
            {
                tcs.TrySetException(e);
            }

            return tcs.Task;

            // ReSharper disable once ImplicitlyCapturedClosure
            void OnUnloaded(object sender, RoutedEventArgs args)
            {
                dialog.Unloaded -= OnUnloaded;
                SetResult();
            }

            void SetResult()
            {
                try
                {
                    tcs.TrySetResult(dialog.GetViewModel<IDialogResultable<TResult>>().Result);
                }
                catch (Exception e)
                {
                    tcs.TrySetException(e);
                }
            }
        }

        public static AlertUtil Initialize<TViewModel>(this AlertUtil dialog, Action<TViewModel> configure)
        {
            configure?.Invoke(dialog.GetViewModel<TViewModel>());

            return dialog;
        }

        public static TViewModel GetViewModel<TViewModel>(this AlertUtil dialog)
        {
            if (!(dialog.Content is FrameworkElement frameworkElement))
                throw new InvalidOperationException("The dialog is not a derived class of the FrameworkElement. ");

            if (!(frameworkElement.DataContext is TViewModel viewModel))
                throw new InvalidOperationException($"The view model of the dialog is not the {typeof(TViewModel)} type or its derived class. ");

            return viewModel;
        }
    }
}
