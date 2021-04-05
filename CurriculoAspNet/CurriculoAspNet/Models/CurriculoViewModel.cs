using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.Models
{
    public class CurriculoViewModel: PadraoViewModel
    {
        public CurriculoViewModel()
        {
            empresas = new List<EmpresaViewModel>();
            main = new MainViewModel(CPF);
            enderecos = new EnderecoViewModel();
            estudos = new List<EstudosViewModel>();
            idiomas = new List<IdiomaViewModel>();
        }

        public MainViewModel main { get; set; }

        public List<EmpresaViewModel> empresas { get; set; }

        public EnderecoViewModel enderecos { get; set; }

        public List<EstudosViewModel> estudos { get; set; }

        public List<IdiomaViewModel> idiomas { get; set; }
    }
}
