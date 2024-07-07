using Microsoft.AspNetCore.Mvc;
using OmerOlkunWebApi.Dtos;
using Microsoft.AspNetCore.Cors;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace OmerOlkunWebApi.Controllers;

[ApiController]
[Route("")]
public class OmController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public OmController(HttpClient hc)
    {
        _httpClient = hc;
    }

    [HttpPost("make-pdf")]
    public async Task<string> GeneratePdf([FromBody] SoldItemDTO item)
    {
        string path = item.Id + ".pdf";
        string result = "Information of " + item.Name + "is following... \nId:" + item.Id + "\nName: " + item.Name + "\nStok Kodu: " + item.Code + "\nKdv Orani: " + item.KdvRatio + "\nKdv Dahil Birim Fiyati: " + item.UnitPrice;

        
        using( PdfWriter writer = new PdfWriter(path))
        {
            using (PdfDocument doc = new PdfDocument(writer))            
            {
                Document d = new Document(doc);
                d.Add(new Paragraph(result));
                d.Close();

            }
        }
        Console.WriteLine("hadi bakalim");

        
        return "hey";
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
    


    
    [EnableCors("Policy1")]
    [HttpPost]
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
            var contents  = await response.Content.ReadAsStreamAsync();

            Console.WriteLine("type of contents: " + contents.GetType());

        
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

