using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AdrManager.MvvmUtilities
{

    // https://blogs.msdn.microsoft.com/jebarson/2017/07/26/writing-an-asynchronous-relaycommand-implementing-icommand/
    public class RelayCommandAsync<T> : ICommand
    {
        private readonly Func<T, Task> executedMethod;
        private readonly Func<T, bool> canExecuteMethod;
        private Func<Task> _scanCommand;

        public event EventHandler CanExecuteChanged;
        public RelayCommandAsync(Func<T, Task> execute) : this(execute, null) { }

        public RelayCommandAsync(Func<T, Task> execute, Func<T, bool> canExecute)
        {
            this.executedMethod = execute ?? throw new ArgumentNullException("execute");
            this.canExecuteMethod = canExecute;
        }

        public bool CanExecute(object parameter) => this.canExecuteMethod == null || this.canExecuteMethod((T)parameter);
        public async void Execute(object parameter) => await this.executedMethod((T)parameter);
        public void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
