using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.ViewModels;

namespace LogicLayer.Interfaces
{
    public interface IGameService
    {
        GameViewModel GetGameById(int id);
        void DeleteGame(int id);
        List<GameViewModel> GetAllGames();
        List<GameViewModel> GetGamesByTag(int tagId);
        void CreateGame(GameViewModel model);
        void ConnectPlayer(int gameId);
        List<string> GetTagStringList();
        List<TagViewModel> GetAllTags();
        void DisconnectPlayer(int gameId);
    }
}
