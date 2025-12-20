using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NutriGuide.Models;

public partial class NutriGuideDbContext : DbContext
{
    public NutriGuideDbContext()
    {
    }

    public NutriGuideDbContext(DbContextOptions<NutriGuideDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvoidFood> AvoidFoods { get; set; }

    public virtual DbSet<ChronicDisease> ChronicDiseases { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DailyWieght> DailyWieghts { get; set; }

    public virtual DbSet<GoalPlan> GoalPlans { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientMeal> IngredientMeals { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<MedicineTime> MedicineTimes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Plan> Plans { get; set; }

    public virtual DbSet<ShoppingList> ShoppingLists { get; set; }

    public virtual DbSet<ShoppingListItem> ShoppingListItems { get; set; }

    public virtual DbSet<SystemTypePlan> SystemTypePlans { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=.;Database=NutriGuideDB;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvoidFood>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AvoidFoodsID_PK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.FoodName).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.AvoidFoods)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CustomerAvoidFoodsID_FK");
        });

        modelBuilder.Entity<ChronicDisease>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ChronicDiseasesID_PK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DiseaseName).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.ChronicDiseases)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("DiseasesCustomerID_FK");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CustomerID_PK");

            entity.ToTable("Customer");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ActivityLevel).HasColumnType("decimal(5, 3)");
            entity.Property(e => e.Age).HasComputedColumnSql("(datediff(year,[BirthDate],getdate()))", false);
            entity.Property(e => e.Bmi)
                .HasComputedColumnSql("(CONVERT([decimal](5,2),[Weight]/power([Height]/(100.0),(2))))", false)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("BMI");
            entity.Property(e => e.Bmr)
                .HasComputedColumnSql("(CONVERT([decimal](7,2),case when [Gender]='Male' then (((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))+(5) when [Gender]='Female' then (((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))-(161)  end))", false)
                .HasColumnType("decimal(7, 2)")
                .HasColumnName("BMR");
            entity.Property(e => e.Calories)
                .HasComputedColumnSql("(CONVERT([decimal](7,2),case when [Gender]='Male' then ((((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))+(5))*[ActivityLevel] when [Gender]='Female' then ((((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))-(161))*[ActivityLevel]  end))", false)
                .HasColumnType("decimal(7, 2)");
            entity.Property(e => e.Gender).HasMaxLength(6);
            entity.Property(e => e.Height).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IdealWeight)
                .HasComputedColumnSql("(CONVERT([decimal](5,2),case when [Gender]='Male' then (48)+(2.7)*(([Height]-(152))/(2.54)) when [Gender]='Female' then (45.5)+(2.2)*(([Height]-(152))/(2.54))  end))", false)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.IdNavigation).WithOne(p => p.Customer)
                .HasForeignKey<Customer>(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CustomerID_FK");

            entity.HasMany(d => d.Meals).WithMany(p => p.Customers)
                .UsingEntity<Dictionary<string, object>>(
                    "FavoriteMeal",
                    r => r.HasOne<Meal>().WithMany()
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FavoriteMeal_FK"),
                    l => l.HasOne<Customer>().WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("CustomerFavorite_FK"),
                    j =>
                    {
                        j.HasKey("CustomerId", "MealId").HasName("FavoriteMeal_PK");
                        j.ToTable("FavoriteMeal");
                        j.IndexerProperty<int>("CustomerId").HasColumnName("CustomerID");
                        j.IndexerProperty<int>("MealId").HasColumnName("MealID");
                    });
        });

        modelBuilder.Entity<DailyWieght>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("DailyWieghtID_PK");

            entity.ToTable("DailyWieght");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Date).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Weight).HasColumnType("decimal(6, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.DailyWieghts)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("CustomerDailyCalories_FK");
        });

        modelBuilder.Entity<GoalPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GoalPlanID_PK");

            entity.ToTable("GoalPlan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3214EC2760389CD1");

            entity.ToTable("Ingredient");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Unit).HasMaxLength(20);
        });

        modelBuilder.Entity<IngredientMeal>(entity =>
        {
            entity.HasKey(e => new { e.MealId, e.IngredientId }).HasName("IngredientMeal_PK");

            entity.ToTable("IngredientMeal");

            entity.Property(e => e.MealId).HasColumnName("MealID");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientMeals)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("IngredientFoodID_FK");

            entity.HasOne(d => d.Meal).WithMany(p => p.IngredientMeals)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MealFoodID_FK");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MealID_PK");

            entity.ToTable("Meal");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Budget).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Calories).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Carbs).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Fat).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MealTime).HasMaxLength(20);
            entity.Property(e => e.MealType).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PlanId).HasColumnName("PlanID");
            entity.Property(e => e.Protein).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(10);

            entity.HasOne(d => d.Plan).WithMany(p => p.Meals)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("MealPlanID_FK");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("MedicinesID_PK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.MedicineName).HasMaxLength(50);
            entity.Property(e => e.Option).HasMaxLength(20);

            entity.HasOne(d => d.Customer).WithMany(p => p.Medicines)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("MedicinesCustomerID_FK");
        });

        modelBuilder.Entity<MedicineTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Medicine__3214EC2730893B60");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MedicineId).HasColumnName("MedicineID");

            entity.HasOne(d => d.Medicine).WithMany(p => p.MedicineTimes)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MedicineTimes_Medicine");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("NotificationID_PK");

            entity.ToTable("Notification");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.ScheduleTime).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("CustomerNotificationID_FK");
        });

        modelBuilder.Entity<Plan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PlanID_PK");

            entity.ToTable("Plan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.DailyCalories).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.GoalId).HasColumnName("GoalID");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.SystemTypeId).HasColumnName("SystemTypeID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Plans)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CustomerPlanID_FK");

            entity.HasOne(d => d.Goal).WithMany(p => p.Plans)
                .HasForeignKey(d => d.GoalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GoalPlanID_FK");

            entity.HasOne(d => d.SystemType).WithMany(p => p.Plans)
                .HasForeignKey(d => d.SystemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SystemTypeID_FK");
        });

        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ShoppingListID_PK");

            entity.ToTable("ShoppingList");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GeneratedDate).HasColumnType("datetime");
            entity.Property(e => e.IsCompleted).HasDefaultValue(false);
            entity.Property(e => e.PlanId).HasColumnName("PlanID");

            entity.HasOne(d => d.Plan).WithMany(p => p.ShoppingLists)
                .HasForeignKey(d => d.PlanId)
                .HasConstraintName("PlanShoppingList_FK");
        });

        modelBuilder.Entity<ShoppingListItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ShoppingListItems_PK");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.IngredientId).HasColumnName("IngredientID");
            entity.Property(e => e.IsBought).HasDefaultValue(false);
            entity.Property(e => e.ListId).HasColumnName("ListID");
            entity.Property(e => e.Unit).HasMaxLength(20);

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ShoppingListItems)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("IngredientItemID_FK");

            entity.HasOne(d => d.List).WithMany(p => p.ShoppingListItems)
                .HasForeignKey(d => d.ListId)
                .HasConstraintName("ListItemsID_FK");
        });

        modelBuilder.Entity<SystemTypePlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SystemTypePlan_PK");

            entity.ToTable("SystemTypePlan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_PK");

            entity.HasIndex(e => e.Email, "unique_EmailUser").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(300);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.ResetAttempts).HasDefaultValue(0);
            entity.Property(e => e.ResetPasswordCode).HasMaxLength(20);
            entity.Property(e => e.ResetPasswordExpiry).HasColumnType("datetime");
            entity.Property(e => e.Role).HasMaxLength(8);
            entity.Property(e => e.VerificationCode).HasMaxLength(20);
            entity.Property(e => e.VerificationCodeExpiry).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
