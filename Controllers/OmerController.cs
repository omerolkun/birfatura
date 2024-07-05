using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
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
    
    [HttpPost("satislar-getir")]
    public async Task<IActionResult> SatislarGetir()
    {
         try
        {
            string username = "test@test.com";
            string password = "Test123.";
            var token= await GetTokenAsync("http://istest.birfatura.net/token", username, password);
            dynamic data = JObject.Parse(token);
            string tokenValue = data.access_token.ToString();
            string url = "http://istest.birfatura.net/api/test/SatislarGetir";

            HttpClient client = new HttpClient();

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenValue);
            var response = await client.SendAsync(requestMessage);
            var contents  = await response.Content.ReadAsStringAsync();


        
            return Ok(contents);

        }
        catch (Exception e)
        {
            return BadRequest(new { Error = e.Message });
        }        
    }


    [HttpGet]
    public IActionResult Get()
    {
        return Ok("hello om");
    }
}

