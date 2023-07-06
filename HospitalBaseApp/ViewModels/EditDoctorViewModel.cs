using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using DevExpress.Mvvm;
using HospitalBaseApp.DB;
using System.ComponentModel;
using HospitalBaseApp.Services;
using HospitalBaseApp.Extensions;
using HospitalBaseApp.Models;
using System.Windows.Input;
using HospitalBaseApp.Views;
using System.Windows;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HospitalBaseApp.ViewModels
{
    public class EditDoctorViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;

        public EditDoctorViewModel(DatabaseContext db, MessageBus messageBus)
        {
            _db = db;
            messageBus.Receive<DoctorModelMessage>(this, async message => 
            {
                SelectedDoctor = message.DoctorModel;
                Name = message.DoctorModel.NSP.Split(" ")[1];
                Surname = message.DoctorModel.NSP.Split(" ")[0];
                Patronymic = message.DoctorModel.NSP.Split(" ")[2];
                Login = message.DoctorModel.Login;
                Password = message.DoctorModel.Password;
                PhoneNumber = message.DoctorModel.PhoneNumber;
                Email = message.DoctorModel.Email;
                Birthday = message.DoctorModel.Birthday;
                DoctorType = message.DoctorModel.Role.GetValueFromDescription<DoctorTypes>();
                DoctorId = message.DoctorModel.Id;
            });
        }

        public enum DoctorTypes
        {
            [Description("Врач")]
            Doctor,

            [Description("Хирург")]
            Surgeon,

            [Description("Глав. врач")]
            GeneralDoctor,

        }


        public DoctorTypes DoctorType { get; set; }
        public List<DoctorTypes> DoctorTypesList { get; set; } =
       Enum.GetValues<DoctorTypes>().ToList();

        public DoctorModel SelectedDoctor;

        public int DoctorId;

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Birthday { get; set; }

      

        public ICommand EditDoctorCommand => new DelegateCommand<EditDoctorDialog>(async (w) =>
        {
            try
            {
                var result = _db.Doctors.Find(DoctorId);
                result.NSP = $"{Surname} {Name} {Patronymic}";
                result.Login = Login;
                result.Password = Password;
                result.Email = Email;
                result.PhoneNumber = PhoneNumber;
                result.Birthday = Birthday;
                result.Role = DoctorType.GetEnumDescription();
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Такой доктор уже есть в списке!");
            }
           
        }, (_) => !string.IsNullOrWhiteSpace(Name)
        && !string.IsNullOrWhiteSpace(Surname)
        && !string.IsNullOrWhiteSpace(Patronymic)
        && !string.IsNullOrWhiteSpace(Login)
        && !string.IsNullOrWhiteSpace(Password)
        && !string.IsNullOrWhiteSpace(Birthday)
        && !string.IsNullOrWhiteSpace(Email)
        && !string.IsNullOrWhiteSpace(DoctorType.GetEnumDescription())
            && !string.IsNullOrWhiteSpace(PhoneNumber)
            && Regex.IsMatch(PhoneNumber, @"^(\+7|7|8)?[\s\-]?\(?[489][0-9]{2}\)?[\s\-]?[0-9]{3}[\s\-]?[0-9]{2}[\s\-]?[0-9]{2}$")
            && Regex.IsMatch(Email, @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")
        && DateTime.TryParseExact(Birthday, "dd'.'MM'.'yyyy",
                           CultureInfo.InvariantCulture,
                           DateTimeStyles.None,
                           out DateTime date));
    }
}
