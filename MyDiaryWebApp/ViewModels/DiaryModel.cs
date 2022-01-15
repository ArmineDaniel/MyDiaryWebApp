using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyDiaryWebApp.ViewModels
{
    public class DiaryModel
    {
        public DateTime DateOfDiary { get; set; }
        public string TextOfDiary { get; set; }
        public int UserId { get; set; }
        public int Id { get; set; }
    }
}
