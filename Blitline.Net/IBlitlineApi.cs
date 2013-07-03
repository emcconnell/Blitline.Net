using System.Collections.Generic;
using System.Threading.Tasks;
using Blitline.Net.Request;
using Blitline.Net.Response;

namespace Blitline.Net
{
    public interface IBlitlineApi
    {
        BlitlineResponse ProcessImages(BlitlineRequest blitlineRequest);
        BlitlineBatchResponse ProcessImages(IEnumerable<BlitlineRequest> blitlineRequests);
        Task<BlitlineResponse> ProcessImagesAsync(BlitlineRequest blitlineRequest);
        Task<BlitlineBatchResponse> ProcessImagesAsync(IEnumerable<BlitlineRequest> blitlineRequests);
    }
}