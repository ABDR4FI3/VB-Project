using System;
using System.Collections.Generic; // Make sure to include this for List<T>
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    public static async Task Main()
    {
        await Programme();
    }

    public static async Task Programme()
    {
        using (var client = new HttpClient())
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://moviesdatabase.p.rapidapi.com/titles"),
                Headers =
                {
                    { "X-RapidAPI-Key", "4facf1567bmshd8a5c5f9df142b8p164d39jsn9cd03c18125b" },
                    { "X-RapidAPI-Host", "moviesdatabase.p.rapidapi.com" },
                },
            };

            try
            {
                using (var response = await client.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var body = await response.Content.ReadAsStringAsync();
                        var titlesResponse = JsonSerializer.Deserialize<TitlesResponse>(body);
                        

                        // Access the desired field (e.g., the first title)
                        if (titlesResponse.Titles.Count > 0)
                        {
                            var firstTitle = titlesResponse.Titles[0].Name;
                            Console.WriteLine($"First Title: {firstTitle}");
                        }
                        else
                        {
                            Console.WriteLine("No titles found in the response.");
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        Console.WriteLine("Access Forbidden. Check your API key and permissions.");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle exceptions, such as network errors
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }

    // Define a class to represent the structure of the JSON response
    public class TitlesResponse
    {
        public List<Title> Titles { get; set; }
    }

    public class Title
    {
        public string Name { get; set; }
        // Add other properties as needed
    }
}
