using DocumentationAppsApi.Src.IServices;

namespace DocumentationAppsApi.Src.Services
{
    public class EmailService : IEmailService
    {
        private readonly HttpClient _httpClient;

        public EmailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SendEmailAsync( string to, string subject, string htmlBody)
        {
            var request = new
            {
                sender = "Documentation Apps",
                to,
                subject,
                body = htmlBody,
            };

            var response = await _httpClient.PostAsJsonAsync("https://emailmicroservice.onrender.com/send", request);

            return response.IsSuccessStatusCode;
        }
    }
}
