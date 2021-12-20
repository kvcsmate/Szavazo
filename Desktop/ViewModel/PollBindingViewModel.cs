using Persistence;
using Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop.ViewModel
{
    public class PollBindingViewModel:ViewModelBase
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        private User _user;
        public  User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        private int _pollId;
        public int PollId
        {
            get
            {
                return _pollId;
            }
            set
            {
                _pollId = value;
                OnPropertyChanged();
            }
        }

        private bool _isVoted;
        public bool IsVoted
        {
            get
            {
                return _isVoted;
            }
            set
            {
                _isVoted = value;
                OnPropertyChanged();
            }
        }
        public static explicit operator PollBindingViewModel(PollBindingDto dto) => new PollBindingViewModel
        {
            Id = dto.Id,
            IsVoted = dto.IsVoted,
            PollId = dto.PollId,
            User = dto.User
        };
        public static explicit operator PollBindingDto(PollBindingViewModel vm) => new PollBindingDto
        {
            Id = vm.Id,
            IsVoted = vm.IsVoted,
            PollId = vm.PollId,
            User = vm.User
        };
    }
}
