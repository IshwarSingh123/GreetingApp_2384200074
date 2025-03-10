﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int GreetingId { get; set; }

        [Required]
        public string GreetingMessage { get; set; }

        [Required]

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserEntity User { get; set; }


    }
}
