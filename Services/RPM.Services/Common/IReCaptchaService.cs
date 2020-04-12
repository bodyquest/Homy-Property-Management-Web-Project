namespace RPM.Services.Common
{
    using System.Threading.Tasks;
    using RPM.Services.Common.Implementations;

    public interface IReCaptchaService
    {
        Task<GoogleResponse> ValidateResponse(string token);
    }
}
