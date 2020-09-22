using System;
using System.Collections.Generic;
//using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAcess.Entities;

namespace DataAcess.Interfaces
{
    public interface ICollectionRepository 
    {
        List<ItemCollection> GetAll();
        List<Item> GetItems();
        List<Tag> GetTags();
        void AddTag(Tag tag);
        void AddComment(ItemComment comment);
        void SaveChanges();
        void RemoveLike(string userId, int itemId);
        void UpdateItem(Item item);
        void UpdateItemTags(int itemId, List<Tag> tags);
        void RemoveItem(int itemId);
        void RemoveCollection(int collectionId);
        void UpdateCollection(ItemCollection collection);
        void RemoveNullCollections();
        void DeleteField(int fieldId);
        CustomField GetFieldById(int fieldId);
        HashSet<int> GetSearchResults(string text);
        Item GetItemById(int id);
        List<Item> GetItemsByCollectionId(int collectionId);
        ItemCollection GetCollectionById(int collectionId);
        List<ItemCollection> GetMaxItemCollections(int count);
        List<Item> GetItemsByTag(int tagId);
    }
}
