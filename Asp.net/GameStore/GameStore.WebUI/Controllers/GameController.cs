using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Entities;
using GameStore.Domain.Abstract;
using GameStore.WebUI.Models;

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        private IGameRepository repository;
        public int pageSize = 4;
        
        //Объявление зависимости от интерфейса IGameRepository для Ninject
        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        //Визуализация представления
        public ViewResult List(string category, int page = 1)
        {
            GamesListViewModel model = new GamesListViewModel
            {
                Games = repository.GetUsers()
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(game => game.GameId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                repository.GetUsers().Count() :
                repository.GetUsers().Where(game => game.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}