using System;
using System.Windows.Input;

namespace FrEnCV
{
    public class Cmd : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Func<object, bool> CanExec { get; set; }
        public Action<object> Exec { get; set; }


        public Cmd(Func<object, bool> canExex, Action<object> exec)
        {
            CanExec = canExex;
            Exec = exec;
        }

        public Cmd(Func<object, bool> canExex, Action exec)
        {
            CanExec = canExex;
            Exec = (o) => { exec(); };
        }

        public Cmd(Action<object> exec)
        {
            CanExec = (o) => { return true; };
            Exec = exec;
        }

        public Cmd(Action exec)
        {
            CanExec = (o) => { return true; };
            Exec = (o) => { exec(); };
        }

        public bool CanExecute(object parameter)
        {
            return CanExec(parameter);
        }

        public void Execute(object parameter)
        {
            Exec(parameter);
        }
    }
}
