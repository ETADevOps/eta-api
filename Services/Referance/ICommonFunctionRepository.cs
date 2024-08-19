using System.Text;

namespace ETA.API.Services.Referance
{
    public interface ICommonFunctionRepository
    {
        public StringBuilder GetDynamicQuery(string[] searchField, string searchQuery, Type p);
    }
}
