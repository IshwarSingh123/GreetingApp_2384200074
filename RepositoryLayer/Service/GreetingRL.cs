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

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        //ResponseModel<string> responseModel = new ResponseModel<string>();
        HelloGreetingAppContext _dbContext;
        public GreetingRL(HelloGreetingAppContext dbContext)
        {
            _dbContext = dbContext;
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
                GreetingMessage = greetingModel.GreetingMessage
            };
            _dbContext.Greeting.Add(newMessage); // Add entity (not just message)
            _dbContext.SaveChanges();
            return greetingModel;  // Return saved message
            }

       
    }
  
    }



