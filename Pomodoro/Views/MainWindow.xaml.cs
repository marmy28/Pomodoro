// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System;
using System.Windows;

namespace Pomodoro.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow() => this.InitializeComponent();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="dataContext"/></exception>
        public MainWindow(ViewModels.MainViewModel dataContext)
            : this()
        {
            this.DataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }
    }
}