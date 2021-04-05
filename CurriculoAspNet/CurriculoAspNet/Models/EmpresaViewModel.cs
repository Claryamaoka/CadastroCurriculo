using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.Models
{
    public class EmpresaViewModel : PadraoViewModel
    {
        public EmpresaViewModel(string cpf)
        {
            CPF = cpf;
        }

        public EmpresaViewModel()
        {

        }


        public int Id { get; set; }
        public string Empresa { get; set; }
        public string Cargo { get; set; }
    }
}
