using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;

public class EmailService
{
    public async Task SendEmailAsync(string recipientEmail, string recipientName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("AlimentandoEsperanzas", "no-reply@alimentandoesperanzas.com")); // Cambia esto por tu dirección de correo electrónico
        message.To.Add(new MailboxAddress(recipientName, recipientEmail));
        message.Subject = "Recordatorio de donación mensual";

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = $"Hola {recipientName},\n\nEste es un recordatorio amable para informarte que tu donación mensual está próxima. ¡Gracias por tu apoyo continuo!\n\nAtentamente,\nAlimentandoEsperanzas";
        message.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("sandbox.smtp.mailtrap.io", 587, SecureSocketOptions.StartTls); // Usa las credenciales y el host de Mailtrap
            await client.AuthenticateAsync("e557db31a00dce", "7f82e535c4ceef"); // Cambia esto por tu nombre de usuario y contraseña de Mailtrap
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
