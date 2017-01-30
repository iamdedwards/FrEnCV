using System;

namespace FrEnCV
{
	public class FrEn<T> where T : class, new()
	{
		public T Fr {get; set;}
		public T En {get; set;}
		public Func<T> Current {get;set; }

		public FrEn()
		{
			Fr = new T();
			En = new T();
			Current = () => {return En; };
		}

		public FrEn(Func<T> setFr, Func<T> setEn)
		{
			Fr = setFr();
			En = setEn();
			Current = () => {return En; };
		}

		public void SwitchLanguage()
		{
			if (IsFrench)
				UseEnglish();
			else
				UseFrench();
		}

		public bool IsFrench
		{
			get { return (Current() == Fr); }
		}

		public bool IsEnglish
		{
			get {  return (Current() == En); }
		}

		public void UseFrench()
		{
			Current = () => { return Fr; };
		}

		public void UseEnglish()
		{
			Current = () => { return En; };
		}
	}
}
