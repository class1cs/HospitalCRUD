using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using HospitalBaseApp.DB;
using System.ComponentModel;
using HospitalBaseApp.Services;
using HospitalBaseApp.Extensions;
using System.Windows.Input;
using HospitalBaseApp.Views;
using System.Windows;
using HospitalBaseApp.Models;

namespace HospitalBaseApp.ViewModels
{
    public class EditOperationViewModel :  ViewModelBase
    {
        private readonly DatabaseContext _db;

        public List<OperationsDifficults> OperationDifficults { get; set; } =
          Enum.GetValues<OperationsDifficults>().ToList();
        public OperationsDifficults OperationDifficult { get; set; }

        public string OperationName { get; set; }

        public int OperationId { get; set; }

        public enum OperationsDifficults
        {
            [Description("Высокая")]
            High,

            [Description("Средняя")]
            Medium,

            [Description("Низкая")]
            Low
        }



        public OperationModel Message;

        public EditOperationViewModel(DatabaseContext db, MessageBus messageBus)
        {
            _db = db;
            messageBus.Receive<OperationModelMessage>(this, async message =>
            {
                Message = message.OperationModel;
                OperationName = message.OperationModel.OperationName;
                OperationDifficult = message.OperationModel.OperationComplexity.GetValueFromDescription<OperationsDifficults>();
                OperationId = message.OperationModel.Id;
            });
        }
     

        public ICommand EditOperationCommand => new DelegateCommand<EditOperationDialog>(async (w) =>
        {
            try
            {
                OperationModel? result = await _db.Operations.FindAsync(OperationId);
                result.OperationName = OperationName;
                result.OperationComplexity = OperationDifficult.GetEnumDescription();
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Такая операция уже есть в списке!");
            }
            

        });

    }
}
