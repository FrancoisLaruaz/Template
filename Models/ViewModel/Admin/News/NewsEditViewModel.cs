using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class NewsEditViewModel : IAdminViewModel
    {


        public string Title { get; set; }

        public string Description { get; set; }


        public int Id { get; set; }
        public DateTime PublishDate { get; set; }

        public string NewsTitle { get; set; }
        public string NewsDescription { get; set; }

        public string MailSubject { get; set; }

        public int TypeId { get; set; }

        public bool Active { get; set; }

        public int? TypeUserMailingId { get; set; }

        public List<Category> TeamMemberOrderList { get; set; }

        public List<Category> TypeUserMailingList { get; set; }

        public NewsEditViewModel()
        {
            TeamMemberOrderList = new List<Category>();
            TypeUserMailingList = new List<Category>();
        }
    }
}
