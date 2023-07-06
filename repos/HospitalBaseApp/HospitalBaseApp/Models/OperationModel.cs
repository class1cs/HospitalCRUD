using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using PropertyChanged;

namespace HospitalBaseApp.Models
{
    [AddINotifyPropertyChangedInterface]
    public class OperationModel
    {
        [Key]
        public int Id { get; set; }

       
        public string OperationName { get; set; }
        
        public string OperationComplexity { get; set; }
    }
}
