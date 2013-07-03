using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Blitline.Net.Request;
using Blitline.Net.Response;
using Newtonsoft.Json;

namespace Blitline.Net
{
    public class BlitlineApi : IBlitlineApi
    {
        const string RootUrl = "http://api.blitline.com/job";
        
        public BlitlineResponse ProcessImages(BlitlineRequest blitlineRequest)
        {
            var payload = JsonConvert.SerializeObject(blitlineRequest);
            
            using (var client = new HttpClient())
            {
                var result = client.PostAsync(RootUrl, new FormUrlEncodedContent(new Dictionary<string, string>{{"json", payload}}));
                var o = result.Result.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<BlitlineResponse>(o);

                var correctS3BucketList = FixS3Urls(new[]{blitlineRequest});
                if (correctS3BucketList.Any()) response.FixS3Urls(correctS3BucketList);

                return response;
            }
        }

        public async Task<BlitlineResponse> ProcessImagesAsync(BlitlineRequest blitlineRequest)
        {
            using (var httpClient = new HttpClient())
            {
                var payload = JsonConvert.SerializeObject(blitlineRequest);
                var result = await httpClient.PostAsync(RootUrl, new FormUrlEncodedContent(new Dictionary<string, string> { { "json", payload } }));


                return await Task.Run(() =>
                {
                    result.EnsureSuccessStatusCode();
                    string content = result.Content.ReadAsStringAsync().Result;
                    var response = JsonConvert.DeserializeObject<BlitlineResponse>(content);
                    var correctS3BucketList = FixS3Urls(new[] { blitlineRequest });
                    if (correctS3BucketList.Any()) response.FixS3Urls(correctS3BucketList);

                    return response;
                });
            }

            //using (var httpClient = new HttpClient())
            //{
            //    var payload = JsonConvert.SerializeObject(blitlineRequest);
            //    BlitlineResponse result = null;
            //    return await httpClient.PostAsync(RootUrl, new FormUrlEncodedContent(new Dictionary<string, string> { { "json", payload } }))
            //              .ContinueWith((requestTask) =>
            //                  {
            //                      var response = requestTask.Result;
            //                      response.EnsureSuccessStatusCode();

            //                      response.Content.ReadAsStringAsync().ContinueWith((readTask) =>
            //                          {
            //                              result = JsonConvert.DeserializeObject<BlitlineResponse>(readTask.Result);
            //                              return result;
            //                          });

            //                      return result;

            //                  });
            //}
        }

        public BlitlineBatchResponse ProcessImages(IEnumerable<BlitlineRequest> blitlineRequests)
        {
            var payload = JsonConvert.SerializeObject(blitlineRequests.ToArray());

            using (var client = new HttpClient())
            {
                var result = client.PostAsync(RootUrl, new FormUrlEncodedContent(new Dictionary<string, string> { { "json", payload } }));
                var o = result.Result.Content.ReadAsStringAsync().Result;
                var response = JsonConvert.DeserializeObject<BlitlineBatchResponse>(o);

                var correctS3BucketList = FixS3Urls(blitlineRequests);
                if(correctS3BucketList.Any()) response.FixS3Urls(correctS3BucketList);
                
                return response;
            }
        }

        public async Task<BlitlineBatchResponse> ProcessImagesAsync(IEnumerable<BlitlineRequest> blitlineRequests)
        {
            var httpClient = new HttpClient();
            var payload = JsonConvert.SerializeObject(blitlineRequests.ToArray());
            var result = await httpClient.PostAsync(RootUrl, new FormUrlEncodedContent(new Dictionary<string, string> { { "json", payload } }));
            result.EnsureSuccessStatusCode();

            string content = await result.Content.ReadAsStringAsync();

            return await Task.Run(() =>
            {
                var response = JsonConvert.DeserializeObject<BlitlineBatchResponse>(content);
                var correctS3BucketList = FixS3Urls(blitlineRequests);
                if (correctS3BucketList.Any()) response.FixS3Urls(correctS3BucketList);

                return response;
            });
        }

        private Dictionary<string, string> FixS3Urls(IEnumerable<BlitlineRequest> blitlineRequests)
        {
            var imageKeyBucketList = new Dictionary<string, string>();

            if (blitlineRequests.Any(r => r.FixS3ImageUrl))
            {
                imageKeyBucketList = blitlineRequests.SelectMany(f => f.Functions).Select(f =>
                {
                    if (f.Save != null && f.Save.S3Destination != null)
                    {
                        return new { Image = f.Save.S3Destination.Key, f.Save.S3Destination.Bucket };
                    }
                    return null;
                }).ToDictionary(k => k.Image, v => v.Bucket);
            }

            return imageKeyBucketList;
        }
    }
}
