using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CurriculoAspNet.Models;
using CurriculoAspNet.DAO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CurriculoAspNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Sobre()
        {
            return View("Sobre");
        }

        public void Idioma(int idioma)
        {
            int x = idioma;
        }

        public IActionResult Print(string cpf)
        {
            CurriculoDAO dao = new CurriculoDAO();
            return View("Print", dao.Consulta(cpf,"p"));
        }

        public IActionResult Edit(string cpf)
        {
            try
            {
                //alterar
                ViewBag.Operacao = "A";
                PreparaListaEnderecosParaCombo();
                CurriculoDAO dao = new CurriculoDAO();
                return View("Form", dao.Consulta(cpf));
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult Salvar(CurriculoViewModel curriculo, string Operacao)
        {
            try
            {
                ValidaDados(curriculo, Operacao);
                if (ModelState.IsValid)
                {
                    CurriculoDAO dao = new CurriculoDAO();

                    //Preencher todos os CPFs para mantê-los iguais na hora de salvar no banco 
                    curriculo.main.CPF = curriculo.CPF;

                    if (Operacao == "I")
                        dao.Inserir(curriculo);
                    else
                        dao.Alterar(curriculo);
                    return RedirectToAction("index");
                }
                else
                {
                    ViewBag.Operacao = Operacao;
                    PreparaListaEnderecosParaCombo();
                    curriculo = Instancia(curriculo);
                    return View("Form", curriculo);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Delete(string cpf)
        {
            try
            {
                CurriculoDAO dao = new CurriculoDAO();
                dao.Deletar(cpf);
                return RedirectToAction("Index");
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult SalvarEndereco(CurriculoViewModel curriculo)
        {
            try
            {
                ValidaDadosEndereco(curriculo);
                if (ModelState.IsValid)
                {
                    CurriculoDAO dao = new CurriculoDAO();
                    dao.InserirEnd(curriculo);

                    PreparaListaEnderecosParaCombo();

                    curriculo = Instancia(curriculo);

                    //incluir
                    ViewBag.Operacao = "I";
                    return View("Form", curriculo);
                }
                else
                {
                    return View("FormAddress", curriculo);
                }
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        public IActionResult Form(CurriculoViewModel curriculo)
        {
            try
            {
                ViewBag.Operacao = "I";
                curriculo = Instancia(curriculo);
                return View("Form", curriculo);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult FormAddress(CurriculoViewModel curriculo)
        {
            try
            {
                //incluir
                ViewBag.Operacao = "I";
                return View("FormAddress", curriculo);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }
        }

        public IActionResult NovoCurriculo()
        {
            try
            {
                CurriculoViewModel curriculo = new CurriculoViewModel();
                PreparaListaEnderecosParaCombo();

                curriculo = Instancia(curriculo);

                //incluir
                ViewBag.Operacao = "I";
                return View("Form", curriculo);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.Message));
            }

        }

        public IActionResult List()
        {
            try
            {
                CurriculoDAO dao = new CurriculoDAO();
                List<CurriculoViewModel> lista = dao.Listagem();
                return View(lista);
            }
            catch (Exception erro)
            {
                return View("Error", new ErrorViewModel(erro.ToString()));
            }
        }

        #region aux
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        #region Métodos Auxiliares

        public CurriculoViewModel Instancia (CurriculoViewModel curriculo)
        {
            EstudosViewModel aux = new EstudosViewModel();
            curriculo.estudos.Add(aux);
            curriculo.estudos.Add(aux);
            curriculo.estudos.Add(aux);
            IdiomaViewModel idi = new IdiomaViewModel();
            curriculo.idiomas.Add(idi);
            curriculo.idiomas.Add(idi);
            curriculo.idiomas.Add(idi);
            EmpresaViewModel em = new EmpresaViewModel();
            curriculo.empresas.Add(em);
            curriculo.empresas.Add(em);
            curriculo.empresas.Add(em);
            return curriculo;
        }

        /// <summary>
        /// Realiza a validação de dados que estão sendo salvos em um curriculo
        /// </summary>
        /// <param name="Curriculo"></param>
        public void ValidaDados(CurriculoViewModel Curriculo, string operacao)
        {
            ModelState.Clear();
            MainDAO dao = new MainDAO();

            if (!ValidaCPF(Curriculo.CPF))
                ModelState.AddModelError("CPF", "O CPF não está em um formato válido");

            if(operacao == "I" && dao.Consulta(Curriculo.CPF) != null)
                ModelState.AddModelError("CPF", "O CPF já foi cadastrado");

            if (Curriculo.main.Telefone.Length < 0)
                ModelState.AddModelError("main.Telefone", "Preencha o campo Telefone");

            try
            {
                long x = Convert.ToInt64(Curriculo.main.Telefone);
            }
            catch
            {
                ModelState.AddModelError("main.Telefone", "Preencha o campo Telefone apenas com números");
            }

            if (string.IsNullOrEmpty(Curriculo.main.Email))
                ModelState.AddModelError("main.Email", "Preencha o campo Email");

            if (Curriculo.main.Email.IndexOf('@') == -1)
                ModelState.AddModelError("main.Email", "Preencha o campo com um email");

            if (string.IsNullOrEmpty(Curriculo.main.CargoPretendido))
                ModelState.AddModelError("main.CargoPretendido", "Preencha o campo Cargo Pretendido");

            if (string.IsNullOrEmpty(Curriculo.idiomas[0].Idioma))
                ModelState.AddModelError("idiomas.Idioma", "Preencha o campo Idioma");

            if (Curriculo.idiomas[0].Habilidade < 0)
                ModelState.AddModelError("idiomas.Habilidade", "Preencha o campo Habilidade");

            if (string.IsNullOrEmpty(Curriculo.empresas[0].Empresa))
                ModelState.AddModelError("empresas.Empresa", "Preencha o campo Empresa");

            if (string.IsNullOrEmpty(Curriculo.empresas[0].Cargo))
                ModelState.AddModelError("empresas.Cargo", "Preencha o campo Cargo");

            if (string.IsNullOrEmpty(Curriculo.estudos[0].Curso))
                ModelState.AddModelError("estudos.Curso", "Preencha o campo Curso");

            if (string.IsNullOrEmpty(Curriculo.estudos[0].Instituicao))
                ModelState.AddModelError("estudos.Instituicao", "Preencha o campo Instituição");
        }

        /// <summary>
        /// Realiza a validação de dados que estão sendo salvos em um endereço
        /// </summary>
        /// <param name="Curriculo"></param>
        private void ValidaDadosEndereco(CurriculoViewModel Curriculo)
        {
            ModelState.Clear();
            EnderecoDAO dao = new EnderecoDAO();

            if (Curriculo.main.endereco.CEP.ToString().Length <= 6)
                ModelState.AddModelError("main.endereco.CEP", "O CEP não esta em um formato válido");

            if (dao.Consulta(Curriculo.main.endereco.CEP.ToString()) != null)
                ModelState.AddModelError("main.endereco.CEP", "O CEP já foi cadastrado");

            if (string.IsNullOrEmpty(Curriculo.main.endereco.State))
                ModelState.AddModelError("main.endereco.State", "Preencha o campo Estado");

            if (string.IsNullOrEmpty(Curriculo.main.endereco.City))
                ModelState.AddModelError("main.endereco.City", "Preencha o campo Cidade");

            if (string.IsNullOrEmpty(Curriculo.main.endereco.District))
                ModelState.AddModelError("main.endereco.District", "Preencha o campo do Bairro");

            if (string.IsNullOrEmpty(Curriculo.main.endereco.Street))
                ModelState.AddModelError("main.endereco.Street", "Preencha o campo Rua");

            if (Curriculo.main.endereco.Number < 0)
                ModelState.AddModelError("main.endereco.Number", "Preencha o campo Número");
        }

        /// <summary>
        /// Preenche os CEPs existentes na tela
        /// </summary>
        private void PreparaListaEnderecosParaCombo()
        {
            CurriculoDAO dao = new CurriculoDAO();
            var enderecos = dao.ListaCEPs();
            List<SelectListItem> listaEnderecos = new List<SelectListItem>();
            listaEnderecos.Add(new SelectListItem("Selecione um CEP...", "0"));

            foreach (var endereco in enderecos)
            {
                SelectListItem item = new SelectListItem(endereco.CEP.ToString(), endereco.CEP.ToString());
                listaEnderecos.Add(item);
            }
            ViewBag.Enderecos = listaEnderecos;
        }

        /// <summary>
        /// Método de validação de CPF
        /// </summary>
        /// <param name="vrCPF"></param>
        /// <returns></returns>
        public static bool ValidaCPF(string vrCPF)
        {
            string valor = vrCPF.Replace(".", "");

            valor = valor.Replace("-", "");

            if (valor.Length != 11)
                return false;

            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)
                if (valor[i] != valor[0])
                    igual = false;

            if (igual || valor == "12345678909")
                return false;

            int[] numeros = new int[11];

            for (int i = 0; i < 11; i++)
                numeros[i] = int.Parse(
                  valor[i].ToString());

            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else if (numeros[10] != 11 - resultado)
                return false;

            return true;

        }

        /// <summary>
        /// Retorna o endereço selecionado pra tela
        /// </summary>
        /// <param name="cep"></param>
        /// <returns></returns>
        public JsonResult RetornaEndereco (string cep)
        {
            EnderecoDAO dao = new EnderecoDAO();
            var enderecos = dao.Consulta(cep);

            var resultado = new
            {
                sucesso = true,
                CEP = enderecos.CEP,
                State = enderecos.State,
                City = enderecos.City,
                District = enderecos.District,
                Number = enderecos.Number,
                Street = enderecos.Street

            };
            return Json(resultado);
        }
        #endregion
    }
}
