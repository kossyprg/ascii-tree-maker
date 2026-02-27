using AsciiTreeMaker.UI.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace AsciiTreeMaker.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            StatusRow.Height = new GridLength(0);
#if DEBUG
            StatusRow.Height = new GridLength(25);
#endif
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                if (!vm.ConfirmClose())
                {
                    e.Cancel = true;
                }

                if (!e.Cancel)
                {
                    vm.SaveUserPreferences();
                }
            }
        }
    }
}
