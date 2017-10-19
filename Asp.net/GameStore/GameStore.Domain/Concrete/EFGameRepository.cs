using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Collections.Generic;

namespace GameStore.Domain.Concrete
{
    //хранилище
    public class EFGameRepository : IGameRepository
    {
        public IEnumerable<Game> GetUsers()
        {
            return StorageProcedures.GetUsers();
        }
    }
}
