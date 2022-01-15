using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiaryWebApp.Models
{
    public class Diaries
    {
        public int Id { get; set; }
        public DateTime DateOfDiary { get; set; }
        public string TextOfDiary { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
