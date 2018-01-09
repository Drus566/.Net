using System.Collections.Generic;

namespace PartyInvites.Models
{
    public interface IRepository
    {
         IEnumerable<GuestReponse> Reponses {get;}
        
        void AddResponse(GuestReponse reponse);
    }
}