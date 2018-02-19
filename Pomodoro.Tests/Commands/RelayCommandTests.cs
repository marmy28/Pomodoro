using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pomodoro.Commands;
namespace Pomodoro.Tests.Commands
{
    [TestClass]
    public class RelayCommandTests
    {
        [TestMethod]
        public void Constructor_NullArgs_ArgumentNullException()
        {
            Predicate<object> canExecute = (o => true);
            Action<object> execute = (o => { });
            new Action(() => new RelayCommand(null, null)).ShouldThrow<ArgumentNullException>();
            new Action(() => new RelayCommand(execute, null)).ShouldThrow<ArgumentNullException>();
            new Action(() => new RelayCommand(null, canExecute)).ShouldThrow<ArgumentNullException>();
            new Action(() => new RelayCommand(null)).ShouldThrow<ArgumentNullException>();
        }
        [TestMethod]
        public void SingleArgumentConstructor_CanExecute_ReturnsTrueAlways()
        {
            Action<object> execute = (o => { });
            var target = new RelayCommand(execute);
            target.CanExecute(null).Should().BeTrue();
            target.CanExecute(true).Should().BeTrue();
            target.CanExecute(false).Should().BeTrue();
        }
        [TestMethod]
        public void CanExecute_ReturnsFalse_ReturnsFalseAlways()
        {
            Predicate<object> canExecute = (o => false);
            Action<object> execute = (o => { });
            var target = new RelayCommand(execute, canExecute);
            target.CanExecute(null).Should().BeFalse();
            target.CanExecute(true).Should().BeFalse();
            target.CanExecute(false).Should().BeFalse();
        }
        [TestMethod]
        public void Execute_IncreasesCount_GetsCalled()
        {
            int counter = 0;
            Action<object> execute = (o => { ++counter; });
            var target = new RelayCommand(execute);
            counter.Should().Be(0);
            target.Execute(null);
            counter.Should().Be(1);
            target.Execute(true);
            counter.Should().Be(2);
            target.Execute(false);
            counter.Should().Be(3);
        }
    }
}
