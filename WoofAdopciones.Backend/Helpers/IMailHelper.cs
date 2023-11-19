using WoofAdopciones.Shared.Responses;

namespace WoofAdopciones.Backend.Helpers
{
    namespace WoofAdopciones.Backend.Helpers
    {
        public interface IMailHelper
        {
            Response<string> SendMail(string toName, string toEmail, string subject, string body);
        }
    }
}