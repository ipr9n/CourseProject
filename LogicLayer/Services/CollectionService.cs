using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcess.Entities;
using LogicLayer.Interfaces;
using LogicLayer.ViewModels;
using DataAcess.Interfaces;
using Microsoft.AspNet.Identity;
using DataAccess.Entities;
using Markdig;
using LogicLayer.Configs;
using AutoMapper;

namespace LogicLayer.Services
{
    class CollectionService : ICollectionService
    {
        IUnitOfWork Database { get; set; }

        Mapper _mapper = new Mapper(MapperConfigs.MapperConfiguration);

        public CollectionService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public List<CollectionViewModel> GetAllCollections()
        {
            var collections = Database.CollectionRepository.GetAll();
            List<CollectionViewModel> outputList = new List<CollectionViewModel>();

            foreach (var collection in collections)
            {
                outputList.Add(new CollectionViewModel()
                {
                    CollectionCreatorId = collection.ApplicationUser.Id,
                    CollectionDescription = collection.CollectionDescription,
                    CollectionName = collection.CollectionName,
                    ImageUrl = collection.ImageUrl,
                    CollectionType = collection.CollectionType,
                    CreatorName = collection.ApplicationUser.ClientProfile.FirstName + collection.ApplicationUser.ClientProfile.SecondName,
                    Id = collection.Id
                });
            }

            return outputList;
        }

        public List<CollectionViewModel> GetUserCollections(string id)
            => GetAllCollections().Where(x => x.CollectionCreatorId == id).ToList();

        public void AddCollectionToUser(CollectionViewModel collectionViewModel)
        {
            if (String.IsNullOrWhiteSpace(collectionViewModel.ImageUrl))
            {
                collectionViewModel.ImageUrl = "https://res.cloudinary.com/ipr9n/image/upload/v1600416769/itpxkx8jqmagcvgdxfqg.png";
            }

            Database.UserManager.FindById(collectionViewModel.CollectionCreatorId).ItemCollections.Add(new ItemCollection()
            {
                CollectionDescription = collectionViewModel.CollectionDescription,
                CollectionName = collectionViewModel.CollectionName,
                ApplicationUserId = collectionViewModel.CollectionCreatorId,
                ImageUrl = collectionViewModel.ImageUrl,
                CollectionType = collectionViewModel.CollectionType.ToString(),
            });
            Database.CollectionRepository.SaveChanges();
        }
      

        public CollectionViewModel GetCollectionById(int id)
        {
            var collection = Database.CollectionRepository.GetAll().FirstOrDefault(x => x.Id == id);

            return (_mapper.Map<CollectionViewModel>(collection));
        }

        private bool IsTagExist(string tag)
            => Database.CollectionRepository.GetTags().Count(x => x.Name == tag) > 0
            ? true
            : false;

        private void AddTagsToDb(string[] tags)
        {
            foreach (var tag in tags)
            {
                if (!IsTagExist(tag))
                {
                    Database.CollectionRepository.AddTag(new Tag()
                    {
                        Name = tag
                    });
                }
            }
        }

        private List<Tag> GetTagsFromDb(string[] tags)
        {
            List<Tag> outputList = new List<Tag>();
            var allTags = Database.CollectionRepository.GetTags();

            foreach (var tag in tags)
                outputList.Add(allTags.FirstOrDefault(x => x.Name == tag));

            return outputList;
        }

        public void AddItem(ItemViewModel model)
        {
            AddTagsToDb(model.Tags.Split());

            Database.CollectionRepository.GetAll().FirstOrDefault(x => x.Id == model.GroupId).Items.Add(new Item()
            {
                CustomValue = _mapper.Map<List<CustomValue>>(model.CustomValues),
                Tags = GetTagsFromDb(model.Tags.Split()),
                Name = model.Name,
                Description = model.Description
            });
            Database.CollectionRepository.SaveChanges();
        }

        public ItemViewModel GetItemById(int id)
        {
            var item = Database.CollectionRepository.GetItems().FirstOrDefault(x => x.Id == id);

            return _mapper.Map<ItemViewModel>(item);
        }

        public List<string> GetTagStringList()
            => Database.CollectionRepository.GetTags().Select(x => x.Name).ToList();

        public void AddComment(string commentText, int itemId, string userId)
        {
            Database.CollectionRepository.AddComment(new ItemComment()
            {
                Text = commentText,
                ApplicationUserId = userId,
                ItemId = itemId
            });
        }

        public void ItemLike(string userId, int itemId)
        {
            Database.CollectionRepository.GetItems().First(x => x.Id == itemId).ItemLikes.Add(new ItemLike()
            {
                ItemId = itemId,
                ApplicationUserId = userId
            });
            Database.SaveAsync();

        }

        public void ItemDislike(string userId, int itemId)
           => Database.CollectionRepository.RemoveLike(userId, itemId);

        public void UpdateItem(ItemViewModel item)
        {
            AddTagsToDb(item.Tags.Split());
            Database.CollectionRepository.UpdateItemTags(item.Id, GetTagsFromDb(item.Tags.Split()));

            Database.CollectionRepository.UpdateItem(new Item()
            {
                Name = item.Name,
                Id = item.Id,
                CustomValue = _mapper.Map<List<CustomValue>>(item.CustomValues),
                ItemCollectionId = item.GroupId,
                Description = item.Description
            });
        }

        public void DeleteItem(int itemId)
           => Database.CollectionRepository.RemoveItem(itemId);

        public bool IsUserCollectionAdmin(string userId, int collectionId) =>
            Database.CollectionRepository
            .GetAll()
            .First(x => x.Id == collectionId).ApplicationUserId == userId;

        public void DeleteCollection(int collectionId)
            => Database.CollectionRepository.RemoveCollection(collectionId);

        public List<CollectionSearchViewModel> SearchItems(string text)
        {
            List<Item> outputList = new List<Item>();
            HashSet<int> uniqueCollectionId = new HashSet<int>();
            List<CollectionSearchViewModel> viewModelList = new List<CollectionSearchViewModel>();

            var result = Database.CollectionRepository.GetSearchResults(text);
            foreach (var id in result)
            {
                outputList.Add(Database.CollectionRepository.GetItemById(id));
            }

            outputList.ForEach(x => uniqueCollectionId.Add(x.ItemCollectionId));

            foreach (var collectionId in uniqueCollectionId)
            {
                var collection = Database.CollectionRepository.GetCollectionById(collectionId);
                viewModelList.Add(new CollectionSearchViewModel()
                {
                    CollectionItems = _mapper.Map<List<ItemViewModel>>(outputList.Where(x=>x.ItemCollectionId==collectionId).ToList()),
                    CollectionName = collection.CollectionName,
                 
                });
            }
            return viewModelList;
        }

        public List<ItemViewModel> GetLastItems()
        {
            var outputList = _mapper.Map<List<ItemViewModel>>(Database.CollectionRepository.GetItems());
            outputList.Reverse();

            return outputList;
        }

        public List<CollectionViewModel> GetMaxItemCollections(int count)
           => _mapper.Map<List<CollectionViewModel>>(Database.CollectionRepository.GetMaxItemCollections(count));

        public List<ItemViewModel> GetItemsByTag(int tagId)
           => _mapper.Map<List<ItemViewModel>>(Database.CollectionRepository.GetItemsByTag(tagId));
 

        public List<TagViewModel> GetAllTags()
         => _mapper.Map<List<TagViewModel>>(Database.CollectionRepository.GetTags());

        public void ChangeCollectionSettings(CollectionViewModel model)
        {
            var collection = Database.CollectionRepository.GetCollectionById(model.Id);
            collection.CollectionDescription = model.CollectionDescription;
            collection.CollectionName = model.CollectionName;
            collection.ImageUrl = model.ImageUrl;
            collection.CollectionType = model.CollectionType;
            Database.CollectionRepository.SaveChanges();
        }

        public void AddField(CreateFieldViewModel model)
        {
            Database.CollectionRepository.GetCollectionById(model.CollectionId).CustomFields.Add(_mapper.Map<CustomField>(model));
            Database.CollectionRepository.SaveChanges();
         
        }

        public void UpdateField(CustomFieldViewModel model)
        {
            var field = Database.CollectionRepository.GetFieldById(model.Id);
            field.Name = model.Name;
            Database.CollectionRepository.SaveChanges();
        }

        public CustomFieldViewModel GetFieldById(int fieldId)
            => _mapper.Map<CustomFieldViewModel>(Database.CollectionRepository.GetFieldById(fieldId));

        public void DeleteField(int fieldId)
        {
            Database.CollectionRepository.DeleteField(fieldId);
        }
    }
}
