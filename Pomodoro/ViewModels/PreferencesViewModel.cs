// <copyright file="PreferencesViewModel.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shell;
using Pomodoro.Commands;
using R = Pomodoro.Localization.My.Resources.Resources;
using S = Pomodoro.Localization.My.MySettings;

namespace Pomodoro.ViewModels
{
    /// <summary>
    /// Represents the preferences view model.
    /// </summary>
    /// <seealso cref="Pomodoro.ViewModels.ViewModel" />
    /// <seealso cref="Pomodoro.ViewModels.IViewModel" />
    public class PreferencesViewModel : ViewModel, IViewModel
    {
        private int previousWorkIntervalInMin;
        private int previousShortBreakIntervalInMin;
        private int previousLongBreakIntervalInMin;
        private int previousSitIntervalInMin;
        private int previousStandIntervalInMin;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreferencesViewModel"/> class.
        /// </summary>
        public PreferencesViewModel()
            : base()
        {
            Application.Current?.Dispatcher?.Invoke(() =>
            {
                var taskbarItemInfo = Application.Current?.MainWindow?.TaskbarItemInfo;
                if (taskbarItemInfo != null)
                {
                    taskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                }
            });
            this.SaveCommand = new RelayCommand(o => this.SaveExecute(), o => this.CanSaveExecute());
            this.SetPreviousValues();
        }

        /// <summary>
        /// Gets or sets the work interval in minutes.
        /// </summary>
        /// <value>
        /// The work interval in minutes.
        /// </value>
        public int WorkIntervalInMin
        {
            get => S.Default.WorkIntervalInMin;
            set
            {
                if (!EqualityComparer<int>.Default.Equals(S.Default.WorkIntervalInMin, value))
                {
                    S.Default.WorkIntervalInMin = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the short break interval in minutes.
        /// </summary>
        /// <value>
        /// The short break interval in minutes.
        /// </value>
        public int ShortBreakIntervalInMin
        {
            get => S.Default.ShortBreakIntervalInMin;
            set
            {
                if (!EqualityComparer<int>.Default.Equals(S.Default.ShortBreakIntervalInMin, value))
                {
                    S.Default.ShortBreakIntervalInMin = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the long break interval in minutes.
        /// </summary>
        /// <value>
        /// The long break interval in minutes.
        /// </value>
        public int LongBreakIntervalInMin
        {
            get => S.Default.LongBreakIntervalInMin;
            set
            {
                if (!EqualityComparer<int>.Default.Equals(S.Default.LongBreakIntervalInMin, value))
                {
                    S.Default.LongBreakIntervalInMin = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the sit interval in minutes.
        /// </summary>
        /// <value>
        /// The sit interval in minutes.
        /// </value>
        public int SitIntervalInMin
        {
            get => S.Default.SitIntervalInMin;
            set
            {
                if (!EqualityComparer<int>.Default.Equals(S.Default.SitIntervalInMin, value))
                {
                    S.Default.SitIntervalInMin = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the stand interval in minutes.
        /// </summary>
        /// <value>
        /// The stand interval in minutes.
        /// </value>
        public int StandIntervalInMin
        {
            get => S.Default.StandIntervalInMin;
            set
            {
                if (!EqualityComparer<int>.Default.Equals(S.Default.StandIntervalInMin, value))
                {
                    S.Default.StandIntervalInMin = value;
                    this.OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets the long break interval text.
        /// </summary>
        /// <value>
        /// The long break interval text.
        /// </value>
        public string LongBreakInterval => R.LongBreakIntervalInMin;

        /// <summary>
        /// Gets the short break interval text.
        /// </summary>
        /// <value>
        /// The short break interval text.
        /// </value>
        public string ShortBreakInterval => R.ShortBreakIntervalInMin;

        /// <summary>
        /// Gets the sit interval text.
        /// </summary>
        /// <value>
        /// The sit interval text.
        /// </value>
        public string SitInterval => R.SitIntervalInMin;

        /// <summary>
        /// Gets the stand interval text.
        /// </summary>
        /// <value>
        /// The stand interval text.
        /// </value>
        public string StandInterval => R.StandIntervalInMin;

        /// <summary>
        /// Gets the work interval text.
        /// </summary>
        /// <value>
        /// The work interval text.
        /// </value>
        public string WorkInterval => R.WorkIntervalInMin;

        /// <summary>
        /// Gets the save tooltip.
        /// </summary>
        /// <value>
        /// The save tooltip.
        /// </value>
        public string Save => R.Save;

        /// <summary>
        /// Gets the save command.
        /// </summary>
        /// <value>
        /// The save command.
        /// </value>
        public ICommand SaveCommand { get; }

        /// <inheritdoc/>
        public override void UpdateControlsBasedOnLanguage()
        {
            this.OnPropertyChanged(nameof(this.LongBreakInterval));
            this.OnPropertyChanged(nameof(this.ShortBreakInterval));
            this.OnPropertyChanged(nameof(this.SitInterval));
            this.OnPropertyChanged(nameof(this.StandInterval));
            this.OnPropertyChanged(nameof(this.WorkInterval));
            this.OnPropertyChanged(nameof(this.Save));
        }

        private void SetPreviousValues()
        {
            this.previousLongBreakIntervalInMin = this.LongBreakIntervalInMin;
            this.previousShortBreakIntervalInMin = this.ShortBreakIntervalInMin;
            this.previousSitIntervalInMin = this.SitIntervalInMin;
            this.previousStandIntervalInMin = this.StandIntervalInMin;
            this.previousWorkIntervalInMin = this.WorkIntervalInMin;
        }

        private bool CanSaveExecute()
        {
            if (this.IsValueLessThanZero().Any(i => i))
            {
                return false;
            }

            return this.IsPreviousDifferent().Any(i => i);
        }

        private IEnumerable<bool> IsValueLessThanZero()
        {
            yield return this.LongBreakIntervalInMin < 0;
            yield return this.ShortBreakIntervalInMin < 0;
            yield return this.SitIntervalInMin < 0;
            yield return this.StandIntervalInMin < 0;
            yield return this.WorkIntervalInMin < 0;
        }

        private IEnumerable<bool> IsPreviousDifferent()
        {
            yield return !EqualityComparer<int>.Default.Equals(this.previousLongBreakIntervalInMin, this.LongBreakIntervalInMin);
            yield return !EqualityComparer<int>.Default.Equals(this.previousShortBreakIntervalInMin, this.ShortBreakIntervalInMin);
            yield return !EqualityComparer<int>.Default.Equals(this.previousSitIntervalInMin, this.SitIntervalInMin);
            yield return !EqualityComparer<int>.Default.Equals(this.previousStandIntervalInMin, this.StandIntervalInMin);
            yield return !EqualityComparer<int>.Default.Equals(this.previousWorkIntervalInMin, this.WorkIntervalInMin);
        }

        private void SaveExecute()
        {
            S.Default.Save();
            this.SetPreviousValues();
        }
    }
}