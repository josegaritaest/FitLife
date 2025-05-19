using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Attendance
{
    public class ResCheckOut : ResBase
    {
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }
        public int? DurationMinutes { get; set; }
    }
}
