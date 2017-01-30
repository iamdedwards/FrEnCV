using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FrEnCV
{
	public partial class MainWindow
	{
		/*Translation Helper Methods */
		private Dictionary<string, string> _translate;
		public Dictionary<string, string> Translate
		{
			get { return _translate = _translate ?? new Dictionary<string, string>(); }
		}

		private void setText(FrameworkElement elem, Action<FrameworkElement, string> set, string En, string Fr)
		{
			set(elem, En);
			Translate[En] = Fr;
			Translate[Fr] = En;
		}

		private void setTextBlock(TextBlock block, string en, string fr)
		{
			setText(block, (elem, s) => {((TextBlock)elem).Text = s; }, en, fr);
		}

		private void setCheckBox(CheckBox box, string en, string fr)
		{
			setText(box, (elem, s) => {((CheckBox)elem).Content = s;}, en, fr);
		}

		private void translateTextBlocks(params TextBlock[] elems)
		{
			foreach(var f in elems)
				f.Text = Translate[f.Text];
		}

		private void changeUIElementsLanguage(string lang)
		{
			ResourceControl.Data.SwitchLanguage();
			Term.SwitchLanguage();
			AvailableFrom.Language = System.Windows.Markup.XmlLanguage.GetLanguage(lang);
			translateTextBlocks(AvailableBlock, 
				DurationBlock, SpeedBlock, SendMailBlock, ClickBlock);
			NeedBox.Content = Translate[NeedBox.Content.ToString()];
			setListView();
			if (Stdout != null)
				Stdout();
		}

		/* Translation Events */
		private void setTranslationEvents()
		{
			FrButton.Click += (o, e) => 
			{
				if (ResourceControl.Data.IsEnglish)
					changeUIElementsLanguage("Fr");
			};

			EnButton.Click += (o, e) => 
			{
				if (ResourceControl.Data.IsFrench)
					changeUIElementsLanguage("En");
			};
		}
	}
}
