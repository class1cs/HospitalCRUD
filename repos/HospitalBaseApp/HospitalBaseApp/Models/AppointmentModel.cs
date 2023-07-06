using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalBaseApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class AppointmentModel
    {
        public DoctorModel Doctor { get; set; }
        public PatientModel Patient { get; set; }
        public string Ill { get; set; }
        public string Time { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
