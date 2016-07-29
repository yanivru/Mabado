using System;
using System.Windows;
using Mabado.View.Domain;

namespace Emulator
{
    public class MessageBoxLogger : ILogger
    {
        public void WriteError(Exception exception)
        {
            MessageBox.Show(exception.ToString());
        }

        public void WriteInfo(string message)
        {
            
        }
    }
}