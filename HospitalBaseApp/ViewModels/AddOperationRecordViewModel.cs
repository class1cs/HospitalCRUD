using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using DevExpress.Mvvm;
using System.Windows.Input;
using HospitalBaseApp.Views;
using HospitalBaseApp.DB;
using HospitalBaseApp.Models;
using System.ComponentModel;
using HospitalBaseApp.Services;
using HospitalBaseApp.Extensions;
using System.Collections.ObjectModel;
using System.Windows;

namespace HospitalBaseApp.ViewModels
{

    public class AddOperationRecordViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;

        public List<PatientModel> Operables { get; set; }
        public List<OperationModel> OperationsToDo { get; set; }

        public PatientModel SelectedOperable { get; set; }

        public OperationModel SelectedOperation { get; set; }


        public List<DoctorModel> Doctors
        {
            get;
            set;
        } = new();
        
        public DoctorModel SelectedDoctor { get; set; }

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

        public AddOperationRecordViewModel(DatabaseContext db, MessageBus messageBus)
        {
            _db = db;
            OperationsToDo = _db.Operations.Local.ToList();
            Operables = _db.Patients.Local.ToList();
            Doctors = _db.Doctors.Local.Where(x => x.Role == "Хирург").ToList();
        }

        public ICommand AddOperationRecordCommand => new DelegateCommand<AddOperationRecordDialog>(async(w) =>
        {
            try
            {
                await _db.OperationsRecords.AddAsync(new OperationRecordModel { Operable = SelectedOperable, Operation = SelectedOperation, Status = EnumDescriptionsExtensions.GetEnumDescription(OperationStatus), Surgeon = SelectedDoctor });
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Такая запись на операцию уже есть!");
            }
        }, (_) => Operables.Count > 0 && OperationsToDo.Count > 0);

       
    }
}
