using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Middleware.GlobalExceptionHandler;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Service;

namespace HelloGreetingApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserBL _userBL;
        private readonly ILogger<UserRL> _logger;
        public UserController(IUserBL userBL, ILogger<UserRL> logger)
        {
            _userBL = userBL;
            _logger = logger;
        }
        /// <summary>
        /// Post Method for Login User
        /// </summary>
        /// <param name="userLoginModel"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginUserModel userLoginModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var user = _userBL.Login(userLoginModel);
                if (user != null)
                {
                    response.Success = true;
                    response.Message = "User Login Successfully.";
                    response.Data = user;
                    return Ok(response);
                }
                return BadRequest("Invalid Credentials!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Id not found Error.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(errorResponse);
            }
           
        }
        /// <summary>
        /// Postmethod to register the user
        /// </summary>
        /// <param name="registrationUserModel"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegistrationUserModel registrationUserModel)
        {
            try
            {
                var response = new ResponseModel<string>();
                var data = await _userBL.Registration(registrationUserModel);


                response.Success = true;
                response.Message = "User registered Successfully.";
                response.Data = "";
                return Ok(response);
            }


            catch (Exception ex)
            {
                _logger.LogError("User Already Registered.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return BadRequest(errorResponse);
            }

            
        }
        /// <summary>
        /// Post method to Forget password
        /// </summary>
        /// <param name="forgetPasswordModel"></param>
        /// <returns></returns>
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel forgetPasswordModel)
        {
            var result = await _userBL.ForgetPasswordAsync(forgetPasswordModel);
            if (!result)
            {
                return BadRequest("Email not found!");
            }
               
            return Ok("Reset password email sent successfully.");
        }
        /// <summary>
        /// post method for Reset password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            var result = await _userBL.ResetPasswordAsync(model);
            if (!result) 
            { 
                return BadRequest("Invalid or expired token."); 
            }
            return Ok("Password reset successfully.");
        }

    }
}
