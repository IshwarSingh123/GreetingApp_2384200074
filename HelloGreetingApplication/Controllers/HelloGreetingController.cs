using BusinessLayer.Service;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using NLog.Web;
using RepositoryLayer.Service;
using BusinessLayer.Interface;


namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private readonly IGreetingBL _greetingBL;
        private static List<RequestModel> requests = new List<RequestModel>
        {
        new RequestModel { Key = "User001", Value = 100 },
        new RequestModel { Key = "User002", Value = 200 }
        };
        private readonly ILogger<HelloGreetingController> _logger;

        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger)
        {
            _logger = logger;//Dependency Inject
            _greetingBL = greetingBL;
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
        /// Put method to update the details of key value
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

        [HttpPost]
        [Route("helloGreeeting")]
        public IActionResult PostGreeting(GreetingBL _greetingBL)
        {
            return Ok(_greetingBL.GetGreeting());
        }

    }
}
