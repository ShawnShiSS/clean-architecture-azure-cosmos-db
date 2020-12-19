using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureCosmosDB.WebAPI.Models.Attachment
{
    /// <summary>
    ///     Attachment
    /// </summary>
    public class AttachmentModel
    {
        public AttachmentModel()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        public Guid Id { get; set; }

        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string OriginalFileName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentType { get; set; }
    }
}
