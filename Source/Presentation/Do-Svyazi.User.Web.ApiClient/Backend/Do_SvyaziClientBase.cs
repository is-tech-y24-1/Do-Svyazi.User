namespace Do_Svyazi.User.Web.ApiClient.Backend;

public abstract class Do_SvyaziClientBase
{
    public string BearerToken { get; private set; }

    public void SetBearerToken(string token)
    {
        BearerToken = token;
    }

    // Called by implementing swagger client classes
    public Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        var msg = new HttpRequestMessage();

        msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", BearerToken);
        return Task.FromResult(msg);
    }

}