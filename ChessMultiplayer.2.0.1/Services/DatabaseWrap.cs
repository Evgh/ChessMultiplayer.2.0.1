using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Xamarin.Forms;
using ChessMultiplayer.Models;

namespace ChessMultiplayer.Services
{
    public static class DatabaseWrap
    {
        public const string DBFILENAME = "chessMultiplayer.db";
        static string dbPath;
        static object locker;
        static ApplicationContext context;

        static DatabaseWrap()
        {
            dbPath = DependencyService.Get<IPath>().GetDataBasePath(DBFILENAME);

            context = new ApplicationContext(dbPath);

            context.Database.EnsureCreated();
            locker = new object();
        }
        
        public async static Task<bool> RegisterUserAsync(string login, string password)
        {
            User user = await context.Users.FindAsync(login);

            if (user == null)
            {
                context.Users.Add(new User() { Id = login, Password = password });

                await context.SaveChangesAsync();
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }


        public static User AutorizeUser(string login, string password)
        {
            var currentUser = (from user in context.Users
                               where login == user.Id && password == user.Password
                               select user).FirstOrDefault();

            if (currentUser == null)
                return currentUser;

            currentUser.Games = new ObservableCollection<Game>(context.Games.Where((game) => game.UserID == currentUser.Id).ToList<Game>());
            Debug.WriteLine($"Количество игр, загружаемых из бд: {currentUser.Games.Count}");


            if (currentUser.Games == null)
            {
                currentUser.Games = new ObservableCollection<Game>();
            }

            foreach (var game in currentUser.Games)
            {
                game.Moves = new ObservableCollection<MoveParameters>(context.Moves.Where((move) => move.GameID == game.Id).ToList<MoveParameters>());
            }

            return currentUser;
        }
        
        public async static Task UpdateGame(Game game)
        {
            lock (locker)
            {
                if (context.Users.Find(game.UserID) == null)
                    return;

                if (context.Games.Find(game.Id) == null)
                {
                    context.Games.Add(game);
                }

                context.SaveChanges();

                foreach (var move in game.Moves)
                {
                    var dbMove = context.Moves.Where(m => m.ActionNum == move.ActionNum && m.GameID == game.Id).FirstOrDefault();

                    if (dbMove == null && move.ActionNum != 0)
                    {
                        context.Moves.Add(move);
                    }
                }
            }
            await context.SaveChangesAsync();
        }

        public static ObservableCollection<UserStatistics> GetStatistics()
        {
            var collection = new ObservableCollection<UserStatistics>();

            foreach (var user in context.Users)
            {
                int amount = context.Games.Where(g => g.UserID == user.Id).Count();

                collection.Add(new UserStatistics() { Username = user.Id, GamesAmount = amount });
            }

            return collection;
        }

    }
}
