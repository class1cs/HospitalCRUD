using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using DevExpress.Mvvm;
using System.Windows.Input;
using System.ComponentModel;
using HospitalBaseApp.DB;
using HospitalBaseApp.Models;
using HospitalBaseApp.Extensions;
using System.Text.RegularExpressions;
using HospitalBaseApp.Views;
using System.Windows;
using System.Globalization;

namespace HospitalBaseApp.ViewModels
{
    public class AddDoctorViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;

        public AddDoctorViewModel(DatabaseContext db)
        {
            _db = db;
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
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Birthday { get; set; }

        public ICommand AddDoctorCommand => new DelegateCommand<AddDoctorDialog>(async (w) =>
        {
            try
            {
                await _db.Doctors.AddAsync(new DoctorModel { NSP = $"{Surname} {Name} {Patronymic}", Birthday = Birthday, PhoneNumber = PhoneNumber, Email = Email, Login = Login, Password = Password, Role = DoctorType.GetEnumDescription() });
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Такой врач уже существует!");
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
