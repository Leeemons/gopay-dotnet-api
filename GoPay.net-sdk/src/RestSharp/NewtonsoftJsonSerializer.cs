using Newtonsoft.Json;
using RestSharp.Serializers;

namespace GoPay.RestSharp
{
    public class NewtonsoftJsonSerializer : ISerializer
    {        
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public string ContentType { get; set; } = "application/json";
    }
}