using System.Net.Http;

namespace MobileDevApp.RemoteProviders.Interfaces
{
    public interface IHttpProvider
    {
        TResult SendRequest<TResult>(HttpRequestMessage requestMessage);
    }
}
