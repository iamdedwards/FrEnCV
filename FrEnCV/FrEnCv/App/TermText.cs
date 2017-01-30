using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrEnCV
{
    public class TermText
    {
		public int		OutPutCount { get; set; }
		private int?	_linesLength;
		public int		LinesLength
		{
			get
			{
				if (_linesLength == null)
				{
					var len = Lines.Select((l) => { return l.Length; }).Sum();
					int replaceLine = 0;
					if (Replace != null && Replace.En.Count() > 0)
					{ 
					   replaceLine = Replace.En.Select((l) => { return l.Length; }).Sum();
					}
					_linesLength = len + replaceLine;
				}
				return _linesLength.Value;
			}
		}
		public List<string>			Lines { get ; set; }
		public Action				OnSuccess { get; set; }
		private FrEn<List<string>>	_replace;
		public FrEn<List<string>>	Replace
		{
			get
			{
				return (_replace);
			}
			set
			{
				_replace = value;
				if (ResourceController.Instance.Data.IsFrench)
					_replace.UseFrench();
				else
					_replace.UseEnglish();
			}
        }
    }
}