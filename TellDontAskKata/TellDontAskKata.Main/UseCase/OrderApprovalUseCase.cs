﻿using TellDontAskKata.Main.Repository;

namespace TellDontAskKata.Main.UseCase
{
    public class OrderApprovalUseCase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderApprovalUseCase(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Run(OrderApprovalRequest request)
        {
            var order = _orderRepository.GetById(request.OrderId);
            order.ChangeApprove(request.Approved);
            _orderRepository.Save(order);
        }
    }
}