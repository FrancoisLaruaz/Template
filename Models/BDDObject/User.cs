﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Models.BDDObject
{
    public partial class User
    {
        public int Id { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ResetPasswordToken { get; set; }
        public string Description { get; set; }

        public string Password { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }

        public string Adress3 { get; set; }

        public int? CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public int LanguageId { get; set; }

        public bool? IsMasculine { get; set; }

        public string FacebookId { get; set; }
    }
}
