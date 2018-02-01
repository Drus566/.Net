using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MvcModels.Models
{
    public class AddressSummary
    {
        //[BindNever]
        public string City { get; set; }

        public string Country { get; set; }
    }
}
