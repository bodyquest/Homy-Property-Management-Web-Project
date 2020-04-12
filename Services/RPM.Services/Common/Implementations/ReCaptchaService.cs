namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using RPM.Services.Common.Models.ReCAPTHASettings;

    using static RPM.Common.GlobalConstants;

    public class ReCaptchaService
    {
        private readonly ReCaptchaSettings settings;

        public ReCaptchaService(IOptions<ReCaptchaSettings> settings)
        {
            this.settings = settings.Value;
        }

        public virtual async Task<GoogleResponse> ValidateResponse(string token)
        {
            GoogleReCapthaData data = new GoogleReCapthaData
            {
                ResponseToken = token,
                SecretKey = this.settings.ReCAPTCHA_Sectret_Key,
            };

            HttpClient httpClient = new HttpClient();
            var response = await httpClient
                .GetStringAsync(
                ReCaptchaVerifyLink + $"?secret={data.SecretKey}&response={data.ResponseToken}");

            var capturedResponse = JsonConvert.DeserializeObject<GoogleResponse>(response);

            return capturedResponse;
        }
    }

    public class GoogleReCapthaData
    {
        public string ResponseToken { get; set; }

        public string SecretKey { get; set; }
    }

    public class GoogleResponse
    {
        public bool Success { get; set; }

        public double Score { get; set; }

        public string Action { get; set; }

        public DateTime ChallangeTimeStamp { get; set; }

        public string HostName { get; set; }
    }
}
