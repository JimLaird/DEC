﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEC.Shared.Models
{
    public class FormResult
    {
        public bool Succeeded { get; set; }
        public string[] ErrorList { get; set; } = [];
    }
}
