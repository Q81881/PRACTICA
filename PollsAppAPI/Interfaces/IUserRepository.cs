using PollsApp.Models;

namespace PollsApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task<User> AddUserAsync(User user);
    }
}
