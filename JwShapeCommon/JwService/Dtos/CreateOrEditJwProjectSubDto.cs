﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RGB.Jw.JW.Dtos
{
    public class CreateOrEditJwProjectSubDto
    {
        public long? Id { get; set; }
        public string SubName { get; set; }

        public bool IsCreated { get; set; }

        public string Jwctemp { get; set; }

        public DateTime ParseTime { get; set; }

        public int? BeamCount { get; set; }

        public string JwctempPath { get; set; }

        public long? JwProjectId { get; set; }

    }
}