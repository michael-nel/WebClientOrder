using Flunt.Notifications;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Order;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Interfaces.Contracts;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Services;
using WebClientOrder.Domain.Shared;
using WebClientOrder.Domain.ValueOfObjects;
using static WebClientOrder.Domain.Commands.Order.CreateOrderCommand;

namespace WebClientOrder.Domain.Handler
{

    public class OrderHandler : Notifiable, IHandler<CreateOrderCommand>, IHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICepService _cepService;
        public OrderHandler(IOrderRepository orderReposity, IClientRepository clientRepository, IProductRepository productRepository, ICepService cepService)
        {
            _orderRepository = orderReposity;
            _clientRepository = clientRepository;
            _productRepository = productRepository;
            _cepService = cepService;
        }

        public async Task<ICommandResult> Handle(CreateOrderCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            var address = await _cepService.Execute(command.ZipCode);

            var client = await GetClientById(command.ClientId);

            var listProducts = await GetProducts(command.Products);

            var order = new Order(client, address, listProducts);
            
            _orderRepository.Add(order);

            return new GenericCommandResult(true, "SuccessFully", order);
        }

        public async Task<ICommandResult> Handle(DeleteOrderCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            var order = await GetById(command.Id);

            _productRepository.Remove(order.Id);

            return new GenericCommandResult(true, "SuccessFully", order);
        }

        private async Task<Order> GetById(Guid id)
        {
            var order = await _orderRepository.GetById(id);

            if (order == null)
                throw new ValidationException("Order not found it!");

            return order;
        }

        private async Task<Client> GetClientById(Guid id)
        {
            var client = await _clientRepository.GetById(id);

            if (client == null)
                throw new ValidationException("Client not found it!");

            return client;
        }

        private async Task<List<ProductQuantity>> GetProducts(IEnumerable<ProductsListCommand> productCommand)
        {
            var listProducts = new List<ProductQuantity>();
            
            foreach(ProductsListCommand product in productCommand)
            {
                var checkProduct = await  _productRepository.GetById(product.Id);

                if (checkProduct == null)
                    throw new Exception("Invalid product!!! Not Found it!!");

                listProducts.Add(new ProductQuantity
                {
                    Product = checkProduct,
                    Quantity = product.Quantity
                });
            }

            return listProducts;
        }
    }
}
