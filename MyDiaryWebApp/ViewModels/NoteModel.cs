using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyDiaryWebApp.ViewModels
{
    public class NoteModel
    {
        [Required(ErrorMessage = "Incorrect date")]
        public DateTime DateOfDiary { get; set; }
    }
}
