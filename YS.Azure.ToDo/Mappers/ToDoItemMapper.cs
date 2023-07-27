using AutoMapper;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Mappers
{
    public class ToDoItemMapper : Profile
    {
        public ToDoItemMapper()
        {
            CreateMap<ToDoItemModel, ToDoEntity>()
                .ReverseMap();
        }
    }
}