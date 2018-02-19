using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public class ResetPasswordViewModel
    {

        public int UserId { get; set; }

        [Required(ErrorMessage = "[[[A password is required.]]]")]
        [Display(Name = "[[[Password]]]")]
        public string Password { get; set; }

        public string Token { get; set; }

        public ResetPasswordViewModel()
        {
            
        }
    }
}
