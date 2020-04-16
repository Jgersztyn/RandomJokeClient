using System;
using System.Collections.Generic;

namespace RandomJokeClient.Models
{
    public class RandomJoke
    {
        public List<string> Categories { get; set; }
        public int Id { get; set; }
        public string Joke { get; set; }
    }
}
