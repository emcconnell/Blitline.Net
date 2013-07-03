using System.Collections.Generic;
using System.Threading.Tasks;
using Blitline.Net.Response;

namespace Blitline.Net.Request
{
    public static class BlitlineRequestExtensions
    {
        public static BlitlineResponse Send(this BlitlineRequest request)
        {
            var api = new BlitlineApi();
            return api.ProcessImages(request);
        }

        public static Task<BlitlineResponse> SendAsync(this BlitlineRequest request)
        {
            var api = new BlitlineApi();
            return api.ProcessImagesAsync(request);
        }

        public static BlitlineBatchResponse Send(this IEnumerable<BlitlineRequest> requests)
        {
            var api = new BlitlineApi();
            return api.ProcessImages(requests);
        }
    }
}