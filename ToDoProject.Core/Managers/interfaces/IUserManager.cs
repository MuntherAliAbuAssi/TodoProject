using TODOProject.ViewModel.Dto;
using TODOProject.ViewModel.ViewModel;
using TODOProject.ViewModel.ViewModels;
 
namespace TODOProject.Core.Managers.interfaces
{
    public interface IUserManager : IManager
    {
        UserViewModel Login(LoginDto dto);

        UserViewModel SignUp(CreateUserDto dto);

        UserModel UpdateProfile(UserModel currentUser, UserModel request);
        void Delete(UserModel currentUser ,int id); 

    }
}
