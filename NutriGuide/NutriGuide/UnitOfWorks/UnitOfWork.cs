using NutriGuide.Models;
using NutriGuide.Repository;

namespace NutriGuide.UnitOfWorks
{
    public class UnitOfWork
    {

        NutriGuideDbContext Db;

        GenericRepository<AvoidFood> _AvoidFoodRepo;

        GenericRepository<ChronicDisease> _ChronicDiseaseRepo;

        CustomerRepository _CustomerRepo;

        GenericRepository<DailyWieght> _DailyWieghtRepo;

        GoalPlanRepository _GoalPlanRepo;

        IngredientRepository _IngredientRepo;

        GenericRepository<IngredientMeal> _IngredientMealRepo;

        MealRepository _MealRepo;

        MedicineRepository _MedicineRepo;

        GenericRepository<Notification> _NotificationRepo;

        PlanRepository _PlanRepo;

        ShoppingListRepository _ShoppingListRepo;

        ShoppingListItemRepository _ShoppingListItemRepo;

        SystemTypeRepository _SystemTypePlanRepo;

        UserRepository _UserRepo;

        public UnitOfWork(NutriGuideDbContext db)
        {
            Db = db;
        }

        public GenericRepository<AvoidFood> avoidFoodRepo
        {
            get
            {
                if (_AvoidFoodRepo == null)
                    _AvoidFoodRepo = new GenericRepository<AvoidFood>(Db);
                return _AvoidFoodRepo;
            }
        }

        public GenericRepository<ChronicDisease> ChronicDiseaseRepo
        {
            get
            {
                if (_ChronicDiseaseRepo == null)
                    _ChronicDiseaseRepo = new GenericRepository<ChronicDisease>(Db);
                return _ChronicDiseaseRepo;
            }
        }

        public CustomerRepository CustomerRepo
        {
            get
            {
                if (_CustomerRepo == null)
                    _CustomerRepo = new CustomerRepository(Db);
                return _CustomerRepo;
            }
        }

        public GenericRepository<DailyWieght> DailyWieghtRepo
        {
            get
            {
                if (_DailyWieghtRepo == null)
                    _DailyWieghtRepo = new GenericRepository<DailyWieght>(Db);
                return _DailyWieghtRepo;
            }
        }

        public GoalPlanRepository GoalPlanRepo
        {
            get
            {
                if (_GoalPlanRepo == null)
                    _GoalPlanRepo = new GoalPlanRepository(Db);
                return _GoalPlanRepo;
            }
        }

        public IngredientRepository IngredientRepo
        {
            get
            {
                if (_IngredientRepo == null)
                    _IngredientRepo = new IngredientRepository(Db);
                return _IngredientRepo;
            }
        }

        public GenericRepository<IngredientMeal> IngredientMealRepo
        {
            get
            {
                if (_IngredientMealRepo == null)
                    _IngredientMealRepo = new GenericRepository<IngredientMeal>(Db);
                return _IngredientMealRepo;
            }
        }

        public MealRepository MealRepo
        {
            get
            {
                if (_MealRepo == null)
                    _MealRepo = new MealRepository(Db);
                return _MealRepo;
            }
        }

        public MedicineRepository MedicineRepo
        {
            get
            {
                if (_MedicineRepo == null)
                    _MedicineRepo = new MedicineRepository(Db);
                return _MedicineRepo;
            }
        }

        public GenericRepository<Notification> NotificationRepo
        {
            get
            {
                if (_NotificationRepo == null)
                    _NotificationRepo = new GenericRepository<Notification>(Db);
                return _NotificationRepo;
            }
        }

        public PlanRepository PlanRepo
        {
            get
            {
                if (_PlanRepo == null)
                    _PlanRepo = new PlanRepository(Db);
                return _PlanRepo;
            }
        }

        public ShoppingListRepository ShoppingListRepo
        {
            get
            {
                if (_ShoppingListRepo == null)
                    _ShoppingListRepo = new ShoppingListRepository(Db);
                return _ShoppingListRepo;
            }
        }

        public ShoppingListItemRepository ShoppingListItemRepo
        {
            get
            {
                if (_ShoppingListItemRepo == null)
                    _ShoppingListItemRepo = new ShoppingListItemRepository(Db);
                return _ShoppingListItemRepo;
            }
        }

        public SystemTypeRepository SystemTypePlanRepo
        {
            get
            {
                if (_SystemTypePlanRepo == null)
                    _SystemTypePlanRepo = new SystemTypeRepository(Db);
                return _SystemTypePlanRepo;
            }
        }

        public UserRepository UserRepo
        {
            get
            {
                if (_UserRepo == null)
                    _UserRepo = new UserRepository(Db);
                return _UserRepo;
            }
        }

        public void Save()
        {
            Db.SaveChanges();
        }
    }
}
