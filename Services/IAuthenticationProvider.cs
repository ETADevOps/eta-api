namespace ETA_API.Services
{
    public interface IAuthenticationProvider
    {
        Task AuthenticateRequestAsync(HttpRequestMessage request);
    }
}