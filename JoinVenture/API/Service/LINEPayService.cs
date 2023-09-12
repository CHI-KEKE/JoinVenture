using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.DTOs.LinePayDto;
using API.Provider;

namespace API.Service
{
    public class LINEPayService
    {
        private readonly HttpClient _client;
        private readonly JSONProvider _jsonProvider;
        public LINEPayService(IHttpClientFactory httpClientFactory,JSONProvider jsonProvider)
        {
            _jsonProvider = jsonProvider;
            _client = httpClientFactory.CreateClient();
        }

        private static string ChannelId = Environment.GetEnvironmentVariable("LINEChannelId");
        private static string ChannelSecretKey = Environment.GetEnvironmentVariable("LINEChannelSecretKey");

        private readonly string channelId = ChannelId;
        private readonly string channelSecretKey = ChannelSecretKey;


        private readonly string linePayBaseApiUrl = "https://sandbox-api-pay.line.me";


        //付款請求
        public async Task<PaymentRequestResponseDto> SendPaymentRequest(PaymentRequestDto dto)
        {
            var json = _jsonProvider.Serialize(dto);
            var nonce = Guid.NewGuid().ToString();
            var requestUrl = "/v3/payments/request";

            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, linePayBaseApiUrl + requestUrl)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            _client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            _client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await _client.SendAsync(request);
            var linePayRequestResponse = _jsonProvider.Deserialize<PaymentRequestResponseDto>(await response.Content.ReadAsStringAsync());

            return linePayRequestResponse;
        } 


        //確認付款請求
        public async Task<PaymentRequestConfirmResponseDto> ConfirmPayment(string transactionId, string orderId, PaymentRequestConfirmDto dto) 
        {
            var json = _jsonProvider.Serialize(dto);

            var nonce = Guid.NewGuid().ToString();
            var requestUrl = string.Format("/v3/payments/{0}/confirm", transactionId);
            var signature = SignatureProvider.HMACSHA256(channelSecretKey, channelSecretKey + requestUrl + json + nonce);

            var request = new HttpRequestMessage(HttpMethod.Post, String.Format(linePayBaseApiUrl + requestUrl, transactionId))
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _client.DefaultRequestHeaders.Add("X-LINE-ChannelId", channelId);
            _client.DefaultRequestHeaders.Add("X-LINE-Authorization-Nonce", nonce);
            _client.DefaultRequestHeaders.Add("X-LINE-Authorization", signature);

            var response = await _client.SendAsync(request);
            var ConfirmresponseDto = _jsonProvider.Deserialize<PaymentRequestConfirmResponseDto>(await response.Content.ReadAsStringAsync());
            return ConfirmresponseDto;
        }



        public async void TransactionCancel(string transactionId)
        {
            //使用者取消交易則會到這
            Console.WriteLine($"訂單 {transactionId} 已取消");
        }

    }



  
}