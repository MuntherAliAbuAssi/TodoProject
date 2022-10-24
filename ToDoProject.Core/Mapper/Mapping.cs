using AutoMapper;
using System;
using System.Collections.Generic;
using TODOProject.DbModel.Models;
using TODOProject.Extensions;
using TODOProject.ViewModel.Dto;
using TODOProject.ViewModel.ViewModel;
using TODOProject.ViewModel.ViewModels;

namespace TODOProject.Core.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<UserResult, User>().ReverseMap();
            CreateMap<ToDoViewModel, ToDo>().ReverseMap();
            CreateMap<ToDoViewModel,ToDoResponse>().ReverseMap();
            CreateMap<PagedResult<ToDoViewModel>, PagedResult<ToDo>>().ReverseMap();

        }
    }
}
