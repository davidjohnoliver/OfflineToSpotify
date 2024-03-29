﻿#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OfflineToSpotify.Presentation
{
	/// <summary>
	/// Simple <see cref="ICommand"/> implementation which takes a synchronous type-checked delegate and exposes a boolean flag for CanExecute.
	/// </summary>
	public class SimpleCommand<T> : ICommand where T : class
	{
		private readonly Action<T?> _execute;
		private bool _canExecute = true;

		public SimpleCommand(Action<T?> execute)
		{
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
		}

		public bool CanExecute
		{
			get => _canExecute;
			set
			{
				var wasChanged = _canExecute != value;
				_canExecute = value;
				if (wasChanged)
				{
					CanExecuteChanged?.Invoke(this, new EventArgs());
				}
			}
		}

		public event EventHandler? CanExecuteChanged;

		bool ICommand.CanExecute(object? parameter) => CanExecute;

		void ICommand.Execute(object? parameter)
		{
			if (parameter is T t)
			{
				_execute(t);
			}
			else if (parameter == null)
			{
				_execute(null);
			}
		}
	}

	public static class SimpleCommand
	{

		public static SimpleCommand<T> Create<T>(Action<T?> execute) where T : class => new SimpleCommand<T>(execute);

		public static SimpleCommand<object> Create(Action execute)
		{
			if (execute is null)
			{
				throw new ArgumentNullException(nameof(execute));
			}

			return new SimpleCommand<object>(ExecuteWrapper);

			void ExecuteWrapper(object? _)
			{
				execute();
			}
		}
	}
}
