using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;



namespace BusinessLayer.Service
    {
        public class GreetingBL : IGreetingBL
        {
            IGreetingRL _greetingRL;
        ILogger<GreetingRL> _logger;

        public GreetingBL(IGreetingRL greetingRL, ILogger<GreetingRL> logger)
        {
            _greetingRL = greetingRL;
            _logger = logger;
        }

        public GreetingEntity FindGreetingMessage(int id)
        {
            return _greetingRL.FindGreetingMessage(id);
        }
        

        public string GetGreeting()
            {
            return _greetingRL.GetGreeting();
            }

        public  GreetingModel GreetingMessage(GreetingModel greetingModel)
        {
           return  _greetingRL.AddGreetingMessage(greetingModel);
            
        }

        public List<GreetingEntity> GetAllGreetings()
        {
            return _greetingRL.GetAllGreetings();
        }


        public string SendGetGreeting(GreetIdModel greetIdModel)
        {
            var data = _greetingRL.SendGetGreeting(greetIdModel);
                if (data.FirstName!=null && data.LastName != null)
                {
                    return $"Hello, {data.FirstName} {data.LastName}!";
                }

                if (data.FirstName != null)
                {
                    return $"Hello, {data.FirstName}!";
                }

                if (data.LastName != null)
                {
                    return $"Hello, Mr./Ms. {data.LastName}!";
                }

                return "Hello, World!";
        }

        public GreetingEntity EditMessage(GreetingModel greetingModel)
        {
            try
            {
                var result = _greetingRL.EditMessage(greetingModel);
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
                var result = _greetingRL.DeleteMessage(greetIdModel);
                return "Message Delete Successfully";
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


    }
    }
