using Newtonsoft.Json;
using Rootedfuture.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Rootedfuture.Services
{
    class ApiService
    {
        readonly static HttpClient _httpClient = new HttpClient();
        private static JsonSerializer _serializer = new JsonSerializer();
        private static string apiURL = "https://rootedfutu.re/api/";

        private static async Task<JsonTextReader> SendRequest(string requestURL)
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = new HttpResponseMessage();
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                response = await _httpClient.GetAsync(apiURL + requestURL);
                if (!response.IsSuccessStatusCode)
                {

                    return null;
                }
            }
            else
            {
                return null;
            }
            var reader = new StreamReader(await response.Content.ReadAsStreamAsync());

            var str = new JsonTextReader(reader);

            return str;
        }

        public static async Task<JsonTextReader> SendMultipartRequest(string requestURL, MultipartFormDataContent requestData = null)
        {

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = new HttpResponseMessage();

            HttpContent content = requestData;

            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                try
                {
                    response = await _httpClient.PostAsync(apiURL + requestURL, content);
                }
                catch (Exception)
                {

                    return null;
                }
            }
            else
            {

                return null;
            }

            var reader = new StreamReader(await response.Content.ReadAsStreamAsync());

            var str = new JsonTextReader(reader);

            return str;
        }

        public static async Task<TreeData> GetTreeInfoAsync(string qrCodeNum)
        {
            var responseData = await SendRequest("retrieve-tree/qr/" + qrCodeNum);
            if (responseData == null)
            {
                return null;
            }
            return _serializer.Deserialize<TreeData>(responseData);
        }

        public static async Task<List<GalleryItem>> GetTreeGallery(int id)
        {
            var responseData = await SendRequest("get-tree-gallery/id/" + id);
            if (responseData == null)
            {
                return null;
            }
            return _serializer.Deserialize<List<GalleryItem>>(responseData);
        }

        public static async Task<TreePhoto> SendImageToServer(MultipartFormDataContent content)
        {
            var responseData = await SendMultipartRequest("update-tree", content);

            if (responseData == null)
            {
                return null;
            }
            return _serializer.Deserialize<TreePhoto>(responseData);
        }


    }
}
