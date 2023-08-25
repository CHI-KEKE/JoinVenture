using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.LinePayDto
{
    public class PaymentRequestConfirmDto
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
    }


}