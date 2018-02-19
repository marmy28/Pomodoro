// <copyright file="App.xaml.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System.Windows;
using Autofac;
using static Autofac.RegistrationExtensions;
using static Autofac.ResolutionExtensions;

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The meta type key. Used to determine which view model you have.
        /// </summary>
        public const string MetaTypeKey = "Type";
        private ILifetimeScope scope;

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.scope = BuildContainer().BeginLifetimeScope();
            this.MainWindow = this.scope.Resolve<Views.MainWindow>();
            this.MainWindow.Show();

            IContainer BuildContainer()
            {
                var containerBuilder = new Autofac.ContainerBuilder();
                containerBuilder.RegisterType<ViewModels.MainViewModel>();
                containerBuilder.RegisterType<ViewModels.TimerViewModel>().WithParameter(new NamedParameter(ViewModels.TimerViewModel.ConstructorIntervalCollectionName, Models.Interval.PomodoroIntervalCollection)).WithMetadata(MetaTypeKey, ViewModels.ViewModelType.Pomodoro).As<ViewModels.IViewModel>();
                containerBuilder.RegisterType<ViewModels.TimerViewModel>().WithParameter(new NamedParameter(ViewModels.TimerViewModel.ConstructorIntervalCollectionName, Models.Interval.SitStandIntervalCollection)).WithMetadata(MetaTypeKey, ViewModels.ViewModelType.SitStand).As<ViewModels.IViewModel>();
                containerBuilder.RegisterType<ViewModels.PreferencesViewModel>().AsSelf().WithMetadata(MetaTypeKey, ViewModels.ViewModelType.Preferences).As<ViewModels.IViewModel>();
                containerBuilder.RegisterType<Views.MainWindow>();
                return containerBuilder.Build();
            }
        }

        /// <inheritdoc/>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.scope.Dispose();
        }
    }
}