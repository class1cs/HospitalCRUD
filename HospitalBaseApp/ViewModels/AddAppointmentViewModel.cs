using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using DevExpress.Mvvm;
using HospitalBaseApp.DB;
using HospitalBaseApp.Models;
using System.Collections.ObjectModel;
using HospitalBaseApp.Views;
using System.Windows.Input;
using System.Windows;
using HospitalBaseApp.Extensions;
using System.Text.RegularExpressions;

namespace HospitalBaseApp.ViewModels
{
    public class AddAppointmentViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;

        public string Ill { get; set; }

        public string Time { get; set; }

        public List<PatientModel> Patients { get; set; }

        public List<DoctorModel> Doctors { get; set; }

        public DoctorModel SelectedDoctor { get; set; }
        
        public PatientModel SelectedPatient { get; set; }

        public AddAppointmentViewModel(DatabaseContext db)
        {
            _db = db;
            Patients = _db.Patients.Local.ToList();
            Doctors = _db.Doctors.Local.Where(x => x.Role == "Врач").ToList();
        }

        public ICommand AddAppointmentCommand => new DelegateCommand<AddAppointmentDialog>(async (w) =>
        {

            try
            {
                await _db.Appointments.AddAsync(new AppointmentModel { Doctor = SelectedDoctor, Ill = Ill, Patient = SelectedPatient, Time = Time });
                ;
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch (Exception e)
            {

                MessageBox.Show("Такая запись на прием уже есть!");
            }


        }, (_) => !string.IsNullOrWhiteSpace(Ill) && !string.IsNullOrWhiteSpace(Time) && Regex.IsMatch(Time, @"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"));

    }
}
