using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.Api.Services
{
    public class SendCloudMail
    {
        private readonly IConfiguration _configuration;
        public SendCloudMail(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public void SendMail(string subject, string message)
        {
            Debug.WriteLine($"Mail from {_configuration["mailSettings:mailFromAddress"]} to {_configuration["mailSettings:mailToAddress"]} sent from CloudService");
            Debug.WriteLine($"Subject: {subject}");
            Debug.WriteLine($"message: {message}");
        }
    }
}
