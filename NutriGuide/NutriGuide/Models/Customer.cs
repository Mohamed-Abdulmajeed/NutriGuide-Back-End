using System;
using System.Collections.Generic;

namespace NutriGuide.Models;

public partial class Customer
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

    public virtual ICollection<AvoidFood> AvoidFoods { get; set; } = new List<AvoidFood>();

    public virtual ICollection<ChronicDisease> ChronicDiseases { get; set; } = new List<ChronicDisease>();

    public virtual ICollection<DailyWieght> DailyWieghts { get; set; } = new List<DailyWieght>();

    public virtual User IdNavigation { get; set; } = null!;

    public virtual ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Plan> Plans { get; set; } = new List<Plan>();

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
