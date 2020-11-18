using System;
using System.ComponentModel.DataAnnotations;
using WebLayer.Validators;

namespace WebLayer.Viewmodels
{
    public class EventFormModel
    {
        public string Name { get; set; }

        [Range(1, Double.MaxValue)]
        public double? OddsForFirstTeam { get; set; }

        [Range(1, Double.MaxValue)]
        public double? OddsForDraw { get; set; }

        [Range(1, Double.MaxValue)]
        public double? OddsForSecondTeam { get; set; }
        
        [DateInFuture]
        public DateTime StartDate { get; set; }
    }
}
