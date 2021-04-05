using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.Models
{
    public class IdiomaViewModel : PadraoViewModel
    {
        public IdiomaViewModel(string cpf)
        {
            CPF = cpf;
        }
        public IdiomaViewModel()
        {

        }
        public int Id { get; set; }
        public string Idioma { get; set; }
        public int Habilidade { get; set; }
    }
}
