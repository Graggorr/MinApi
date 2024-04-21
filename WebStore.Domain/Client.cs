﻿using FluentResults;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace WebStore.Domain
{
    public class Client
    {
        private Client() { }

        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public IList<Order> Orders { get; set; }

        public void SendEmailMessageAfterOrderReceiving()
        {
            //var emailAddress = new MailAddress("Email");
        }

        public static async Task<Result<Client>> CreateClientAsync(ClientDto dto, IClientRepository clientRepository, IOrderRepository orderRepository,
            bool verifyUniqueNumber = true, bool verifyUniqueEmail = true)
        {
            if (verifyUniqueNumber)
            {
                var result = await clientRepository.IsNumberUniqueAsync(dto.PhoneNumber);

                if (result.IsFailed)
                {
                    return Result.Fail(result.Errors);
                }
            }

            if (verifyUniqueEmail)
            {
                var result = await clientRepository.IsEmailUniqueAsync(dto.Email);

                if (result.IsFailed)
                {
                    return Result.Fail(result.Errors);
                }
            }

            if (dto.Orders.Count == 0)
            {
                return Result.Fail(new Error("The order container is empty"));
            }

            var orders = new List<Order>();

            foreach(var order in dto.Orders)
            {
                var result = await orderRepository.GetOrderAsync(order.Id);

                if (result.IsSuccess)
                {
                    orders.Add(result.Value);
                }
            }

            var client = new Client { PhoneNumber = dto.PhoneNumber, Email = dto.Email, Name = dto.Name, Orders = orders };

            return client;
        }
    }
}