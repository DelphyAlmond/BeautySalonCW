using BSUcontractmodels.OfficePackage;
using BSUmodels.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace BSUbusinesslogic.OfficePackage;

/// Реализация сервиса для отправки отчётов по email
/// Использует SMTP для отправки письма с прикрепленным файлом отчёта
public class EmailSenderBLC : IEmailSenderBLC
{
    private readonly ILogger<EmailSenderBLC> _logger;
    private readonly IConfiguration _configuration;

    public EmailSenderBLC(ILogger<EmailSenderBLC> logger, IConfiguration configuration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// Отправить отчёт по посещениям мастера на email
    public async Task<bool> SendMasterVisitReportAsync(
        string toEmail,
        string masterName,
        Stream reportStream,
        string fileName,
        DateTime dateFrom,
        DateTime dateTo,
        CancellationToken ct)
    {
        try
        {
            // Валидация входных данных
            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ValidationException("< Email Sender BLC: Email address is empty >");

            if (!IsValidEmail(toEmail))
                throw new ValidationException($"< Email Sender BLC: Invalid email format: {toEmail} >");

            if (reportStream == null || reportStream.Length == 0)
                throw new ValidationException("< Email Sender BLC: Report stream is empty >");

            // Получаем SMTP конфигурацию
            var smtpHost = _configuration["Email:SmtpHost"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
            var smtpUsername = _configuration["Email:Username"];
            var smtpPassword = _configuration["Email:Password"];
            var fromEmail = _configuration["Email:FromAddress"];
            var fromDisplayName = _configuration["Email:DisplayName"] ?? "Beauty Salon Report";

            // Валидация конфигурации
            if (string.IsNullOrWhiteSpace(smtpHost) || string.IsNullOrWhiteSpace(smtpUsername) || 
                string.IsNullOrWhiteSpace(smtpPassword) || string.IsNullOrWhiteSpace(fromEmail))
            {
                _logger.LogError("SMTP configuration is incomplete. Check appsettings.json");
                throw new InvalidOperationException("< Email configuration is not properly configured >");
            }

            // Формируем тему письма
            var subject = $"Отчёт по посещениям: {masterName} ({dateFrom:dd.MM.yyyy} - {dateTo:dd.MM.yyyy})";

            // Формируем тело письма
            var body = GenerateEmailBody(masterName, dateFrom, dateTo);

            // Отправляем письмо
            await SendEmailWithAttachmentAsync(
                smtpHost,
                smtpPort,
                smtpUsername,
                smtpPassword,
                fromEmail,
                fromDisplayName,
                toEmail,
                subject,
                body,
                reportStream,
                fileName,
                ct);

            _logger.LogInformation($"Report email sent successfully to {toEmail} for master {masterName}");
            return true;
        }
        catch (ValidationException ex)
        {
            _logger.LogError(ex, "Validation error while sending report email");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending report email");
            throw;
        }
    }

    /// Отправить письмо с прикрепленным файлом через SMTP
    private async Task SendEmailWithAttachmentAsync(
        string smtpHost,
        int smtpPort,
        string smtpUsername,
        string smtpPassword,
        string fromEmail,
        string fromDisplayName,
        string toEmail,
        string subject,
        string body,
        Stream attachmentStream,
        string attachmentFileName,
        CancellationToken ct)
    {
        using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
        {
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
            smtpClient.Timeout = 30000; // 30 секунд

            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(fromEmail, fromDisplayName);
                mailMessage.To.Add(new MailAddress(toEmail));
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                mailMessage.Priority = MailPriority.Normal;

                // Добавляем прикрепленный файл
                attachmentStream.Position = 0;
                var attachment = new Attachment(attachmentStream, attachmentFileName, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                mailMessage.Attachments.Add(attachment);

                try
                {
                    // Отправляем письмо асинхронно с поддержкой отмены через timeout
                    using (var cts = CancellationTokenSource.CreateLinkedTokenSource(ct))
                    {
                        // Устанавливаем timeout 30 секунд
                        cts.CancelAfter(TimeSpan.FromSeconds(30));

                        try
                        {
                            await smtpClient.SendMailAsync(mailMessage);
                        }
                        catch (OperationCanceledException)
                        {
                            _logger.LogWarning("Email send operation was cancelled or timed out");
                            throw;
                        }
                    }
                }
                catch (SmtpException ex)
                {
                    _logger.LogError(ex, "SMTP error while sending email");
                    throw new InvalidOperationException($"< Failed to send email via SMTP: {ex.Message} >", ex);
                }
            }
        }
    }

    /// Генерировать HTML тело письма
    private static string GenerateEmailBody(string masterName, DateTime dateFrom, DateTime dateTo)
    {
        return $@"
<!DOCTYPE html>
<html lang=""ru"">
<head>
    <meta charset=""UTF-8"">
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9; }}
        .header {{ background-color: #4CAF50; color: white; padding: 15px; border-radius: 5px; text-align: center; }}
        .content {{ background-color: white; padding: 20px; margin-top: 15px; border-radius: 5px; }}
        .footer {{ text-align: center; margin-top: 20px; font-size: 12px; color: #999; }}
        .info-box {{ background-color: #f0f0f0; padding: 10px; margin: 10px 0; border-left: 4px solid #4CAF50; }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <h2>Отчёт по посещениям</h2>
        </div>
        
        <div class=""content"">
            <p>Уважаемый клиент,</p>
            
            <p>К этому письму прикреплен отчёт по посещениям для мастера:</p>
            
            <div class=""info-box"">
                <strong>Мастер:</strong> {masterName}<br/>
                <strong>Период:</strong> с {dateFrom:dd.MM.yyyy} по {dateTo:dd.MM.yyyy}<br/>
                <strong>Дата создания:</strong> {DateTime.Now:dd.MM.yyyy HH:mm:ss}
            </div>
            
            <p>Отчёт содержит подробную информацию о всех посещениях за указанный период, включая:</p>
            <ul>
                <li>Дату и время посещений</li>
                <li>Информацию о клиентах</li>
                <li>Перечень оказанных услуг</li>
                <li>Стоимость услуг</li>
            </ul>
            
            <p>Если у вас возникли вопросы, пожалуйста, свяжитесь с нами.</p>
            
            <p>С уважением,<br/>
            <strong>Beauty Salon Management System</strong></p>
        </div>
        
        <div class=""footer"">
            <p>Это автоматическое письмо. Пожалуйста, не отвечайте на него напрямую.</p>
            <p>&copy; 2025 Beauty Salon. Все права защищены.</p>
        </div>
    </div>
</body>
</html>";
    }

    /// Валидировать email адрес
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
