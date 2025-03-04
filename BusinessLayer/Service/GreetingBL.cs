using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;



namespace BusinessLayer.Service
    {
        public class GreetingBL : IGreetingBL
        {
            IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }
        public string GetGreeting()
            {
            return _greetingRL.GetGreeting();
            }

        public  GreetingModel GreetingMessage(GreetingModel greetingModel)
        {
           return  _greetingRL.AddGreetingMessage(greetingModel);
            
        }

        public string SendGreeting(string firstName, string lastName)
            {
                if (firstName!=null && lastName != null)
                {
                    return $"Hello, {firstName} {lastName}!";
                }

                if (firstName!=null)
                {
                    return $"Hello, {firstName}!";
                }

                if (lastName!=null)
                {
                    return $"Hello, Mr./Ms. {lastName}!";
                }

                return "Hello, World!";
            }
        }
    }
