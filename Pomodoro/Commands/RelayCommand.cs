// <copyright file="RelayCommand.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Pomodoro.Commands
{
    /// <inheritdoc/>
    public sealed class RelayCommand : ICommand
    {
        private readonly Predicate<object> canExecute;
        private readonly Action<object> execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="execute" />
        /// </exception>
        public RelayCommand(Action<object> execute)
            : this(execute, o => true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="canExecute" />
        /// or
        /// <paramref name="execute" />
        /// </exception>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
            : base()
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
        }

        /// <inheritdoc/>
        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <inheritdoc/>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) => this.canExecute.Invoke(parameter);

        /// <inheritdoc/>
        public void Execute(object parameter) => this.execute.Invoke(parameter);
    }
}