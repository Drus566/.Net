using System.Collections.Generic;

namespace PartyInvites.Models
{
    public class EFRepository : IRepository
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public IEnumerable<GuestReponse> Reponses => context.Invites;

        public void AddResponse(GuestReponse response){
            context.Invites.Add(response);
            context.SaveChanges();
        }
    }
}