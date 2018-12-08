using System;
using RestSharp;

namespace GoPay.RestSharp
{
    public class NewtonsoftJsonRestRequest : RestRequest
    {
        public NewtonsoftJsonRestRequest()
        {
            IntializeJsonSerializer();
        }

        public NewtonsoftJsonRestRequest(Method method) : base(method)
        {
            IntializeJsonSerializer();
        }

        public NewtonsoftJsonRestRequest(string resource, Method method = Method.GET, DataFormat dataFormat = DataFormat.Xml) : base(resource, method, dataFormat)
        {
            IntializeJsonSerializer();
        }

        public NewtonsoftJsonRestRequest(Uri resource, Method method = Method.GET, DataFormat dataFormat = DataFormat.Xml) : base(resource, method, dataFormat)
        {
            IntializeJsonSerializer();
        }

        protected void IntializeJsonSerializer()
        {
            JsonSerializer = new NewtonsoftJsonSerializer();
        }
    }
}