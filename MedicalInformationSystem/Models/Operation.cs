using System;

namespace MedicalInformationSystem.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public int MedicalHistoryId { get; set; }
    }
}