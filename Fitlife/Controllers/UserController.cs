using BackEnd.ResAndReq.Req.User;
using BackEnd.ResAndReq.Res.User;
using BackEnd.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fitlife.Controllers
{

    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public UserController() { }

        [HttpPost]
        [Route("insert")]
        public ResAddUser insertUser(ReqAddUser req)
        {
            return new LogUser().Registrar(req);
                
         }

        [HttpPost]
        [Route("login")]
        public ResUserLogin loginUser(ReqUserLogin req)
        {
            return new LogUser().Login(req);
        }

    }
}
