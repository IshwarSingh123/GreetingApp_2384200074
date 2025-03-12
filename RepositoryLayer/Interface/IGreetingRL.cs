using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        string GetGreeting();

        GreetingModel AddGreetingMessage(GreetingModel greetingModel);
        GreetingEntity FindGreetingMessage(int id);
        List<GreetingEntity> GetAllGreetings();

        GreetingEntity EditMessage(GreetingModel greetingModel);
        UserEntity SendGetGreeting(GreetIdModel greetIdModel);

        string DeleteMessage(GreetIdModel greetIdModel);
    }
}
