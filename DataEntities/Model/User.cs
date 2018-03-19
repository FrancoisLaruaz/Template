//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataEntities.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.EmailAudits = new HashSet<EmailAudit>();
            this.News = new HashSet<News>();
            this.ScheduledTasks = new HashSet<ScheduledTask>();
        }
    
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public Nullable<int> CountryId { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public string Adress3 { get; set; }
        public int LanguageId { get; set; }
        public Nullable<int> ProvinceId { get; set; }
        public string UserName { get; set; }
        public string PictureSrc { get; set; }
        public bool ReceiveNews { get; set; }
        public string PictureThumbnailSrc { get; set; }
        public Nullable<int> GenderId { get; set; }
        public string ResetPasswordToken { get; set; }
        public string EmailConfirmationToken { get; set; }
        public System.DateTime DateLastConnection { get; set; }
        public System.DateTime ModificationDate { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string AspNetUserId { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Category Gender { get; set; }
        public virtual Category Language { get; set; }
        public virtual Country Country { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailAudit> EmailAudits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }
        public virtual Province Province { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScheduledTask> ScheduledTasks { get; set; }
    }
}
