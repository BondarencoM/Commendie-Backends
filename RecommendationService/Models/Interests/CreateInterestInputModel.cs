﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Interests
{
    public class CreateInterestInputModel
    {
        [Required]
        public string WikiId { get; set; }
    }
}
