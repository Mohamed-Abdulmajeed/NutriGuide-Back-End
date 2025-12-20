using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutriGuide.DTOs.mealDTOs;
using NutriGuide.DTOs.planDTOs;
using NutriGuide.Models;
using NutriGuide.UnitOfWorks;
using System.Numerics;
using System.Security.Claims;

namespace NutriGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        UnitOfWork unit;
        IMapper Mapper;
        public MealsController(UnitOfWork _unit, IMapper mapper)
        {
            unit = _unit;
            Mapper = mapper;
        }
        //=================================================
        [Authorize]
        [HttpPost("AddMeal")]
        public IActionResult AddMeal(AddMealDTO mealDTO)
        {
            try
            {
                if (mealDTO == null)
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

            Meal meal = Mapper.Map<Meal>(mealDTO);

            meal.IngredientMeals = new List<IngredientMeal>();

            foreach (var ingDto in mealDTO.ingredientsDTO)
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

            meal.Customers.Add(unit.CustomerRepo.GetById(int.Parse(CustomerId))!);

            unit.MealRepo.Add(meal);

                unit.Save();

            return Created(string.Empty, new { massege = "Meal added successfully", MealId = meal.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        [Authorize]
        [HttpGet("getById")]
        public IActionResult getMealByID( int mealId)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();

            Meal meal = unit.MealRepo.GetById(mealId)!;

            if (meal == null)
                return BadRequest($"Meal does not exist.");

            AddMealDTO mealDTO = Mapper.Map<AddMealDTO>(meal);

            return Ok(mealDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //==================================================
        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateStatusMeal(updateMealDTO UpMealDTO)
        {
            try
            {
                if (UpMealDTO == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            Meal meal = unit.MealRepo.GetById(UpMealDTO.MealId)!;

            if (meal == null)
                return BadRequest($"Meal does not exist.");

            meal.Status = UpMealDTO.NewStatus;
            unit.Save();

            return Ok(new { massege = "Status Updated Success" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpPost("addFavorite")]
        public IActionResult AddFavoriteMeal([FromBody] int mealId)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();

            if (unit.MealRepo.GetIncludeCustomer(mealId) == null)
                return BadRequest($"Meal does not exist.");


            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (idClaim == null)
                return BadRequest("Id not found in token");

            int CustomerId = int.Parse(idClaim.Value);


            if (unit.CustomerRepo.GetById(CustomerId) == null)
                return BadRequest($"Customer does not exist.");

            Meal meal = unit.MealRepo.GetIncludeCustomer(mealId)!;

            if (meal.Customers.Any(c => c.Id == CustomerId))
                return BadRequest("Meal is Already in favorites");

            meal.Customers.Add(unit.CustomerRepo.GetById(CustomerId)!);


                unit.Save();

            return Ok(new { massege = "Meal added to favorites successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpDelete("deleteFavorite")]
        public IActionResult DeleteFavoriteMeal( int mealId)
        {
            try
            {
                if (!ModelState.IsValid)
                return BadRequest();

            if (unit.MealRepo.GetIncludeCustomer(mealId) == null)
                return BadRequest($"Meal does not exist.");


            var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
            if (idClaim == null)
                return BadRequest("Id not found in token");

            int CustomerId = int.Parse(idClaim.Value);

            if (unit.CustomerRepo.GetById(CustomerId) == null)
                return BadRequest($"Customer does not exist.");

            Meal meal = unit.MealRepo.GetIncludeCustomer(mealId)!;

            if (!meal.Customers.Any(c => c.Id == CustomerId))

                return BadRequest("Meal is not in favorites");

            var customer = unit.CustomerRepo.GetById(CustomerId)!;

            meal.Customers.Remove(customer);

            unit.Save();

            return Ok(new { massege = "Meal removed from favorites successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //==================================================
        [Authorize]
        [HttpGet("GetAllFavorite")]
        public IActionResult GetAllFavorite()
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");

                int CustomerId = int.Parse(idClaim.Value);

                if (unit.CustomerRepo.GetById(CustomerId) == null)
                    return BadRequest($"Customer does not exist.");

                List<Meal> meals = unit.MealRepo.GetAllFavorite(CustomerId)!;

                if (meals ==null)
                    return BadRequest("No favorite meal to the customer");

                List<AddMealDTO> mealDTOs = Mapper.Map<List<AddMealDTO>>(meals);

                return Ok(mealDTOs);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
    }
}