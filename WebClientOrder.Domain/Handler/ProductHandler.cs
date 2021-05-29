using Flunt.Notifications;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebClientOrder.Domain.Commands;
using WebClientOrder.Domain.Commands.Product;
using WebClientOrder.Domain.Entities;
using WebClientOrder.Domain.Interfaces.Contracts;
using WebClientOrder.Domain.Repositories;
using WebClientOrder.Domain.Shared;

namespace WebClientOrder.Domain.Handler
{
    public class ProductHandler : Notifiable, IHandler<CreateProductCommand>, IHandler<UpdateProductCommand>, IHandler<DeleteProductCommand>
    {
        private readonly IProductRepository _productRepository;
        public ProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<ICommandResult> Handle(CreateProductCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            await CheckProductDescriptionExist(command.Description);

            var product = new Product(command.Description, command.Value);

            _productRepository.Add(product);

            return new GenericCommandResult(true, "SuccessFully", product);
        }
        public async Task<ICommandResult> Handle(UpdateProductCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            var product = await GetById(command.Id);

            product.Update(command.Description, command.Value);

            _productRepository.Update(product);

            return new GenericCommandResult(true, "SuccessFully", product);
        }
        public async Task<ICommandResult> Handle(DeleteProductCommand command)
        {
            command.Validate();

            if (command.Invalid)
                return ErrorNotification.Error(command.Notifications);

            var product = await GetById(command.Id);

            _productRepository.Remove(product.Id);

            return new GenericCommandResult(true, "SuccessFully", product);
        }
        private async Task<Product> GetById(Guid id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
                throw new ValidationException("Product not found it!");

            return product;
        }
        private async Task CheckProductDescriptionExist(string description)
        {
            var exists = await _productRepository.GetProductDescription(description);

            if (exists != null)
                throw new ValidationException("Product already registred!");
        }

        
    }
}
