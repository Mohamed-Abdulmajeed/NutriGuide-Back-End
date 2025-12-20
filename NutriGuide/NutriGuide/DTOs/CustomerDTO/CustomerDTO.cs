namespace NutriGuide.DTOs.CustomerDTO
{
    public class CustomerDTO
    {
        public int Id { get; set; }
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

    }
}
