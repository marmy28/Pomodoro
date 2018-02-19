// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Autofac.Features.Metadata;
using Autofac.Features.OwnedInstances;
using Pomodoro.Commands;
using R = Pomodoro.Localization.My.Resources.Resources;

namespace Pomodoro.ViewModels
{
    /// <summary>
    /// Represents the main view model.
    /// </summary>
    /// <seealso cref="Pomodoro.ViewModels.ViewModel" />
    /// <seealso cref="Pomodoro.ViewModels.IViewModel" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public sealed class MainViewModel : ViewModel, IViewModel, INotifyPropertyChanged
    {
        private const string English = "en-US";
        private const string Italian = "it-IT";
        private readonly IReadOnlyDictionary<ViewModelType, Func<Owned<IViewModel>>> viewModelDictionary;
        private ViewModelType currentViewModelType;
        private Owned<IViewModel> currentOwnedViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="viewModelCollection">The view model collection.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="viewModelCollection"/>
        /// </exception>
        public MainViewModel(IEnumerable<Meta<Func<Owned<IViewModel>>>> viewModelCollection)
            : base()
        {
            if (viewModelCollection == null)
            {
                throw new ArgumentNullException(nameof(viewModelCollection));
            }

            this.viewModelDictionary = viewModelCollection.ToDictionary(i => (ViewModelType)i.Metadata[App.MetaTypeKey], i => i.Value);
            this.SetViewModel(ViewModelType.Preferences);
            this.PomodoroCommand = new RelayCommand(o => this.PomodoroTimerExecute(), o => this.CanPomodoroExecute());
            this.SitStandCommand = new RelayCommand(o => this.SitStandExecute(), o => this.CanSitStandExecute());
            this.CloseCommand = new RelayCommand(o => CloseExecute(o));
            this.ChangeToEnglishCommand = new RelayCommand(o => this.ChangeLanguageExecute(English), o => CanChangeLanguageExecute(English));
            this.ChangeToItalianCommand = new RelayCommand(o => this.ChangeLanguageExecute(Italian), o => CanChangeLanguageExecute(Italian));
            this.PreferencesCommand = new RelayCommand(o => this.PreferencesExecute(), o => this.CanPreferencesExecute());
        }

        /// <summary>
        /// Gets the current view model.
        /// </summary>
        /// <value>
        /// The current view model.
        /// </value>
        public IViewModel CurrentViewModel => this.currentOwnedViewModel.Value;

        /// <summary>
        /// Gets the pomodoro.
        /// </summary>
        /// <value>
        /// The pomodoro.
        /// </value>
        public string Pomodoro => R.Pomodoro;

        /// <summary>
        /// Gets the languages.
        /// </summary>
        /// <value>
        /// The languages.
        /// </value>
        public string Languages => R.Languages;

        /// <summary>
        /// Gets the sit stand.
        /// </summary>
        /// <value>
        /// The sit stand.
        /// </value>
        public string SitStand => R.SitStand;

        /// <summary>
        /// Gets the stop tool tip.
        /// </summary>
        /// <value>
        /// The stop tool tip.
        /// </value>
        public string StopToolTip => R.StopToolTip;

        /// <summary>
        /// Gets the preferences.
        /// </summary>
        /// <value>
        /// The preferences.
        /// </value>
        public string Preferences => R.Preferences;

        /// <summary>
        /// Gets the pomodoro command.
        /// </summary>
        /// <value>
        /// The pomodoro command.
        /// </value>
        public ICommand PomodoroCommand { get; }

        /// <summary>
        /// Gets the sit stand command.
        /// </summary>
        /// <value>
        /// The sit stand command.
        /// </value>
        public ICommand SitStandCommand { get; }

        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>
        /// The close command.
        /// </value>
        public ICommand CloseCommand { get; }

        /// <summary>
        /// Gets the preferences command.
        /// </summary>
        /// <value>
        /// The preferences command.
        /// </value>
        public ICommand PreferencesCommand { get; }

        /// <summary>
        /// Gets the change to english command.
        /// </summary>
        /// <value>
        /// The change to english command.
        /// </value>
        public ICommand ChangeToEnglishCommand { get; }

        /// <summary>
        /// Gets the change to italian command.
        /// </summary>
        /// <value>
        /// The change to italian command.
        /// </value>
        public ICommand ChangeToItalianCommand { get; }

        /// <summary>
        /// Updates the controls based on language.
        /// </summary>
        public override void UpdateControlsBasedOnLanguage()
        {
            this.OnPropertyChanged(nameof(this.Pomodoro));
            this.OnPropertyChanged(nameof(this.Languages));
            this.OnPropertyChanged(nameof(this.SitStand));
            this.OnPropertyChanged(nameof(this.StopToolTip));
            this.OnPropertyChanged(nameof(this.Preferences));
            this.CurrentViewModel.UpdateControlsBasedOnLanguage();
        }

        private static bool CanChangeLanguageExecute(string language) => System.Globalization.CultureInfo.CurrentCulture.Name != language;

        private static void CloseExecute(object obj) => (obj as System.Windows.Window)?.Close();

        private bool CanPomodoroExecute() => this.currentViewModelType != ViewModelType.Pomodoro;

        private void PomodoroTimerExecute() => this.SetViewModel(ViewModelType.Pomodoro);

        private bool CanSitStandExecute() => this.currentViewModelType != ViewModelType.SitStand;

        private void SitStandExecute() => this.SetViewModel(ViewModelType.SitStand);

        private bool CanPreferencesExecute() => this.currentViewModelType != ViewModelType.Preferences;

        private void PreferencesExecute() => this.SetViewModel(ViewModelType.Preferences);

        private void ChangeLanguageExecute(string language)
        {
            var cultureInfo = System.Globalization.CultureInfo.GetCultureInfo(language);
            System.Globalization.CultureInfo.CurrentCulture = cultureInfo;
            System.Globalization.CultureInfo.CurrentUICulture = cultureInfo;
            this.UpdateControlsBasedOnLanguage();
        }

        private void SetViewModel(ViewModelType viewModelType)
        {
            this.currentOwnedViewModel?.Dispose();
            this.currentViewModelType = viewModelType;
            this.currentOwnedViewModel = this.viewModelDictionary[viewModelType]();
            this.OnPropertyChanged(nameof(this.CurrentViewModel));
        }
    }
}