using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog.Web;
using RepositoryLayer.Service;
using BusinessLayer.Interface;
using RepositoryLayer.Entity;
using Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Authorization;


namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        ResponseModel<string> responseModel ;
        private readonly IGreetingBL _greetingBL;
        private static List<RequestModel> requests = new List<RequestModel>
        {
        new RequestModel { Key = "User001", Value = 100 },
        new RequestModel { Key = "User002", Value = 200 }
        };
        private readonly ILogger<HelloGreetingController> _logger;

        private readonly GreetingModel _greetingModel;

        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger, GreetingModel greetingModel)
        {
            _logger = logger;//Dependency Inject
            _greetingBL = greetingBL;
            _greetingModel = greetingModel;
        }
        

        /// <summary>
        /// Get  method to get the greeting message
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        public IActionResult Get()
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Hello to Greeting App API EndPoint";
            responseModel.Data = "Hello, World!";
            return Ok(responseModel);
        }
        /// <summary>
        /// Post method to send data to the Response Model
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>response model</returns>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            ResponseModel<string> responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Data recived successfully";
            responseModel.Data = $"Key: {requestModel.Key} , Value: {requestModel.Value}";
            return Ok(responseModel);
        }
        /// <summary>
        /// Put method to update the details of key value of entire
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel) 
        {
            
                if (requestModel == null)
                {
                    return BadRequest("Invalid request data.");
                }

            // Simulating an update
                requestModel.Key +="Age";
                requestModel.Value += 10;
                

                return Ok($"Updated Key: {requestModel.Key}, Updated Value: {requestModel.Value}");
            

        }
        /// <summary>
        /// patch method update a specfic task not the entire database
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            if (requestModel == null)
            {
                return BadRequest("Invalid request data.");
            }

            // Simulating a partial update
            requestModel.Value += 5; // Example update: Incrementing Value

            return Ok($"Patched Key: {requestModel.Key}, Updated Value: {requestModel.Value}");
        }

        /// <summary>
        /// Delete method to Delete a key value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        [HttpDelete("{key}")]
        public IActionResult Delete(string key)
        {
            var request = requests.FirstOrDefault(r => r.Key == key);
            if (request == null)
            {
                return NotFound($"Request with Key '{key}' not found.");
            }

            requests.Remove(request);
            return Ok($"Request with Key '{key}' deleted successfully.");
        }
        /// <summary>
        /// get method to greeting Hello Uc2
        /// </summary>
        /// <param name="_greetingBL"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("helloGreeeting")]
        public IActionResult PostGreeting()
        {
            return Ok(_greetingBL.GetGreeting());
        }
        /// <summary>
        /// GetGreeting method to print hello firstName lastName
        /// </summary>
        /// <param name="frontendRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getGreeting")]
        public IActionResult GetGreeting(FrontendRequest frontendRequest)
        {
            string message = _greetingBL.SendGreeting(frontendRequest.FristName, frontendRequest.LastName);
            return Ok(new { Success = true, Message = message });
        }

        /// <summary>
        /// GreetMessage method to  save greeting message to the database
        /// </summary>
        /// <param name="greetingModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route("GreetMessage")]

        public IActionResult GreetMessage(GreetingModel greetingModel)
        {
            var result = _greetingBL.GreetingMessage(greetingModel);

            responseModel = new ResponseModel<string>();
            responseModel.Success = true;
            responseModel.Message = "Message Added Successfully";
            responseModel.Data = "";
            return Created("Message Added: ", responseModel);
        }
        /// <summary>
        /// FindGreetingMessage method to print message by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("findMessage")]
        public IActionResult FindGreetingMessage(int id)
        {
            responseModel = new ResponseModel<string>();
            var greeting = _greetingBL.FindGreetingMessage(id);
            if (greeting == null)
            {
                responseModel.Success = false;
                responseModel.Message = "Id not found";
                responseModel.Data = "";
                return NotFound(responseModel);
            }
            responseModel.Success = true;
            responseModel.Message = greeting.GreetingMessage;
            responseModel.Data = "";
            return Ok(responseModel);
        }

        /// <summary>
        /// GetAllGreetings method to print all greeting message
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route("getAllGreetings")]
        public IActionResult GetAllGreetings()
        {
            ResponseModel<List<GreetingEntity>> responseModel = new ResponseModel<List<GreetingEntity>>();
            List<GreetingEntity> greetings = _greetingBL.GetAllGreetings();
            if (greetings == null)
            {
                responseModel.Success = false;
                responseModel.Message = "No Greeting Find";
                responseModel.Data = new List<GreetingEntity>(); ;
                return NotFound(responseModel);
            }
            else
            {
                responseModel.Success = true;
                responseModel.Message = "List of All Messages";
                responseModel.Data = greetings;
            }
            
            return Ok(responseModel);
        }

        /// <summary>
        /// EditMessage to edit the message by using Id
        /// </summary>
        /// <param name="greetingModel"></param>
        /// <returns></returns>
        [HttpPatch]
        [Route("editMessage")]

        public IActionResult EditMessage(GreetingModel greetingModel)
        {
            try
            {
                responseModel = new ResponseModel<string>();
                var result = _greetingBL.EditMessage(greetingModel);

                responseModel.Success = true;
                responseModel.Message = "Message Updated Successfully";
                responseModel.Data = "";



                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Key not found Error.");
                var errorResponse = ExceptionHandler.HandleException(ex,_logger);
                return NotFound(errorResponse);
            }

            
        }

        /// <summary>
        /// DeteleMessage method to delete a message
        /// </summary>
        /// <param name="greetIdModel"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteMessage")]
        public IActionResult DeleteMessage(GreetIdModel greetIdModel)
        {
            try
            {
                responseModel = new ResponseModel<string>();
                var result = _greetingBL.DeleteMessage(greetIdModel);

                responseModel.Success = true;
                responseModel.Message = "Message Deleted Successfully";
                responseModel.Data = "";
                return Ok(responseModel);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError("Key not found Error.");
                var errorResponse = ExceptionHandler.HandleException(ex, _logger);
                return NotFound(errorResponse);
            }
        }
    }

}

