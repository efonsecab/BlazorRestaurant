using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRestaurant.Common.Interfaces
{
    public interface ICurrentUserProvider
    {
        string GetUsername();
    }
}
