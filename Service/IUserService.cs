using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBlog.Service
{
    public interface IUserService
    {
        bool CheckUser(string userName, string password);
    }
}
