using System;
using Mabado.View.Domain;
using System.Windows;

namespace InfrastructureStandAlone
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