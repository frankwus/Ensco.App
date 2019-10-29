using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Ensco.Irma.Oap.WebClient.Rig
{
    using Ensco.Irma.Oap.Common.WebClient;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    //Base class used by WebClient Models

    public partial class RigOapChecklistClient
    {

        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder)
        {
            if (request.Method.Method.Equals("GET"))
            {
                request.Content = null;
            }
        }

        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response)
        {

        }

        partial void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
        {
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }

    }
}