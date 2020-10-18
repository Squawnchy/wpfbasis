using System;
using System.Windows.Input;
using wpfbasis.MVVM;

namespace wpfbasis.Factories
{
	public static class CommandFactory 
	{
		public static ICommand GetCommand(Action action, Func<bool> canExecute = null) =>new ParameterLessCommand(action, canExecute);
		public static ICommand GetCommand<TParameter>(Action<TParameter> action, Func<TParameter, bool> canExecute = null) where TParameter : class => new ParameterCommand<TParameter>(action, canExecute);
	}
}
