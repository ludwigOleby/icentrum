using icentrum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace icentrum.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string json = JsonConvert.SerializeObject(model);

            string url = "https://services2.i-centrum.se/recruitment/auth";
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();


            dynamic data = JsonConvert.DeserializeObject(responseContent);
            string token = data.token;

            //GET request 
            string getUrl = "https://services2.i-centrum.se/recruitment/profile/avatar";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var getResponse = await _httpClient.GetAsync(getUrl);
            var getResponseContent = await getResponse.Content.ReadAsStringAsync();
            dynamic getData = JsonConvert.DeserializeObject(getResponseContent);
            string picture = getData.data;
            ViewBag.Picture = picture;


            return View();
        }
    }
}