using System.Threading.Tasks;

// Asynchronous Operation Management System
namespace SKC.AOMS
{
    public interface IOperation
    {
        Task Perform();
    }
}
