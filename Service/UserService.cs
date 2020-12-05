using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FoodBlog.Service
{
    public class UserService : IUserService
    {
        public bool CheckUser(string userName, string password)
        {
            return userName.Equals("Arv", StringComparison.OrdinalIgnoreCase)
                && password.Equals("Arv123", StringComparison.OrdinalIgnoreCase);
        }
    }
}
