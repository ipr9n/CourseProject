using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LogicLayer.Interfaces;
using LogicLayer.Services;
using LogicLayer.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Last.Hubs;
using Last.Filters;

namespace Last.Controllers
{
    [LocalizeLanguage]
    [UnblockedOnly]
    public class CollectionController : Controller
    {
        private ServiceCreator serviceCreator = new ServiceCreator();

        private ICollectionService CollectionService
        {
            get
            {
                return serviceCreator.CreateCollectionService();

            }
        }

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateCollection(CollectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                CollectionService.AddCollectionToUser(model);
                return RedirectToAction("Collections","Collection");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateField(CreateFieldViewModel model)
        {
            if (ModelState.IsValid)
            {
                CollectionService.AddField(model);
                return RedirectToAction($"Collection/{model.CollectionId}");
            }
            else
            {
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult DeleteCollection(int Id)
        {
            if (User.IsInRole("admin") || CollectionService.IsUserCollectionAdmin(User.Identity.GetUserId(), Id))
            {
                CollectionService.DeleteCollection(Id);
            }
            return RedirectToAction("Collections");
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult CreateCollection(string Id)
        {
            return View(new CollectionViewModel() { CollectionCreatorId = Id ?? User.Identity.GetUserId() });
        }

        [HttpPost]
        public ActionResult AddComment(string commentText, int itemId)
        {
            if (CollectionService.GetItemById(itemId) != null)
            {
                SendMessage(itemId, commentText, UserService.GetProfileInformation(User.Identity.GetUserId()).Name);
                CollectionService.AddComment(commentText, itemId, User.Identity.GetUserId());
                return RedirectToAction($"Item/{itemId}");
            }
            else
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult CreateItem(ItemViewModel model, string[] text, int[] fieldId)
        {
            if (ModelState.IsValid)
            {
                if (text != null)
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        model.CustomValues.Add(new CustomValueViewModel()
                        {
                            CustomFieldId = fieldId[i],
                            Value = text[i],
                            ItemId = model.Id
                        });
                    }
                }
                CollectionService.AddItem(model);
            }

            return RedirectToAction($"Collection/{model.GroupId}");
        }

        public ActionResult Main()
        {
            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "", null);

            var model = new MainPageViewModel();
            ViewBag.IsGroupAdmin = false;
            model.LastItems = CollectionService.GetLastItems();
            model.MaxItemCollections = CollectionService.GetMaxItemCollections(6);
            model.Tags = CollectionService.GetAllTags();
            return View(model);
        }


        public ActionResult EditFieldView(int fieldId)
          =>  PartialView(CollectionService.GetFieldById(fieldId));

        [HttpPost]
        public ActionResult UpdateField(CustomFieldViewModel model)
        {
            if (ModelState.IsValid)
            {
                CollectionService.UpdateField(model);
            }

            return RedirectToAction($"Collection/{model.CollectionId}");
        }

        public ActionResult GetItemsByTag(int id)
        {

            var items = CollectionService.GetItemsByTag(id);
            ViewBag.IsGroupAdmin = false;
            if (items.Count == 0) return null;

            return PartialView("SearchResultTable", items);
        }

        [HttpGet]
        public ActionResult Search(string search)
        {
            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "", null);

            ViewBag.IsGroupAdmin = false;
            var items = CollectionService.SearchItems(search);
            return View(items);
        }

        [HttpPost]
        public ActionResult DeleteField(int fieldId)
        {
            CollectionService.DeleteField(fieldId);
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            return Redirect(returnUrl);
        }

        public ActionResult ChangeCollection(int id)
        {
            var collection = CollectionService.GetCollectionById(id);
            return View(collection);
        }

        [HttpPost]
        public ActionResult ChangeCollectionSettings(CollectionViewModel model)
        {
            CollectionService.ChangeCollectionSettings(model);
            return RedirectToAction($"Collection/{model.Id}");
        }

        [HttpGet]
        public ActionResult Tags(string term)
        {
            return Json(CollectionService.GetTagStringList().Where(x => x.Contains(term.Split().Last())).ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChangeCollectionForm(List<CustomFieldViewModel> collectionViewModel)
        {
            return RedirectToAction($"Collection/");
        }

        public ActionResult Item(int id)
        {
            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "", null);

            var item = CollectionService.GetItemById(id);
            if (item != null)
                return View(CollectionService.GetItemById(id));
            else
                return View("Error");
        }

        public ActionResult Collection(int id)
        {
            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "", null);

            var collection = CollectionService.GetCollectionById(id);

            if (collection != null)
            {
                string myId = User.Identity.GetUserId();

                ViewBag.MyId = myId;
                ViewBag.GroupId = id;
                ViewBag.IsGroupAdmin = collection.CollectionCreatorId == myId ? true : false;

                return View(collection);
            }
            else
            {
                return View("CollectionError");
            }
        }

        public ActionResult Collections(string searchText)
        {
            if (UserService.IsBlocked(User.Identity.GetUserId()))
                return View("Error", "", null);

            var collections = CollectionService.GetAllCollections();

            if (!String.IsNullOrWhiteSpace(searchText))
                collections = collections.Where(x => x.CollectionName.ToUpper().Contains(searchText.ToUpper())).ToList();

            return View(collections);
        }

        public ActionResult LikeDislikeItem(int itemId)
        {
            var item = CollectionService.GetItemById(itemId);
            string returnUrl = Request.UrlReferrer.AbsolutePath;

            if (item != null)
            {
                if (item.ItemLikes.Count(x => x.ApplicationUserId == User.Identity.GetUserId()) > 0)
                    CollectionService.ItemDislike(User.Identity.GetUserId(), itemId);
                else
                    CollectionService.ItemLike(User.Identity.GetUserId(), itemId);
            }

            return Redirect(returnUrl);
        }

        public ActionResult MyCollections(string searchText)
        {
            var collections = CollectionService.GetUserCollections(User.Identity.GetUserId());

            if (!String.IsNullOrWhiteSpace(searchText))
                collections = collections.Where(x => x.CollectionName.ToUpper().Contains(searchText.ToUpper())).ToList();

            return View(collections);
        }


        public ActionResult UpdateItem(ItemViewModel model, string[] text, int[] fieldId)
        {
            if (ModelState.IsValid)
            {
                if (text != null)
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        model.CustomValues.Add(new CustomValueViewModel()
                        {
                            CustomFieldId = fieldId[i],
                            Value = text[i],
                            ItemId = model.Id
                        });
                    }
                }
                CollectionService.UpdateItem(model);
            }
            return RedirectToAction($"Collection/{model.GroupId}");
        }

        [HttpPost]
        public ActionResult EditItem(int itemId)
        {
            return View(CollectionService.GetItemById(itemId));
        }

        [HttpPost]
        public ActionResult DeleteItem(int itemId, int collectionId)
        {
            CollectionService.DeleteItem(itemId);
            return RedirectToAction($"Collection/{collectionId}", "Collection");
        }

        private void SendMessage(int message, string commentText, string authorName)
        {
            // Получаем контекст хаба
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<CommentHub>();
            // отправляем сообщение

            context.Clients.All.displayMessage(message, commentText, authorName);
        }
    }
}