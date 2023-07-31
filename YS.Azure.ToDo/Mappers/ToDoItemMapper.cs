using AutoMapper;
using YS.Azure.ToDo.Models;

namespace YS.Azure.ToDo.Mappers
{
    public class ToDoItemMapper : Profile
    {
        public ToDoItemMapper()
        {
            CreateMap<ToDoItemModel, ToDoEntity>()
                .ForMember(
                    dest => dest.Files,
                    opt => opt.MapFrom(
                        src => src.FileNames != null
                            ? src.FileNames.Select(f => new TaskFilesEntity
                            {
                                Id = Guid.NewGuid().ToString(),
                                FileName = f,
                                TaskId = src.Id
                            })
                            : null));

            CreateMap<ToDoEntity, ToDoItemModel>()
                .ForMember(
                    dest => dest.FileNames,
                    opt => opt.MapFrom(
                        src => src.Files != null ? src.Files.Select(f => f.FileName) : null));
        }
    }
}