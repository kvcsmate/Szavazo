using Desktop.Model;
using Persistence;
using Persistence.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Desktop.ViewModel
{
    class MainViewModel : ViewModelBase
    {
        private readonly ApiServiceClient _service;
        private ObservableCollection<PollViewModel> _polls;
        private ObservableCollection<AnswerViewModel> _answers;
        private ObservableCollection<PollBindingViewModel> _pollBindings;

        public ObservableCollection<PollViewModel> Polls
        { get { return _polls; } set { _polls = value; OnPropertyChanged(); } }

        public ObservableCollection<AnswerViewModel> Answers
        { get { return _answers; } set { _answers = value; OnPropertyChanged(); } }

        public ObservableCollection<PollBindingViewModel> PollBindings
        { get { return _pollBindings; } set { _pollBindings = value; OnPropertyChanged(); } }

        private PollViewModel _selectedPoll;
        public PollViewModel SelectedPoll
        { get { return _selectedPoll; } set { _selectedPoll = value; OnPropertyChanged(); } }

        private ObservableCollection<UserViewModel> _users;
        public ObservableCollection<UserViewModel> Users
        { get { return _users; } set { _users = value; OnPropertyChanged(); } }

        public ObservableCollection<AnswerViewModel> NewAnswers { get; set; }

        public PollViewModel _newPoll { get; set; }
        public PollViewModel NewPoll
        { get { return _newPoll; } set { _newPoll = value; OnPropertyChanged(); } }

        private AnswerViewModel _selectedAnswer;
        public AnswerViewModel SelectedAnswer
        { get { return _selectedAnswer; } set { _selectedAnswer = value; OnPropertyChanged(); } }

        private string _selectedAnswerName;
        public string SelectedAnswerName
        { get { return _selectedAnswerName; } set { _selectedAnswerName = value; OnPropertyChanged(); } }

        private UserViewModel _selectedUser;
        public UserViewModel SelectedUser
        { get { return _selectedUser; } set { _selectedUser = value; OnPropertyChanged(); } }

        private DateTime _start;
        public DateTime Start
        { get { return _start; } set { _start = value; OnPropertyChanged(); } }

        private DateTime _end;
        public DateTime End
        { get { return _end; } set { _end = value; OnPropertyChanged(); } }

        private string _answerText;
        public string Answertext
        { get { return _answerText; } set { _answerText = value; OnPropertyChanged(); } }
        public DelegateCommand RefreshPollsCommand { get; private set; }
        public DelegateCommand SelectPollCommand { get; private set; }
        public DelegateCommand SelectAnswerCommand { get; private set; }
        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand AddPollCommand { get; private set; }
        public DelegateCommand AddAnswerCommand { get; private set; }
        public DelegateCommand BindUserCommand { get; private set; }
        public DelegateCommand LoadUsersCommand { get; private set; }
        public DelegateCommand CancelNewPollCommand { get; private set; }
        public DelegateCommand SaveNewPollCommand { get; private set; }
        public DelegateCommand SelectUserCommand { get; private set; }
        public DelegateCommand StartPollCreateCommand { get; private set; }
        public DelegateCommand AddNewAnswerCommand { get; private set; }

        public event EventHandler StartingCreatePoll;
        public event EventHandler FinishingCreatePoll;
        public event EventHandler LogoutSucceeded;

        public MainViewModel(ApiServiceClient service)
        {
            Start = DateTime.Now.AddMinutes(5);
            End = DateTime.Now.AddDays(15);
            _service = service;

            RefreshPollsCommand = new DelegateCommand(_ => LoadPollsAsync());
            SelectPollCommand = new DelegateCommand(_ => LoadDetailsAsync(SelectedPoll));
            LogoutCommand = new DelegateCommand(_ => LogoutAsync());
            LoadUsersCommand = new DelegateCommand(_ => LoadusersAsync());
            CancelNewPollCommand = new DelegateCommand(_ => CancelNewPoll());
            SaveNewPollCommand = new DelegateCommand(_ => SaveNewPoll());
            AddNewAnswerCommand = new DelegateCommand(_ => AddNewAnswer());
            SelectUserCommand = new DelegateCommand(_ => SelectUser());
            StartPollCreateCommand = new DelegateCommand(_ => StartPollCreation());
        }

        private void StartPollCreation()
        {
            NewPoll = new PollViewModel();
            NewAnswers = new ObservableCollection<AnswerViewModel>();
            LoadusersAsync();
            StartingCreatePoll?.Invoke(this, EventArgs.Empty);
        }

        private void SelectUser()
        {
        }

        private void AddNewAnswer()
        {
            if (NewPoll!=null)
            {
                NewAnswers.Add(new AnswerViewModel { PollId = NewPoll.Id,Text=Answertext });
                Answertext = String.Empty;
            }
            else
            {
                OnMessageApplication($"Unexpected error occured! (üres szavazáshoz nem lehet opciót megadni!)");
            }
            
        }

        private async void LoadusersAsync()
        {
            
            try
            {
                Users = new ObservableCollection<UserViewModel>();
                var response = await _service.LoadUsersAsync();
                foreach (var user in response)
                {
                    Users.Add(new UserViewModel { IsSelected = false, User=user});
                }
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private async void SaveNewPoll()
        {
            try
            {
                NewPoll.Creator = await _service.GetCurrentUser();
                NewPoll.End = End;
                NewPoll.Start = Start;
                PollCreateRequest request = new PollCreateRequest
                {
                    Poll = (PollDto)NewPoll,
                    Answers = NewAnswers.Select(a => (AnswerDto)a).ToList(),
                    UserIds = Users.Where(u => u.IsSelected).Select(u => u.User.Id).ToList()
                };
                await _service.CreatePollAsync(request);
                CancelNewPoll();
                OnMessageApplication($"Szavazás létrehozása sikeres.");
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private void CancelNewPoll()
        {
            NewAnswers = null;
            End = DateTime.Now.AddDays(15);
            Start = DateTime.Now.AddMinutes(5);
            foreach (var user in Users)
            {
                user.IsSelected = false;
            }
            NewPoll = null;
            FinishingCreatePoll?.Invoke(this, EventArgs.Empty);
        }
        private async void LogoutAsync()
        {
            try
            {
                await _service.LogoutAsync();
                LogoutSucceeded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private  async void LoadDetailsAsync(PollViewModel selectedPoll)
        {
            if (selectedPoll is null)
            {
                return;
            }
            try
            {
                Answers = new ObservableCollection<AnswerViewModel>((await _service.LoadAnswersAsync(selectedPoll.Id)).Select(answer => (AnswerViewModel)answer));
                PollBindings = new ObservableCollection<PollBindingViewModel>((await _service.LoadPollBindingsAsync(selectedPoll.Id)).Select(answer => (PollBindingViewModel)answer));
                Start = selectedPoll.Start;
                End = selectedPoll.End;
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }

        private async void LoadPollsAsync()
        {
            try
            {
                Polls = new ObservableCollection<PollViewModel>
                    ((await _service.LoadPollsAsync())
                    .Select(poll => (PollViewModel)poll));
            }
            catch (Exception ex) when (ex is NetworkException || ex is HttpRequestException)
            {
                OnMessageApplication($"Unexpected error occured! ({ex.Message})");
            }
        }
    }
}
