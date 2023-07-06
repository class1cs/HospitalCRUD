using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalBaseApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class PatientModel
    {
        [Key]
        public int ID { get; set; }
        public string NSP { get; set; }

        public string Sex { get; set; }


        
    }
}
