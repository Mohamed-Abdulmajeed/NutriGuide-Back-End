using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriGuide.DTOs.planDTOs;
using NutriGuide.DTOs.ShoppingListDTOs;
using NutriGuide.DTOs.userDTOs;
using NutriGuide.Models;
using NutriGuide.UnitOfWorks;
using System.Numerics;
using System.Security.Claims;

namespace NutriGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlansController : ControllerBase
    {
        UnitOfWork unit;
        IMapper Mapper;
        public PlansController(UnitOfWork _unit, IMapper mapper)
        {
            unit = _unit;
            Mapper = mapper;
        }
        //=================================================

        [Authorize]
        [HttpPost("AddPlan")]
        public async Task<IActionResult> AddPlan(AddPlanDTO addPlanDTO)
        {
            try
            {
                if (addPlanDTO == null)
                return BadRequest();
            //------------------
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //------------------
            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (idClaim == null)
                return BadRequest("Id not found in token");

            var CustomerId = idClaim.Value;

            if (CustomerId == null)
                return BadRequest($"Customer does not exist.");

            if (unit.CustomerRepo.GetById(int.Parse(CustomerId)) == null)
                return BadRequest($"Customer does not exist.");
                //-----------------
                var goal = unit.GoalPlanRepo.GetByName(addPlanDTO.Goal);
                var systemType = unit.SystemTypePlanRepo.GetByName(addPlanDTO.SystemType);

                if (goal == null || systemType == null)
                    return BadRequest("Goal or SystemType not found");


                Plan plan = new Plan
                {
                    GoalId = goal.Id,
                    SystemTypeId = systemType.Id,
                    Name = addPlanDTO.Name,
                    NumberOfDays = addPlanDTO.NumberOfDays,
                    NumberOfMeals = addPlanDTO.NumberOfMeals,
                    DailyCalories = addPlanDTO.DailyCalories,
                    StartDate = addPlanDTO.StartDate,
                    Meals = Mapper.Map<List<Meal>>(addPlanDTO.Meals)
                };
                
                plan.CustomerId = int.Parse(CustomerId);
            foreach (Meal meal in plan.Meals)
            {
                meal.PlanId = plan.Id;
                meal.IngredientMeals = new List<IngredientMeal>();

                var mealDto = addPlanDTO.Meals.First(m => m.Name == meal.Name);

                foreach (var ingDto in mealDto.ingredientsDTO)
                {

                    Ingredient ingredient = unit.IngredientRepo.GetByNameAndUnit(ingDto.Name!, ingDto.Unit!)!;

                    if (ingredient == null)
                    {
                        ingredient = Mapper.Map<Ingredient>(ingDto);
                        unit.IngredientRepo.Add(ingredient);

                        meal.IngredientMeals.Add(new IngredientMeal
                        {
                            Ingredient = ingredient,
                            Amount = ingDto.Amount
                        });
                    }
                    else
                    {
                        meal.IngredientMeals.Add(new IngredientMeal
                        {
                            IngredientId = ingredient.Id,
                            MealId = meal.Id,
                            Amount = ingDto.Amount
                        });
                    }
                }
            }
                unit.PlanRepo.Add(plan);
                unit.Save();
                //------------------ Generate Shopping List ------------------
                ShoppingList shoppingList = new ShoppingList
            {
                PlanId = plan.Id,
                GeneratedDate = DateTime.Now,
                IsCompleted = false
            };
            //plan.ShoppingLists = new List<ShoppingList> { shoppingList };
                //-----------------
                var ingredientsGrouped = plan.Meals
                .SelectMany(m => m.IngredientMeals)
                .GroupBy(i => i.IngredientId)
                .Select(g => new
                {
                    IngredientId = g.Key,
                    TotalAmount = g.Sum(x => x.Amount),
                    Unit = unit.IngredientRepo.GetById(g.Key)!.Unit
                }).ToList();

                shoppingList.ShoppingListItems = new List<ShoppingListItem>();

                foreach (var item in ingredientsGrouped)
                {
                    shoppingList.ShoppingListItems.Add(new ShoppingListItem
                    {
                        IngredientId = item.IngredientId,
                        Amount = item.TotalAmount,
                        Unit = item.Unit
                    });
                }
                unit.ShoppingListRepo.Add(shoppingList);

                //-----------------
                Notification notifcation = new Notification
                {
                    CustomerId = int.Parse(CustomerId),
                    Title = "الخطط",
                    Type = "انشاء خطة",
                    Message = "تم اضافة خطة جديدة اليوم اللى خططك الغذائية",
                    ScheduleTime = DateTime.Now,
                };

                unit.NotificationRepo.Add(notifcation);

                unit.Save();

            return Created(string.Empty, new { massege = "Plan added successfully", PlanId = plan.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpGet("GetShoppingList")]
        public IActionResult GetShoppingList(int planId)
        {
            try
            {
                // 1) Check if plan exists
                Plan plan = unit.PlanRepo.GetById(planId)!;
            if (plan == null)
                return NotFound($"Plan with ID {planId} not found");

            ShoppingList shoppingList = unit.ShoppingListRepo.GetWithItems(planId)!;

            if (shoppingList == null)
                return NotFound("No shopping list exists for this plan.");

            ShoppingListDTO shoppingListDTO = Mapper.Map<ShoppingListDTO>(shoppingList);

            return Ok(shoppingListDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [HttpGet("GetPlanById")]
        public IActionResult GetPlanByID(int planId)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();

            Plan plan = unit.PlanRepo.GetwithInclude(planId)!;

            if (plan == null)
                return BadRequest($"Plan does not exist.");

            AddPlanDTO planDTO = Mapper.Map<AddPlanDTO>(plan);

            return Ok(planDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [HttpGet("GetCurrentPlan")]
        public IActionResult GetCurrentPlan()
        {
            try
            {
               
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");

                int CustomerId = int.Parse(idClaim.Value);

                Customer cust = unit.CustomerRepo.GetById(CustomerId)!;
                if (cust == null)
                    return BadRequest($"Customer does not exist.");


                Plan plan = unit.PlanRepo.GetCurrentPlan(CustomerId)!;

                if (plan == null)
                    return BadRequest($"No plan is available today");

                AddPlanDTO planDTO = Mapper.Map<AddPlanDTO>(plan);

                return Ok(planDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================

        [Authorize]
        [HttpGet("GetAllCustomerPlan")]
        public IActionResult GetAllCustomerPlan()
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();

            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (idClaim == null)
                return BadRequest("Id not found in token");

            int CustomerId = int.Parse(idClaim.Value);

            Customer cust = unit.CustomerRepo.GetById(CustomerId)!;
            if (cust == null)
                return BadRequest($"Customer does not exist.");


            List<Plan> plans = unit.PlanRepo.GetAllwithInclude(CustomerId)!;

            if (plans == null)
                return NotFound($"Customer does not has plan.");

            List<AddPlanDTO> planDTO = Mapper.Map<List<AddPlanDTO>>(plans);

            return Ok(planDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        //==================================================
        [Authorize]
        [HttpPost("addGoalPlan")]
        public ActionResult addGoalPlan(GoalPlanDTO goalPlan)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");


                if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (unit.GoalPlanRepo.GetByName(goalPlan.Name) != null)
                return BadRequest($"Goal Plan with name {goalPlan} already exists.");

            GoalPlan GP = new GoalPlan() { Name = goalPlan.Name };

            unit.GoalPlanRepo.Add(GP);
            unit.Save();

            return Created(string.Empty, new { massege = "Goal Plan added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //==================================================
        [Authorize]
        [HttpPut("updateGoalPlan")]
        public ActionResult updateGoalPlan(UpdateGoalPlanDTO updateGoal)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                if (!ModelState.IsValid)
                return BadRequest(ModelState);

            GoalPlan? existingGoalPlan = unit.GoalPlanRepo.GetByName(updateGoal.OldName);

            if (existingGoalPlan == null)
                return BadRequest($"Goal Plan with Name {updateGoal.OldName} does not exist.");

            existingGoalPlan.Name = updateGoal.NewName;
            unit.Save();

            return Ok(new { massege = "Goal Plan Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpDelete("deleteGoalPlan")]
        public ActionResult deleteGoalPlan(string goalPlan)
        {
            try
            {

                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                if (goalPlan == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            GoalPlan GP = unit.GoalPlanRepo.GetByName(goalPlan)!;

            if (GP == null)
                return NotFound($"Goal Plan with name {goalPlan} not Found.");

            unit.GoalPlanRepo.Delete(GP.Id);
            unit.Save();

            return Ok(new { massege = "Goal Plan deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpGet("getAllGoalPlan")]
        public ActionResult getAllGoalPlan()
        {
            try
            {
                List<GoalPlan> goalPlans = unit.GoalPlanRepo.GetAll()!;
            if (goalPlans == null || goalPlans.Count == 0)
                return NotFound("No Goal Plans found.");

            List<GoalPlanDTO> goalPlanDTOs = Mapper.Map<List<GoalPlanDTO>>(goalPlans);
            return Ok(goalPlanDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        //==================================================
        [Authorize]
        [HttpPost("addSystemTypePlan")]
        public ActionResult addSystemTypePlan(SystemTypeDTO systemPlan)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (unit.GoalPlanRepo.GetByName(systemPlan.Name) != null)
                return BadRequest($"System Type Plan with name {systemPlan} already exists.");

            SystemTypePlan STP = new SystemTypePlan() { Name = systemPlan.Name };

            unit.SystemTypePlanRepo.Add(STP);
            unit.Save();


            return Created(string.Empty, new { massege = "System Type Plan added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //==================================================
        [Authorize]
        [HttpPut("updateSystemTypePlan")]
        public ActionResult updateSystemTypePlan(UpdateSystemTypePlanDTO updateSystem)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                if (!ModelState.IsValid)
                return BadRequest(ModelState);

            SystemTypePlan? existingSystem = unit.SystemTypePlanRepo.GetByName(updateSystem.OldName);

            if (existingSystem == null)
                return BadRequest($"System Type Plan with Name {updateSystem.OldName} does not exist.");

            existingSystem.Name = updateSystem.NewName;
            unit.Save();

            return Ok(new { massege = "System Type Plan Updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpDelete("deleteSystemTypePlan")]
        public ActionResult deleteSystemTypePlan(string systemType)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                if (systemType == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            SystemTypePlan STP = unit.SystemTypePlanRepo.GetByName(systemType)!;

            if (STP == null)
                return NotFound($"System Type Plan with name {systemType} not Found.");

            unit.SystemTypePlanRepo.Delete(STP.Id);
            unit.Save();

            return Ok(new { massege = "System Type Plan deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpGet("getAllSystemTypePlan")]
        public ActionResult getAlSystemTypePlan()
        {
            try
            {
                List<SystemTypePlan> systemTypes = unit.SystemTypePlanRepo.GetAll()!;
            if (systemTypes == null || systemTypes.Count == 0)
                return NotFound("No System Type Plans found.");

            List<SystemTypeDTO> systemTypesDTO = Mapper.Map<List<SystemTypeDTO>>(systemTypes);
            return Ok(systemTypesDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        //==================================================


    }
}