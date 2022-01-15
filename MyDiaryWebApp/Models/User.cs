using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiaryWebApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Diaries> Diaries { get; set; }
    }

    public static class ThisUserId
    {
        public static int thisId;
        public static DateTime thisDateOfDiary;
    }
}
