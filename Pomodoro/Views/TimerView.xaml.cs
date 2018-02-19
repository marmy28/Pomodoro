// <copyright file="TimerView.xaml.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System.Windows;
using System.Windows.Controls;

namespace Pomodoro.Views
{
    /// <summary>
    /// Interaction logic for TimerView.xaml
    /// </summary>
    public partial class TimerView : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimerView"/> class.
        /// </summary>
        public TimerView() => this.InitializeComponent();

        private void ProgressBar_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            var pg = (ProgressBar)sender;
            var percent = pg.Value / pg.Maximum;
            Application.Current.Dispatcher.Invoke(() => Application.Current.MainWindow.TaskbarItemInfo.ProgressValue = percent);
            this.txTimer.Text = $"{percent:P1}";
        }
    }
}