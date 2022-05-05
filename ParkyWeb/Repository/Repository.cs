using System;
using System.Collections.Generic;
using System.Linq;
using ParkyWeb.Repository.IRepository;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace ParkyWeb.Repository
{
    public class Repository<T> : IRepository<T> where T: class
    {
        public readonly IHttpClientFactory _ihcf;

        public Repository(IHttpClientFactory ihcf)
        {
            _ihcf = ihcf;
        }

        public async Task<bool> CreateAsync(string url, T objToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,url);      //create the request message

            if(objToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }
            
            var client = _ihcf.CreateClient();      //create a Http Client

            HttpResponseMessage message = await client.SendAsync(request);      //then send the requesr message into that client

            if (message.StatusCode == System.Net.HttpStatusCode.Created)         //if "message" is created
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete,url+id);

            var client = _ihcf.CreateClient();

            HttpResponseMessage msg = await client.SendAsync(request); 

            if(msg.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,url);

            var client = _ihcf.CreateClient();

            HttpResponseMessage msg = await client.SendAsync(request);

            if (msg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var obj = await msg.Content.ReadAsStringAsync();    //convert content ro string

                return JsonConvert.DeserializeObject<IEnumerable<T>>(obj);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
            string helper = "";     //Fixed url path
            if (url.Contains("trails"))
            {
                string temp1 = "?trailid=";
                string c = id.ToString().Trim();
                string temp2 = temp1+c;
                helper = temp2.Trim();
             
            }
            else
            {
                string temp1 = "?nationalparkid=";
                string c = id.ToString().Trim();
                string temp2 = temp1 + c;
                helper = temp2.Trim();
            }
            var request = new HttpRequestMessage(HttpMethod.Get, url+id+helper);

            var client = _ihcf.CreateClient();

            HttpResponseMessage msg = await client.SendAsync(request);

            if (msg.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var obj = await msg.Content.ReadAsStringAsync();    //convert content ro string

                return JsonConvert.DeserializeObject<T>(obj);
            }
            else
                return null;
        }

        public async Task<bool> UpdateAsync(string url, T objToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);      //create the request message

            if (objToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(objToUpdate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _ihcf.CreateClient();      //create a Http Client

            HttpResponseMessage message = await client.SendAsync(request);      //then send the requesr message into that client

            if (message.StatusCode == System.Net.HttpStatusCode.NoContent)         //if "message" is created
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
