using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;
using Models.Class;
using System.Web.Mvc;

namespace Models.ViewModels
{
    public class NewsEditViewModel : IAdminViewModel
    {


        public string Title { get; set; }

        public string Description { get; set; }


        public int Id { get; set; }


        [Required(ErrorMessage = "[[[The PublishDate is required.]]]")]
        [Display(Name = "[[[Publish Date]]]")]
        public DateTime PublishDate { get; set; }

        [Required(ErrorMessage = "[[[The Title is required.]]]")]
        [Display(Name = "[[[Title]]]")]
        public string NewsTitle { get; set; }

        [Required(ErrorMessage = "[[[The Description is required.]]]")]
        [Display(Name = "[[[Description]]]")]
        [AllowHtml]
        public string NewsDescription { get; set; }

        [Display(Name = "[[[Subject]]]")]
        public string MailSubject { get; set; }

        [Required(ErrorMessage = "[[[The Type is required.]]]")]
        [Display(Name = "[[[Type]]]")]
        public int TypeId { get; set; }


        [Display(Name = "[[[Active]]]")]
        public bool Active { get; set; }

        [Display(Name = "[[[Type User Mailing]]]")]
        public int? TypeUserMailingId { get; set; }

        public List<SelectionListItem> NewsTypeList { get; set; }

        public List<SelectionListItem> TypeUserMailingList { get; set; }

        public NewsEditViewModel()
        {
            NewsTypeList = new List<SelectionListItem>();
            TypeUserMailingList = new List<SelectionListItem>();
        }
    }
}
