using Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop.ViewModel
{
    public class AnswerViewModel:ViewModelBase
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

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }
        public static explicit operator AnswerViewModel(AnswerDto dto) => new AnswerViewModel
        {
            Id = dto.Id,
            PollId = dto.PollId,
            Text = dto.Text
        };
        public static explicit operator AnswerDto(AnswerViewModel vm) => new AnswerDto
        {
            Id = vm.Id,
            PollId = vm.PollId,
            Text = vm.Text
        };

    }
}

