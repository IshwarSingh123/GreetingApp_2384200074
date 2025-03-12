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
        string SendGetGreeting(GreetIdModel greetIdModel);

       GreetingModel GreetingMessage(GreetingModel greetingModel);

        GreetingEntity FindGreetingMessage(int id);
        List<GreetingEntity> GetAllGreetings();

        GreetingEntity EditMessage(GreetingModel greetingModel);
        string DeleteMessage(GreetIdModel greetIdModel);
    }
}
