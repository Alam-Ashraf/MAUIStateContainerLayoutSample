using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUIStateContainerLayoutSample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        private ObservableCollection<string> _list;
        public ObservableCollection<string> UserList
        {
            get => _list;
            set
            {
                _list = value;
                OnPropertyChanged();
            }
        }

        private string _state = "Loading";
        public string CurrentState
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public MainPageViewModel()
        {
            AddUserCommand = new Command(() =>
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    if (!string.IsNullOrEmpty(User))
                    {
                        UserList.Add(User);

                        if (UserList.Count > 0)
                        {
                            CurrentState = "Success";
                        }

                        User = string.Empty;
                    }
                });
            });

            DeleteUserCommand = new Command<string>((user) =>
            {
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    if (!string.IsNullOrEmpty(user))
                    {
                        UserList.Remove(user);

                        if (UserList.Count <= 0)
                        {
                            CurrentState = "Empty";
                        }
                    }
                });
            });

            // Load list after 5 Second
            Task.Delay(5000).ContinueWith(t => GetList());
        }

        private void GetList()
        {
            UserList = new ObservableCollection<string>()
            {
                "User 1",
                "User 2",
                "User 3",
                "User 4",
            };

            // Change curren state from "Loading" to "Success"
            CurrentState = "Success";
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
