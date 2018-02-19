// <copyright file="TimerViewModel.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Pomodoro.Commands;
using static Pomodoro.Models.Interval;
using R = Pomodoro.Localization.My.Resources.Resources;

namespace Pomodoro.ViewModels
{
    /// <summary>
    /// Represents the view model that can use time.
    /// </summary>
    /// <seealso cref="Pomodoro.ViewModels.ViewModel" />
    /// <seealso cref="Pomodoro.ViewModels.IViewModel" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="System.IDisposable" />
    public class TimerViewModel : ViewModel, IViewModel, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// The constructor interval collection parameter name. Helps with reference.
        /// </summary>
        public const string ConstructorIntervalCollectionName = "intervalCollection";
        private readonly Timer timer;
        private readonly Queue<IInterval> intervalQueue;
        private bool disposedValue = false;
        private int timeSpent;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimerViewModel"/> class.
        /// </summary>
        /// <param name="intervalCollection">The interval collection.</param>
        /// <exception cref="System.ArgumentNullException">intervalCollection</exception>
        public TimerViewModel(IEnumerable<IInterval> intervalCollection)
            : this()
        {
            if (intervalCollection == null)
            {
                throw new ArgumentNullException(nameof(intervalCollection));
            }

            this.intervalQueue = new Queue<IInterval>(intervalCollection);
            this.timer = new Timer(Models.TimeMeasure.MillisecondPerSecond * Models.TimeMeasure.SecondPerDecasecond);
            this.timer.Elapsed += this.Timer_Elapsed;
            this.timer.AutoReset = true;
            this.timer.Enabled = true;
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                var taskbarItemInfo = Application.Current?.MainWindow?.TaskbarItemInfo;
                if (taskbarItemInfo != null)
                {
                    taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                }
            });
        }

        private TimerViewModel()
            : base()
        {
            this.PauseCommand = new RelayCommand(o => this.PauseExecute(), o => this.CanPauseExecute());
            this.PlayCommand = new RelayCommand(o => this.PlayExecute(), o => this.CanPlayExecute());
            this.SkipForwardCommand = new RelayCommand(o => this.SkipForwardExecute());
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TimerViewModel"/> class.
        /// </summary>
        ~TimerViewModel()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message => this.CurrentInterval.Message;

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public int Duration => this.CurrentInterval.Duration;

        /// <summary>
        /// Gets the time spent.
        /// </summary>
        /// <value>
        /// The time spent.
        /// </value>
        public int TimeSpent
        {
            get => this.timeSpent;
            private set
            {
                if (SetProperty(ref this.timeSpent, value))
                {
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the play.
        /// </summary>
        /// <value>
        /// The play.
        /// </value>
        public string Play => R.Play;

        /// <summary>
        /// Gets the pause.
        /// </summary>
        /// <value>
        /// The pause.
        /// </value>
        public string Pause => R.Pause;

        /// <summary>
        /// Gets the skip forward.
        /// </summary>
        /// <value>
        /// The skip forward.
        /// </value>
        public string SkipForward => R.SkipFoward;

        /// <summary>
        /// Gets the pause command.
        /// </summary>
        /// <value>
        /// The pause command.
        /// </value>
        public ICommand PauseCommand { get; }

        /// <summary>
        /// Gets the play command.
        /// </summary>
        /// <value>
        /// The play command.
        /// </value>
        public ICommand PlayCommand { get; }

        /// <summary>
        /// Gets the skip forward command.
        /// </summary>
        /// <value>
        /// The skip forward command.
        /// </value>
        public ICommand SkipForwardCommand { get; }

        private IInterval CurrentInterval => this.intervalQueue.Peek();

        /// <inheritdoc/>
        public override void UpdateControlsBasedOnLanguage()
        {
            this.OnPropertyChanged(nameof(this.Message));
            this.OnPropertyChanged(nameof(this.Play));
            this.OnPropertyChanged(nameof(this.Pause));
            this.OnPropertyChanged(nameof(this.SkipForward));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.timer.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private static void EnqueueTheDequeue(Queue<IInterval> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            queue.Enqueue(queue.Dequeue());
        }

        private static void ChangeWindowState(IInterval interval)
        {
            var result = WindowState.Normal;
            if (interval.MinimizeWindow)
            {
                result = WindowState.Minimized;
            }

            Application.Current?.Dispatcher?.Invoke(() =>
            {
                var mainWindow = Application.Current?.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.WindowState = result;
                }
            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.timeSpent < (this.CurrentInterval.Duration - 1))
            {
                this.TimeSpent++;
            }
            else
            {
                this.NextInterval();
            }
        }

        private void NextInterval()
        {
            this.TimeSpent = 0;
            EnqueueTheDequeue(this.intervalQueue);
            ChangeWindowState(this.CurrentInterval);
            this.OnPropertyChanged(nameof(this.Duration));
            this.OnPropertyChanged(nameof(this.Message));
        }

        private bool CanPauseExecute() => this.timer.Enabled;

        private void PauseExecute()
        {
            this.timer.Stop();
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                var taskbarItemInfo = Application.Current?.MainWindow?.TaskbarItemInfo;
                if (taskbarItemInfo != null)
                {
                    taskbarItemInfo.ProgressState = TaskbarItemProgressState.Paused;
                }
            });
        }

        private bool CanPlayExecute() => !this.timer.Enabled;

        private void PlayExecute()
        {
            this.timer.Start();
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                var taskbarItemInfo = Application.Current?.MainWindow?.TaskbarItemInfo;
                if (taskbarItemInfo != null)
                {
                    taskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                }
            });
        }

        private void SkipForwardExecute() => this.NextInterval();
    }
}