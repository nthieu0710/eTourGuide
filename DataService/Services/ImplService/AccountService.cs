using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eTourGuide.Data.Entity;
using eTourGuide.Data.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using eTourGuide.Service.Model.Response;
using eTourGuide.Service.Exceptions;
using eTourGuide.Service.Servcies.InterfaceService;

namespace eTourGuide.Service.Servcies.ImplService
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public AccountService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }


        //Implement from Interface IAccountService - đăng nhập lần đầu
        public async Task<string> AuthenticateAsync(string username, string password)
        { 
            Account account = _unitOfWork.Repository<Account>().GetAll().Where(x => x.Username == username).FirstOrDefault();
            if (account.Password != password)
            {
                return null;
            }
            var jwt = GenerateJwtToken(account);
            return jwt;
        }

        //Implement from Interface IAccountService - xác thực đăng nhập
        public VerifyResponse VerifyJwtLogin(string jwt)
        {
            try
            {
                var userPrincipal = this.ValidateToken(jwt);
                var valueExp = "";
                string username = "";
                //Filter specific claim
                if (null != userPrincipal)
                {
                    foreach (Claim claim in userPrincipal.Claims)
                    {
                        if (claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")
                        {
                            username = new String(claim.Value);
                        }
                        if (claim.Type == "exp")
                        {
                            valueExp = claim.Value;
                        }
                    }
                }
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(valueExp));
                var dayRemaining = dateTimeOffset.DateTime - DateTime.UtcNow;
                if (dayRemaining.Days < 3)
                {
                    var account = _unitOfWork.Repository<Account>().GetAll().Where(x => x.Username == username).FirstOrDefault();
                    return new VerifyResponse
                    {
                        Jwt = jwt,
                        RefreshToken = GenerateJwtToken(account)
                    };
                }
                else
                {
                    return new VerifyResponse
                    {
                        Jwt = jwt,
                        RefreshToken = ""
                    };
                }
            }
            catch (Exception)
            {
                throw new CrudException(System.Net.HttpStatusCode.BadRequest, "Error to verify account eTourGuide!!!");
            }
        }


        private string GenerateJwtToken(Account account)
        {          
            var claims = new[]
            {
               new Claim(ClaimTypes.Name, account.Username.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["AppSettings:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["AppSettings:Issuer"],
                _config["AppSettings:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddHours(7).Date.AddDays(7),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,

                ValidAudience = _config["AppSettings:Issuer"],
                ValidIssuer = _config["AppSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["AppSettings:Secret"]))
            };
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out _);

            return principal;
        }

    }
}
