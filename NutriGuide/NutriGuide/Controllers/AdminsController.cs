using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NutriGuide.DTOs.CustomerDTO;
using NutriGuide.DTOs.mealDTOs;
using NutriGuide.DTOs.ShoppingListDTOs;
using NutriGuide.Models;
using NutriGuide.UnitOfWorks;
using System.Security.Claims;

namespace NutriGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        UnitOfWork unit;
        IMapper Mapper;
        public AdminsController(UnitOfWork _unit, IMapper mapper)
        {
            unit = _unit;
            Mapper = mapper;
        }
        //=================================================
        [Authorize]
        [HttpGet("getAllCustomer")]
        public IActionResult GetAllCustomer()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");
                //------------------
                List<Customer> customers = unit.CustomerRepo.GetAll()!;

                List<CustomerDTO> customerDTOs = Mapper.Map<List<CustomerDTO>>(customers);

                return Ok(customerDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        //=================================================
        [Authorize]
        [HttpGet("getAllFavoriteMeal")]
        public IActionResult getAllFavoriteMeal()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized(new { message = "Access denied. Admins only." });

                var customers = unit.CustomerRepo.GetAll();
                if (customers == null || !customers.Any())
                    return NotFound(new { message = "No customers found" });

                List<Meal> allFavoriteMeals = new List<Meal>();

                foreach (var customer in customers)
                {
                    var favorites = unit.MealRepo.GetAllFavorite(customer.Id);

                    if (favorites != null && favorites.Any())
                        allFavoriteMeals.AddRange(favorites);
                }

                if (!allFavoriteMeals.Any())
                    return NotFound(new { message = "No favorite meals found for any customer" });

                var mealDTOs = Mapper.Map<List<AddMealDTO>>(allFavoriteMeals);

                return Ok(mealDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        //=================================================
        [Authorize]
        [HttpGet("getAllAvoidFoods")]
        public IActionResult getAllAvoidFoods()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                List<string> avoidFoods = unit.avoidFoodRepo.GetAll()!.Select(af => af.FoodName).ToList();

                return Ok(avoidFoods);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        //=================================================
        [Authorize]
        [HttpGet("getAllDiseases")]
        public IActionResult getAllDiseases()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                List<string> diseases = unit.ChronicDiseaseRepo.GetAll()!.Select(af => af.DiseaseName).ToList();

                return Ok(diseases);




                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        //=================================================
        [Authorize]
        [HttpGet("getAllMedicines")]
        public IActionResult getAllMedicines()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                List<string> medicines = unit.MedicineRepo.GetAll()!.Select(af => af.MedicineName).ToList();

                return Ok(medicines);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        //=================================================
        [Authorize]
        [HttpGet("getAllShoppingLists")]
        public IActionResult getAllShoppingLists()
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                List<ShoppingListItem> shoppingLists = unit.ShoppingListItemRepo.GetWithInclude()!;

                List<ShoppingListItemDTO> shoppingListDTOs = Mapper.Map<List<ShoppingListItemDTO>>(shoppingLists);

                return Ok(shoppingListDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        //=================================================
        [Authorize]
        [HttpGet("getCustomerbyId")]
        public IActionResult getCustomerbyId(int id)
        {
            try
            {
                var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (role != "Admin")
                    return Unauthorized("Access denied. Admins only.");

                Customer customer = unit.CustomerRepo.GetWithInclude(id)!;
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                User user = unit.UserRepo.GetById(id)!;
                //------------------
                getCustomerDTO customerDTO = Mapper.Map<Customer, getCustomerDTO>(customer);
                customerDTO.Email = user.Email;
                customerDTO.Name = user.Name;
                customerDTO.Phone = user.Phone;

                return Ok(customerDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================







    }
}
