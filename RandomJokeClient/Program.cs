using Microsoft.Owin.Hosting;
using RandomJokeClient.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace RandomJokeClient
{
    class Program
    {
        private static string baseAddress = "http://localhost:9000/";
        private static HttpClient client = new HttpClient();
        private static HttpResponseMessage response = null;

        public static void Main(string[] args)
        {
            // The following construct allows the Main method in C# to run asynchronously
            Task.Run(async () =>
            {
                // Start OWIN host for a local web server
                using (WebApp.Start<Startup>(url: baseAddress))
                {
                    // Set the default address of the API
                    client.BaseAddress = new Uri(baseAddress);

                    string randomName = "";
                    RandomJokeBase randomJoke = null;

                    // Get a random name
                    try
                    {
                        randomName = FetchRandomNameAsync().Result;
                    }
                    catch (HttpResponseException e)
                    {
                        Console.WriteLine("Error retrieving name from external server.");
                    }

                    // default value added since the server is currently generating a 503, unavailable error
                    // (this was only added now, as this endpoint stopped working without any code changes)
                    if (randomName == null || randomName == "")
                    {
                        randomName = "Chuck Norris";
                    }

                    Console.WriteLine("Random name retrieved:");
                    Console.WriteLine(randomName + "\n");

                    // Get a random joke
                    try
                    {
                        randomJoke = FetchRandomJokeAsync("Chuck", "Norris").Result;
                    }
                    catch (HttpResponseException e)
                    {
                        Console.WriteLine("Error retrieving joke from external server. Please try again later");

                        // Close the app immediately, server is unavailable or has unresolved errors
                        Environment.Exit(0);
                    }

                    Console.WriteLine("Random joke retrieved:");
                    Console.WriteLine(randomJoke + "\n");

                    // Retrieve response from the self-hosted API endpoint
                    JokeResponse joke = new JokeResponse { Type = randomJoke.Type, Joke = randomJoke.Value.Joke };
                    string returnedJoke = await SaveJokeAsync(joke);

                    Console.WriteLine(returnedJoke);

                    Console.WriteLine("\nPress any key to exit.");
                    Console.ReadKey();
                }
            }).GetAwaiter().GetResult();
        }

        static async Task<string> FetchRandomNameAsync()
        {
            PersonName name = null;
            string fullName = "";

            response = await client.GetAsync("http://uinames.com/api/");

            if (response.IsSuccessStatusCode)
            {
                name = await response.Content.ReadAsAsync<PersonName>();

                if (name != null)
                {
                    fullName = name.Name + " " + name.Surname;
                }
            }

            return fullName;
        }

        static async Task<RandomJokeBase> FetchRandomJokeAsync(string firstName, string lastName)
        {
            RandomJokeBase joke = null;

            string requestUri = string.Format("http://api.icndb.com/jokes/random?firstName={0}&lastName={1}&limitTo=[nerdy]",
                         firstName, lastName);

            response = await client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                joke = await response.Content.ReadAsAsync<RandomJokeBase>();
            }

            return joke;
        }

        static async Task<string> SaveJokeAsync(JokeResponse myJokeResponse)
        {
            // Clear header data, in case any of that happens to exist
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            response = await client.PostAsJsonAsync(
                "api/joke", myJokeResponse);
            response.EnsureSuccessStatusCode();

            string jokeToReturn = await response.Content.ReadAsStringAsync();

            return jokeToReturn;
        }
    }
}
