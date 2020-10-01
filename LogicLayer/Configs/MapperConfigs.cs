using AutoMapper;
using DataAccess.Entities;
using DataAcess.Entities;
using LogicLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Configs
{
    public static class MapperConfigs
    {
        public static MapperConfiguration MapperConfiguration
        {
            get
            {
                return new MapperConfiguration(cfg =>
                 {

                     cfg.CreateMap<Tag, TagViewModel>()
                     .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                     .ForMember(x => x.Name, x => x.MapFrom(m => m.Name));

                     cfg.CreateMap<GameTag, TagViewModel>()
                         .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                         .ForMember(x => x.Name, x => x.MapFrom(m => m.Name));

        
                     cfg.CreateMap<Game, GameViewModel>()
                         .ForMember(x => x.PlayersCount, x => x.MapFrom(m => m.PlayersCount))
                         .ForMember(x => x.Title, x => x.MapFrom(m => m.Title))
                         .ForMember(x => x.Tags,
                             x => x.MapFrom(m => m.GameTags.Aggregate(new StringBuilder(),
                                 (acc, item) => acc.Append($"{item.Name.ToString()} "))))
                         .ForMember(x=>x.Id,x=>x.MapFrom(m=>m.Id));

                     cfg.CreateMap<ItemComment, CommentViewModel>()
                     .ForMember(x => x.AuthorName, x => x.MapFrom(m => $"{m.ApplicationUser.ClientProfile.FirstName} {m.ApplicationUser.ClientProfile.SecondName}"))
                     .ForMember(x=>x.ImageUrl, x=> x.MapFrom(m=>m.ApplicationUser.ClientProfile.AvatarUrl))
                     .ForMember(x => x.CommentText, x => x.MapFrom(m => m.Text))
                     .ForMember(x => x.Id, x => x.MapFrom(m => m.Id));

                     cfg.CreateMap<ItemLike, ItemLikeViewModel>()
                     .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                     .ForMember(x => x.ItemId, x => x.MapFrom(m => m.ItemId))
                     .ForMember(x => x.ApplicationUserId, x => x.MapFrom(m => m.ApplicationUserId));

                     cfg.CreateMap<CustomValueViewModel, CustomValue>()
                     .ForMember(x => x.CustomFieldId, x => x.MapFrom(m => m.CustomFieldId))
                     .ForMember(x => x.Value, x => x.MapFrom(m => m.Value))
                     .ForMember(x => x.ItemId, x => x.MapFrom(m => m.ItemId))
                     .ReverseMap()
                     .ForPath(x=>x.CustomFieldId, x=>x.MapFrom(m=>m.CustomFieldId))
                     .ForPath(x=>x.ItemId, x=>x.MapFrom(m=>m.ItemId))
                     .ForPath(x=>x.Value, x=>x.MapFrom(m=>m.Value));

                     cfg.CreateMap<CreateFieldViewModel, CustomField>()
                     .ForMember(x => x.ItemCollectionId, x => x.MapFrom(m => m.CollectionId))
                     .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                     .ForMember(x => x.Type, x => x.MapFrom(m => m.Type));

                     cfg.CreateMap<Item, ItemViewModel>()
                     .ForMember(x => x.GroupId, x => x.MapFrom(m => m.ItemCollectionId))
                     .ForMember(x => x.Tags, x=>x.MapFrom(m=> m.Tags.Aggregate(new StringBuilder(), (acc, item) => acc.Append($"{item.Name.ToString()} "))))
                     .ForMember(x => x.ItemLikes, x=>x.MapFrom(m=>m.ItemLikes))
                     .ForMember(x=>x.CustomValues, x=>x.MapFrom(m=>m.CustomValue))
                     .ForMember(x=>x.CustomFieldViewModels, x=>x.MapFrom(m=>m.ItemCollection.CustomFields))
                     .ForMember(x => x.Comments, x=>x.MapFrom(m=>m.ItemComments))
                     .ReverseMap()
                     .ForPath(x=>x.Tags, options=>options.Ignore())
                     .ForPath(x=>x.ItemCollection,options=> options.Ignore())
                     .ForPath(x=>x.ItemComments, options => options.Ignore())
                     .ForPath(x=>x.ItemLikes, options => options.Ignore())
                     .ForPath(x=>x.ItemCollectionId, options=>options.Ignore());

               

                     cfg.CreateMap<CustomField, CustomFieldViewModel>()
                    .ForMember(x => x.Name, x => x.MapFrom(m => m.Name))
                    .ForMember(x => x.Id, x => x.MapFrom(m => m.Id))
                    .ForMember(x => x.CollectionId, x => x.MapFrom(m => m.ItemCollectionId))
                    .ForMember(x => x.Type, x=> x.MapFrom(m=>m.Type));

                     cfg.CreateMap<ItemCollection, CollectionViewModel>()
                .ForMember(x => x.CollectionItems, x => x.MapFrom(m => m.Items))
                .ForMember(x => x.CollectionCreatorId, x => x.MapFrom(m => m.ApplicationUserId))
                .ForMember(x => x.ImageUrl, x => x.MapFrom(m => m.ImageUrl))
                .ForMember(x => x.CreatorName, x => x.MapFrom(m => $"{m.ApplicationUser.ClientProfile.FirstName} {m.ApplicationUser.ClientProfile.SecondName}"))
                .ForMember(x => x.CustomFieldViewModels, x => x.MapFrom(m => m.CustomFields))
                .ForMember(x => x.CollectionDescription, x => x.MapFrom(m => m.CollectionDescription))
                .ForMember(x => x.CollectionName, x => x.MapFrom(m => m.CollectionName))
                .ForMember(x => x.CollectionType, x => x.MapFrom(m => m.CollectionType));
                 });
            }
        }
    }
}


