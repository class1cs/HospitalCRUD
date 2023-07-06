using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.ComponentModel;
using HospitalBaseApp.DB;
using HospitalBaseApp.Views;
using System.Windows.Input;
using HospitalBaseApp.Extensions;
using HospitalBaseApp.Services;
using HospitalBaseApp.Models;
using System.Collections.ObjectModel;
using System.Windows;

namespace HospitalBaseApp.ViewModels
{
    public class EditOperationRecordViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;
        private readonly MessageBus _messageBus;

        public int OperationId { get; set; }

        public ObservableCollection<OperationModel> OperationsToDo { get; set; }

        public OperationModel SelectedOperation { get; set; }

        public PatientModel SelectedOperable { get; set; }
        public List<PatientModel> Operables { get; set; }
        public DoctorModel SelectedDoctor { get; set; }
        public List<DoctorModel> Doctors
        {
            get;
            set;
        } = new();
        public List<OperationStatusesEnum> OperationsStatuses { get; set; } =
          Enum.GetValues<OperationStatusesEnum>().ToList();
        public OperationStatusesEnum OperationStatus { get; set; }

        public enum OperationStatusesEnum
        {
            [Description("Завершена")]
            Completed,
            [Description("Проводится")]
            Processing,
            [Description("В очереди")]
            InQueue
        }
        public EditOperationRecordViewModel(DatabaseContext db, MessageBus messageBus)
        {
            _db = db;
            _messageBus = messageBus;
            OperationsToDo = _db.Operations.Local.ToObservableCollection();
            
            Operables = _db.Patients.Local.ToList();
            Doctors = _db.Doctors.Local.Where(x => x.Role == "Хирург").ToList();
            _messageBus.Receive<OperationRecordModelMessage>(this, async message =>
            {
                SelectedOperation = OperationsToDo.FirstOrDefault(s => s == message.OperationRecordModel.Operation);
                SelectedOperable = Operables.FirstOrDefault(s => s == message.OperationRecordModel.Operable);
                OperationStatus = message.OperationRecordModel.Status.GetValueFromDescription<OperationStatusesEnum>();
                SelectedDoctor = Doctors.FirstOrDefault(x => x == message.OperationRecordModel.Surgeon);
                OperationId = message.OperationRecordModel.Id;
            });
        }

      

        public ICommand EditOperationRecordCommand => new DelegateCommand<EditOperationRecordDialog>(async (w) =>
        {
            try
            {
                var result = await _db.OperationsRecords.FindAsync(OperationId);
                result.Operation = SelectedOperation;
                ;
                result.Operable = SelectedOperable;
                result.Status = EnumDescriptionsExtensions.GetEnumDescription(OperationStatus);
                result.Surgeon = SelectedDoctor;
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Такая запись на операцию уже есть!");
            }
            
           
        });


    }
}
