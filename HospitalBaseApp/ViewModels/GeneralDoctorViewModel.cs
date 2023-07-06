using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.Windows.Input;
using HospitalBaseApp.Models;
using HospitalBaseApp.Views;
using HospitalBaseApp.Services;
using HospitalBaseApp.DB;
using System.Collections.ObjectModel;
using System.Windows;

namespace HospitalBaseApp.ViewModels
{
    public class GeneralDoctorViewModel : ViewModelBase
    {
        private readonly MessageBus _messageBus;

        private readonly DatabaseContext _db;

        public DoctorModel CurrentUser;

        //Прикрепленные к XAML свойства.
        public ObservableCollection<DoctorModel> Doctors
        {
            get;
            set;
        } = new();

        public ObservableCollection<PatientModel> Patients
        {
            get;
            set;
        } = new();

        public ObservableCollection<OperationModel> Operations
        {
            get;
            set;
        } = new();


        public DoctorModel SelectedDoctor { get; set; }

        public ObservableCollection<AppointmentModel> Appointments { get; set; }

        public AppointmentModel SelectedAppointment { get; set; }
        public ObservableCollection<OperationRecordModel> OperationRecords
        {
            get;
            set;
        } = new();

        public PatientModel SelectedPatient { get; set; }
        public OperationRecordModel SelectedOperationRecord { get; set; }
        public OperationModel SelectedOperation { get; set; }

        //Инициализация DI и таблиц.
        public GeneralDoctorViewModel(MessageBus messageBus, DatabaseContext db)
        {
            _messageBus = messageBus;
            _db = db;
            Doctors = _db.Doctors.Local.ToObservableCollection();
            Patients = _db.Patients.Local.ToObservableCollection();
            Operations = _db.Operations.Local.ToObservableCollection();
            OperationRecords = _db.OperationsRecords.Local.ToObservableCollection();
            Appointments = _db.Appointments.Local.ToObservableCollection();
            _messageBus.Receive<DoctorModelMessage>(this, async message =>
            {
                //Получение пользователя, за которого зашли.
                CurrentUser = message.DoctorModel;
            });
        }

       

        public ICommand ShowAddDoctorDialogCommand => new DelegateCommand(async () =>
        {
            var AddDoctor = new AddDoctorDialog();

            AddDoctor.ShowDialog();


        });

        public ICommand ShowEditDoctorDialogCommand => new DelegateCommand(async() =>
        {
            if (_db.OperationsRecords.Local.ToList().Any(x => x.Surgeon == SelectedDoctor) || _db.Appointments.Local.ToList().Any(x => x.Doctor == SelectedDoctor))
            {
                MessageBox.Show("Этот врач занят операцией либо приёмом!");
                return;
            }
            var EditDoctor = new EditDoctorDialog();
            await _messageBus.SendTo<EditDoctorViewModel>(new DoctorModelMessage(SelectedDoctor));
            EditDoctor.ShowDialog();



        }, () => SelectedDoctor != null && SelectedDoctor != CurrentUser);

        public ICommand RemoveDoctorCommand => new DelegateCommand(() =>
        {
            if (_db.OperationsRecords.Local.ToList().Any(x => x.Surgeon == SelectedDoctor) || _db.Appointments.Local.ToList().Any(x => x.Doctor == SelectedDoctor))
            {
                MessageBox.Show("Этот врач занят операцией либо приёмом!");
                return;
            }
            _db.Doctors.Remove(SelectedDoctor);


            _db.SaveChanges();



        }, () => SelectedDoctor != null && SelectedDoctor != CurrentUser);
      

        



      
        //Показ диалогов добавления
        public ICommand ShowAddOperationRecordDialogCommand => new DelegateCommand(() =>
        {

            var AddOperationRecord = new AddOperationRecordDialog();

            AddOperationRecord.ShowDialog();


        }, () => Patients.Count != 0 && Operations.Count != 0 && Doctors.Where(x => x.Role == "Хирург").ToList().Count != 0);

        public ICommand ShowAddPatientDialogCommand => new DelegateCommand(() =>
        {
            var AddPatient = new AddPatientDialog();

            AddPatient.ShowDialog();


        });

        public ICommand ShowAddOperationDialogCommand => new DelegateCommand(() =>
        {
            var AddOperation = new AddOperationDialog();

            AddOperation.ShowDialog();


        });
        public ICommand ShowAddAppointmentDialogCommand => new DelegateCommand(() =>
        {



            var AddAppointment = new AddAppointmentDialog();

            AddAppointment.ShowDialog();


        }, () => Patients.Count != 0 && Doctors.Where( x => x.Role == "Врач").ToList().Count != 0);

       
        //Показ диалогов удаления
        public ICommand RemoveAppointmentCommand => new DelegateCommand(() =>
        {

            _db.Appointments.Remove(SelectedAppointment);


            _db.SaveChanges();



        }, () => SelectedAppointment != null);
        public ICommand RemoveOperationRecordCommand => new DelegateCommand(() =>
        {

            _db.OperationsRecords.Remove(SelectedOperationRecord);


            _db.SaveChanges();



        }, () => SelectedOperationRecord != null);

        public ICommand RemoveOperationCommand => new DelegateCommand(() =>
        {
            if (_db.OperationsRecords.Local.ToList().Any(x => x.Operation == SelectedOperation))
            {
                MessageBox.Show("На эту операцию есть запись!");
                return;
            }
            _db.Operations.Remove(SelectedOperation);


            _db.SaveChanges();
        }, () => SelectedOperation != null);

        public ICommand RemovePatientCommand => new DelegateCommand(() =>
        {
            if (_db.OperationsRecords.Local.ToList().Any(x=> x.Operable == SelectedPatient) || _db.Appointments.Local.ToList().Any(x => x.Patient == SelectedPatient))
            {
                MessageBox.Show("Этот пациент записан на операцию или на приём!");
                return;
            }
            _db.Patients.Remove(SelectedPatient);


            _db.SaveChanges();
        }, () => SelectedPatient != null);

        // Показ диалогов редактирования

        public ICommand ShowEditAppointmentDialog => new DelegateCommand<AppointmentModel>(async (appointment) =>
        {


            var EditAppointment = new EditAppointmentDialog();
            await _messageBus.SendTo<EditAppointmentViewModel>(new AppointmentModelMessage(appointment));
            EditAppointment.ShowDialog();


        }, (_) => Patients.Count != 0 && SelectedAppointment != null);

        
        public ICommand ShowEditOperationRecordDialogCommand => new DelegateCommand(async () =>
        {
            var EditOperationRecord = new EditOperationRecordDialog();
            await _messageBus.SendTo<EditOperationRecordViewModel>(new OperationRecordModelMessage(SelectedOperationRecord));
            EditOperationRecord.ShowDialog();


        }, () => SelectedOperationRecord != null);
        public ICommand ShowEditOperationDialogCommand => new DelegateCommand(async () =>
        {
            if (_db.OperationsRecords.Local.ToList().Any(x => x.Operable == SelectedPatient) || _db.Appointments.Local.ToList().Any(x => x.Patient == SelectedPatient))
            {
                MessageBox.Show("На эту операцию есть запись!");
                return;
            }
            var EditOperation = new EditOperationDialog();
            await _messageBus.SendTo<EditOperationViewModel>(new OperationModelMessage(SelectedOperation));
            EditOperation.ShowDialog();


        }, () => SelectedOperation != null);

        public ICommand ShowEditPatientDialogCommand => new DelegateCommand(async () =>
        {
            if (_db.OperationsRecords.Local.ToList().Any(x => x.Operable == SelectedPatient) || _db.Appointments.Local.ToList().Any(x => x.Patient == SelectedPatient))
            {
                MessageBox.Show("Этот пациент записан на операцию или на приём!");
                return;
            }
            var EditPatient = new EditPatientDialog();
            await _messageBus.SendTo<EditPatientViewModel>(new PatientModelMessage(SelectedPatient));
            EditPatient.ShowDialog();


        }, () => SelectedPatient != null);

       

    }
}
