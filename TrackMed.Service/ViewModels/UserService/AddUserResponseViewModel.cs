﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMed.Service.ViewModels
{
    public class AddUserResponseViewModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
