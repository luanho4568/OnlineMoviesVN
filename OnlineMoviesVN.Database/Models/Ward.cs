﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineMoviesVN.Database.Models
{
    public class Ward
    {
        [Key]
        public string Code { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }
        public string? FullName { get; set; }
        public string? FullNameEn { get; set; }
        public string? CodeName { get; set; }

        [ForeignKey(nameof(District))]
        public string? DistrictCode { get; set; }
        public District? District { get; set; }

        [ForeignKey(nameof(AdministrativeUnit))]
        public int? AdministrativeUnitId { get; set; }
        public AdministrativeUnit? AdministrativeUnit { get; set; }
    }
}