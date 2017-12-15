using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Models.BDDObject;

namespace Models.ViewModels
{
    public interface IAdminViewModel
    {
        string Title { get; set; }

        string Description { get; set; }
    }
}
