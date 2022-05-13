using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Bitly.Core.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public IList<Url> Urls { get; set; }
    }
}
