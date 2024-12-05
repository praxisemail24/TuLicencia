using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using SmartLicencia.Models;
using SmartLicencia.Entity;
using System.Text;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing;

namespace SmartLicencia.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _config;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPagoRepository _pagoRepository;

        public EmailRepository(IConfiguration config, IClienteRepository cr, IPagoRepository pagor)
        {
            _config = config;
            _clienteRepository = cr;
            _pagoRepository = pagor;
        }

        public async Task<(bool success, string errorMessage)> EmailNewPass(Email request)
        {
            try
            {
                Cliente cliente = await _clienteRepository.GetClienteByCorreo2Async(request.Para);
                if (cliente == null)
                {
                    return (false, "Usuario no encontrado");
                }
                string nombreCliente = cliente.cl_nombre + " " + cliente.cl_primerApellido + " " + cliente.cl_segundoApellido;
                string usuarioCliente = cliente.cl_nombreUsuario;

                string keyTemporal = Guid.NewGuid().ToString("N").Substring(0, 14);
                cliente.cl_keyTemporal = keyTemporal;
                await _clienteRepository.UpdateClienteKeyTemporal(cliente.cl_id, keyTemporal);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
                email.To.Add(MailboxAddress.Parse(request.Para));
                email.Subject = "Restablecer Contraseña"; // request.Asunto;

                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $@"
                        <!DOCTYPE html>
                        <html lang='es'>
                          <head>
                            <meta charset='UTF-8' />
                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                            <title>Gracias por subir tus documentos</title>
                            <style>
                              @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

                              body {{
                                margin: 0;
                                padding: 0;
                                font-family: 'Poppins', Arial, sans-serif;
                                background-color: #f6f6f6;
                              }}

                              h1 {{
                                font-size: 24px;
                                color: #5342a6;
                                margin: 0 0 20px 0;
                              }}

                              h2 {{
                                font-size: 28px;
                                font-weight: 700;
                                color: #5342a6;
                                margin: 0 0 0px 0;
                              }}

                              p {{
                                color: #333333;
                              }}

                              .greeting {{
                                font-size: 20px;
                                font-weight: 500;
                              }}

                              .btn {{
                                color: #ffffff;
                                background-color: #4caf50;
                                text-decoration: none;
                                border-radius: 5px;
                              }}

                              .footer-title {{
                                font-size: 24.897px;
                                font-weight: 700;
                              }}

                              .footer-subtitle {{
                                font-size: 11.499px;
                                font-weight: 400;
                              }}

                              .footer-icons img {{
                                padding: 0 10px;
                                width: 20px;
                                height: auto;
                                display: inline-block;
                                border: 0;
                              }}

                              .highlight {{
                                font-weight: 700;
                                color: #5342a6;
                              }}

                              .link {{
                                color: #4caf50;
                                text-decoration: none;
                              }}
                            </style>
                          </head>
                          <body>
                            <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                              <tr>
                                <td align='center' style='padding: 20px 0'>
                                  <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                    <tr>
                                      <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                        <h1>Tu licencia</h1>
                                        <h2>RESTABLECER CONTRASEÑA</h2>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td style='padding: 0px 30px 40px 30px'>                   
                                        <p class='greeting'>¡Hola {nombreCliente}!</p>
                                        <p>Ud desea restablecer su contraseña.</p>
                                        <p>
                                          <a href='https://app.tulicenciapr.com/cambiarcontrasena?{keyTemporal}' class='btn' style='display: inline-block; padding: 10px 20px; margin: 20px 0 0 0; font-size: 16px; text-decoration: none; color: white;'>
                                            Click para cambiar su contraseña
                                          </a>
                                        </p>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                        <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                        <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                        <div class='footer-icons' style='margin-top: 20px'>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                        </div>
                                        <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>2024 Todos los derechos reservados</p>
                                      </td>
                                    </tr>
                                  </table>
                                </td>
                              </tr>
                            </table>
                          </body>
                        </html>"
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient(); // {request.Contenido}

                smtp.Connect(
                    _config.GetSection("Email:Host").Value,
                    Convert.ToInt32(_config.GetSection("Email:Port").Value),
                    SecureSocketOptions.StartTls
                    );

                smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                return (true, string.Empty);
            }
            catch (SqlException sqlEx)
            {
                return (false, $"Error de base de datos: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        public async Task<(bool success, string errorMessage)> EmailNewUsuario(Email request)
        {
            try
            {
                Cliente cliente = await _clienteRepository.GetClienteByCorreo2Async(request.Para);
                if (cliente == null)
                {
                    return (false, "Usuario no encontrado");
                }
                string nombreCliente = cliente.cl_nombre + " " + cliente.cl_primerApellido + " " + cliente.cl_segundoApellido;
                string usuarioCliente = cliente.cl_nombreUsuario;

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
                email.To.Add(MailboxAddress.Parse(request.Para));
                email.Subject = "Confirmacion de Usuario"; // request.Asunto;

                email.Body = new TextPart(TextFormat.Html)
                {

                    Text = $@"
                    <!DOCTYPE html>
                    <html lang='es'>
                        <head>
                        <meta charset='UTF-8' />
                        <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                        <title>Gracias por subir tus documentos</title>
                        <style>
                            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

                            body {{
                            margin: 0;
                            padding: 0;
                            font-family: 'Poppins', Arial, sans-serif;
                            background-color: #f6f6f6;
                            }}

                            h1 {{
                            font-size: 24px;
                            color: #5342a6;
                            margin: 0 0 20px 0;
                            }}

                            h2 {{
                            font-size: 28px;
                            font-weight: 700;
                            color: #5342a6;
                            margin: 0 0 0px 0;
                            }}

                            p {{
                            color: #333333;
                            }}

                            .greeting {{
                            font-size: 20px;
                            font-weight: 500;
                            }}

                            .btn {{
                            color: #ffffff;
                            background-color: #4caf50;
                            text-decoration: none;
                            border-radius: 5px;
                            }}

                            .footer-title {{
                            font-size: 24.897px;
                            font-weight: 700;
                            }}

                            .footer-subtitle {{
                            font-size: 11.499px;
                            font-weight: 400;
                            }}

                            .footer-icons img {{
                            padding: 0 10px;
                            width: 20px;
                            height: auto;
                            display: inline-block;
                            border: 0;
                            }}

                            .highlight {{
                            font-weight: 700;
                            color: #5342a6;
                            }}

                            .link {{
                            color: #4caf50;
                            text-decoration: none;
                            }}
                        </style>
                        </head>
                        <body>
                        <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                            <tr>
                            <td align='center' style='padding: 20px 0'>
                                <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                <tr>
                                    <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                    <h1>Tu licencia</h1>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 0px 30px 40px 30px'>                   
                                    <p class='greeting'>¡Hola {nombreCliente}!</p>
                                            <p>  Le enviamos su usuario </p>
                                            <p>  Usuario: {usuarioCliente}  </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                    <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                    <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                    <div class='footer-icons' style='margin-top: 20px'>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                    </div>
                                    <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>2024 Todos los derechos reservados</p>
                                    </td>
                                </tr>
                                </table>
                            </td>
                            </tr>
                        </table>
                        </body>
                    </html>"
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient(); // {request.Contenido}

                smtp.Connect(
                    _config.GetSection("Email:Host").Value,
                    Convert.ToInt32(_config.GetSection("Email:Port").Value),
                    SecureSocketOptions.StartTls
                    );

                smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
                return (true, string.Empty);
            }
            catch (SqlException sqlEx)
            {
                return (false, $"Error de base de datos: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error inesperado: {ex.Message}");
            }
        }

        public void EmailConfirmacionCambioPass(Cliente cliente)
        {
            string nombreCliente = cliente.cl_nombre + " " + cliente.cl_primerApellido + " " + cliente.cl_segundoApellido;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(cliente.cl_correo));
            email.Subject = "Confirmación Cambio Contraseña"; // request.Asunto;

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
                    <!DOCTYPE html>
                    <html lang='es'>
                        <head>
                        <meta charset='UTF-8' />
                        <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                        <title>Gracias por subir tus documentos</title>
                        <style>
                            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

                            body {{
                            margin: 0;
                            padding: 0;
                            font-family: 'Poppins', Arial, sans-serif;
                            background-color: #f6f6f6;
                            }}

                            h1 {{
                            font-size: 24px;
                            color: #5342a6;
                            margin: 0 0 20px 0;
                            }}

                            h2 {{
                            font-size: 28px;
                            font-weight: 700;
                            color: #5342a6;
                            margin: 0 0 0px 0;
                            }}

                            p {{
                            color: #333333;
                            }}

                            .greeting {{
                            font-size: 20px;
                            font-weight: 500;
                            }}

                            .btn {{
                            color: #ffffff;
                            background-color: #4caf50;
                            text-decoration: none;
                            border-radius: 5px;
                            }}

                            .footer-title {{
                            font-size: 24.897px;
                            font-weight: 700;
                            }}

                            .footer-subtitle {{
                            font-size: 11.499px;
                            font-weight: 400;
                            }}

                            .footer-icons img {{
                            padding: 0 10px;
                            width: 20px;
                            height: auto;
                            display: inline-block;
                            border: 0;
                            }}

                            .highlight {{
                            font-weight: 700;
                            color: #5342a6;
                            }}

                            .link {{
                            color: #4caf50;
                            text-decoration: none;
                            }}
                        </style>
                        </head>
                        <body>
                        <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                            <tr>
                            <td align='center' style='padding: 20px 0'>
                                <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                <tr>
                                    <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                    <h1>Tu licencia</h1>
                                    <h2>CONFIRMACION DE CAMBIO DE CONTRASEÑA</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 0px 30px 40px 30px'>
                                    <p class='greeting'>¡Hola {nombreCliente}!</p>
                                    <p> Se confirma su cambio de contraseña</p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                    <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                    <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                    <div class='footer-icons' style='margin-top: 20px'>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                    </div>
                                    <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>2024 Todos los derechos reservados</p>
                                    </td>
                                </tr>
                                </table>
                            </td>
                            </tr>
                        </table>
                        </body>
                    </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
                );

            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void EmailRegistro(Cliente cliente)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(cliente.cl_correo));
            email.Subject = "Confirmación de Registro exitoso en TuLicencia";
            email.Body = new TextPart(TextFormat.Html)
            {

                Text = $@"
                    <!DOCTYPE html>
                    <html lang='es'>
                        <head>
                        <meta charset='UTF-8' />
                        <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                        <title>Gracias por subir tus documentos</title>
                        <style>
                            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

                            body {{
                            margin: 0;
                            padding: 0;
                            font-family: 'Poppins', Arial, sans-serif;
                            background-color: #f6f6f6;
                            }}

                            h1 {{
                            font-size: 24px;
                            color: #5342a6;
                            margin: 0 0 20px 0;
                            }}

                            h2 {{
                            font-size: 28px;
                            font-weight: 700;
                            color: #5342a6;
                            margin: 0 0 0px 0;
                            }}

                            p {{
                            color: #333333;
                            }}

                            .greeting {{
                            font-size: 20px;
                            font-weight: 500;
                            }}

                            .btn {{
                            color: #ffffff;
                            background-color: #4caf50;
                            text-decoration: none;
                            border-radius: 5px;
                            }}

                            .footer-title {{
                            font-size: 24.897px;
                            font-weight: 700;
                            }}

                            .footer-subtitle {{
                            font-size: 11.499px;
                            font-weight: 400;
                            }}

                            .footer-icons img {{
                            padding: 0 10px;
                            width: 20px;
                            height: auto;
                            display: inline-block;
                            border: 0;
                            }}

                            .highlight {{
                            font-weight: 700;
                            color: #5342a6;
                            }}

                            .link {{
                            color: #4caf50;
                            text-decoration: none;
                            }}
                        </style>
                        </head>
                        <body>
                        <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                            <tr>
                            <td align='center' style='padding: 20px 0'>
                                <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                <tr>
                                    <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                    <h1>Tu licencia</h1>
                                    <h2>Gracias por registrarte</h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 0px 30px 40px 30px'>                   
                                    <p class='greeting'>¡Hola {cliente.cl_nombre}!</p>                                                                 
                                    <p>
                                            Gracias por registrarte en Tu Licencia, hemos recibido tus datos de manera exitosa. Estamos
                                            comprometidos en brindarte la mejor experiencia posible mientras avanzas con tus trámites.

                                            
                                        </p>
                                            <br /><br />   
 <p
                                              style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'
                                            >
                                              Si tienes alguna pregunta o necesitas asistencia adicional, no
                                              dudes en contactarnos. Gracias por confiar en nosotros.
                                            </p> 
                                         <a
                                                          href='http://app.tulicenciapr.com/'
                                                          target='_blank'
                                                          class='btn'
                                                          style='
                                                            display: inline-block;
                                                            padding: 10px 20px;
                                                            margin: 20px 0 0 0;
                                                            font-size: 16px;
                                                            text-decoration: none;
                                                            color: white;
                                                          '
                                                          >Ingresar ahora</a
                                                        >
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                    <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                    <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                    <div class='footer-icons' style='margin-top: 20px'>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                    </div>
                                    <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>2024 Todos los derechos reservados</p>
                                    </td>
                                </tr>
                                </table>
                            </td>
                            </tr>
                        </table>
                        </body>
                    </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
                );
            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void EmailConfirmacionFormulario(Cliente cliente, string nombreForm)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(cliente.cl_correo));
            email.Subject = "¡" + cliente.cl_nombre + ", llenaste el formulario correctamente";

            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
                <!DOCTYPE html>
<html lang='es'>
  <head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Gracias por llenar tu formulario</title>
    <style>
      @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

      body {{
        margin: 0;
        padding: 0;
        font-family: 'Poppins', Arial, sans-serif;
        background-color: #f6f6f6;
      }}

      h1 {{
        font-size: 24px;
        color: #5342a6;
        margin: 0 0 20px 0;
      }}

      h2 {{
        font-size: 28px;
        font-weight: 700;
        color: #5342a6;
        margin: 0 0 0px 0;
      }}

      p {{
        color: #333333;
      }}

      .greeting {{
        font-size: 20px;
        font-weight: 500;
      }}

      .btn {{
        color: #ffffff;
        background-color: #4caf50;
        text-decoration: none;
        border-radius: 5px;
      }}

      .footer-title {{
        font-size: 24.897px;
        font-weight: 700;
      }}

      .footer-subtitle {{
        font-size: 11.499px;
        font-weight: 400;
      }}

      .footer-icons img {{
        padding: 0 10px;
        width: 20px;
        height: auto;
        display: inline-block;
        border: 0;
      }}

      .highlight {{
        font-weight: 700;
        color: #5342a6;
      }}

      .link {{
        color: #4caf50;
        text-decoration: none;
      }}
    </style>
  </head>
  <body>
    <table
      role='presentation'
      style='
        width: 100%;
        border-collapse: collapse;
        border: 0;
        border-spacing: 0;
        background-color: #f6f6f6;
      '
    >
      <tr>
        <td align='center' style='padding: 20px 0'>
          <table
            role='presentation'
            style='
              width: 600px;
              border-collapse: collapse;
              border: 1px solid #cccccc;
              border-spacing: 0;
              background-color: #ffffff;
            '
          >
            <tr>
              <td
                align='left'
                style='padding: 40px 0 0px 30px; background-color: #ffffff'
              >
                <h1>Tu licencia</h1>
                <h2>Gracias por llenar tu formulario</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 0px 30px 40px 30px'>
                <p class='greeting'>¡Hola {cliente.cl_nombre}!</p>
                <p
                  style='
                    margin: 16px 0 20px 0;
                    font-size: 16px;
                    line-height: 24px;
                  '
                >
                  Gracias por completar los datos en el formulario de
                  <span class='highlight'>{nombreForm}</span>. Te
                  informamos que toda la información ha sido recibida
                  correctamente.
                </p>
                <p
                  style='
                    margin: 16px 0 20px 0;
                    font-size: 16px;
                    line-height: 24px;
                  '
                >
                  El siguiente y último paso para finalizar tu proceso de
                  tu {nombreForm} es subir los documentos requeridos en
                  formato <span href='#' class='link'>PDF</span> o
                  <span href='#' class='link'>imagen</span>. Una vez recibidos,
                  estaremos listos para proceder con tu trámite.
                </p>
                <p
                  style='
                    margin: 16px 0 20px 0;
                    font-size: 16px;
                    line-height: 24px;
                  '
                >
                  Si tienes alguna pregunta o necesitas asistencia durante este
                  proceso, no dudes en ponerte en contacto con nosotros.
                </p>
                <a
                  href='http://app.tulicenciapr.com/'
                  class='btn'
                  style='
                    display: inline-block;
                    padding: 10px 20px;
                    margin: 20px 0 0 0;
                    font-size: 16px;
                    text-decoration: none;
                    color: white;
                  '
                  >CONTINÚA CON EL PROCESO AQUÍ</a
                >
                <p
                  style='margin: 20px 0 0 0; font-size: 16px; line-height: 24px'
                >
                  Gracias nuevamente por confiar en nosotros y por tu paciencia.
                </p>
              </td>
            </tr>
            <tr>
              <td
                style='padding: 30px; background-color: #eff1fe'
                align='center'
              >
                <p
                  class='footer-title'
                  style='color: #5342a6; padding: 0px; margin: 0px'
                >
                  Tu licencia
                </p>
                <p
                  class='footer-subtitle'
                  style='padding: 0px; margin: 6px 0px 0px 0px'
                >
                  Olvídate de las filas y esperas
                </p>
                <div class='footer-icons' style='margin-top: 20px'>
                  <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                </div>
                <p
                  style='
                    margin: 16px 0 0 0;
                    font-size: 12px;
                    line-height: 16px;
                    color: #333333;
                  '
                >
                  2024 Todos los derechos reservados
                </p>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
                );
            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void EmailConfirmacionArchivos(Cliente cliente)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(cliente.cl_correo));
            email.Subject = "¡" + cliente.cl_nombre + ", estamos a un paso de obtener tu Licencia!";
            email.Body = new TextPart(TextFormat.Html)
            {                
                Text = $@"
            <!DOCTYPE html>
<html lang='es'>
  <head>
    <meta charset='UTF-8' />
    <meta name='viewport' content='width=device-width, initial-scale=1.0' />
    <title>Gracias por subir tus documentos</title>
    <style>
      @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

      body {{
        margin: 0;
        padding: 0;
        font-family: 'Poppins', Arial, sans-serif;
        background-color: #f6f6f6;
      }}

      h1 {{
        font-size: 24px;
        color: #5342a6;
        margin: 0 0 20px 0;
      }}

      h2 {{
        font-size: 28px;
        font-weight: 700;
        color: #5342a6;
        margin: 0 0 0px 0;
      }}

      p {{
        color: #333333;
      }}

      .greeting {{
        font-size: 20px;
        font-weight: 500;
      }}

      .btn {{
        color: #ffffff;
        background-color: #4caf50;
        text-decoration: none;
        border-radius: 5px;
      }}

      .footer-title {{
        font-size: 24.897px;
        font-weight: 700;
      }}

      .footer-subtitle {{
        font-size: 11.499px;
        font-weight: 400;
      }}

      .footer-icons img {{
        padding: 0 10px;
        width: 20px;
        height: auto;
        display: inline-block;
        border: 0;
      }}

      .highlight {{
        font-weight: 700;
        color: #5342a6;
      }}

      .link {{
        color: #4caf50;
        text-decoration: none;
      }}
    </style>
  </head>
  <body>
    <table
      role='presentation'
      style='
        width: 100%;
        border-collapse: collapse;
        border: 0;
        border-spacing: 0;
        background-color: #f6f6f6;
      '
    >
      <tr>
        <td align='center' style='padding: 20px 0'>
          <table
            role='presentation'
            style='
              width: 600px;
              border-collapse: collapse;
              border: 1px solid #cccccc;
              border-spacing: 0;
              background-color: #ffffff;
            '
          >
            <tr>
              <td
                align='left'
                style='padding: 40px 0 0px 30px; background-color: #ffffff'
              >
                <h1>Tu licencia</h1>
                <h2>Gracias por subir tus documentos</h2>
              </td>
            </tr>
            <tr>
              <td style='padding: 0px 30px 40px 30px'>
                <p class='greeting'>¡Hola {cliente.cl_nombre}!</p>
                <p
                  style='
                    margin: 16px 0 20px 0;
                    font-size: 16px;
                    line-height: 24px;
                  '
                >
                  Hemos recibido exitosamente tus datos del formulario así como
                  los documentos requeridos para culminar tu trámite solicitado.
                </p>
                <p
                  style='
                    margin: 16px 0 20px 0;
                    font-size: 16px;
                    line-height: 24px;
                  '
                >
                  En las próximas horas, el tramitador asignado a tu caso se
                  pondrá en contacto contigo para informarte sobre los
                  siguientes pasos a seguir y para brindarte cualquier
                  asistencia adicional que puedas necesitar.
                </p>
                <p
                  style='
                    margin: 16px 0 20px 0;
                    font-size: 16px;
                    line-height: 24px;
                  '
                >
                  No dudes en contactarnos si tienes alguna pregunta o inquietud
                  en el proceso.
                </p>
                <a
                  href='http://app.tulicenciapr.com/'
                  class='btn'
                  style='
                    display: inline-block;
                    padding: 10px 20px;
                    margin: 20px 0 0 0;
                    font-size: 16px;
                    text-decoration: none;
                    color: white;
                  '
                  >EXPLORAR MÁS TRÁMITES</a
                >
                <p
                  style='margin: 20px 0 0 0; font-size: 16px; line-height: 24px'
                >
                  Gracias por confiar en nosotros.
                </p>
                <p
                  style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'
                >
                </p>
              </td>
            </tr>
            <tr>
              <td
                style='padding: 30px; background-color: #eff1fe'
                align='center'
              >
                <p
                  class='footer-title'
                  style='color: #5342a6; padding: 0px; margin: 0px'
                >
                  Tu licencia
                </p>
                <p
                  class='footer-subtitle'
                  style='padding: 0px; margin: 6px 0px 0px 0px'
                >
                  Olvídate de las filas y esperas
                </p>
                <div class='footer-icons' style='margin-top: 20px'>
                   <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                </div>
                <p
                  style='
                    margin: 16px 0 0 0;
                    font-size: 12px;
                    line-height: 16px;
                    color: #333333;
                  '
                >
                  2024 Todos los derechos reservados
                </p>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </body>
</html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
                );
            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public async Task<ResponseEntity> EmailPago(Pago pago)
        {
            ResponseEntity response = new ResponseEntity();
            try
            {
                string nombreArchivo = $"pago_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                await _pagoRepository.GenerarReciboDePagoPDF(pago.pg_codigo, nombreArchivo);
                var rutaArchivoPDF = Path.Combine("", nombreArchivo);

                if (File.Exists(rutaArchivoPDF))
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
                    email.To.Add(MailboxAddress.Parse(pago.Email));
                    email.Subject = "¡" + pago.Nombre + ", hemos recibido tu pago! \U0001F601";
                    var bodyBuilder = new BodyBuilder();

                    var TextBody = $@"
                    <!DOCTYPE html>
                    <html lang='es'>
                        <head>
                        <meta charset='UTF-8' />
                        <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                        <title>Gracias por subir tus documentos</title>
                        <style>
                            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');

                            body {{
                            margin: 0;
                            padding: 0;
                            font-family: 'Poppins', Arial, sans-serif;
                            background-color: #f6f6f6;
                            }}

                            h1 {{
                            font-size: 24px;
                            color: #5342a6;
                            margin: 0 0 20px 0;
                            }}

                            h2 {{
                            font-size: 28px;
                            font-weight: 700;
                            color: #5342a6;
                            margin: 0 0 0px 0;
                            }}

                            p {{
                            color: #333333;
                            }}

                            .greeting {{
                            font-size: 20px;
                            font-weight: 500;
                            }}

                            .btn {{
                            color: #ffffff;
                            background-color: #4caf50;
                            text-decoration: none;
                            border-radius: 5px;
                            }}

                            .footer-title {{
                            font-size: 24.897px;
                            font-weight: 700;
                            }}

                            .footer-subtitle {{
                            font-size: 11.499px;
                            font-weight: 400;
                            }}

                            .footer-icons img {{
                            padding: 0 10px;
                            width: 20px;
                            height: auto;
                            display: inline-block;
                            border: 0;
                            }}

                            .highlight {{
                            font-weight: 700;
                            color: #5342a6;
                            }}

                            .link {{
                            color: #4caf50;
                            text-decoration: none;
                            }}
                        </style>
                        </head>
                        <body>
                        <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                            <tr>
                            <td align='center' style='padding: 20px 0'>
                                <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                <tr>
                                    <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                    <h1>Tu licencia</h1>
                                    </td>
                                </tr>
                                <tr>
                                    <td style='padding: 0px 30px 40px 30px'>                   
                                        <p class='greeting'>¡Hola {pago.Nombre}!</p>   
                                                <p>Hemos recibido exitosamente la confirmación de tu pago {pago.pg_codigo}. A continuación, te enviamos un
                                                    resumen de tu compra:
                                                    <br /><br />
                                                    Pedido #101</p>   
<table
                                        style='
                                        width: 100%;
                                        border-collapse: collapse;
                                        margin-bottom: 20px;
                                      '
                                    >
                                      <thead>
                                        <tr>
                                          <th style='border: 1px solid #cccccc; padding: 8px'>
                                            Tramite
                                          </th>
                                          <th style='border: 1px solid #cccccc; padding: 8px'>
                                            Precio
                                          </th>
                                          <th style='border: 1px solid #cccccc; padding: 8px'>
                                            Total
                                          </th>
                                        </tr>
                                      </thead>
                                      <tbody>
                                        <tr>
                                          <td
                                            style='border: 1px solid #cccccc; padding: 8px; text-align: center'
                                          >
                                            {pago.Description}
                                          </td>
                                          <td
                                            style='border: 1px solid #cccccc; padding: 8px; text-align: center'
                                          >
                                           {pago.Monto}
                                          </td>
                                          <td
                                            style='border: 1px solid #cccccc; padding: 8px; text-align: center'
                                          >
                                            {pago.Monto}
                                          </td>
                                        </tr>
                                      </tbody>
                                    </table>
                                    </td>

                                </tr>
                                <tr>
                                    <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                    <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                    <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                    <div class='footer-icons' style='margin-top: 20px'>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                        <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                    </div>
                                    <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>2024 Todos los derechos reservados</p>
                                    </td>
                                </tr>
                                </table>
                            </td>
                            </tr>
                        </table>
                        </body>
                    </html>";
                    
                    bodyBuilder.HtmlBody = TextBody;

                    var attachment = new MimePart("application", "pdf")
                    {
                        ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(rutaArchivoPDF)
                    };

                    using (var fileStream = File.OpenRead(rutaArchivoPDF))
                    {
                        attachment.Content = new MimeContent(fileStream, ContentEncoding.Default);
                        bodyBuilder.Attachments.Add(attachment);
                        email.Body = bodyBuilder.ToMessageBody();

                        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                        {
                            smtp.Connect(
                                _config.GetSection("Email:Host").Value,
                                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                                SecureSocketOptions.StartTls
                                );
                            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
                            smtp.Send(email);
                            smtp.Disconnect(true);
                        }
                    }
                    File.Delete(rutaArchivoPDF);
                }
                else
                {
                    Console.WriteLine("El archivo PDF no se encontró en la ruta especificada.");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Console.WriteLine("Se borro el pdf generado");
            }
            //await Task.Delay(1000);
            return response;
        }

        public ResponseEntity EmailPagoX(Pago pago)
        {
            ResponseEntity response = new ResponseEntity();
            try
            {
                string nombreArchivo = $"pago_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                _pagoRepository.GenerarReciboDePagoPDFM(nombreArchivo, pago);
                var rutaarchivopdf = Path.Combine("", nombreArchivo);

                if (File.Exists(rutaarchivopdf))
                {
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
                    email.To.Add(MailboxAddress.Parse(pago.Email));
                    email.Subject = "¡" + pago.Nombre + ", hemos recibido tu pago! \U0001F601";
                    var bodyBuilder = new BodyBuilder();

                    var TextBody = $@"
                        <!DOCTYPE html>
                        <html lang='es'>
                        <head>
                            <meta charset='UTF-8' />
                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                            <title>Confirmación de pago total</title>
                            <style>
                              @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');
                              body {{
                                margin: 0;
                                padding: 0;
                                font-family: 'Poppins', Arial, sans-serif;
                                background-color: #f6f6f6;
                              }}
                              h1 {{
                                font-size: 24px;
                                color: #5342a6;
                                margin: 0 0 20px 0;
                              }}
                              h2 {{
                                font-size: 28px;
                                font-weight: 700;
                                color: #5342a6;
                                margin: 0 0 0px 0;
                              }}
                              p {{
                                color: #333333;
                              }}
                              .greeting {{
                                font-size: 20px;
                                font-weight: 500;
                              }}
                              .btn {{
                                color: #ffffff;
                                background-color: #4caf50;
                                text-decoration: none;
                                border-radius: 5px;
                              }}
                              .footer-title {{
                                font-size: 24.897px;
                                font-weight: 700;
                              }}
                              .footer-subtitle {{
                                font-size: 11.499px;
                                font-weight: 400;
                              }}
                              .footer-icons img {{
                                padding: 0 10px;
                                width: 20px;
                                height: auto;
                                display: inline-block;
                                border: 0;
                              }}
                            </style>
                        </head>
                        <body>
                            <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                              <tr>
                                <td align='center' style='padding: 20px 0'>
                                  <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                    <tr>
                                      <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                        <h1>Tu licencia</h1>
                                        <h2>Confirmación de pago total</h2>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td style='padding: 0px 30px 40px 30px'>
                                        <p class='greeting'>¡Hola, {pago.Nombre}!</p>
                                        <p style='margin: 16px 0 20px 0; font-size: 16px; line-height: 24px;'>
                                          Hemos recibido exitosamente la confirmación de tu pago {pago.pg_codigo}. A continuación, te enviamos un resumen de tu compra:
                                        </p>
                                        <table align='left' style='width: 100%; border-collapse: collapse; margin-bottom: 20px;'>
                                          <thead>
                                            <tr>
                                              <th style='border: 1px solid #cccccc; padding: 8px'>Trámite</th>
                                              <th style='border: 1px solid #cccccc; padding: 8px'>Descripción</th>
                                              <th style='border: 1px solid #cccccc; padding: 8px'>Monto</th>
                                              <th style='border: 1px solid #cccccc; padding: 8px'>Método de pago</th>
                                              <th style='border: 1px solid #cccccc; padding: 8px'>Total</th>
                                            </tr>
                                          </thead>
                                          <tbody>
                                            <tr>
                                              <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{pago.pg_codigo}</td>
                                              <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{pago.Description}</td>
                                              <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{pago.Monto}</td>
                                              <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>Visa 8887</td>
                                              <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{pago.Monto}</td>
                                            </tr>
                                          </tbody>
                                        </table>
                                        <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'>
                                          El siguiente paso es completar tus datos en el formulario correspondiente aquí. Una vez completado,
                                          estaremos en posición de avanzar con tus trámites.<br />
                                          Si necesitas alguna ayuda durante este proceso, no dudes en ponerte en contacto con nosotros
                                        </p>
                                        <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'>
                                          Gracias,<br />
                                          Tu Licencia<br />
                                          Teléfono: (787) 655-5555
                                        </p>
                                        <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'>
                                          Se adjunta el recibo de pago.
                                        </p>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                        <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                        <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                        <div class='footer-icons' style='margin-top: 20px'>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                        </div>
                                        <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>
                                          2024 Todos los derechos reservados
                                        </p>
                                      </td>
                                    </tr>
                                  </table>
                                </td>
                              </tr>
                            </table>
                        </body>
                        </html>";
                    bodyBuilder.HtmlBody = TextBody;

                    var attachment = new MimePart("application", "pdf")
                    {
                        ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = Path.GetFileName(rutaarchivopdf)
                    };

                    using (var fileStream = File.OpenRead(rutaarchivopdf))
                    {
                        attachment.Content = new MimeContent(fileStream, ContentEncoding.Default);
                        bodyBuilder.Attachments.Add(attachment);
                        email.Body = bodyBuilder.ToMessageBody();

                        using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                        {
                            smtp.Connect(
                                _config.GetSection("Email:Host").Value,
                                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                                SecureSocketOptions.StartTls
                            );
                            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
                            smtp.Send(email);
                            smtp.Disconnect(true);
                        }
                    }
                    File.Delete(rutaarchivopdf);
                }
                else
                {
                    Console.WriteLine("El archivo PDF no se encontró en la ruta especificada.");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                Console.WriteLine("Se borró el PDF generado");
            }
            return response;
        }


        //public async Task<ResponseEntity> EmailPagoX(Pago pago)
        //{
        //    ResponseEntity response = new ResponseEntity();
        //    try
        //    {
        //        string nombreArchivo = $"pago_{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
        //        await _pagoRepository.GenerarReciboDePagoPDFM(pago.pg_codigo, nombreArchivo, pago);
        //        var rutaarchivopdf = Path.Combine("", nombreArchivo);

        //        if (File.Exists(rutaarchivopdf))
        //        {
        //            var email = new MimeMessage();
        //            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
        //            email.To.Add(MailboxAddress.Parse(pago.Email));
        //            email.Subject = "¡" + pago.Nombre + ", hemos recibido tu pago! \U0001F601";
        //            var bodyBuilder = new BodyBuilder();

        //            var TextBody = $@"
        //                    <html>
        //                    <head>
        //                    <meta content=""text/html; charset=UTF-8"" http-equiv=""Content-Type"" />
        //                    <style>
        //                        * {{
        //                            font-family: 'Inter', sans-serif;
        //                        }}
        //                    </style>
        //                    </head>
        //                    <body>
        //                        <table align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
        //                            style=""max-width: 600px; min-width: 300px; width: 100%; margin-left: auto; margin-right: auto; padding: 0.5rem;"">
        //                            <tbody>
        //                                <tr style=""width: 100%"">
        //                                    <td>
        //                                        <table align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
        //                                            style=""max-width: 37.5em; height: 64px"">
        //                                        </table>
        //                                        <h2 style=""text-align: left; color: rgb(17, 24, 39); margin-bottom: 12px; margin-top: 0px;
        //                                            font-size: 30px; line-height: 36px; font-weight: 700;"">
        //                                            <strong> ¡Hola, {pago.Nombre}! </strong>
        //                                        </h2>

        //                                        <p style=""font-size: 15px; line-height: 24px; margin: 16px 0; text-align: left; margin-bottom: 20px;
        //                                            margin-top: 0px; color: rgb(55, 65, 81); -webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale;"">
        //                                            Hemos recibido exitosamente la confirmación de tu pago {pago.pg_codigo}. A continuación, te enviamos un
        //                                            resumen de tu compra:
        //                                            <br /><br />
        //                                            Pedido #101
        //                                        </p>
        //                                        <table align=""left""  border=""1"" cellpadding=""5"" cellspacing=""0"">          
        //                                          <tr style=""padding-left: 0%; "">
        //                                            <td>Tramite: </td>
        //                                            <td>{pago.Description}</td>
        //                                          </tr>
        //                                          <tr>
        //                                            <td>Precio ($): </td>
        //                                            <td>{pago.Monto}</td>
        //                                          </tr>
        //                                          <tr>
        //                                            <td>Metodo de pago:</td>
        //                                            <td>Visa 8887</td>
        //                                          </tr>
        //                                          <tr>
        //                                            <td>Total ($): </td>
        //                                            <td> {pago.Monto}</td>
        //                                          </tr>
        //                                        </table>

        //                                    </td>
        //                                </tr>
        //                            </tbody>                                        
        //                                <p style=""font-size: 15px; line-height: 24px; margin: 16px 0; text-align: left; margin-bottom: 20px;
        //                                    margin-top: 0px; color: rgb(55, 65, 81); -webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale;"">
        //                                    <br /> 
        //                                    El siguiente paso es completar tus datos en el formulario correspondiente aquí. Una vez completado,
        //                                    estaremos en posición de avanzar con tus trámites.<br />
        //                                    <br /> Si necesitas alguna ayuda durante este proceso, no dudes en ponerte en contacto con nosotros
        //                                <br /> <br /> 
        //                                <strong>Gracias,</strong>
        //                                <br />Tu Licencia
        //                                <br /> Teléfono: (787) 655-5555
        //                                </p>                                        
        //                        </table>
        //                    </body>
        //                </html>
        //                ";
        //            bodyBuilder.HtmlBody = TextBody;

        //            var attachment = new MimePart("application", "pdf")
        //            {
        //                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
        //                ContentTransferEncoding = ContentEncoding.Base64,
        //                FileName = Path.GetFileName(rutaarchivopdf)
        //            };

        //            using (var fileStream = File.OpenRead(rutaarchivopdf))
        //            {
        //                attachment.Content = new MimeContent(fileStream, ContentEncoding.Default);
        //                bodyBuilder.Attachments.Add(attachment);
        //                email.Body = bodyBuilder.ToMessageBody();

        //                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
        //                {
        //                    smtp.Connect(
        //                        _config.GetSection("Email:Host").Value,
        //                        Convert.ToInt32(_config.GetSection("Email:Port").Value),
        //                        SecureSocketOptions.StartTls
        //                        );
        //                    smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
        //                    smtp.Send(email);
        //                    smtp.Disconnect(true);
        //                }
        //            }
        //            File.Delete(rutaarchivopdf);
        //        }
        //        else
        //        {
        //            Console.WriteLine("El archivo PDF no se encontró en la ruta especificada.");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Se borro el pdf generado");
        //    }
        //    //await Task.Delay(1000);
        //    return response;
        //}
        public void EmailPagoError(string correo, string nombre, string apellido, string errores)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(correo));
            email.Subject = "Compra Rechazada";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"                       
                        <!DOCTYPE html>
                        <html lang='es'>
                        <head>
                            <meta charset='UTF-8' />
                            <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                            <title>Confirmación de pago total</title>
                            <style>
                              @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');
                              body {{
                                margin: 0;
                                padding: 0;
                                font-family: 'Poppins', Arial, sans-serif;
                                background-color: #f6f6f6;
                              }}
                              h1 {{
                                font-size: 24px;
                                color: #5342a6;
                                margin: 0 0 20px 0;
                              }}
                              h2 {{
                                font-size: 28px;
                                font-weight: 700;
                                color: #5342a6;
                                margin: 0 0 0px 0;
                              }}
                              p {{
                                color: #333333;
                              }}
                              .greeting {{
                                font-size: 20px;
                                font-weight: 500;
                              }}
                              .btn {{
                                color: #ffffff;
                                background-color: #4caf50;
                                text-decoration: none;
                                border-radius: 5px;
                              }}
                              .footer-title {{
                                font-size: 24.897px;
                                font-weight: 700;
                              }}
                              .footer-subtitle {{
                                font-size: 11.499px;
                                font-weight: 400;
                              }}
                              .footer-icons img {{
                                padding: 0 10px;
                                width: 20px;
                                height: auto;
                                display: inline-block;
                                border: 0;
                              }}
                            </style>
                        </head>
                        <body>
                            <table role='presentation' style='width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;'>
                              <tr>
                                <td align='center' style='padding: 20px 0'>
                                  <table role='presentation' style='width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;'>
                                    <tr>
                                      <td align='left' style='padding: 40px 0 0px 30px; background-color: #ffffff'>
                                        <h1>Tu licencia</h1>
                                        <h2>Confirmación de pago total</h2>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td style='padding: 0px 30px 40px 30px'>
                                        <strong> Estimado, {" " + nombre + " " + apellido} </strong>
                                        <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'>
                                          ¡Tu compra No se ha procesado!,<br />
                                          Tu Licencia<br />
                                          Teléfono: (787) 655-5555
                                        </p>
                                        <p style='margin: 0 0 20px 0; font-size: 16px; line-height: 24px'>
                                          Se adjunta el recibo de pago.
                                        </p>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td style='padding: 30px; background-color: #eff1fe' align='center'>
                                        <p class='footer-title' style='color: #5342a6; padding: 0px; margin: 0px'>Tu licencia</p>
                                        <p class='footer-subtitle' style='padding: 0px; margin: 6px 0px 0px 0px'>Olvídate de las filas y esperas</p>
                                        <div class='footer-icons' style='margin-top: 20px'>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                          <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                        </div>
                                        <p style='margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;'>
                                          2024 Todos los derechos reservados
                                        </p>
                                      </td>
                                    </tr>
                                  </table>
                                </td>
                              </tr>
                            </table>
                        </body>
                        </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
                );
            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        //    public void EmailPagoDiferido(string correo, string nombre, string apellido, List<Cuota> Cuota)
        //    {
        //        var email = new MimeMessage();
        //        email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
        //        email.To.Add(MailboxAddress.Parse(correo));
        //        email.Subject = "Cronograma de Cuotas";
        //        email.Body = new TextPart(TextFormat.Html)
        //        {
        //            Text = $@"
        //    <html>
        //    <head>
        //    <meta content=""text/html; charset=UTF-8"" http-equiv=""Content-Type"" />
        //    <style>                            
        //        * {{
        //            font-family: 'Inter', sans-serif;
        //        }}                            
        //    </style>
        //</head>
        //<body>
        //    <table align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
        //        style=""max-width: 600px; min-width: 300px; width: 100%; margin-left: auto; margin-right: auto; padding: 0.5rem;"">
        //        <tbody>
        //            <tr style=""width: 100%"">
        //                <td>
        //                    <table align=""center"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation""
        //                        style=""max-width: 37.5em; height: 64px"">
        //                    </table>
        //                    <h2 style=""text-align: left; color: rgb(17, 24, 39); margin-bottom: 12px; margin-top: 0px;
        //                        font-size: 30px; line-height: 36px; font-weight: 700;"">
        //                        <strong> Estimado, {" " + nombre + " " + apellido} </strong>
        //                    </h2>                                        
        //                    <p style=""font-size: 15px; line-height: 24px; margin: 16px 0; text-align: left; margin-bottom: 20px;
        //                        margin-top: 0px; color: rgb(55, 65, 81); -webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale;"">
        //                        A continuación, encontrarás el cronograma de tus cuotas:<br /><br />       
        //                    </p>
        //                    <table border=""1"" cellpadding=""5"" cellspacing=""0"" style=""width: 100%; border-collapse: collapse; text-align: left;"">
        //                        <thead>
        //                            <tr>
        //                                <th>Cuota</th>
        //                                <th>Monto</th>
        //                                <th>Fecha</th>
        //                            </tr>
        //                        </thead>
        //                        <tbody>
        //                            {GenerateCuotasTable(Cuota)}
        //                        </tbody>
        //                    </table>
        //                    <p> <strong>Gracias,</strong><br />Tu Licencia </p>
        //                </td>
        //            </tr>
        //        </tbody>
        //    </table>
        //</body>
        //</html>"
        //        };

        //        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        //        smtp.Connect(
        //            _config.GetSection("Email:Host").Value,
        //            Convert.ToInt32(_config.GetSection("Email:Port").Value),
        //            SecureSocketOptions.StartTls
        //        );
        //        smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
        //        smtp.Send(email);
        //        smtp.Disconnect(true);

        //        string GenerateCuotasTable(List<Cuota> cuotas)
        //        {
        //            if (cuotas == null)
        //            { return ""; }
        //            else
        //            {

        //                var rows = new StringBuilder();
        //                foreach (var cuota in cuotas)
        //                {
        //                    rows.AppendLine($"<tr><td>{cuota.nro}</td><td>{cuota.monto}</td><td>{cuota.fecha}</td></tr>");
        //                }
        //                return rows.ToString();

        //            }
        //            }
        public void EmailPagoDiferido(string correo, string nombre, string apellido, List<Cuota> Cuota)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("Email:UserName").Value));
            email.To.Add(MailboxAddress.Parse(correo));
            email.Subject = "Confirmación de pago en cuotas";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"
                        <html lang=""es"">
                        <head>
                            <meta charset=""UTF-8"" />
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                            <title>Confirmación de pago en cuotas</title>
                            <style>
                                @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@400;500;600;700&display=swap');
                                body {{
                                margin: 0;
                                padding: 0;
                                font-family: 'Poppins', Arial, sans-serif;
                                background-color: #f6f6f6;
                                }}
                                h1 {{
                                font-size: 24px;
                                color: #5342a6;
                                margin: 0 0 20px 0;
                                }}
                                h2 {{
                                font-size: 28px;
                                font-weight: 700;
                                color: #5342a6;
                                margin: 0 0 0px 0;
                                }}
                                p {{
                                color: #333333;
                                }}
                                .greeting {{
                                font-size: 20px;
                                font-weight: 500;
                                }}
                                .btn {{
                                color: #ffffff;
                                background-color: #4caf50;
                                text-decoration: none;
                                border-radius: 5px;
                                }}
                                .footer-title {{
                                font-size: 24.897px;
                                font-weight: 700;
                                }}
                                .footer-subtitle {{
                                font-size: 11.499px;
                                font-weight: 400;
                                }}
                                .footer-icons img {{
                                padding: 0 10px;
                                width: 20px;
                                height: auto;
                                display: inline-block;
                                border: 0;
                                }}
                            </style>
                        </head>
                        <body>
                            <table role=""presentation"" style=""width: 100%; border-collapse: collapse; border: 0; border-spacing: 0; background-color: #f6f6f6;"">
                                <tr>
                                <td align=""center"" style=""padding: 20px 0"">
                                    <table role=""presentation"" style=""width: 600px; border-collapse: collapse; border: 1px solid #cccccc; border-spacing: 0; background-color: #ffffff;"">
                                    <tr>
                                        <td align=""left"" style=""padding: 40px 0 0px 30px; background-color: #ffffff"">
                                        <h1>Tu licencia</h1>
                                        <h2>Confirmación de pago en cuotas</h2>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""padding: 0px 30px 40px 30px"">
                                        <p class=""greeting"">¡Hola, {nombre} {apellido}!</p>
                                        <p style=""margin: 16px 0 20px 0; font-size: 16px; line-height: 24px;"">
                                            Le enviamos un detalle del plan de pago seleccionado, hemos recibido la primera cuota por un total de {Cuota.First().monto}.
                                        </p>
                                        <p style=""margin: 0 0 20px 0; font-size: 16px; line-height: 24px"">
                                            El pago de las cuotas restantes es su responsabilidad y es importante que se comunique con nosotros en caso de que no pueda realizar alguna.
                                        </p>
                                        <h3 style=""margin: 20px 0 10px 0; font-size: 18px; color: #5342a6"">
                                            A continuación, el cronograma de pago:
                                        </h3>
                                        <table style=""width: 100%; border-collapse: collapse; margin-bottom: 20px;"">
                                            <thead>
                                            <tr>
                                                <th style=""border: 1px solid #cccccc; padding: 8px"">Cuota</th>
                                                <th style=""border: 1px solid #cccccc; padding: 8px"">Fecha de vencimiento</th>
                                                <th style=""border: 1px solid #cccccc; padding: 8px"">Monto</th>
                                            </tr>
                                            </thead>
                                            <tbody>
                                            {GenerateCuotasTable(Cuota)}
                                            </tbody>
                                        </table>
                                        <p style=""margin: 0 0 20px 0; font-size: 16px; line-height: 24px"">
                                            Gracias,<br />
                                            Tu Licencia
                                        </p>
                                        <p style=""margin: 0 0 20px 0; font-size: 16px; line-height: 24px"">
                                            ADJUNTO EL INVOICE
                                        </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""padding: 30px; background-color: #eff1fe"" align=""center"">
                                        <p class=""footer-title"" style=""color: #5342a6; padding: 0px; margin: 0px"">Tu licencia</p>
                                        <p class=""footer-subtitle"" style=""padding: 0px; margin: 6px 0px 0px 0px"">Olvídate de las filas y esperas</p>
                                        <div class=""footer-icons"" style=""margin-top: 20px"">
                                            <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/e/e8/Linkedin-logo-blue-In-square-40px.png' alt='LinkedIn'/></a>
                                            <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/c/c3/FB_Logo_PNG.png' alt='Facebook'/></a>
                                            <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/a/a5/Instagram_icon.png' alt='Instagram'/></a>
                                            <a href='#'><img src='https://upload.wikimedia.org/wikipedia/commons/f/f6/Whatsapp_tile_logo_icon_169898.png' alt='WhatsApp'/></a>
                                        </div>
                                        <p style=""margin: 16px 0 0 0; font-size: 12px; line-height: 16px; color: #333333;"">
                                            2024 Todos los derechos reservados
                                        </p>
                                        </td>
                                    </tr>
                                    </table>
                                </td>
                                </tr>
                            </table>
                        </body>
                        </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(
                _config.GetSection("Email:Host").Value,
                Convert.ToInt32(_config.GetSection("Email:Port").Value),
                SecureSocketOptions.StartTls
            );
            smtp.Authenticate(_config.GetSection("Email:UserName").Value, _config.GetSection("Email:Password").Value);
            smtp.Send(email);
            smtp.Disconnect(true);

            string GenerateCuotasTable(List<Cuota> cuotas)
            {
                if (cuotas == null)
                {
                    return "";
                }
                else
                {
                    var rows = new StringBuilder();
                    foreach (var cuota in cuotas)
                    {
                        rows.AppendLine($@"
                        <tr>
                            <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{cuota.nro}</td>
                            <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{cuota.fecha}</td>
                            <td style='border: 1px solid #cccccc; padding: 8px; text-align: center'>{cuota.monto}</td>
                        </tr>");
                    }
                    return rows.ToString();
                }
            }
        }
    }
}