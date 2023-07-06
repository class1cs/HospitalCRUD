using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using PropertyChanged;
namespace HospitalBaseApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class OperationRecordModel
    {
        [Key]
        public int Id { get; set; }

        public OperationModel Operation
        {
            get;set;
        }

        public DoctorModel Surgeon
        {
            get; set;
        }

        public string Status
        {
            get; set;
        }

        public PatientModel Operable
        {
            get; set;
        }

       

    }
}
