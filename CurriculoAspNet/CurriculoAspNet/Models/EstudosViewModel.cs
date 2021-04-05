using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.Models
{
    public class EstudosViewModel : PadraoViewModel
    {
        public EstudosViewModel(string cpf)
        {
            CPF = cpf;
        }

        public EstudosViewModel()
        {
            
        }

        public int Id { get; set; }
        public string Curso { get; set; }
        public string Instituicao { get; set; }
    }
}
