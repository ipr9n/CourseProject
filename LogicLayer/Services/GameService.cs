using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataAccess.Entities;
using DataAcess.Interfaces;
using LogicLayer.Configs;
using LogicLayer.Interfaces;
using LogicLayer.ViewModels;

namespace LogicLayer.Services
{
    class GameService : IGameService
    {
        IUnitOfWork Database { get; set; }

        readonly Mapper _mapper = new Mapper(MapperConfigs.MapperConfiguration);

        public GameService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void DeleteGame(int id)
        {
           Database.GameRepository.DeleteGame(id);
        }

        public GameViewModel GetGameById(int id)
        {
            if (Database.GameRepository.TryGetGame(id, out Game game))
                return _mapper.Map<GameViewModel>(game);
            else return null;
        }

        public List<TagViewModel> GetAllTags()
        {
           return _mapper.Map<List<TagViewModel>>(Database.GameRepository.GetTags());
        }

        public List<GameViewModel> GetGamesByTag(int tagId)
        {
            return _mapper.Map<List<GameViewModel>>(Database.GameRepository.GetGamesByTag(tagId));
        }

        public List<GameViewModel> GetAllGames()
        {
            return _mapper.Map<List<GameViewModel>>(Database.GameRepository.GetAllGames());
        }

        private bool IsTagExist(string tag)
            => Database.GameRepository.GetTags().Count(x => x.Name == tag) > 0
                ? true
                : false;

        private void AddTagsToDb(string[] tags)
        {
            foreach (var tag in tags)
            {
                if (!IsTagExist(tag))
                {
                    Database.GameRepository.AddTag(new GameTag()
                    {
                        Name = tag
                    });
                }
            }
        }

        private List<GameTag> GetTagsFromDb(string[] tags)
        {
            List<GameTag> outputList = new List<GameTag>();
            var allTags = Database.GameRepository.GetTags();

            foreach (var tag in tags)
                outputList.Add(allTags.FirstOrDefault(x => x.Name == tag));

            return outputList;
        }

        public List<string> GetTagStringList()
            => Database.GameRepository.GetTags().Select(x => x.Name).ToList();

        public void CreateGame(GameViewModel model)
        {
            AddTagsToDb(model.Tags.Split());

            Game newGame = new Game()
            {
                PlayersCount = 0, 
                Title = model.Title,
                GameTags = GetTagsFromDb(model.Tags.Split())
            };
            Database.GameRepository.CreateGame(newGame);
        }

        public void ConnectPlayer(int gameId)
        {
            Database.GameRepository.ConnectPlayer(gameId);
        }

        public void DisconnectPlayer(int gameId)
        {
            Database.GameRepository.DisconnectPlayer(gameId);
        }
    }
}
