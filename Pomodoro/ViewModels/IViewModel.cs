// <copyright file="IViewModel.cs" company="PlaceholderCompany">
// Copyright (c) 2018. All rights reserved.
// </copyright>

using System.ComponentModel;

namespace Pomodoro.ViewModels
{
    /// <summary>
    /// Represents a view model
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Occurs when a property changes.
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Updates the controls based on language.
        /// </summary>
        void UpdateControlsBasedOnLanguage();
    }
}