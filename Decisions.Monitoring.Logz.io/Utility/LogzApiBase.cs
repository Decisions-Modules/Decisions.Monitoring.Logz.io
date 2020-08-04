using Decisions.Monitoring.Logz.io.Data;
using DecisionsFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Decisions.Monitoring.Logz.io.Utility
{
    public static  partial class LogzApi
    {

        private static HttpClient GetClient(string baseAddress)
        {
            HttpClient httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            return httpClient;
        }

        private static string ParseRequestContent<T>(T content)
        {
            string data = JsonConvert.SerializeObject(content);
            return data;
        }
        private static LogzErrorResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var err = ParseResponse<LogzErrorResponse>(response);
                return err;
            };
            return null;
        }

        private static R ParseResponse<R>(HttpResponseMessage response) where R : new()
        {

            var responseString = response.Content.ReadAsStringAsync().Result;

            var result = JsonConvert.DeserializeObject<R>(responseString);

            return result;
        }

        private static R PostRequest<R, T>(LogzCredential connection, string requestUri, T[] content) where R : new()
        {
            var data = new StringBuilder();
            foreach (var item in content)
            {
                if (data.Length > 0) data.Append("\n");
                data.Append(ParseRequestContent(item));
            }
            var contentStr = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            HttpResponseMessage response = GetClient(connection.BaseUrl).PostAsync(requestUri, contentStr).Result;

            CheckResponse(response);
            return ParseResponse<R>(response);
        }



    }
}
