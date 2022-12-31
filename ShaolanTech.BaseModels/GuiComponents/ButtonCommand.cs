using System;
using System.Windows.Input;

namespace ShaolanTech 
{
    /// <summary>
    /// Click事件响应类
    /// </summary>
    public class ButtonCommand : ICommand
    {

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        public event EventHandler Executing;
        public object CommandParameter { get; set; }
        public string CommandName { get; set; }
        public void Execute(object parameter)
        {

            this.CommandName = parameter.ToString();

            if (this.Executing != null)
            {
                this.Executing(this, new EventArgs());
            }
        }
    }
}
