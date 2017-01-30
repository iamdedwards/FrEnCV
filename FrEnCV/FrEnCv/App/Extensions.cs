using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FrEnCV
{
	public static class FrameworkElementHelperMethods
	{
		public static void Hide(this FrameworkElement frwk)
		{
			frwk.Visibility = Visibility.Hidden;
		}
		public static void Show(this FrameworkElement frwk)
		{
				
			frwk.Visibility = Visibility.Visible;
		}

		public static bool IsHidden(this FrameworkElement frwk)
		{
			return frwk.Visibility == Visibility.Hidden;
		}
	}

	public static class IEnumerableHelperMethods
	{
		public static string Concat(this IEnumerable<string> strings)
		{
			return (strings.Aggregate("", (absorb, next) =>{
				return (absorb + next);
			}));
		}
	}
}
