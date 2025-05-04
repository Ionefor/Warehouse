using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Warehourse.ProductPlacement.Application.Abstractions;
using Warehourse.ProductPlacement.Domain.Aggregate;
using Warehouse.WarehouseManagement.Contracts.Dtos;

namespace Warehourse.ProductPlacement.Infrastructure.Services;

public class EmailNotificationService : IEmailNotificationService
{ 
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    
    public EmailNotificationService(IConfiguration config)
    {
        _fromEmail = config["EmailSettings:Username"];
        
        _smtpClient = new SmtpClient(config["EmailSettings:SmtpHost"])
        {
            Port = int.Parse(config["EmailSettings:SmtpPort"]),
            Credentials = new NetworkCredential(
                config["EmailSettings:Username"],
                config["EmailSettings:Password"]),
            EnableSsl = true
        };
    }
    
    public async Task SendNotification(Product product, WarehouseDto warehouse)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_fromEmail),
            Subject = $"Товар прибыл на склад {warehouse.Name}",
            Body = $"""
                    <h1>Уведомление о прибытии товара</h1>
                    <p>Товар {product.Name.Value} (ID: {product.Id.Id}) был размещен:</p>
                    <ul>
                        <li>Склад: {warehouse.Name}</li>
                        <li>Дата: {DateTime.Now:dd.MM.yyyy HH:mm}</li>
                    </ul>
                    """,
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(warehouse.Email);

        try
        {
            await _smtpClient.SendMailAsync(mailMessage);
        }
        catch
        {
            
        }
    }
}