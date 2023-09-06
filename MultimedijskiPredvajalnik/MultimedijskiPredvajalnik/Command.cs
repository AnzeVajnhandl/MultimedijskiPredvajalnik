using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MultimedijskiPredvajalnik
{
    public class Command : ICommand
    {
        private Action task;
  
        public Command(Action input) { 
            task = input;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
                task();    
        }

        public event EventHandler? CanExecuteChanged;
    }
}
