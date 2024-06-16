using ISPAN.Izakaya.BLL_Service_.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;  // 格式化json

namespace ISPAN.Izakaya.BLL_Service_
{
    public class LinePayService
    {


        private readonly ILogger<LinePayService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _channelId;
        private readonly string _channelSecretKey;
        private readonly string _linePayBaseApiUrl = "https://sandbox-api-pay.line.me";

        public LinePayService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<LinePayService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _channelId = configuration["LinePay:ChannelId"];
            _channelSecretKey = configuration["LinePay:ChannelSecretKey"];
            _linePayBaseApiUrl = configuration["LinePay:BaseApiUrl"];
        }

        // 送出建立交易請求至 Line Pay Server
        public async Task<PaymentResponseDto> SendPaymentRequest(LinePayOrderDetail orderDetail)
        {
            return await ExecuteLinePayApi<PaymentResponseDto>("/v3/payments/request", orderDetail);
        }

        // 前端傳回 transactionId, orderId, PaymentConfirmDto {Amount, Currency} 後端向linePayServer確認付款，Header 一樣要加密
        public async Task<PaymentConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentConfirmDto dto)
        {
            var requestUrl = $"/v3/payments/{transactionId}/confirm";
            return await ExecuteLinePayApi<PaymentConfirmResponseDto>(requestUrl, dto);
        }

        private async Task<T> ExecuteLinePayApi<T>(string requestUrl, object requestData)
        {
            var nonce = Guid.NewGuid().ToString();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase, WriteIndented = true };
            var body = JsonSerializer.Serialize(requestData, options);
            var signature = SignatureHMACSHA256.GenerateSignature(_channelSecretKey, requestUrl, body, nonce);
            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Add("X-LINE-ChannelId", _channelId);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_linePayBaseApiUrl + requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Line Pay API Success: {responseContent}");
                return JsonSerializer.Deserialize<T>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                _logger.LogError($"Line Pay API Request failed: {response.StatusCode}");
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }

        public string TransactionCancel(string transactionId)
        {
            return $"訂單 {transactionId} 已取消";
        }
    }
}
