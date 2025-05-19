using BackEnd.Entities;
using BackEnd.Enum;
using BackEnd.Logic;
using BackEnd.ResAndReq.Req.User;
using BackEnd.Helpers;
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
                string password = Helper.GenerarPassword(8);
                string passHash = Helper.HashearPassword(password);
                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var resultado = linq.sp_RegisterUser(
                        req.Cedula,
                        req.FirstName,
                        req.LastName,
                        req.Email,
                        passHash,
                        req.PhoneNumber,
                        req.BirthDate,
                        req.Address,
                        "User"

                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
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
        public ResUserLogin Login(ReqUserLogin req)
        {
            ResUserLogin res = new ResUserLogin()
            {
                Error = new List<Entities.Error>(),
                Result = false,
                User = new Entities.User(),
                Token = string.Empty,
                ExpiresAt = DateTime.MinValue
            };
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
                    if (string.IsNullOrEmpty(req.Email))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.cedulaFaltante,
                            Message = "Cédula vacío"
                        });
                    }
                    if (string.IsNullOrEmpty(req.Password))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordFaltante,
                            Message = "Password vacío"
                        });
                    }
                }
                #endregion
                if (res.Error.Any())
                {
                    return res;
                }
                using (FitLifeDataContext linq = new FitLifeDataContext())
                {
                    var passwordHash = linq.Users.Where(u => u.Email == req.Email).Select(u => u.PasswordHash).FirstOrDefault();
                    if (passwordHash == null)
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.usuarioNoEncontrado,
                            Message = "Usuario no encontrado"
                        });
                        return res;
                    }
                    if (!Helper.VerificarPassword(req.Password, passwordHash))
                    {
                        res.Error.Add(new Error
                        {
                            ErrorCode = (int)EnumErrores.passwordIncorrecto,
                            Message = "Password incorrecto"
                        });
                        return res;
                    }
                    var resultado = linq.sp_UserLogin(
                        req.Email,
                        req.Password
                    ).FirstOrDefault();

                    if (resultado == null || resultado.Result == "FAILED")
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
                        res.User.Cedula = resultado.Cedula;
                        res.User.FirstName = resultado.FirstName;
                        res.User.LastName = resultado.LastName;
                        res.User.Email = resultado.Email;
                        res.User.PhoneNumber = resultado.PhoneNumber;
                        res.User.BirthDate = resultado.BirthDate;
                        res.User.Address = resultado.Address;
                        res.User.Status = resultado.Status;
                        res.User.Role = resultado.RoleName;     
                        res.Token = resultado.Token;
                        res.ExpiresAt = (DateTime)resultado.ExpiresAt;
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
        #endregion

    }
}
