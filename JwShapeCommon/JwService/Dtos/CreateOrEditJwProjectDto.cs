using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class CreateOrEditJwProjectDto
    {
        public long? Id { get; set; }
        public string ProjectName { get; set; }

        public string CustomerName { get; set; }

        public decimal ProjectCost { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? DateCompleted { get; set; }

        public string PlaceofDelivery { get; set; }

        public int? BeamsNumber { get; set; }

        public string CreateDesigner { get; set; }

        public int? JwCustomerId { get; set; }

    }
}