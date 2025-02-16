using Authentication.Application.DTOs;
using SharedLibrary.Responses;

namespace Authentication.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> RegisterUser(UserDTO userDTO);
        Task<Response> LoginUser(LoginDTO loginDTO);
        Task<GetUserDTO> GetUserAsync(Guid userId);
    }
}
