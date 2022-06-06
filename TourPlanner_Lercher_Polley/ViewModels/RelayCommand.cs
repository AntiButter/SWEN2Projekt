using System;
using System.Windows.Input;

namespace TourPlanner_Lercher_Polley.ViewModels
{

    //Helper class, from Moodle course

    public class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;}
            remove { CommandManager.RequerySuggested -= value;} 
        }
        private readonly Action<object> executeAction;
        private readonly Predicate<object> canExecutePredicate;

        public RelayCommand(Action<object> execute): this(execute, null)
        {

        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if(execute == null)
            {
                throw new ArgumentNullException("execute");   
            }
            executeAction = execute;
            canExecutePredicate = canExecute;   
        }

        

        public bool CanExecute(object parameter)
        {
            return canExecutePredicate == null ? true : canExecutePredicate(parameter);
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
    }
}
