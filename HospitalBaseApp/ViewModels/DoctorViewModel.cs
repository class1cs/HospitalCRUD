using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using HospitalBaseApp.DB;
using HospitalBaseApp.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using HospitalBaseApp.Views;
using HospitalBaseApp.Services;

namespace HospitalBaseApp.ViewModels
{
    public class DoctorViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;
        private readonly MessageBus _messageBus;

        public ObservableCollection<AppointmentModel> Appointments { get; set; }
        public ObservableCollection<PatientModel> Patients { get; set; }
        public ObservableCollection<DoctorModel> Doctors { get;  set; }
        public AppointmentModel SelectedAppointment { get; set; }

        public DoctorViewModel(DatabaseContext db, MessageBus messageBus)
        {
            _db = db;
            Appointments = _db.Appointments.Local.ToObservableCollection();
            Patients = _db.Patients.Local.ToObservableCollection();
            Doctors = _db.Doctors.Local.ToObservableCollection();
            _messageBus = messageBus;
        }

        public ICommand RemoveAppointmentCommand => new DelegateCommand(() =>
        {

            _db.Appointments.Remove(SelectedAppointment);
            

            _db.SaveChanges();



        }, () => SelectedAppointment != null);

        public ICommand ShowAddAppointmentDialogCommand => new DelegateCommand(() =>
        {



            var AddAppointment = new AddAppointmentDialog();

            AddAppointment.ShowDialog();


        }, () => Patients.Count != 0 && Doctors.Where(x => x.Role == "Врач").ToList().Count != 0);

        public ICommand ShowEditAppointmentDialog => new DelegateCommand<AppointmentModel>(async(appointment) =>
        {


            var EditAppointment = new EditAppointmentDialog();
            await _messageBus.SendTo<EditAppointmentViewModel>(new AppointmentModelMessage(appointment));
            EditAppointment.ShowDialog();


        }, (_) => Patients.Count != 0 && SelectedAppointment != null);
    }
}
