using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;
using System.ComponentModel;
using System.Windows.Input;
using System.Data;
using HospitalBaseApp.DB;
using HospitalBaseApp.Views;
using HospitalBaseApp.Models;
using HospitalBaseApp.Extensions;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace HospitalBaseApp.ViewModels
{
    public class AddOperationViewModel : ViewModelBase
    {
        private readonly DatabaseContext _db;

        public List<OperationsDifficults> OperationDifficults { get; set; } =
          Enum.GetValues<OperationsDifficults>().ToList();
        public OperationsDifficults OperationDifficult { get; set; }

        public string OperationName { get; set; }

        public enum OperationsDifficults
        {
            [Description("Высокая")]
            High,

            [Description("Средняя")]
            Medium,

            [Description("Низкая")]
            Low
        }

        public AddOperationViewModel(DatabaseContext db)
        {
            _db = db;
            
        }

        public ICommand AddOperationCommand => new DelegateCommand<AddOperationDialog>(async (w) =>
        {

            try
            {
                await _db.Operations.AddAsync(new OperationModel { OperationComplexity = OperationDifficult.GetEnumDescription(), OperationName = OperationName });
                await _db.SaveChangesAsync();
                w?.Close();
            }
            catch (Exception e) {

                MessageBox.Show("Такая операция уже есть!");
            }
           
          
        }, (_) => !string.IsNullOrWhiteSpace(OperationName));
    }
}
