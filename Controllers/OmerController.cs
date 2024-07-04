using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json.Linq;


namespace OmerOlkunWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OmController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public OmController(HttpClient hc)
    {
        _httpClient = hc;
    }

    [HttpPost("get-token")]
    public async Task<IActionResult> GetToken()
    {
        try
        {
            string username = "test@test.com";
            string password = "Test123.";
            var token= await GetTokenAsync("http://istest.birfatura.net/token", username, password);
            dynamic data = JObject.Parse(token);
            string tokenValue = data.access_token.ToString();
            return Ok(tokenValue);

        }
        catch (Exception e)
        {
            return BadRequest(new { Error = e.Message });
        }
    }

    private async Task<string> GetTokenAsync(string url, string username, string password)
    {
        var requestContent = new FormUrlEncodedContent(new[]
           {
        new KeyValuePair<string, string>("username", username),
        new KeyValuePair<string, string>("password", password),
        new KeyValuePair<string, string>("grant_type", "password")
    });

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = requestContent
        };

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent; // Assuming the token is returned as plain text. Adjust as necessary.
        }
        else
        {
            throw new Exception("Failed to retrieve token");
        }
    }


    [HttpGet]
    public IActionResult Get()
    {
        return Ok("hello om");
    }
}

