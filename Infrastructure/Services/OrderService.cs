﻿using Domain.Interfaces;
using Domain.Model.Entities;
using Domain.Model.Entities.OrderAggregate;
using Domain.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;

        public OrderService( IUnitOfWork unitOfWork, IBasketRepository basketRepository)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered= new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var subtotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal);

            order.PaymentIntentId = "";

            _unitOfWork.Repository<Order>().Add(order);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return null;

            await _basketRepository.DeleteBasketAsync(basketId);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var specification = new OrderWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpecification(specification);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var specification = new OrderWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(specification);
        }
    }
}
