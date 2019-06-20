using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [RoutePrefix("api/email")]
    public class EmailController : ApiController
    {
        private IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [Route("send")]
        public async Task<IHttpActionResult> SendMessage(Email model)
        {
            try
            {
                await _emailService.SendEmailAsync(model.To, model.Subject, model.Message);
            }
            catch (SmtpException e)
            {
                return BadRequest(e.Message);
            }
            return Ok();

        }
    }
}
