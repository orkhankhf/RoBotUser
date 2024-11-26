namespace Entities.RequestModels
{
    public class GetUnsentPhoneNumbersRequest
    {

    }

    public class GetUnsentPhoneNumbersResponse
    {
        public List<string> PhoneNumbers { get; set; }
    }
}
