using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using DevExpress.Mvvm;
using System.ComponentModel;
using HospitalBaseApp.Views;
using System.Windows.Input;
using HospitalBaseApp.DB;
using HospitalBaseApp.Models;
using HospitalBaseApp.Extensions;
using System.Windows;

namespace HospitalBaseApp.ViewModels
{
    public class AddPatientViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;
        
        //  Данные, прикрепленные к контролам в XAML
        public enum SexesEnum
        {
            [Description("Мужской")]
            Male,

            [Description("Женский")]
            Female,
        }
        public List<SexesEnum> Sexes { get; set; } =
          Enum.GetValues<SexesEnum>().ToList();
        public SexesEnum Sex { get; set; }

        public string? Patronymic { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public AddPatientViewModel(DatabaseContext db)
        {
            // инициализация DB от DI
            _db = db;
        }
        public ICommand AddPatientCommand => new DelegateCommand<AddPatientDialog>(async (w) =>
        {
            // Добавление пациента, если пытаемся добавить существующего - исключение.
            try
            {
                await _db.Patients.AddAsync(new PatientModel { NSP = $"{Surname} {Name} {Patronymic}", Sex = Sex.GetEnumDescription() });
                await _db.SaveChangesAsync();
                w?.Close();
            }
           catch(Exception e)
            {
                MessageBox.Show("Такой пациент уже есть в списке!");
            }
            // CanExecute проверка на пустоту строк
        }, (_) => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Patronymic));

    }
}
