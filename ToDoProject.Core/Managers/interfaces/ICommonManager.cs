using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using TODOProject.ViewModel.ViewModels;
 
namespace TODOProject.Core.Managers.interfaces
{
    public interface ICommonManager
    {
        UserModel GetUserRole(UserModel user);
         
    }
}
