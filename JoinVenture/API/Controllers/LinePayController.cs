using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.LinePayDto;
using API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LinePayController :BaseController
    {

        private readonly LINEPayService _linePayService;

        public LinePayController(LINEPayService linePayService)
        {
            _linePayService = linePayService;
            
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<PaymentRequestResponseDto> CreatePayment(PaymentRequestDto dto)
        {

            var result =  await _linePayService.SendPaymentRequest(dto);
            Console.WriteLine(result.ReturnCode);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("Confirm")]
        public async Task<PaymentRequestConfirmResponseDto> ConfirmPayment([FromQuery] string transactionId, [FromQuery] string orderId, PaymentRequestConfirmDto dto )
        {
            return await _linePayService.ConfirmPayment(transactionId, orderId,dto);
        }
        [AllowAnonymous]
        [HttpGet("Cancel")]
        public async void CancelTransaction([FromQuery] string transactionId)
        {
            _linePayService.TransactionCancel(transactionId);
        }
    }
    
}