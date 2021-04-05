using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.Models
{
    public class EnderecoViewModel
    {
        public int CEP { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public int Number { get; set; }
        public string Street { get; set; }
    }
}
