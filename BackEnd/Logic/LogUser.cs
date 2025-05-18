using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Logic;
using BackEnd.ResAndReq.Req.User;
using BackEnd.ResAndReq.Res.User;
using Conexion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BackEnd.Logic
{
    public class LogUser
    {
        public ResAddUser Registrar(ReqAddUser req)
        {
            ResAddUser res = new ResAddUser()
            {
                Error = new List<Entities.Error>(),
                Result = false,
            }
            ;

            try
            {
                #region Validaciones
                if (req == null)
                {
                    res.Error.Add(new Error
                    {
                        ErrorCode = (int)EnumErrores.requestNulo,
                        Message = "Request nulo"
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(req.FirstName))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.nombreFaltante,
                            Message = "Nombre vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.LastName))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.apellidoFaltante,
                            Message = "Apellido vacío"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Cedula))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.cedulaFaltante,
                            Message = "Cédula vacía"
                        });
                    }

                    if (string.IsNullOrEmpty(req.Email))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoFaltante,
                            Message = "Correo vacío"
                        });
                    }
                    else if (!EsCorreoValido(req.Email))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.correoIncorrecto,
                            Message = "Correo no válido"
                        });
                    }   

                    if (string.IsNullOrEmpty(req.PhoneNumber))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.telefonoFaltante,
                            Message = "Teléfono vacío"
                        });
                    }
                }
                #endregion
   
                if (res.Error.Any())
                {
                    return res;
                }
                string password = "asdadasd";
                string llave = Guid.NewGuid().ToString("N");
                string passHash = HashearPassword(password, llave);
                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var resultado = linq.sp_RegisterUser(
                        req.Cedula,
                        req.FirstName,
                        req.LastName,
                        req.Email,
                        llave,
                        passHash,
                        req.PhoneNumber,
                        req.BirthDate,
                        req.Address,
                        "User"

                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result == "0")
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.errorDesconocido,
                            Message = resultado.Message
                        });
                    }
                    else
                    {
                        res.Result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Error.Add(new Error
                {
                    ErrorCode = (int)EnumErrores.excepcionLogica,
                    Message = ex.Message
                });
            }

            return res;
        }


        #region Helpers
        public bool EsCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            try
            {
                return Regex.IsMatch(correo,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase,
                    TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public bool EsPasswordSeguro(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            // Mínimo 8 caracteres, al menos una letra mayúscula, un número y un carácter especial
            return Regex.IsMatch(password,
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
        }

        public string GenerarPin(int longitud)
        {
            if (longitud <= 0) return string.Empty;

            Random rnd = new Random();
            StringBuilder pin = new StringBuilder();

            for (int i = 0; i < longitud; i++)
            {
                pin.Append(rnd.Next(0, 10));
            }

            return pin.ToString();
        }

        private string HashearPassword(string passwordUsuario, string key)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(passwordUsuario + key);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        #endregion
    }
}
