using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAcess.Entities;
using DataAcess.Interfaces;

namespace DataAcess.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        public EF.ApplicationContext Db { get; set; }

        public CollectionRepository(EF.ApplicationContext db)
        {
            Db = db;
        }

        public List<ItemCollection> GetAll() => Db.ItemCollections.ToList();

        public void SaveChanges()
        {
            Db.SaveChanges();
        }

        public HashSet<int> GetSearchResults(string text)
        {
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter("@text", text);
            var phones = Db.Database.SqlQuery<int>("FullTextSearch @text", param);
            return phones.ToHashSet();
        }

        public List<Item> GetItems()
        {
            return Db.Items.ToList();
        }

        public List<Tag> GetTags()
        {
            return Db.Tags.ToList();
        }

        public void AddTag(Tag tag)
        {
            Db.Tags.Add(tag);
            Db.SaveChanges();
        }

        public Item GetItemById(int id) => Db.Items.FirstOrDefault(x => x.Id == id);

        public void AddComment(ItemComment comment)
        {
            Db.itemComments.Add(comment);
            Db.SaveChanges();
        }

        public void RemoveLike(string userId, int itemId)
        {
            Db.itemLikes.Remove(Db.itemLikes.First(x => x.ItemId == itemId && x.ApplicationUserId == userId));
            Db.SaveChanges();
        }

        public List<Item> GetItemsByCollectionId(int collectionId)
        {
            return Db.Items.Where(x => x.ItemCollectionId == collectionId).ToList();
        }

        public ItemCollection GetCollectionById(int collectionId)
        {
            return Db.ItemCollections.Find(collectionId);
        }

        public void UpdateItemTags(int itemId, List<Tag> tags)
        {
            var dbItem = Db.Items.First(x => x.Id == itemId);
            dbItem.Tags.RemoveAll(x => x.Id != -1);
            dbItem.Tags = tags;
            Db.SaveChanges();
        }

        public CustomField GetFieldById(int fieldId)
            => Db.CustomFields.Find(fieldId);

        public void RemoveItem(int itemId)
        {
            if (Db.Items.First(x => x.Id == itemId) != null)
            {
                Db.Items.Remove(Db.Items.First(x => x.Id == itemId));
                Db.SaveChanges();
            }

        }

        public void UpdateItem(Item item)
        {
            var oldItem = Db.Items.Find(item.Id);
            oldItem.CustomValue = item.CustomValue;
            oldItem.Description = item.Description;
            oldItem.Name = item.Name;

            Db.SaveChanges();
        }

        public void RemoveCollection(int collectionId)
        {
            //  var collection = Db.ItemCollections.Find(collectionId);

            Db.ItemCollections.Remove(Db.ItemCollections.Find(collectionId));
            Db.SaveChanges();
        }

        public void RemoveNullCollections()
        {
            Db.ItemCollections.RemoveRange(Db.ItemCollections.Where(x => x.ApplicationUserId == null));
            Db.SaveChanges();

        }

        public List<Item> GetItemsByTag(int tagId)
        {
            return Db.Items.Where(x => x.Tags.Count(y => y.Id == tagId) > 0).ToList();
        }

        public List<ItemCollection> GetMaxItemCollections(int count)
        {
            return Db.ItemCollections.OrderByDescending(x => x.Items.Count).Take(count).ToList();
        }

        public void UpdateCollection(ItemCollection collection)
        {

        }

        public void DeleteField(int fieldId)
        {
            Db.CustomFields.Remove(Db.CustomFields.First(x => x.Id == fieldId));
            Db.SaveChanges();
        }
    }
}
