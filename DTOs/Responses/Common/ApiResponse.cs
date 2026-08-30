namespace ClinicManagementAPI.DTOs.Responses.Common;

public class ApiResponse<T>
{
        public string ResponseCode { get; set; } = string.Empty;

        public string ResponseMessage { get; set; } = string.Empty;

        public T? Data { get; set; }
}