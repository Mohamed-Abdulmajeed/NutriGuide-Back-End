using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NutriGuide.Migrations
{
    /// <inheritdoc />
    public partial class createDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoalPlan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GoalPlanID_PK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ingredient",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ingredie__3214EC2760389CD1", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SystemTypePlan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("SystemTypePlan_PK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    VerificationCodeExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResetPasswordCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ResetPasswordExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    ResetAttempts = table.Column<int>(type: "int", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_PK", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ActivityLevel = table.Column<decimal>(type: "decimal(5,3)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true, computedColumnSql: "(datediff(year,[BirthDate],getdate()))", stored: false),
                    IdealWeight = table.Column<decimal>(type: "decimal(5,2)", nullable: true, computedColumnSql: "(CONVERT([decimal](5,2),case when [Gender]='Male' then (48)+(2.7)*(([Height]-(152))/(2.54)) when [Gender]='Female' then (45.5)+(2.2)*(([Height]-(152))/(2.54))  end))", stored: false),
                    BMI = table.Column<decimal>(type: "decimal(5,2)", nullable: true, computedColumnSql: "(CONVERT([decimal](5,2),[Weight]/power([Height]/(100.0),(2))))", stored: false),
                    BMR = table.Column<decimal>(type: "decimal(7,2)", nullable: true, computedColumnSql: "(CONVERT([decimal](7,2),case when [Gender]='Male' then (((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))+(5) when [Gender]='Female' then (((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))-(161)  end))", stored: false),
                    Calories = table.Column<decimal>(type: "decimal(7,2)", nullable: true, computedColumnSql: "(CONVERT([decimal](7,2),case when [Gender]='Male' then ((((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))+(5))*[ActivityLevel] when [Gender]='Female' then ((((10)*[Weight]+(6.25)*[Height])-(5)*datediff(year,[BirthDate],getdate()))-(161))*[ActivityLevel]  end))", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CustomerID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "CustomerID_FK",
                        column: x => x.ID,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AvoidFoods",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AvoidFoodsID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "CustomerAvoidFoodsID_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ChronicDiseases",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ChronicDiseasesID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "DiseasesCustomerID_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DailyWieght",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(CONVERT([date],getdate()))"),
                    Weight = table.Column<decimal>(type: "decimal(6,2)", nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("DailyWieghtID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "CustomerDailyCalories_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Option = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MedicinesID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "MedicinesCustomerID_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ScheduleTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CustomerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("NotificationID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "CustomerNotificationID_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    GoalID = table.Column<int>(type: "int", nullable: false),
                    SystemTypeID = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    NumberOfDays = table.Column<int>(type: "int", nullable: false),
                    NumberOfMeals = table.Column<int>(type: "int", nullable: false),
                    DailyCalories = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PlanID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "CustomerPlanID_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "GoalPlanID_FK",
                        column: x => x.GoalID,
                        principalTable: "GoalPlan",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "SystemTypeID_FK",
                        column: x => x.SystemTypeID,
                        principalTable: "SystemTypePlan",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MedicineTimes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineID = table.Column<int>(type: "int", nullable: false),
                    TakeTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medicine__3214EC2730893B60", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MedicineTimes_Medicine",
                        column: x => x.MedicineID,
                        principalTable: "Medicines",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Preparation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calories = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Protein = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Carbs = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Fat = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    HealthBenefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MealType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MealTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Budget = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PlanID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("MealID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "MealPlanID_FK",
                        column: x => x.PlanID,
                        principalTable: "Plan",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingList",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanID = table.Column<int>(type: "int", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ShoppingListID_PK", x => x.ID);
                    table.ForeignKey(
                        name: "PlanShoppingList_FK",
                        column: x => x.PlanID,
                        principalTable: "Plan",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteMeal",
                columns: table => new
                {
                    CustomerID = table.Column<int>(type: "int", nullable: false),
                    MealID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FavoriteMeal_PK", x => new { x.CustomerID, x.MealID });
                    table.ForeignKey(
                        name: "CustomerFavorite_FK",
                        column: x => x.CustomerID,
                        principalTable: "Customer",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FavoriteMeal_FK",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "IngredientMeal",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false),
                    IngredientID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("IngredientMeal_PK", x => new { x.MealID, x.IngredientID });
                    table.ForeignKey(
                        name: "IngredientFoodID_FK",
                        column: x => x.IngredientID,
                        principalTable: "Ingredient",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "MealFoodID_FK",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingListItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListID = table.Column<int>(type: "int", nullable: true),
                    IngredientID = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsBought = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ShoppingListItems_PK", x => x.ID);
                    table.ForeignKey(
                        name: "IngredientItemID_FK",
                        column: x => x.IngredientID,
                        principalTable: "Ingredient",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "ListItemsID_FK",
                        column: x => x.ListID,
                        principalTable: "ShoppingList",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvoidFoods_CustomerID",
                table: "AvoidFoods",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_ChronicDiseases_CustomerID",
                table: "ChronicDiseases",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_DailyWieght_CustomerID",
                table: "DailyWieght",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteMeal_MealID",
                table: "FavoriteMeal",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientMeal_IngredientID",
                table: "IngredientMeal",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_PlanID",
                table: "Meal",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_CustomerID",
                table: "Medicines",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineTimes_MedicineID",
                table: "MedicineTimes",
                column: "MedicineID");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_CustomerID",
                table: "Notification",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_CustomerID",
                table: "Plan",
                column: "CustomerID");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_GoalID",
                table: "Plan",
                column: "GoalID");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_SystemTypeID",
                table: "Plan",
                column: "SystemTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingList_PlanID",
                table: "ShoppingList",
                column: "PlanID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_IngredientID",
                table: "ShoppingListItems",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_ListID",
                table: "ShoppingListItems",
                column: "ListID");

            migrationBuilder.CreateIndex(
                name: "unique_EmailUser",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvoidFoods");

            migrationBuilder.DropTable(
                name: "ChronicDiseases");

            migrationBuilder.DropTable(
                name: "DailyWieght");

            migrationBuilder.DropTable(
                name: "FavoriteMeal");

            migrationBuilder.DropTable(
                name: "IngredientMeal");

            migrationBuilder.DropTable(
                name: "MedicineTimes");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Ingredient");

            migrationBuilder.DropTable(
                name: "ShoppingList");

            migrationBuilder.DropTable(
                name: "Plan");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "GoalPlan");

            migrationBuilder.DropTable(
                name: "SystemTypePlan");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
