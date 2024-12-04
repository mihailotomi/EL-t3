namespace EL_t3.Infrastructure.Gateway.Helpers;

internal interface IProballersUriHelper
{
    public abstract string[] GetClubUri(string clubCode);
    public abstract IEnumerable<string> GetCodes();
}

