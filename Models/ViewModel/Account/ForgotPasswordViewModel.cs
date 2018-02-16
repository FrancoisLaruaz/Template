using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "[[[A username is required.]]]")]
        [Display(Name = "[[[User Name]]]")]
        public string UserName { get; set; }

        public ForgotPasswordViewModel()
        {
            
        }
    }
}
