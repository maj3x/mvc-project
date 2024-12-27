using AutoMapper;
using TaskManagementSystem.Models;
using TaskManagementSystem.ViewModels;


namespace TaskManagementSystem.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Category, CategoryModel>().ReverseMap();
            

            CreateMap<Assignment, AssignmentModel>()
                .ForMember(dest => dest.CategoryName, 
                    opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.AssignedByName, 
                    opt => opt.MapFrom(src => src.AssignedBy.UserName))
                .ForMember(dest => dest.AssignedToName, 
                    opt => opt.MapFrom(src => src.AssignedTo.UserName));
            CreateMap<AssignmentModel, Assignment>();            
            
            CreateMap<Todo, TodoModel>()
                .ForMember(dest => dest.UserName, 
                    opt => opt.MapFrom(src => src.User.UserName));
            CreateMap<TodoModel, Todo>();
        }
    }
}
