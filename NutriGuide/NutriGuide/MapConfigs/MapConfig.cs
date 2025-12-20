using AutoMapper;
using NutriGuide.DTOs.CustomerDTO;
using NutriGuide.DTOs.ingredientDTO;
using NutriGuide.DTOs.mealDTOs;
using NutriGuide.DTOs.planDTOs;
using NutriGuide.DTOs.ShoppingListDTOs;
using NutriGuide.DTOs.userDTOs;
using NutriGuide.Helpers;
using NutriGuide.Models;
using System.Security.Cryptography;

namespace NutriGuide.MapConfigs
{
    public class MapConfig : Profile
    {
        public MapConfig()
        {
            CreateMap<registerDTO, User>().AfterMap
                (
                (src, dest) =>
                {
                    dest.PasswordHash = Utils.generatePasswordHash(src.Password);
                });


            

            CreateMap<Plan, AddPlanDTO>()
                .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal.Name))
                .ForMember(dest => dest.SystemType, opt => opt.MapFrom(src => src.SystemType.Name))
                .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.Meals));


            CreateMap<AddMealDTO, Meal>()
                .ForMember(dest => dest.IngredientMeals, opt => opt.Ignore());



            CreateMap<IngredientDTO, Ingredient>()
                .ForMember(dest => dest.IngredientMeals, opt => opt.Ignore());

            CreateMap<ShoppingList, ShoppingListDTO>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ShoppingListItems));

            CreateMap<ShoppingListItem, ShoppingListItemDTO>()
                .AfterMap((src, dest) =>
                {
                    dest.IngredientName = src.Ingredient!.Name!;
                });
            //CreateMap<Plan, AddPlanDTO>();

            CreateMap<Meal, AddMealDTO>()
            .ForMember(dest => dest.ingredientsDTO, opt => opt.MapFrom(src =>
                src.IngredientMeals.Select(im => new IngredientDTO
                {
                    Name = im.Ingredient.Name,
                    Unit = im.Ingredient.Unit,
                    Amount = im.Amount
                }).ToList()
            ));
            //---------------------------------------------
            CreateMap<GoalPlan, GoalPlanDTO>();
            CreateMap<GoalPlanDTO, GoalPlan>();
            CreateMap<SystemTypePlan, SystemTypeDTO>();
            CreateMap<SystemTypeDTO, SystemTypePlan>();
            //---------------------------------------------
            CreateMap<AddCustomerDTO, Customer>()
                 .ForMember(dest => dest.ActivityLevel, opt => opt.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.ActivityLevel = src.ActivityLevel;
                 });

            CreateMap<Customer, getCustomerDTO>()
                .ForMember(dest => dest.ActivityLevel, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.ActivityLevel = src.ActivityLevel;

                    dest.Medicines = src.Medicines.Select(m => new addMedicineDTO
                    {
                        MedicineName = m.MedicineName,
                        Option = m.Option,
                        Times = m.MedicineTimes.Select(mt => mt.TakeTime).ToList()
                    }).ToList();

                    dest.DailyWeights = src.DailyWieghts.Select(m => new DailyWieghtDTO
                    {
                        Date = m.Date,
                        Weight = m.Weight,
                    }).ToList();

                    dest.ChronicDiseases = src.ChronicDiseases.Select(cd => cd.DiseaseName).ToList();
                    dest.AvoidFoods = src.AvoidFoods.Select(af => af.FoodName).ToList();
                });
            //---------------------
            CreateMap<addMedicineDTO, Medicine>()
                .ForMember(dest => dest.MedicineTimes, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore());

            CreateMap<Medicine, addMedicineDTO>()
                .ForMember(
                        dest => dest.Times,
                        opt => opt.MapFrom(src =>src.MedicineTimes.Select(mt => mt.TakeTime).ToList()
                        )
    );

            CreateMap<Notification, NotificationDTO>();

            CreateMap<Customer, CustomerDTO>();


        }
    }
}
