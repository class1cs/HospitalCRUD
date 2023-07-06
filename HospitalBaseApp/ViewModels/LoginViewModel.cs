using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using HospitalBaseApp.DB;
using HospitalBaseApp.Extensions;
using HospitalBaseApp.Pages;
using HospitalBaseApp.Services;
using PropertyChanged;

namespace HospitalBaseApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private readonly DatabaseContext _db;
        private readonly PageService _pageService;
        public enum Roles
        {
            [Description("Врач")]
            Doctor,

            [Description("Хирург")]
            Surgeon,

            [Description("Глав. врач")]
            GeneralDoctor,

        }


        public Roles LoginRole { get; set; }
        public List<Roles> RolesList { get; set; } =
       Enum.GetValues<Roles>().ToList();

        public string Login { get; set; }

        public string Password { get; set; }
        public LoginViewModel(DatabaseContext db, PageService pageService, MessageBus messageBus)
        {
            _messageBus = messageBus;
            _db = db;
            _pageService = pageService;
        }
        public ICommand LoginCommand => new DelegateCommand(async() =>
        {

                var user = _db.Doctors.FirstOrDefault(x => x.Login == Login && x.Password == Password && x.Role == LoginRole.GetEnumDescription());

                if (user == null)
                {
                    MessageBox.Show("Проверьте правильность введенных данных");
                    return;
                }
                MessageBox.Show($"Добро пожаловать, {user.NSP}!");
            App.Current.MainWindow.Title = $"Областная больница - {user.NSP}";
                switch (LoginRole)
                {
                    case Roles.Surgeon:
                        _pageService.ChangePage(new SurgeonPage());
                        break;
                    case Roles.Doctor:
                        _pageService.ChangePage(new DoctorPage());
                        break;
                    case Roles.GeneralDoctor:
                        
                        _pageService.ChangePage(new GeneralDoctorPage());
                        await _messageBus.SendTo<GeneralDoctorViewModel>(new DoctorModelMessage(user));
                        break;
                    default:
                        break;
                }
              

        }, () => !string.IsNullOrWhiteSpace(Login), !string.IsNullOrWhiteSpace(Password));
    }
}
