using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IGameRepository
    {
        Game GetGameById(int id);
        void CreateGame(Game game);
        void DeleteGame(int gameId);
        bool TryGetGame(int gameId, out Game game);
        List<GameTag> GetTags();
        void AddTag(GameTag tag);
        List<Game> GetAllGames();
        List<Game> GetGamesByTag(int tagId);
        void ConnectPlayer(int gameId);
        void DisconnectPlayer(int gameId);
    }
}
