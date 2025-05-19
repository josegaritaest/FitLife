using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res.Attendance
{
    public class ResAttendanceStatus : ResBase
    {
        public string AttendanceStatus { get; set; }
        public bool HasActiveCheckin { get; set; }
        public string CheckInTime { get; set; }
        public int? MinutesElapsed { get; set; }
        public string ActionAvailable { get; set; }
    }
}
