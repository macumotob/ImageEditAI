using _7E_Server.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace ImageEditAI.Controllers
{
    public class _7E_Controller : Controller
    {
        protected _7e_session _session = new _7e_session();
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            base.OnActionExecuting(context);

            var req = context.HttpContext.Request;
            var auth = req.Headers["Authorization"].ToString(); // this is null here
            if (string.IsNullOrEmpty(auth))
            {
                _session.uid = null;
                return;
            }
            if (auth == "Bearer")
            {
                _session.uid = null;
                return;
            }
            if (!String.IsNullOrEmpty(auth))
            {
                var token = auth.Replace("Bearer ", "");
                ParseToken(token);
            }
        }
        protected Result _AuthorizationRequired()
        {
            this.Response.StatusCode = 401;
            return Result.Error("Not Authorized");
        }

        private void ParseToken(string token)
        {
            try
            {
                _session.access_token = token;
                if (token == null)
                {
                    return;
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                var tkn = tokenHandler.ReadJwtToken(token);
              

                var valide_to = tkn.Claims.FirstOrDefault(x => x.Type == "valide_to")?.Value;
                var valid_to = DateTime.Parse(valide_to);

                _session.uid = tkn.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
                _session.valid_to = valid_to;
                if(valid_to < DateTime.UtcNow)
                {
                    _session.uid = null;
                }
                
            }
            catch (Exception ex)
            {
                _session.uid = null;
            }
        }
    
        
    }

}
