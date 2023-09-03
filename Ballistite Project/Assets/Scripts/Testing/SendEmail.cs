using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MimeKit;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;

public class SendEmail : MonoBehaviour
{
    
    public static void SendLog(string LOG_PATH, string text)
    {
        string senderEmailAddress = "ballistitelogs@outlook.com";
        string senderEmailPassword = "3$X2@K&5m%9Y";
        string receiverEmailAddress = "ballistitelogs@outlook.com";

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Ballistite Log Sender", senderEmailAddress));
        message.To.Add(new MailboxAddress("Email Receiver", receiverEmailAddress));
        message.Subject = "Ballistite Performance Logs";



        var multipartBody = new Multipart("mixed");
        {
            var textPart = new TextPart("plain")
            {
                Text = text

            };
            multipartBody.Add(textPart);

            string logPath = LOG_PATH;
            var logPart = new MimePart("text/plain")
            {
                Content = new MimeContent(File.OpenRead(logPath), ContentEncoding.Default),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(logPath)
            };
            multipartBody.Add(logPart);
        }
        message.Body = multipartBody;

        using (var client = new SmtpClient())
        {
            // This section must be changed based on your sender's email host
            // Do not use Gmail
            client.Connect("smtp-mail.outlook.com", 587, false);

            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(senderEmailAddress, senderEmailPassword);
            client.Send(message);
            client.Disconnect(true);
            Debug.Log("Sent email");
        }
    }
    
}
