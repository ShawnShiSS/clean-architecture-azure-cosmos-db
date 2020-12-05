namespace CleanArchitectureCosmosDB.Infrastructure.AppSettings
{
    /// <summary>
    ///     SendGrid email settings
    /// </summary>
    public class SendGridEmailSettings
    {
        /// <summary>
        ///     API Key
        /// </summary>
        public string SendGridApiKey { get; set; }
        /// <summary>
        ///     From Email
        /// </summary>
        public string FromEmail { get; set; }
        /// <summary>
        ///     From Name
        /// </summary>
        public string FromName { get; set; }
    }

}
