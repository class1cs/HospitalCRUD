using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using DevExpress.Mvvm;
using System.Windows.Input;
using HospitalBaseApp.Views;
using HospitalBaseApp.Services;
using HospitalBaseApp.DB;
using HospitalBaseApp.Models;
using System.Collections.ObjectModel;

namespace HospitalBaseApp.ViewModels
{
    public class SurgeonViewModel : ViewModelBase
    {
        private readonly MessageBus _messageBus;

        private DatabaseContext _db;
        public ObservableCollection<OperationRecordModel> OperationRecords
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

        public ObservableCollection<DoctorModel> Doctors
        {
            get;
            set;
        } = new();
        public OperationRecordModel SelectedOperationRecord { get; set; }
        public SurgeonViewModel(MessageBus messageBus, DatabaseContext db)
        {
            _messageBus = messageBus;
            _db = db;

            Patients = _db.Patients.Local.ToObservableCollection();
            OperationRecords = _db.OperationsRecords.Local.ToObservableCollection();
            Operations = _db.Operations.Local.ToObservableCollection();
            Doctors = _db.Doctors.Local.ToObservableCollection();

        }

        public ICommand RemoveOperationRecordCommand => new DelegateCommand(() =>
        {

            _db.OperationsRecords.Remove(SelectedOperationRecord);


            _db.SaveChanges();



        }, () => SelectedOperationRecord != null);

        public ICommand ShowEditOperationRecordDialogCommand => new DelegateCommand(async () =>
        {
            var EditOperationRecord = new EditOperationRecordDialog();
            await _messageBus.SendTo<EditOperationRecordViewModel>(new OperationRecordModelMessage(SelectedOperationRecord));
            EditOperationRecord.ShowDialog();


        }, () => SelectedOperationRecord != null);

        public ICommand ShowAddOperationRecordDialogCommand => new DelegateCommand(() =>
        {
            var AddOperationRecord = new AddOperationRecordDialog();

            AddOperationRecord.ShowDialog();


        }, () => Patients.Count != 0 && Operations.Count != 0 && Doctors.Where(x => x.Role == "Хирург").ToList().Count != 0);
    }
}
