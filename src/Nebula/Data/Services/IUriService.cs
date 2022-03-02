using System;
using Nebula.Data.Helpers;

namespace Nebula.Data.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
