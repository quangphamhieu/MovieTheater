﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.CinemaRoom
{
    public class CinameRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CinemaName { get; set; }
        public int SeatRows { get; set; }
        public int SeatColumns { get; set; }
    }
}
