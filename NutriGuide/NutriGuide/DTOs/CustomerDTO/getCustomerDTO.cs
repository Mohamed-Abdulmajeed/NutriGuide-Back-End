namespace NutriGuide.DTOs.CustomerDTO
{
    public class getCustomerDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly BirthDate { get; set; }

        public string Gender { get; set; } = null!;

        public decimal Height { get; set; }

        public decimal Weight { get; set; }

        public decimal ActivityLevel { get; set; }

        public int? Age { get; set; }

        public decimal? IdealWeight { get; set; }

        public decimal? Bmi { get; set; }

        public decimal? Bmr { get; set; }

        public decimal? Calories { get; set; }

        public List<string> AvoidFoods { get; set; } = new List<string>();
        public List<string> ChronicDiseases { get; set; } = new List<string>();
        public List<addMedicineDTO> Medicines { get; set; } = new List<addMedicineDTO>();
        public List<DailyWieghtDTO> DailyWeights { get; set; } = new List<DailyWieghtDTO>();



    }
}
