using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class CreateOrEditJwCustomerDto 
    {
        public int? Id { get; set; }
        [Required]
        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string Contact { get; set; }

        public string Telephone { get; set; }

    }
}