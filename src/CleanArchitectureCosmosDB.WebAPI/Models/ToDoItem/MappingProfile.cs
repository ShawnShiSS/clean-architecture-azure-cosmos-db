using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.ToDoItem
{
    /// <summary>
    ///     Mapping Profile for AutoMapper
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        ///     ctor
        /// </summary>
        public MappingProfile()
        {
            // Get
            CreateMap<Core.Entities.ToDoItem, ToDoItemModel>().ReverseMap();

            // Create
            CreateMap<ToDoItem.Create.CreateCommand, Core.Entities.ToDoItem>();

            // Audit
            CreateMap<Core.Entities.Audit, ToDoItemAuditModel>()
                .ForMember(t => t.ToDoItemModel, s => s.MapFrom(audit => Newtonsoft.Json.JsonConvert.DeserializeObject<ToDoItemModel>(audit.Entity)));
        }
        
    }
}
