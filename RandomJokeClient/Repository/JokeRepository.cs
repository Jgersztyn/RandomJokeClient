using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RandomJokeClient.Models;

// it is possible to remove this warning for the async method below
// if we were doing some stuff with an actual repository here, the following function would be async
// we would want to await calls to the db in order to actually save this joke
#pragma warning disable 1998

namespace RandomJokeClient
{
    public class JokeRepository : IJokeRepository
    {
        private List<JokeResponse> jokes = new List<JokeResponse>();

        public async Task<JokeResponse> AddJokeAsync(JokeResponse newJoke)
        {
            if (newJoke == null)
            {
                throw new ArgumentNullException("There is nothing funny about an empty joke.");
            }

            jokes.Add(newJoke);
            return newJoke;
        }
    }
}
