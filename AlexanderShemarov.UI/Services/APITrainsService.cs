using AlexanderShemarov.Domain.Entities;
using AlexanderShemarov.Domain.Models;
using System.Text.Json;


namespace AlexanderShemarov.UI.Services
{
    public class APITrainsService(HttpClient httpClient) : ITrainsService
    {
        public async Task<ResponseData<Trains>> CreateTrainAsync(Trains train, IFormFile? formFile)
        {
            var serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var responseData = new ResponseData<Trains>();
            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, train);
            if (!response.IsSuccessStatusCode)
            {
                responseData.Success = false;
                responseData.ErrorMessage = $"'Trains'-class object isn't created: {response.StatusCode}";
                return responseData;
            }

            if (formFile != null)
            {
                var trainContent = await response.Content.ReadFromJsonAsync<Trains>();
                if (trainContent == null)
                {
                    return new ResponseData<Trains>
                    {
                        Success = false,
                        ErrorMessage = "Failed to parse created train data"
                    };
                }

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri($"{httpClient.BaseAddress.AbsoluteUri}{trainContent.ID}/image")
                };
                var content = new MultipartFormDataContent();
                var streamContent = new StreamContent(formFile.OpenReadStream());

                content.Add(streamContent, "image", formFile.FileName);
                request.Content = content;

                response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    responseData.Success = false;
                    responseData.ErrorMessage = $"The image isn't saved: {response.StatusCode}";
                }
            }

            return responseData;
        }


        public async Task<ResponseData<bool>> DeleteTrainAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"{id}");
            return new ResponseData<bool>
            {
                Success = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? null : $"Error: {response.StatusCode}"
            };
        }


        public async Task<ResponseData<Trains>> GetTrainByIdAsync(int id)
        {
            var response = await httpClient.GetAsync($"{id}");

            if (!response.IsSuccessStatusCode)
            {
                return new ResponseData<Trains>
                {
                    Success = false,
                    ErrorMessage = $"Error: {response.StatusCode}"
                };
            }

            try
            {
                var train = await response.Content.ReadFromJsonAsync<Trains>();
                return new ResponseData<Trains>
                {
                    Data = train,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<Trains>
                {
                    Success = false,
                    ErrorMessage = $"JSON Reading Error: {ex.Message}"
                };
            }
        }


        public async Task<ResponseData<ListModel<Trains>>> GetTrainsListAsync(string? trainTypesNormalizedName, int pageNo = 1)
        {
            var uri = httpClient.BaseAddress;

            var queryData = new Dictionary<string, string>();
            queryData.Add("pageNo", pageNo.ToString());
            if (!String.IsNullOrEmpty(trainTypesNormalizedName))
            {
                queryData.Add("trainType", trainTypesNormalizedName);
            }
            var query = QueryString.Create(queryData);

            var result = await httpClient.GetAsync(uri + query.Value);
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadFromJsonAsync<ResponseData<ListModel<Trains>>>();
            };

            var response = new ResponseData<ListModel<Trains>>
            {
                Success = false,
                ErrorMessage = "TrainsAPI Data Reading Error"
            };
            return response;
        }


        public async Task<ResponseData<Trains>> UpdateTrainAsync(int id, Trains train, IFormFile? formFile)
        {
            try
            {
                var updateDto = new TrainUpdateDto
                {
                    ID = train.ID,
                    Name = train.Name,
                    Description = train.Description,
                    Speed = train.Speed,
                    Cost = train.Cost,
                    TrainTypesId = train.TrainTypesId,
                };


                var updateResponse = await httpClient.PutAsJsonAsync($"{id}", updateDto);
                if (!updateResponse.IsSuccessStatusCode)
                {
                    return new ResponseData<Trains>
                    {
                        Success = false,
                        ErrorMessage = $"Updating Error: {updateResponse.StatusCode}"
                    };
                }

                if (formFile != null)
                {
                    var imageUrl = await UploadImageAsync(id, formFile);
                    if (!imageUrl.Success)
                    {
                        return new ResponseData<Trains>
                        {
                            Success = false,
                            ErrorMessage = imageUrl.ErrorMessage
                        };
                    }
                }

                return new ResponseData<Trains> { Success = true };
            }
            catch (Exception ex)
            {
                return new ResponseData<Trains>
                {
                    Success = false,
                    ErrorMessage = $"Unexpected error: {ex.Message}"
                };
            }
        }


        private async Task<ResponseData<string>> UploadImageAsync(int trainId, IFormFile imageFile)
        {
            try
            {
                var requestUri = new Uri(httpClient.BaseAddress, $"{trainId}/image");
                using var content = new MultipartFormDataContent();
                using var fileStream = imageFile.OpenReadStream();

                content.Add(new StreamContent(fileStream), "image", imageFile.FileName);

                var response = await httpClient.PostAsync(requestUri, content);

                if (!response.IsSuccessStatusCode)
                {
                    return new ResponseData<string>
                    {
                        Success = false,
                        ErrorMessage = $"Image upload failed: {response.StatusCode}"
                    };
                }

                var imageUrl = await response.Content.ReadAsStringAsync();
                return new ResponseData<string>
                {
                    Success = true,
                    Data = imageUrl
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    Success = false,
                    ErrorMessage = $"Image upload error: {ex.Message}"
                };
            }
        }
    }


    public class TrainUpdateDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Speed { get; set; }
        public decimal Cost { get; set; }
        public int TrainTypesId { get; set; }
    }
}
