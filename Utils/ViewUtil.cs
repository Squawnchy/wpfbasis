using System.Collections.Generic;
using System.Windows.Controls;

namespace wpfbasis.Utils
{
	public static class ViewUtil
	{
		static ViewUtil()
		{
			string.Intern("Model");
		}

		public static void LocateViewModel(this ContentControl view, IEnumerable<object> objectContainer)
		{
			foreach (var obj in objectContainer)
			{
				var objClassName = obj.GetType().Name;
				if (objClassName?.Equals(objClassName + "Model") == true)
					view.DataContext = obj;
			}
		}
	}
}
