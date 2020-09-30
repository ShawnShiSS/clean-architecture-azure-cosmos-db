using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitectureCosmosDB.Infrastructure.AppSettings
{
    public class SendGridEmailSettings
    {
        public string DefaultFromEmail { get; set; }
        public string DefaultFromName { get; set; }
        public string SendGridKey { get; set; }
    }
}
