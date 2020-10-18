using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wpfbasis.MVVM;

namespace wpfbasis.Manager
{
	public class PropertyChangedEventManager
	{
		#region ManagerBuffer
		static HybridDictionary _managerBuffer;
		public static HybridDictionary ManagerBuffer { get; } = _managerBuffer ??= new HybridDictionary();
		#endregion


		#region PropertyChangedManagement
		protected ObservableCore Observable { get; set; }
		#endregion



		public static PropertyChangedEventManager GetManager(ObservableCore observableCore)
		{
			foreach (var key in ManagerBuffer?.Keys)
				if (key?.Equals(observableCore) == true)
					return ManagerBuffer[key] as PropertyChangedEventManager;

			var manager = new PropertyChangedEventManager()
			{
				Observable = observableCore,
			};

			_managerBuffer?.Add(observableCore, manager);

			return manager;
		}


		public void BindToProperty(INotifyPropertyChanged propertyChangedSender, string senderPropertyName, string propertyName)
		{
			senderPropertyName = string.Intern(senderPropertyName);
			propertyName = string.Intern(propertyName);

			propertyChangedSender.PropertyChanged += BindingPropertyChanged;

			void BindingPropertyChanged(object sender, PropertyChangedEventArgs e)
			{
				if (e.PropertyName == senderPropertyName)
					EmitPropertyChangedEventIntern(Observable, propertyName);
			}
		}



		#region CorePropertyChangedMethod
		void EmitPropertyChangedEventIntern(object sender, string senderPropertyName)
		{
			Observable?.InvokePropertyChangedEvent(sender, senderPropertyName);
		}
		#endregion
	}
}