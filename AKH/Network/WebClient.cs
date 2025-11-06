using DewmoLib.Dependencies;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
namespace Scripts.Network
{
    [Provide,DefaultExecutionOrder(-10)]
    public class WebClient : MonoBehaviour, IDependencyProvider
    {
        private static string _serverUrl = "http://dewmo.kro.kr:3303";
        private static WebClient _instance;
        private HttpClient _httpClient;
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            _httpClient = new(handler);
        }
        public async Task<T> SendGetRequest<T>(string endPoint)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"{_serverUrl}/{endPoint}");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var debug = JsonConvert.DeserializeObject(result);
                return JsonConvert.DeserializeObject<T>(result);
            }
            return default(T);
        }
        public async Task<TResult> SendPostRequest<T, TResult>(string endPoint, T information)
        {
            string json = JsonConvert.SerializeObject(information);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync($"{_serverUrl}/{endPoint}", content);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(result);
            }
            return default(TResult);
        }
        public async Task<bool> SendPostRequest<T>(string endPoint, T information)
        {
            string json = JsonConvert.SerializeObject(information);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{_serverUrl}/{endPoint}", content);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> SendDeleteRequest(string endPoint)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"{_serverUrl}/{endPoint}");
            return response.IsSuccessStatusCode;
        }
    }
}
