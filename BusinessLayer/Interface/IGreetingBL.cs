using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        string GetGreeting();
        string SendGreeting(string firstName, string lastName);

       GreetingModel GreetingMessage(GreetingModel greetingModel);

        GreetingEntity FindGreetingMessage(int id);
        List<GreetingEntity> GetAllGreetings();

        GreetingEntity EditMessage(int id);
    }
}
