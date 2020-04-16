using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using RandomJokeClient.Models;

namespace RandomJokeClient
{
    public class JokeController : ApiController
    {
        //JokeRepository repository = new JokeRepository();
        static IJokeRepository repository = new JokeRepository();

        [Route("api/joke")]
        [HttpPost]
        public async Task<IHttpActionResult> PostJokeAsync([FromBody]JokeResponse randomJoke)
        {
            if (randomJoke == null)
            {
                return BadRequest("No joke to be found.");
            }

            JokeResponse newJoke = await repository.AddJokeAsync(randomJoke);

            return Ok(newJoke);
        }
    }
}
