using System;
using RestSharp;

namespace GoPay.RestSharp
{
    public class NewtonsoftJsonRestRequest : RestRequest
    {
        public NewtonsoftJsonRestRequest()
        {
            InitSerializer();
        }

        public NewtonsoftJsonRestRequest(Method method) : base(method)
        {
            InitSerializer();
        }

        public NewtonsoftJsonRestRequest(string resource, Method method = Method.GET, DataFormat dataFormat = DataFormat.Xml) : base(resource, method, dataFormat)
        {
            InitSerializer();
        }

        public NewtonsoftJsonRestRequest(Uri resource, Method method = Method.GET, DataFormat dataFormat = DataFormat.Xml) : base(resource, method, dataFormat)
        {
            InitSerializer();
        }

        protected void InitSerializer()
        {
            JsonSerializer = new NewtonsoftJsonSerializer();
        }
    }
}