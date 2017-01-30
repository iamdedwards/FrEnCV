using System;
using System.Collections.Generic;
using System.Reflection;

namespace FrEnCV
{
	public class CollectionViewHiddenAttribute : Attribute { }

	public static class PropertyExplorer
        {
            public static object Value(object target, PropertyInfo p)
            {
                return target.GetType().GetProperty(p.Name).GetValue(target);
            }

            public static string Name(PropertyInfo p, string lang)
            {
                var dispNameAttr = Attribute.GetCustomAttribute(p, typeof(FrenchNameAttribute)) as FrenchNameAttribute;
                if (dispNameAttr != null && lang == "Fr")
                {
                    return (dispNameAttr.Name);
                }
                return (p.Name);
            }

			public static bool CollectionViewHidden(PropertyInfo p)
			{
				var attr = Attribute.GetCustomAttribute(p, typeof(CollectionViewHiddenAttribute)) as CollectionViewHiddenAttribute;
				return (attr != null);
			}
        }

    public class CollectionView : List<Dictionary<string, object>>
	{
		public static CollectionView For<T>(string language, List<T> data)
        {
            var dictData = new CollectionView();
            var props = typeof(T).GetProperties();
            foreach (var t in data)
            {
                var dict = new Dictionary<string, object>();
                foreach (var p in props)
                {
					if (!PropertyExplorer.CollectionViewHidden(p))
						dict.Add(PropertyExplorer.Name(p, language), PropertyExplorer.Value(t, p));
                }
                dictData.Add(dict);
            
            }
			return (dictData);
		}
	}

}
