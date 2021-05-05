namespace MedicalInformationSystem.Models
{
    public class Disease
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cause { get; set; }
        public int MedicalHistoryId { get; set; }
    }
}