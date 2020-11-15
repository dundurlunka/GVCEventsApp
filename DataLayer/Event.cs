using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? OddsForFirstTeam { get; set; }

        public double? OddsForDraw { get; set; }

        public double? OddsForSecondTeam { get; set; }

        public DateTime StartDate { get; set; }
    }
}
