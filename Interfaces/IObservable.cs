using System.ComponentModel;
using wpfbasis.Enums.MVVM;

namespace wpfbasis.Interfaces
{
	public interface IObservable : INotifyPropertyChanged
	{
		void OnPropertyChanged(object sender = null, string propertyName = "", PropertyChangedInvokeMode invokeMode = PropertyChangedInvokeMode.Default);
	}
}
