using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.Models
{
    public class MainViewModel : PadraoViewModel
    {
        public MainViewModel(string cpf)
        {
            CPF = cpf;
            endereco = new EnderecoViewModel();
        }
        public string Telefone { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string CargoPretendido { get; set; }
        public EnderecoViewModel endereco { get; set; }
    }
}
