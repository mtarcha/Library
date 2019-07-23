using System.Threading.Tasks;

namespace Library.Data
{
    public interface ISeeder
    {
        Task Seed();
    }
}