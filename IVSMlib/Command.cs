using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IVSMlib
{
    public class Command:ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;

        public Command(Action act)
        {
            _action = act;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
