using BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackEnd.ResAndReq.Res
{
    public class ResBase
    {
        public bool Result { get; set; }
        public List<Error> Error { get; set; }
    }
}
