using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackEnd.Entities;
using Conexion;

namespace BackEnd.ResAndReq.Res.User
{
    public class ResUserLogin : ResBase
    {
        public Entities.User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
