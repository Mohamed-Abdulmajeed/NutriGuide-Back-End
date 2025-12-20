using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NutriGuide.DTOs.userDTOs;
using NutriGuide.Helpers;
using NutriGuide.Models;
using NutriGuide.Services;
using NutriGuide.UnitOfWorks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NutriGuide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        UnitOfWork unit;
        IMapper Mapper;
        IEmailService emailService;
        public UsersController(UnitOfWork _unit, IMapper mapper, IEmailService _emailService)
        {
            unit = _unit;
            Mapper = mapper;
            emailService = _emailService;
        }
        //=================================================
        [HttpPost("login")]
        public ActionResult Login(loginDTO loginData)
        {
            try
            {
               
            
            if (loginData == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            User? user = unit.UserRepo.GetByEmail(loginData.Email!);

            if (user == null)
            {
                return Unauthorized("Invalid Email or Password");
            }
            bool IsVerified = user!.IsVerified ?? false;
            if (!IsVerified)
            {
                return Unauthorized("Email not Verified! Please Verify Email Before Sign in");
            }
            else if (!Utils.verifyPassword(loginData.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid Password");
            }
            else
            {
                return Ok(new { token = Utils.generateToken(user.Role, user.Id.ToString(), user.Name) });
            }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================
        [HttpPost("register")]
        public async Task<ActionResult> Register(registerDTO registerData)
        {
            try
            {
                if (registerData == null)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                User? user = unit.UserRepo.GetByEmail(registerData.Email);

                if (user != null)
                {
                    return Conflict($"User is already exists.");
                }
                else
                {
                    string code = Utils.GenerateCode();
                    await emailService.SendEmailAsync(registerData.Email, "Verification Code of Application NutriGuide", $"Your verification code is: {code}");

                    User newUser = Mapper.Map<User>(registerData);
                    newUser.VerificationCode = code;
                    newUser.VerificationCodeExpiry = DateTime.Now.AddMinutes(15);
                    unit.UserRepo.Add(newUser);
                    unit.Save();

                    return Created(string.Empty, new { massege = "User Registered. Verification code sent to email" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //=================================================

        [HttpPost("verify-email-code")]
        public async Task<ActionResult> VerfyCode(VerifyCodeDTO verifyCodeDTO)
        {
            try
            {
                User? user = unit.UserRepo.GetByEmail(verifyCodeDTO.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            else if (user.VerificationCode != verifyCodeDTO.Code)
            {

                return Unauthorized($"Invalid Code");
            }
            else if (user.VerificationCodeExpiry < DateTime.Now)
            {
                return BadRequest("Code expired");
            }
            else
            {
                user.IsVerified = true;
                user.VerificationCode = null;
                user.VerificationCodeExpiry = null;
                unit.Save();
                return Ok(new { token = Utils.generateToken(user.Role, user.Id.ToString(), user.Name) });

            }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //========================================

        [HttpPost("resend-code-Verification")]
        public async Task<IActionResult> ResendCode([FromBody] string email)
        {
            try
            {
                User? user = unit.UserRepo.GetByEmail(email);
            if (user == null)
                return BadRequest("User not found");

            string code = Utils.GenerateCode();

            await emailService.SendEmailAsync(email, "Verification Code of Application NutriGuide", $"Your verification code is: {code}");

            user.VerificationCode = code;
            user.VerificationCodeExpiry = DateTime.Now.AddMinutes(15);
            unit.Save();

            return Ok(new { massege = "Code sent again to email" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //========================================
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            try
            {
                User? user = unit.UserRepo.GetByEmail(email);
            if (user == null)
                return BadRequest("User not found");

            if (user.ResetAttempts >= 10)
            {
                return BadRequest("You have exceeded the maximum number of reset attempts. Please contact support for further assistance.");
            }
            string code = Utils.GenerateCode();

            await emailService.SendEmailAsync(email, "Reset Password Code of Application NutriGuide", $"Your code is: {code}");

            user.ResetPasswordCode = code;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            user.ResetAttempts++;
            unit.Save();

            return Ok(new { massege = "Reset code sent to email" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }
        //========================================

        [HttpPost("reset-code-Verification")]
        public async Task<ActionResult> ResetCodeVerification(VerifyCodeDTO verifyCodeDTO)
        {
            try
            {
                User? user = unit.UserRepo.GetByEmail(verifyCodeDTO.Email);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            else if (user.ResetPasswordCode != verifyCodeDTO.Code)
            {
                return Unauthorized($"Invalid Code");
            }
            else if (user.ResetPasswordExpiry < DateTime.Now)
            {
                return BadRequest("Code expired");
            }
            else
            {

                return Ok(new { massege = "Code Verified" });

            }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //========================================
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(resetPassDTO resetPass)
        {
            try
            {
                if (resetPass == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            User? user = unit.UserRepo.GetByEmail(resetPass.Email);
            if (user == null)
                return BadRequest("User not found");

            else if (user.ResetPasswordCode != resetPass.Code)
            {
                return Unauthorized($"Invalid Code");
            }
            else if (user.ResetPasswordExpiry < DateTime.Now)
            {
                return BadRequest("Code expired");
            }
            else if (resetPass.NewPassword.Length < 6)
            {
                return BadRequest("Password must be at least 6 characters long");
            }


            // Hash password
            user.PasswordHash = Utils.generatePasswordHash(resetPass.NewPassword);
            user.ResetPasswordCode = null;
            user.ResetPasswordExpiry = null;
            user.ResetAttempts = 0;
            unit.Save();

            return Ok(new { message = "Password reset successfully", token = Utils.generateToken(user.Role, user.Id.ToString(), user.Name) });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message);
            }
        }

        //========================================




    }
}
