using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using Microsoft.Extensions.Logging;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        //ResponseModel<string> responseModel = new ResponseModel<string>();
        HelloGreetingAppContext _dbContext;
        private readonly ILogger<GreetingRL> _logger;
        public GreetingRL(HelloGreetingAppContext dbContext, ILogger<GreetingRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public string GetGreeting()
        {
            return "Hello, World";
        }

        public GreetingModel AddGreetingMessage(GreetingModel greetingModel)
        {
            if (greetingModel == null || string.IsNullOrWhiteSpace(greetingModel.GreetingMessage))
            {
                throw new ArgumentException("Greeting message cannot be empty.");
            }
            var newMessage = new GreetingEntity
            {
                GreetingMessage = greetingModel.GreetingMessage,
                UserId   = greetingModel.UserId
                
            };
            _dbContext.Greeting.Add(newMessage); // Add entity (not just message)
            _dbContext.SaveChanges();
            return greetingModel;  // Return saved message
        }
        

        public GreetingEntity FindGreetingMessage(int id)
        {
            return _dbContext.Greeting.FirstOrDefault(g => g.GreetingId==id);
           
            
        }

        public List<GreetingEntity> GetAllGreetings()
        {
            return _dbContext.Greeting.ToList();
            
        }

        public GreetingEntity EditMessage(GreetingModel greetingModel)
        {
            try
            {
                var result = _dbContext.Greeting.FirstOrDefault(e => e.GreetingId == greetingModel.Id);
                if (result == null)
                {
                    throw new KeyNotFoundException("Id Not found");
                }
                result.GreetingMessage = greetingModel.GreetingMessage;
                _dbContext.SaveChanges();
                return result;
            }
            
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
           
        }

        public string DeleteMessage(GreetIdModel greetIdModel)
        {
            try
            {
                var result = _dbContext.Greeting.FirstOrDefault(e => e.GreetingId == greetIdModel.Id);
                if (result == null)
                {
                    throw new KeyNotFoundException("Id Not Found");
                }
                _dbContext.Remove(result);
                _dbContext.SaveChanges();
                return "Message Delete Successfully!";
                
            }
            catch(KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
  
}



