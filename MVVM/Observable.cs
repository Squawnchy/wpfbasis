using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using wpfbasis.Enums.MVVM;

namespace wpfbasis.MVVM
{
	public abstract class ObservableCore : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		internal virtual void InvokePropertyChangedEvent(object sender, string name)
		{
			PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(name));
		}
	}

	public abstract class Observable : ObservableCore
	{
		#region fields
		Hashtable _fieldHashTable;
		#endregion



		#region properties
		protected PropertyChangedInvokeMode DefaultPropertyChangedInvokeMode { get; set; } = PropertyChangedInvokeMode.Invoke;

		protected Hashtable MemberHashtable => _fieldHashTable ??= new Hashtable();

		Manager.PropertyChangedEventManager Manager => wpfbasis.Manager.PropertyChangedEventManager.GetManager(this);
		#endregion



		#region ctor
		public Observable()
		{
		}
		#endregion



		#region methods		
		protected virtual object Get([CallerMemberName] string propertyName = null)
		{
			return GetFieldValueFromHashTable(propertyName);
		}


		protected virtual string Set<PropertyType>(PropertyType value, [CallerMemberName] string propertyName = null)
		{
			var key = propertyName;
			if (MemberHashtable?.ContainsKey(key) == true)
				SetMember(value, key);
			else
				MemberHashtable?.Add(key, value);
			OnPropertyChanged(null, propertyName);
			return key;
		}


		protected virtual void SetMember<PropertyType>(PropertyType value, string fieldName)
		{
			MemberHashtable[fieldName] = value;
		}


		/// <summary>
		/// <para>Sets the field of the property. this method simply enables you to write properties using the propertychanged-events
		/// using lambdas only</para>
		/// <para>Usage: </para>
		/// <para>int _integerField;</para>
		/// <para>public int IntegerProperty { get => _integerField; set => Set(ref _integerField, value); }</para>
		/// </summary>
		/// <typeparam name="PropertyType"></typeparam>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="sender"><para>sets the sender of the <see cref="PropertyChanged"/> Event 
		/// (e.g.: the WPF User Interface needs to know which ViewModel sends Information)</para>
		/// <para>the sender paramter is per default automatically set to the calling object</para>
		/// <param name="propertyName"><para>sets the name of the Property that has been changed</para>
		/// <para>its recommended to use this parameter in three ways:</para>
		/// <para>leave blank (null): this will automatically set the parameter to the calling Properties name</para>
		/// <para>set speific property name using <see cref="nameof"/> </para></param>
		/// <para>set to <see cref="string.Empty"/> to update all Properties</para></param>
		/// <param name="invokeMode">Mode to use when the event is invoked</param>
		protected virtual void Set<PropertyType>(
			ref PropertyType field,
			PropertyType value,
			object sender = null,
			[CallerMemberName] string propertyName = null,
			PropertyChangedInvokeMode invokeMode = PropertyChangedInvokeMode.Default)
		{
			field = value;
			AutoFillNulledParams(ref sender, ref propertyName);
			InvokePropertyChangedEventIntern(sender, propertyName, invokeMode);
		}


		/// <summary>
		/// <para>OnPropertyChanged</para>
		/// <para>Invokes the <see cref="PropertyChanged"/> Event (with ?.Invoke() Nullcheck)
		/// and autmatically sets specific required arguments which are explained in the parameters
		/// descriptions.</para>
		/// </summary>
		/// <param name="sender"><para>sets the sender of the <see cref="PropertyChanged"/> Event 
		/// (e.g.: the WPF User Interface needs to know which ViewModel sends Information)</para>
		/// <para>the sender paramter is per default automatically set to the calling object</para>
		/// <param name="propertyName"><para>sets the name of the Property that has been changed</para>
		/// <para>its recommended to use this parameter in three ways:</para>
		/// <para>leave blank (null): this will automatically set the parameter to the calling Properties name</para>
		/// <para>set specific property name using <see cref="nameof"/> </para></param>
		/// <para>set to <see cref="string.Empty"/> to update all Properties</para></param>
		/// <param name="invokeMode">Mode to use when the event is invoked</param>
		protected virtual void OnPropertyChanged(
			object sender = null,
			[CallerMemberName] string propertyName = null,
			PropertyChangedInvokeMode invokeMode = PropertyChangedInvokeMode.Default)
		{
			AutoFillNulledParams(ref sender, ref propertyName);
			InvokePropertyChangedEventIntern(sender, propertyName, invokeMode);
		}

		protected virtual void Bind(INotifyPropertyChanged observable, string senderPropertyName, string propertyName, PropertyChangedInvokeMode invokeMode = PropertyChangedInvokeMode.Default)
		{
			Manager?.BindToProperty(observable, senderPropertyName, propertyName);
		}

		EventHandler<PropertyChangedEventArgs> GetPropertyChangedEventHandler(string propertyName, PropertyChangedInvokeMode invokeMode)
		{
			return new EventHandler<PropertyChangedEventArgs>((s, a) => BindingHandler(propertyName, invokeMode));
		}

		void BindingHandler(string propertyName, PropertyChangedInvokeMode invokeMode) => OnPropertyChanged(this, propertyName, invokeMode);


		// searches the value of an property in the hashtable by their name
		object GetFieldValueFromHashTable(string propertyName)
		{
			return MemberHashtable[propertyName];
		}

		// check if propertychanged parameters are null or string.Empty
		void AutoFillNulledParams(ref object sender, ref string propertyName)
		{
			if (sender == null)
				sender = this;
			if (string.IsNullOrEmpty(propertyName))
				propertyName = string.Empty;
		}

		// specification for InvokePropertyChanged()
		void InvokePropertyChangedEventIntern(object sender, string name, PropertyChangedInvokeMode invokeMode)
		{
			switch (invokeMode)
			{
				case PropertyChangedInvokeMode.Default:
				case PropertyChangedInvokeMode.Invoke:
					InvokePropertyChangedEvent(sender, name);
					break;
			}
		}		
		#endregion
	}
}
