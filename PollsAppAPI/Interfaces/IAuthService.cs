using PollsApp.DTOs;

namespace PollsApp.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserDTO userDto);
        Task<string> LoginAsync(UserDTO userDto);
    }
}
