namespace Entities.RequestModels
{
    public class GetUnsentPhoneNumbersRequest
    {

    }

    public class GetUnsentPhoneNumbersResponse
    {
        public List<UnsentPhoneNumberModel> PhoneNumbers { get; set; }
    }

    public class UnsentPhoneNumberModel
    {
        public int AssignedPhoneNumberId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
