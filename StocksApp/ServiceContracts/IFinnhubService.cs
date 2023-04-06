namespace StocksApp.ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>> GetQuote(string symbol);
        Task<Dictionary<string, object>> GetCompanyProfile(string symbol);


    }
}
