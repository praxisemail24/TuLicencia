using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Utility;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TokenRepository _tokenRepository;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenRepository = new TokenRepository(configuration);
        }

        [HttpPost]

        public DataTableJS<Token> ListToken([FromForm] DataTableJSRequest<DataTableSearch> request)
        {
            var response = new DataTableJS<Token>();
            try
            {
                var total = _tokenRepository.TotalRecords();
                var rs = _tokenRepository.ListToken(request?.Search?.Value ?? string.Empty, request?.Start ?? 0, request?.Length ?? 10);
                
                response.Draw = request?.Draw ?? 1;
                response.Data = rs;
                response.RecordsTotal = total;
                response.RecordsFiltered = total;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }

        [HttpPost("Create/{rol}")]
        public ResponseJSON CreateToken(int rol, TokenRequest request)
        {
            var response = new ResponseJSON();
            try
            {
                var token = TokenGenerator.GenerateJwtToken(_configuration, request.UserName, request.GetExpiredAt());
                var objToken = new Token
                {
                    UserName = request.UserName,
                    AccessToken = token,
                    ExpiredAt = request.GetExpiredAt(),
                    UserId = request.UserId,
                    UseOrigin = rol,
                };
                var created = _tokenRepository.Create(objToken);

                if (!created)
                    throw new Exception("Error al intentar crear el token.");

                response.Success = true;
                response.Data = objToken;
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpDelete("Revoked/{id}")]
        public ResponseJSON RevokedToken(int id)
        {
            var response = new ResponseJSON();
            try
            {
                var revoked = _tokenRepository.RevokedToken(id);

                if (!revoked)
                    throw new Exception("Error al intentar revocar el token.");

                response.Success = true;
                response.Message = "OK";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
