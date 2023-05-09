using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Core.ServiceContracts
{
    public interface IEmailAttachmentSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage, Stream attachment, string attachmentName);
    }
}
