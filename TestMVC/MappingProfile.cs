using AutoMapper;
using TestMVC.DTOs;
using TestMVC.Models;

    namespace TestMVC
    {
        public class MappingProfile:Profile
        {
           public  MappingProfile() {
                CreateMap<AuthorModel, AuthorGetDTO>().ReverseMap();
                CreateMap<AuthorModel, AuthorCreateDTO>().ReverseMap();
            }
        }
    }
