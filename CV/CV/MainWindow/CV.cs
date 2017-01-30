using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace FrEnCV
{
	public class FrenchNameAttribute : Attribute
    {
        public string Name { get; set; }

        public FrenchNameAttribute(string name)
        {
            Name = name;
        }
    }

    public class Experience
    {
		public Experience() { }

		[FrenchName("Depuis")]
		public int          From                        { get; set; }
		[FrenchName("Jusqu'a")]
		public int          To                          { get; set; }
		[FrenchName("Employeur")]
		public string       Employer                    { get; set; }
		[FrenchName("Poste")]
		public string       Position                    { get; set; }

		public Experience(string emplyr, int from, int to, string pos)
		{
			Employer = emplyr;
			From = from;
			To = to;
			Position = pos;
		}
		public Experience Clone(Action<Experience> set)
        {
            var dest = new Experience(this.Employer, this.From, this.To, this.Position);
            set(dest);
            return (dest);
        }
    }

	public class Formation
	{
		[FrenchName("Depuis")]
		public int			From                      { get; set; }
		[FrenchName("Jusqu'a")]
		public int			To                        { get; set; }
		[FrenchName("Intitulé")]
		public string		Title                     { get; set; }
		[FrenchName("Etablissment")]
		public string		Establishment             { get; set; }

		[CollectionViewHidden]
		public IEnumerable<string> Modules {get; set; }

		public Formation() { }

		public Formation(string title, int enom, int to, params string[] modules)
		{
			Modules = modules;
		}

		public Formation Clone(Action<Formation> set)
        {
            var dest = new Formation(Title, From, To, Modules.ToArray());
            set(dest);
            return (dest);
        }
    }
}

namespace FrEnCV
{ 
	public class CV
	{
        public int						StartMonth		{ get; set; }
        public int						DurationMonths	{ get; set; }

		public FrEn<List<Experience>>	Experiences		{ get; set; }
		public FrEn<List<Formation>>	Formations		{ get; set; }
		public FrEn<List<string>>		Presentation	{ get; set; }
		public string					TelephoneNo		{ get; set; }
        public string					Email			{ get; set; }

		public List<string> setPresentation(string lang)
		{
			var xdoc = (XDocument.Load($"../../Xml/Presentation.{lang}.Xml"));
			var rootNodes = xdoc.Root;
			var xelem = rootNodes as XElement;
			var val = xelem.Value;
			var split = val.Split('\n');
			return (split.ToList());
		}      

		public CV()
		{
			Experiences = new FrEn<List<Experience>>();
			Formations = new FrEn<List<Formation>>();
			Presentation = new FrEn<List<string>>();
			Experiences.Fr = setData<Experience>("Fr");
			Experiences.En = setData<Experience>("En");
			Formations.En = setData<Formation>("En");
			Formations.Fr = setData<Formation>("Fr");
			Presentation.En = setPresentation("En");
			Presentation.Fr = setPresentation("Fr");
			StartMonth = 3;
			DurationMonths = 6;
            TelephoneNo = "06 95 35 67 10";
            Email = "iamdedwards@gmail.com";
		}

		public List<T> setData<T>(string lang) where T : class, new()
		{
			var xDoc = XDocument.Load($"../../Xml/{typeof(T).Name}s.{lang}.xml");
			var props = typeof(T).GetProperties();
			return parseXmlData<T>(xDoc.Root.FirstNode as XElement, 
				x => x.Name == typeof(T).Name,
				(obj, list) => createObjects<T>(obj, props, list));
		}

		private void createObjects<T>(XElement obj, PropertyInfo[] props, List<T> list) where T : class, new()
		{
				var t = new T();
				foreach (var n in obj.Nodes())
				{
					var elem  = (XElement)n;
					var prop = props.Single((p) => {return p.Name == elem.Name;});
					if (prop.PropertyType == typeof(string))
						prop.SetValue(t, elem.Value);
					else if (prop.PropertyType == typeof(int))
					{
						try		{ prop.SetValue(t, int.Parse(elem.Value)); }
						catch
						{
							Debug.WriteLine($"{t.GetType().ToString()}.{prop.Name} should be parseable as an int");
							throw;
						}
					}
					else if (typeof(IEnumerable<string>).IsAssignableFrom(prop.PropertyType))
					{
						var adds = new List<string>();
						foreach (var s in elem.Nodes())
						{
							adds.Add(((XElement)s).Value);
						}
						prop.SetValue(t, adds);
					}
				}
				list.Add(t);
		}

		public List<T> parseXmlData<T>(XElement xelem, Expression<Func<XElement, bool>> cond, Action<XElement, List<T>> objConstr, List<T> result = null)
		{ 
			result = result ?? new List<T>();
			if (cond.Compile()(xelem))
			{
				objConstr(xelem, result);
				xelem = xelem.NextNode as XElement;
				if (xelem == null)
					return (result);
				else
					return (parseXmlData(xelem, cond, objConstr, result));
			}
			return (result);
		}
	}
}
