using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalBaseApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class DoctorModel
    {
        
        public string NSP { get; set; }

        public string PhoneNumber { get; set; }

        public string Email{get;set;}

        public string Birthday { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Role { get; set; }
    }
}
