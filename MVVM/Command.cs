using System;
using System.Windows.Input;

namespace wpfbasis.MVVM
{
	public abstract class Command : ICommand
	{
		Action<object> Execute;
		Func<object, bool> CanExecute;

		public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
      }
      remove
      {
        CommandManager.RequerySuggested -= value;
      }
    }


    bool ICommand.CanExecute(object parameter) => CanExecute?.Invoke(parameter) ?? true;
		void ICommand.Execute(object parameter) => Execute?.Invoke(parameter);

		public Command(Action<object> execute, Func<object, bool> canExecute = null)
		{
			Execute = execute;
			CanExecute = canExecute;
		}
	}

  public class ParameterCommand<T> : Command where T : class
	{
		public ParameterCommand(Action<T> execute, Func<T, bool> canExecute = null) : base((Action<object>)execute, (Func<object, bool>)canExecute)
		{
		}
	}

	public class ParameterLessCommand : Command 
	{
		public ParameterLessCommand(Action execute, Func<bool> canExecute = null) : base((o) => execute?.Invoke(), (o) => canExecute?.Invoke() ?? true)
		{
		}
	}
}
