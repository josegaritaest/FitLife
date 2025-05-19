using BackEnd.ResAndReq.Req.Attendance;
using BackEnd.ResAndReq.Res.Attendance;
using BackEnd.Logic;
using BackEnd.Entities;
using BackEnd.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Fitlife.Controllers
{
    [RoutePrefix("api/attendance")]
    public class AttendanceController : ApiController
    {
        public AttendanceController() { }

        /// <summary>
        /// Registra la entrada del usuario al gimnasio
        /// </summary>
        /// <param name="req">Request de check-in</param>
        /// <returns>Resultado del registro de entrada</returns>
        [HttpPost]
        [Route("checkin")]
        public ResCheckIn CheckIn(ReqCheckIn req)
        {
            try
            {
                // Obtener el token del header Authorization
                string token = null;
                if (Request.Headers.Authorization != null && Request.Headers.Authorization.Scheme == "Bearer")
                {
                    token = Request.Headers.Authorization.Parameter;
                }

                if (string.IsNullOrEmpty(token))
                {
                    return new ResCheckIn
                    {
                        Result = false,
                        Error = new List<Error>
                        {
                            new Error
                            {
                                ErrorCode = (int)EnumErrores.sesionNula,
                                Message = "Token de autorización requerido"
                            }
                        }
                    };
                }

                return new LogAttendance().RegistrarEntrada(token);
            }
            catch (Exception ex)
            {
                return new ResCheckIn
                {
                    Result = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            ErrorCode = (int)EnumErrores.excepcionLogica,
                            Message = ex.Message
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Registra la salida del usuario del gimnasio
        /// </summary>
        /// <param name="req">Request de check-out</param>
        /// <returns>Resultado del registro de salida</returns>
        [HttpPost]
        [Route("checkout")]
        public ResCheckOut CheckOut(ReqCheckOut req)
        {
            try
            {
                // Obtener el token del header Authorization
                string token = null;
                if (Request.Headers.Authorization != null && Request.Headers.Authorization.Scheme == "Bearer")
                {
                    token = Request.Headers.Authorization.Parameter;
                }

                if (string.IsNullOrEmpty(token))
                {
                    return new ResCheckOut
                    {
                        Result = false,
                        Error = new List<Error>
                        {
                            new Error
                            {
                                ErrorCode = (int)EnumErrores.sesionNula,
                                Message = "Token de autorización requerido"
                            }
                        }
                    };
                }

                return new LogAttendance().RegistrarSalida(token);
            }
            catch (Exception ex)
            {
                return new ResCheckOut
                {
                    Result = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            ErrorCode = (int)EnumErrores.excepcionLogica,
                            Message = ex.Message
                        }
                    }
                };
            }
        }

        /// <summary>
        /// Obtiene el estado actual de asistencia del usuario
        /// </summary>
        /// <returns>Estado actual de asistencia</returns>
        [HttpGet]
        [Route("status")]
        public ResAttendanceStatus GetAttendanceStatus()
        {
            try
            {
                // Obtener el token del header Authorization
                string token = null;
                if (Request.Headers.Authorization != null && Request.Headers.Authorization.Scheme == "Bearer")
                {
                    token = Request.Headers.Authorization.Parameter;
                }

                if (string.IsNullOrEmpty(token))
                {
                    return new ResAttendanceStatus
                    {
                        Result = false,
                        Error = new List<Error>
                        {
                            new Error
                            {
                                ErrorCode = (int)EnumErrores.sesionNula,
                                Message = "Token de autorización requerido"
                            }
                        }
                    };
                }

                return new LogAttendance().ObtenerEstadoAsistencia(token);
            }
            catch (Exception ex)
            {
                return new ResAttendanceStatus
                {
                    Result = false,
                    Error = new List<Error>
                    {
                        new Error
                        {
                            ErrorCode = (int)EnumErrores.excepcionLogica,
                            Message = ex.Message
                        }
                    }
                };
            }
        }
    }
}