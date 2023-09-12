using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Orders;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderController:BaseController
    {

        private readonly IMapper _autoMapper;

        public OrderController(IMapper autoMapper)
        {
            _autoMapper = autoMapper;
            
        }
        
         [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {


            // var orderDtoJson = JsonSerializer.Serialize(orderDto);
            Order order = _autoMapper.Map<OrderDto,Order>(orderDto);

            // var orderJson = JsonSerializer.Serialize(order);
            // Console.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++"+orderJson+"++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            // // return Ok();

            return Ok(await Mediator.Send(new Create.Command {Order = order}));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(Guid id)
        {
            return await Mediator.Send(new Detail.Query{OrderId = id});
        }
    }
}