namespace Ensco.Irma.Oap.WebClient.Corp
{
    //Base class used by WebClient Models

    public partial class AuthenticationClient
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

    }

    public partial class OapHierarchyClient
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

    }

    public partial class OapChecklistGroupClient
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

    }

    public partial class OapChecklistSubGroupClient
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
    }

    public partial class OapProtocolFormTypeClient
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
    }

    public partial class OapFrequencyTypeClient
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
    }

    public partial class OapChecklistTopicClient
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
    }

    public partial class OapChecklistQuestionClient
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
    }
}