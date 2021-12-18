using System.Threading.Tasks;

namespace Nebula.Data.Services
{
    public interface ICpeService
    {
        public Task<bool> CreateBoletaJson(int invoice);
    }
}
