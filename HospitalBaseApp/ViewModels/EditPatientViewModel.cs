using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using HospitalBaseApp.Services;
using HospitalBaseApp.Models;
using System.ComponentModel;
using HospitalBaseApp.DB;
using System.Windows.Input;
using HospitalBaseApp.Views;
using HospitalBaseApp.Extensions;
using System.Windows;

namespace HospitalBaseApp.ViewModels
{
    public class EditPatientViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public int Id { get; set; }

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
        public EditPatientViewModel(MessageBus messageBus, DatabaseContext db)
        {
            _db = db;
            messageBus.Receive<PatientModelMessage>(this, async message =>
            {
                Name = message.PatientModel.NSP.Split(" ")[1];
                Surname = message.PatientModel.NSP.Split(" ")[0];
                Patronymic = message.PatientModel.NSP.Split(" ")[2];
                Id = message.PatientModel.ID;
                Sex = message.PatientModel.Sex.GetValueFromDescription<SexesEnum>();
            });
        }
       

        public ICommand EditPatientCommand => new DelegateCommand<EditPatientDialog>(async (w) =>
        {
            try
            {
                var result = await _db.Patients.FindAsync(Id);

                result.NSP = $"{Surname} {Name} {Patronymic}";
                result.Sex = Sex.GetEnumDescription();
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch
            {
                MessageBox.Show("Такой пациент уже есть!");
            }
        }, (_) => !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Surname) && !string.IsNullOrWhiteSpace(Patronymic));
    }
}
