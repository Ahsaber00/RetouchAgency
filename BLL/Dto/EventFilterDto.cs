﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dto
{
    public class EventFilterDto
    {
        public string ? SearchTerm {  get; set; }
        public int? PageSize {  get; set; }
        public int? PageCount { get; set;}
    }
}
