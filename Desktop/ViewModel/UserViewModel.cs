using Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        public User User { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
