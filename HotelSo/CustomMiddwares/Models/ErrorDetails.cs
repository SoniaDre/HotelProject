namespace HotelSo.CustomMiddwares.Models
{
    public class ErrorDetails
    {
        public string? Tag { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }

        public override string ToString()
        {
            return System.Text.Json.JsonSerializer.Serialize(this);
        }
    }
}
