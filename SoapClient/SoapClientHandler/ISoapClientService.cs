namespace SoapClient.SoapClientHandler
{
    public interface ISoapClientService 
    {
        string CallWebService(string _url, string _action, string _xmlString);
    }
}
