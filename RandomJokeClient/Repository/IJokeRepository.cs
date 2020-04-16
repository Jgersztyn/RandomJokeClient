using RandomJokeClient.Models;
using System.Threading.Tasks;

namespace RandomJokeClient
{
    interface IJokeRepository
    {
        Task<JokeResponse> AddJokeAsync(JokeResponse item);
        
        // Additional API functions, such as GETs and DELETEs
        // can be defined in this interface file as needed
    }
}
