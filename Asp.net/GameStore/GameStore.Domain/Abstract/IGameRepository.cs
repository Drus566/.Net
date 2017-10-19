using GameStore.Domain.Entities;
using GameStore.Domain.Concrete;
using System.Collections.Generic;


namespace GameStore.Domain.Abstract
{
    public interface IGameRepository
    {
        //Последовательность объектов Game для получения её, зависимым от этого интерфейса классом
        IEnumerable<Game> GetUsers();
    }
}
