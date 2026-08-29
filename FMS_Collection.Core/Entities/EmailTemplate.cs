namespace YourProject.Core.Entities
{
    public class EmailTemplate
    {
        public int Id { get; set; }

        public string TemplateCode { get; set; } = string.Empty;

        public string TemplateName { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string BodyHtml { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}