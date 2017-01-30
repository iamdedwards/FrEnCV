using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FrEnCV
{
	public partial class MainWindow
	{
		/* Visibility Helper Methods */
		private void hideUIElements(params FrameworkElement[] elems)
		{
			foreach(var f in elems)
				f.Hide();
		}

		private void showUIElements(params FrameworkElement[] elems)
		{
			foreach(var f in elems)
				f.Show();
		}

		/* Events */
		private void setButtonEvents()
		{
			setTranslationEvents();

			DataBtnForward.Click += (o, e) =>
			{ 
				if (ResourceControl.DataEnd)
				{
					showUIElements(EmailMeButton, MaskBox);
					return ;
				}
				ResourceControl.NextDataItem(this);
				DataBtnForward.Hide();
			};

			MaskBox.Click += (o, e) =>
			{
				init();
            };
			EmailMeButton.Click += (o, e) => 
			{
				var subject = (ResourceControl.Data.IsEnglish) ?
					"Internship[C Sharp .Net]" :
					"Stage[C Sharp .Net]";
				Process.Start(Uri.EscapeUriString($"mailto:iamdedwards@gmail.com?Subject={subject}"));
			};
		}
		
		/*set UI elements */
		private void setCalendar()
        {
			Calendar cal = AvailableFrom;
            var dates = cal.SelectedDates;
            dates.Clear();
            var canStart = new DateTime(DateTime.Now.Year, 3, 1, 8, 42, 42);
            cal.DisplayDate = canStart;
            var span = 0;
            var months = CV.StartMonth;
            while (months < CV.StartMonth + CV.DurationMonths)
                span += DateTime.DaysInMonth(2017, months++);
            var i = 0;
            while (i < span)
            {
                dates.Add(canStart.AddDays(i));
                i++;
            }
            cal.IsTodayHighlighted = false;
        }

		private void	setListView()
		{
			if (!ExperienceView.IsHidden() && !FormationsView.IsHidden())
			{ 
				ExperienceView = setListView<Experience>(ExperienceView);
				FormationsView = setListView<Formation>(FormationsView);
			}
		}

		private ListView setListView<T>(ListView listView)
        {
			var data = ResourceControl.GetData<T>();
            var view = new GridView();
            foreach (var key in data.First().Keys)
            {
                var col = new GridViewColumn();
                col.Header = key;
                col.DisplayMemberBinding = new Binding($"[{key}]");
                view.Columns.Add(col);
            }
            listView.View = view;
			listView.ItemsSource = data;
			return (listView);
        }

		private void init()
		{
			ResourceControl.DataIndex = 0;
			hideUIElements(
				EmailMeButton, MaskBox, FormationsView, ExperienceView,
				ExperienceTitle, FormationsTitle, DataBtnForward
			);
			showUIElements(
				AvailableFrom, DurationBlock, AvailableBlock
				);
			ViewTerminal();
		}

        public void SetResources()
        {
			ResourceControl.SetResources(CV);
            InitializeComponent();
			setButtonEvents();
			setCalendar();
			setTextBlock(this.FormationsTitle, "Training", "Formations");
			setTextBlock(AvailableBlock, "Available", "Disponible");
			setTextBlock(DurationBlock, "For 4 - 6 months", "Pendant 4 - 6 mois");
			setTextBlock(SpeedBlock, "Speed", "Vitesse");
			setTextBlock(ClickBlock, "Click Here", "Cliquez Ici");
			setTextBlock(SendMailBlock, "to send a mail to", "pour envoyer un mail à");
			setCheckBox(NeedBox, "Needs to find an internship", "Doit trouver une stage");
            DataContext = this;
			init();
        }
	}
}