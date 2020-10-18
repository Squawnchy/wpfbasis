namespace wpfbasis.Enums.MVVM
{
	/// <summary>
	/// <para>Modes for PropertyChanged Invokation</para>
	/// <para>Default = Invoke</para>
	/// <para>Invoke = Just invokes the property changed event</para>
	/// <para>Safe = Making sure we are on the dispatcher thread (ToDo)</para>
	/// </summary>
	public enum PropertyChangedInvokeMode
	{
		Default,
		Invoke
	}
}
