using System.Collections.ObjectModel;
using System.ComponentModel;
using ViolationManagement.Models;

namespace ViolationManagement.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private string _fullName;
        private string _phone;
        private string _address;
        private string _gender;

        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged("FullName"); } }
        public string CitizenID { get; set; }
        public string Email { get; set; }
        public string Phone { get => _phone; set { _phone = value; OnPropertyChanged("Phone"); } }
        public string Address { get => _address; set { _address = value; OnPropertyChanged("Address"); } }
        public string Gender { get => _gender; set { _gender = value; OnPropertyChanged("Gender"); } }

        public List<Vehicle> Vehicles { get; set; } = new();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
