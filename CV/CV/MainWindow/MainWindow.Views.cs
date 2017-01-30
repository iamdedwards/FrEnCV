using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrEnCV
{
    public partial class MainWindow
    {
		
        private CV					_cv;
        public CV					CV						{ get { return _cv = _cv ?? new CV(); } } 
        public Terminal				Term                    { get; private set; }
		public Action				Stdout					{ get ; set; }
		public ResourceController	ResourceControl			{ get {return ResourceController.Instance; } }

		private void ViewPresentation()
		{
			DataBtnForward.Show();
			Term.Display.IsEnabled = false;
			List<string> pres = (ResourceControl.Data.IsEnglish) ?
				CV.Presentation.En : CV.Presentation.Fr;	
			StdOutBlock.Text = String.Join("", pres)
				.Replace(@"\n", Environment.NewLine + Environment.NewLine)
				.Replace("\t", "")
				.Replace("\"", "");
			Stdout = () => ViewPresentation();
		}

		private void setModules(string intro, List<Formation> formations)
		{
			StdOutBlock.Text = 
			    formations.Select((f) => {
					if (f.Modules.Count() == 0)
						return ("");
					var ret = $"{Environment.NewLine}{f.Establishment}: {intro} {Environment.NewLine + Environment.NewLine}\t::" +
                    String.Join(Environment.NewLine + "\t::", f.Modules);
					return (ret) + Environment.NewLine;
				}).Concat() + Environment.NewLine;
        }

		private void ViewExpertise()
		{
			DataBtnForward.Show();
			Term.Display.IsEnabled = false;

			hideUIElements(
				AvailableBlock, DurationBlock, AvailableFrom);

			showUIElements(
				ExperienceView, FormationsView,
				FormationsTitle, ExperienceTitle);

			if (ResourceControl.Data.IsEnglish)
			{
				setModules("key modules >>", CV.Formations.En);
			}
			else
			{
				setModules("modules clés >>", CV.Formations.Fr);
			}
			setListView();
			Stdout = () => ViewExpertise();       
		}

        public void ViewTerminal()
        {
			var replace = new FrEn<List<string>>();
			replace.Fr.AddRange(CV.Presentation.Fr);
			replace.En.AddRange(CV.Presentation.En);
            Term = Terminal.Load(new List<TermText> {
				new TermText() {
					Lines = File.ReadAllLines(@"../../Text/Presentation.c").ToList(),
					Replace = replace,
					OnSuccess = () => { ViewPresentation(); }
				},
				new TermText() {
					Lines = File.ReadAllLines(@"../../Text/PresentationV2.cs").ToList(),
					OnSuccess = () => { ViewExpertise(); }
				},
            });
			Term.Pause = new Cmd((o) => { return StdOutBlock.Text == ""; }, () => {Term.Display.IsEnabled = !Term.Display.IsEnabled; });
			Term.Display.Interval = TimeSpan.FromMilliseconds(3);
            Term.Display.Start();
        }
    }
}
