using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.ViewModels;

namespace LogicLayer.Interfaces
{
    public interface ICollectionService
    {
        List<CollectionViewModel> GetAllCollections();
        List<CollectionViewModel> GetUserCollections(string id);
        void AddCollectionToUser(CollectionViewModel collectionViewModel);
        CollectionViewModel GetCollectionById(int id);
        void AddItem(ItemViewModel model);
        ItemViewModel GetItemById(int id);
        List<string> GetTagStringList();
        void AddComment(string commentText, int itemId, string userId);
        void ItemLike(string userId,int itemId);
        void ItemDislike(string userId,int itemId);
        void UpdateItem(ItemViewModel item);
        void ChangeCollectionSettings(CollectionViewModel model);
        void DeleteItem(int itemId);
        bool IsUserCollectionAdmin(string userId, int collectionId);
        void DeleteCollection(int collectionId);
        void AddField(CreateFieldViewModel model);
        void DeleteField(int fieldId);
        void UpdateField(CustomFieldViewModel model);
        CustomFieldViewModel GetFieldById(int fieldId);
        List<CollectionSearchViewModel> SearchItems(string text);
        List<ItemViewModel> GetLastItems();
        List<CollectionViewModel> GetMaxItemCollections(int count);
        List<TagViewModel> GetAllTags();
        List<ItemViewModel> GetItemsByTag(int tagId);
    }
}
