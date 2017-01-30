using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace FrEnCV
{
    public partial class Terminal : NotifyPropertyChanged
    {
        private int             height;
        private List<TermText>  txt_list;
        private int             txt_index;

        private string          curr_buff;
        private int             curr_buff_index;

        private List<string>    lines;
        private int             line_count;
        private int             line_skip;

        private Func<string>    printMethod;

        private string          _output;
        public string           OutPut
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged(); }
        }

        public DispatcherTimer  Display             { get; private set; }
        public ICommand         Slower              { get; private set; }
        public ICommand         Pause               { get; set; }
        public ICommand         Faster              { get; private set; }
        public ICommand         SwitchPrintMethod   { get; private set; }

        private Terminal() { }

        public static Terminal Load(List<TermText> frames)
        {
            Terminal t = new Terminal();
            t.height = 18;
            t.Display = new DispatcherTimer();
            t.printMethod = t.Put;
            t.Display.Interval = TimeSpan.FromMilliseconds(2);
            t.Display.Tick += ((sender, args) =>
            {
                t.OutPut = t.printMethod();
            });
            t.txt_list = frames;
            t.LoadFile(fromStart: true);
            t.initCmds();
            return (t);
        }

        private void initCmds()
        {
            SwitchPrintMethod = new Cmd(() =>
            {
                if (printMethod == Put)
                    printMethod = UnPut;
                else
                    printMethod = Put;
            });
            Slower = new Cmd(() =>
            {
                double msecs = Display.Interval.TotalMilliseconds + 10;
                Display.Interval = TimeSpan.FromMilliseconds(msecs);
            });
            Faster = new Cmd(() =>
            {
                double msecs = Display.Interval.TotalMilliseconds - 10;
                if (msecs > 0)
                    Display.Interval = TimeSpan.FromMilliseconds(msecs);
                else
                {
                    Display.Interval = TimeSpan.FromMilliseconds(0);
                }
            });
            Pause = new Cmd(() =>
			{
				Display.IsEnabled = !Display.IsEnabled;
			}); 
        }
    }
}
