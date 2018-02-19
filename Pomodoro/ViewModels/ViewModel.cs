// <copyright file="ViewModel.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pomodoro.ViewModels
{
    /// <summary>
    /// Represents a base view model.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// <seealso cref="Pomodoro.ViewModels.IViewModel" />
    public abstract class ViewModel : INotifyPropertyChanged, IViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        protected ViewModel()
            : base()
        {
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public abstract void UpdateControlsBasedOnLanguage();

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the value changed; otherwise <c>false</c></returns>
        protected static bool SetProperty<T>(ref T member, T value) => SetProperty(ref member, value, EqualityComparer<T>.Default);

        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="member">The member.</param>
        /// <param name="value">The value.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        /// <returns><c>true</c> if the value changed; otherwise <c>false</c></returns>
        /// <exception cref="System.ArgumentNullException">equalityComparer</exception>
        protected static bool SetProperty<T>(ref T member, T value, IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null)
            {
                throw new ArgumentNullException(nameof(equalityComparer));
            }

            if (equalityComparer.Equals(member, value))
            {
                return false;
            }
            else
            {
                member = value;
                return true;
            }
        }

        /// <summary>
        /// Called when a property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}