namespace MedicalInformationSystem.Dtos
{
    public class NotificationInput
    {
        public string[] DeviceTokens { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public object Data { get; set; }
    }
}
