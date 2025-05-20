using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexanderShemarov.Domain.Entities
{
    public class Trains
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Speed { get; set; }
        public decimal Cost { get; set; }
        public string? Image { get; set; }
        /// <summary> 
        /// Train Types (Passenger, Cargo, Retro, Special) 
        /// </summary>
        public int TrainTypesId { get; set; }
        public TrainTypes? TrainTypes { get; set; }
    }
}
