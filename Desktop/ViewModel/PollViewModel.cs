using Persistence;
using Persistence.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Desktop.ViewModel
{
    public class PollViewModel : ViewModelBase
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

        private string _question;
        [DisplayName("Kérdés")]
        public string Question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = value;
                OnPropertyChanged();
            }
        }


        private DateTime _start;
        [DisplayName("Szavazás kezdete")]
        public DateTime Start
        {
            get
            {
                return _start;
            }
            set
            {
                _start = value;
                OnPropertyChanged();
            }
        }

        private DateTime _end;
        [DisplayName("Szavazás vége")]
        public DateTime End
        {
            get
            {
                return _end;
            }
            set
            {
                _end = value;
                OnPropertyChanged();
            }
        }
        private User _creator;
        [DisplayName("Kiíró felhasználó")]
        public User Creator
        {
            get
            {
                return _creator;
            }
            set
            {
                _creator = value;
                OnPropertyChanged();
            }
        }

        public PollViewModel ShallowClone()
        {
            return (PollViewModel)this.MemberwiseClone();
        }

        public static explicit operator PollViewModel(PollDto dto) => new PollViewModel
        {
            Question = dto.Question,
            Creator = dto.Creator,
            End = dto.End,
            Id = dto.Id,
            Start = dto.Start,
        };
        public static explicit operator PollDto(PollViewModel vm) => new PollDto
        {
            Question = vm.Question,
            Creator = vm.Creator,
            End = vm.End,
            Id = vm.Id,
            Start = vm.Start,
        };
    }
}
