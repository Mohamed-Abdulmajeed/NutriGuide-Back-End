using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriGuide.DTOs.CustomerDTO;
using NutriGuide.Helpers;
using NutriGuide.Models;
using NutriGuide.UnitOfWorks;
using System.Security.Claims;

namespace NutriGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        UnitOfWork unit;
        IMapper Mapper;
        public CustomersController(UnitOfWork _unit, IMapper mapper)
        {
            unit = _unit;
            Mapper = mapper;
        }
        //=================================================
        [Authorize]
        [HttpPost("addCustomer")]
        public IActionResult AddCustomer(AddCustomerDTO customerDTO)
        {
            try
            {
                if (customerDTO == null)
                {
                    return BadRequest();
                }
                //------------------
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //------------------
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");

                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.UserRepo.GetById(id) == null)
                {
                    return NotFound("User not found");
                }
                //------------------
                if (unit.CustomerRepo.GetById(id) != null)
                {
                    return Conflict("Customer already exists for this user");
                }

                Customer customer = Mapper.Map<AddCustomerDTO, Customer>(customerDTO);
                customer.Id = id;
                unit.CustomerRepo.Add(customer);

                unit.Save();

                return Created(string.Empty, new { massege = "Customer added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //=================================================
        [Authorize]
        [HttpGet("getCustomer")]
        public IActionResult GetCustomer()
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                Customer customer = unit.CustomerRepo.GetWithInclude(id)!;
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                //------------------
                getCustomerDTO customerDTO = Mapper.Map<Customer, getCustomerDTO>(customer);
                
                User user = unit.UserRepo.GetById(id)!;


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
        [Authorize]
        [HttpPut("updateHieght")]
        public IActionResult UpdateHieght([FromBody] decimal height)
        {
            try
            {
                if (height <= 0)
                {
                    return BadRequest("Height must be greater than zero.");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                Customer? customer = unit.CustomerRepo.GetById(id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                //------------------
               
                customer.Height = height;


                unit.Save();
                return Ok(new { message = "Height updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //====================================================
        [Authorize]
        [HttpPut("updateActivity")]
        public IActionResult UpdateActivityLevel([FromBody] decimal activity)
        {
            try
            {
                if (activity < 1.1M || activity > 1.9M)
                {
                    return BadRequest("level must between range 1.1 and 1.9");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                Customer? customer = unit.CustomerRepo.GetById(id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                //------------------

                customer.ActivityLevel = activity;

                unit.Save();
                return Ok(new { message = "ActivitLivel updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //========================================================
        [Authorize]
        [HttpPut("updateWieght")]
        public IActionResult UpdateWeight([FromBody] decimal newWieght)
        {
            try
            {
                if (newWieght < 10)
                {
                    return BadRequest("new Wieght cannot be less than 10.");
                }

                //------------------
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                Customer customer = unit.CustomerRepo.GetById(id)!;
                if (customer == null)
                    return NotFound("Customer not found");
                //------------------
                
                DailyWieght dailyWieght = new DailyWieght
                {
                    CustomerId = id,
                    Weight = customer.Weight,
                };
                unit.DailyWieghtRepo.Add(dailyWieght);
                Notification notifcation = new Notification
                {
                    CustomerId = id,
                    Title = "الوزن",
                    Type = " تحديث الوزن",
                    Message = $"تم تحديث الوزن من {customer.Weight} الى {newWieght}",
                    ScheduleTime = DateTime.Now,
                };

                customer.Weight = newWieght;

                unit.NotificationRepo.Add(notifcation);

                unit.Save();
                return Ok(new { message = "Weight updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //====================================================
        //====================================================
        [Authorize]
        [HttpPost("addAvoidFood")]
        public IActionResult AddAvoidFood([FromBody] string food)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(food))
                {
                    return BadRequest("Food cannot be empty.");
                }
                if (food.Length > 20)
                {
                    return BadRequest("Food name is too long.");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                Customer? customer = unit.CustomerRepo.GetById(id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                //------------------
                if (unit.avoidFoodRepo.GetAll()!.Any(af => af.CustomerId == id && af.FoodName.ToLower() == food.ToLower()))
                {
                    return Conflict("This food is already in the avoid list.");
                }
                //------------------

                AvoidFood avoidFood = new AvoidFood
                {
                    CustomerId = id,
                    FoodName = food
                };
                unit.avoidFoodRepo.Add(avoidFood);
                unit.Save();

                return Created(string.Empty, new { massege = "Food added to avoid list successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //====================================================
        [Authorize]
        [HttpPut("updateAvoidFood")]
        public IActionResult UpdateAvoidFood(updateFoodDTO foodDTO)
        {
            try
            {
                if (foodDTO == null)
                {
                    return BadRequest("Food name cannot be empty.");
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //------------------
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");
                //------------------
                AvoidFood avoidFood = unit.avoidFoodRepo.GetAll()!.FirstOrDefault(af => af.CustomerId == id && af.FoodName.ToLower() == foodDTO.OldFood.ToLower())!;

                if (avoidFood == null)
                {
                    return Conflict("This food not in the avoid list of the customer.");
                }

                avoidFood.FoodName = foodDTO.NewFood;
                unit.Save();
                return Ok(new { message = "Avoid food updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //====================================================
        [Authorize]
        [HttpDelete("deleteAvoidFood")]
        public IActionResult DeleteAvoidFood( string food)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(food))
                {
                    return BadRequest("Food name cannot be empty.");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");
                //------------------
                AvoidFood avoidFood = unit.avoidFoodRepo.GetAll()!.FirstOrDefault(af => af.CustomerId == id && af.FoodName.ToLower() == food.ToLower())!;
                if (avoidFood == null)
                {
                    return Conflict("This food not in the avoid list of the customer.");
                }
                unit.avoidFoodRepo.Delete(avoidFood.Id);
                unit.Save();
                return Ok(new { message = "Avoid food deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //====================================================
        //====================================================

        [Authorize]
        [HttpPost("addDisease")]
        public IActionResult AddDisease([FromBody] string diseaseName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(diseaseName))
                {
                    return BadRequest("Disease Name cannot be empty.");
                }
                if (diseaseName.Length > 20)
                {
                    return BadRequest("Disease Name is too long.");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                Customer? customer = unit.CustomerRepo.GetById(id);
                if (customer == null)
                {
                    return NotFound("Customer not found");
                }
                //------------------
                if (unit.ChronicDiseaseRepo.GetAll()!.Any(af => af.CustomerId == id && af.DiseaseName.ToLower() == diseaseName.ToLower()))
                {
                    return Conflict("This food is already in the avoid list.");
                }
                //------------------

                ChronicDisease disease = new ChronicDisease
                {
                    CustomerId = id,
                    DiseaseName = diseaseName
                };
                unit.ChronicDiseaseRepo.Add(disease);
                unit.Save();

                return Created(string.Empty, new { massege = "Disease Name added to Dieases list successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //====================================================
        [Authorize]
        [HttpPut("updateDisease")]
        public IActionResult UpdateDisease(updateDiseaseDTO diseaseDTO)
        {
            try
            {
                if (diseaseDTO == null)
                {
                    return BadRequest("Disease Name cannot be empty.");
                }
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                //------------------
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");
                //------------------
                ChronicDisease chronicDisease = unit.ChronicDiseaseRepo.GetAll()!.FirstOrDefault(af => af.CustomerId == id && af.DiseaseName.ToLower() == diseaseDTO.OldDisease.ToLower())!;

                if (chronicDisease == null)
                {
                    return Conflict("This Disease not in the Diseases list of the customer.");
                }

                chronicDisease.DiseaseName = diseaseDTO.NewDisease;
                unit.Save();
                return Ok(new { message = "Disease Name updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //====================================================
        [Authorize]
        [HttpDelete("deleteDisease")]
        public IActionResult DeleteDisease(string disease)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(disease))
                {
                    return BadRequest("Disease Name cannot be empty.");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");
                //------------------
                ChronicDisease chronicDisease = unit.ChronicDiseaseRepo.GetAll()!.FirstOrDefault(af => af.CustomerId == id && af.DiseaseName.ToLower() == disease.ToLower())!;
                if (chronicDisease == null)
                {
                    return Conflict("This Disease Name not in the disease list of the customer.");
                }
                unit.ChronicDiseaseRepo.Delete(chronicDisease.Id);
                unit.Save();
                return Ok(new { message = "Disease Name deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //====================================================
        //====================================================
        [Authorize]
        [HttpPost("addMedicine")]
        public IActionResult addMedicine(addMedicineDTO medicineDTO)
        {
            try
            {
                if (medicineDTO == null)
                    return BadRequest();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");

                if (unit.MedicineRepo.GetAll()!.Any(af => af.CustomerId == id && af.MedicineName.ToLower() == medicineDTO.MedicineName.ToLower()))
                {
                    return Conflict("This Medicine is already in the Medicine list.");
                }
                Medicine medicine = Mapper.Map<Medicine>(medicineDTO);
                medicine.CustomerId = id;
                 medicine.MedicineTimes = medicineDTO.Times!.Select(t => new MedicineTime{TakeTime = t}).ToList();
                

                unit.MedicineRepo.Add(medicine);
                unit.Save();

                return Created(string.Empty, new { massege = "Medicine Name added to Medicine list successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=======================================================
        [Authorize]
        [HttpDelete("deleteMedicine")]
        public IActionResult deleteMedicine(string medicineName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(medicineName))
                {
                    return BadRequest("medicine Name cannot be empty.");
                }
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");
                //------------------
                Medicine medicine = unit.MedicineRepo.GetAll()!.FirstOrDefault(af => af.CustomerId == id && af.MedicineName.ToLower() == medicineName.ToLower())!;
                if (medicine == null)
                {
                    return Conflict("This medicine Name not in the medicine list of the customer.");
                }
                unit.MedicineRepo.Delete(medicine.Id);
                unit.Save();
                return Ok(new { message = "medicine Name deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //=======================================================
        [Authorize]
        [HttpGet("getNotification")]
        public IActionResult getNotification()
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");

                List<Notification> notifications = unit.NotificationRepo.GetAll()!.Where(af => af.CustomerId == id).OrderByDescending(n=>n.ScheduleTime).Take(15).ToList()!;

                if (notifications == null)
                {
                    return NotFound("Notification is empty");
                }
                List<NotificationDTO> notificationDTOs = Mapper.Map<List<NotificationDTO>>(notifications);

                return Ok(notificationDTOs);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            } 
        }
        [Authorize]
        [HttpPut("updateNotification")]
        public IActionResult updateNotification( int NotId)
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
                if (idClaim == null)
                    return BadRequest("Id not found in token");
                int id = int.Parse(idClaim.Value);
                //------------------
                if (unit.CustomerRepo.GetById(id) == null)
                    return NotFound("Customer not found");

                Notification notification = unit.NotificationRepo.GetById(NotId)!;
                if (notification == null)
                {
                    return NotFound("Notification is empty");
                }
                notification.IsRead = true;
                unit.Save();
                return Ok(new { message = "Notification update is read success"});

            }

            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

    }   
}