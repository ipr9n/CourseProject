using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataAccess.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class GameRepository: IGameRepository
    {
        public DataAcess.EF.ApplicationContext Db { get; set; }

        public GameRepository(DataAcess.EF.ApplicationContext db)
        {
            Db = db;
        }

        public void ConnectPlayer(int gameId)
        {
            var game = Db.Games.First(x => x.Id == gameId);
            game.PlayersCount++;
            Db.SaveChanges();
        }

        public void DisconnectPlayer(int gameId)
        {
            var game = Db.Games.First(x => x.Id == gameId);
            game.PlayersCount--;
            Db.SaveChanges();
        }

        public bool TryGetGame(int gameId, out Game game)
        {
            if (Db.Games.Find(gameId) == null)
            {
                game=new Game();
                return false;
            }
            else
            {
                game = Db.Games.First(x => x.Id == gameId);
                return true;
            }
        }

        public Game GetGameById(int id)
        {
            return Db.Games.First(x => x.Id == id);
        }

        public List<Game> GetGamesByTag(int tagId)
        {
            return Db.GameTags.First(x => x.Id == tagId).Games;
        }

        public List<Game> GetAllGames()
        {
            return Db.Games.ToList();
        }

        public void CreateGame(Game game)
        {
            Db.Games.Add(game);
            Db.SaveChanges();
        }

        public void AddTag(GameTag tag)
        {
            Db.GameTags.Add(tag);
            Db.SaveChanges();
        }

        public List<GameTag> GetTags()
        {
            return Db.GameTags.ToList();
        }

        public void DeleteGame(int id)
        {
            Db.Games.Remove(Db.Games.First(x => x.Id == id));
            Db.SaveChanges();
        }

        
    }
}
