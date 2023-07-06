using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using HospitalBaseApp.Services;
using HospitalBaseApp.DB;
using System.Collections.ObjectModel;
using HospitalBaseApp.Models;
using System.Windows.Input;
using HospitalBaseApp.Views;
using System.Windows;
using System.Text.RegularExpressions;

namespace HospitalBaseApp.ViewModels
{
    public class EditAppointmentViewModel : ViewModelBase
    {
        private readonly MessageBus _messageBus;
        private readonly DatabaseContext _db;

        public string Ill { get; set; }

        public string Time { get; set; }

        public int Id { get; set; }

        public ObservableCollection<PatientModel> Patients { get; set; }

        public DoctorModel SelectedDoctor { get; set; }
        public List<DoctorModel> Doctors { get; set; }
        public PatientModel SelectedPatient { get; set; }

     
        public EditAppointmentViewModel(MessageBus messageBus, DatabaseContext db)
        {
            _messageBus = messageBus;
            _db = db;
            Doctors = _db.Doctors.Local.Where(x => x.Role == "Врач").ToList();
            Patients = _db.Patients.Local.ToObservableCollection();
            _messageBus.Receive<AppointmentModelMessage>(this, async message =>
            {
                
                Ill = message.AppointmentModel.Ill;
                Time = message.AppointmentModel.Time;
                SelectedPatient = Patients.FirstOrDefault(x => x == message.AppointmentModel.Patient);
                Id = message.AppointmentModel.Id;
                SelectedDoctor = Doctors.FirstOrDefault(x => x == message.AppointmentModel.Doctor);
                
            });
        }

        public ICommand EditAppointmentCommand => new DelegateCommand<EditAppointmentDialog>(async(w) =>
        {

            try
            {
                var result = await _db.Appointments.FindAsync(Id);
                result.Patient = SelectedPatient;
                result.Time = Time;
                result.Ill = Ill;
                result.Doctor = SelectedDoctor;
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Такая запись на операцию уже есть!");
            }

        }, (_) => !string.IsNullOrWhiteSpace(Ill) && !string.IsNullOrWhiteSpace(Time) && Regex.IsMatch(Time, @"^(?:0?[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$"));
    }
}
