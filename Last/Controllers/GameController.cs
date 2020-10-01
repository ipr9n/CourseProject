using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using LogicLayer.Interfaces;
using LogicLayer.Services;
using LogicLayer.ViewModels;

namespace Last.Controllers
{
    public class GameController : Controller
    {
        private ServiceCreator serviceCreator = new ServiceCreator();

        private IGameService GameService
        {
            get
            {
                return serviceCreator.CreateGameService();

            }
        }

        private GameIndexViewModel GetGameIndexModel()
        {
            return new GameIndexViewModel()
            {
                Games = GameService.GetAllGames(),
            Tags = GameService.GetAllTags()
            };
        }

        public ActionResult Index()
        {
            return View(GetGameIndexModel());
        }

        [HttpGet]
        public ActionResult Tags(string term)
        {
            return Json(GameService.GetTagStringList().Where(x => x.Contains(term.Split().Last())).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateGame()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGame(GameViewModel model)
        {
            if (ModelState.IsValid)
            {
                GameService.CreateGame(model);
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult EndGame(int id)
        {
            ViewBag.ErrorMessage = "Ваш соперник отключился от игры. Вы можете создать новую";
            return View("Index", GetGameIndexModel());
        }

        public ActionResult GetGamesByTag(int id)
        {

            var games = GameService.GetGamesByTag(id);
            if (games.Count == 0) return null;

            return PartialView("TagGameResult", games);
        }

        [HttpPost]
        public async Task<ActionResult> Game(int Id)
        {

            var game = GameService.GetGameById(Id);

            if (game != null)
            {
              
                if (game.PlayersCount < 2)
                {
                    GameService.ConnectPlayer(Id);
                    
                    return View(game);
                }
                else
                {
                    ViewBag.ErrorMessage = "В игре уже 2 игрока";
                    return View("Index", GetGameIndexModel());
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Игра удалена или не создана";
                return View("Index", GetGameIndexModel());
            }
        }

    }
}