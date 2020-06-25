using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Rootedfuture.Models
{
    public class ErrorModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (status == value)
                {
                    return;
                }
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


        void OnPropertyChanged(string name)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
