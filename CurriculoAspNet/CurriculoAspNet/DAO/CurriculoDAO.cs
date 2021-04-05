using CurriculoAspNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.DAO
{
    public class CurriculoDAO
    {
        EmpresaDAO empresaDAO = new EmpresaDAO();
        EnderecoDAO enderecoDAO = new EnderecoDAO();
        EstudosDAO estudosDAO = new EstudosDAO();
        IdiomaDAO idiomaDAO = new IdiomaDAO();
        MainDAO mainDAO = new MainDAO();

        public CurriculoDAO()
        {
            
        }

        public List<EnderecoViewModel> ListaCEPs()
        {
            try
            {
                List<EnderecoViewModel> lista = new List<EnderecoViewModel>();
                DataTable tabela = HelperDAO.ExecutaSelect("select * from Endereco order by cep", null);
                foreach (DataRow registro in tabela.Rows)
                    lista.Add(MontaEndereco(registro));
                return lista;
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }
            
        }

        private EnderecoViewModel MontaEndereco(DataRow registro)
        {
            EnderecoViewModel c = new EnderecoViewModel()
            {
                CEP = Convert.ToInt32(registro["cep"]),
                Street = registro["rua"].ToString(),
                District = registro["bairro"].ToString(),
                City = registro["cidade"].ToString(),
                State = registro["estado"].ToString(),
                Number = Convert.ToInt32(registro["numero"])
            };
            return c;
        }

        public List<CurriculoViewModel> Listagem()
        {
            return mainDAO.ListaPrincipal(); 
        }

        public void Inserir(CurriculoViewModel curriculo)
        {
            PreencheId(curriculo, curriculo.CPF);
            mainDAO.Inserir(curriculo.main);
            idiomaDAO.Inserir(curriculo.idiomas);
            estudosDAO.Inserir(curriculo.estudos);
            empresaDAO.Inserir(curriculo.empresas);
        }

        public void PreencheId(CurriculoViewModel curriculo, string cpf)
        {
            foreach(IdiomaViewModel idioma in curriculo.idiomas)
            {
                
                idioma.CPF = cpf;

            }

            foreach(EmpresaViewModel empresa in curriculo.empresas)
            {
                
                empresa.CPF = cpf;
            }

            foreach (EstudosViewModel estudo in curriculo.estudos)
            {
                
                estudo.CPF = cpf;
            }
        }

        public void Alterar(CurriculoViewModel curriculo)
        {
            PreencheId(curriculo, curriculo.CPF);
            mainDAO.Alterar(curriculo.main);
            idiomaDAO.Alterar(curriculo.idiomas);
            estudosDAO.Alterar(curriculo.estudos);
            empresaDAO.Alterar(curriculo.empresas);
        }

        public void Deletar(string cpf)
        {
            idiomaDAO.Excluir(cpf);
            estudosDAO.Excluir(cpf);
            empresaDAO.Excluir(cpf);
            mainDAO.Excluir(cpf);
        }

        public void InserirEnd(CurriculoViewModel curriculo)
        {
            enderecoDAO.Inserir(curriculo.main.endereco);
        }

        public CurriculoViewModel Consulta (string cpf)
        {
            CurriculoViewModel curriculo = new CurriculoViewModel();
            curriculo.main = mainDAO.Consulta(cpf);
            curriculo.main.endereco = enderecoDAO.Consulta(curriculo.main.endereco.CEP.ToString());

            curriculo.empresas = empresaDAO.Consulta(cpf);
            curriculo.idiomas = idiomaDAO.Consulta(cpf);
            curriculo.estudos = estudosDAO.Consulta(cpf);

            if(curriculo.empresas.Count < 3)
            {
                do
                {
                    EmpresaViewModel em = new EmpresaViewModel();
                    curriculo.empresas.Add(em);
                } while (curriculo.empresas.Count < 3);
            }

            if (curriculo.estudos.Count < 3)
            {
                do
                {
                    EstudosViewModel em = new EstudosViewModel();
                    curriculo.estudos.Add(em);
                } while (curriculo.estudos.Count < 3);
            }

            if (curriculo.idiomas.Count < 3)
            {
                do
                {
                    IdiomaViewModel em = new IdiomaViewModel();
                    curriculo.idiomas.Add(em);
                } while (curriculo.idiomas.Count < 3);
            }

            return curriculo;
        }

        public CurriculoViewModel Consulta(string cpf,string p)
        {
            CurriculoViewModel curriculo = new CurriculoViewModel();
            curriculo.main = mainDAO.Consulta(cpf);
            curriculo.main.endereco = enderecoDAO.Consulta(curriculo.main.endereco.CEP.ToString());

            curriculo.empresas = empresaDAO.Consulta(cpf);
            curriculo.idiomas = idiomaDAO.Consulta(cpf);
            curriculo.estudos = estudosDAO.Consulta(cpf);

            return curriculo;
        }

        public List<int> ProximoId()
        {
            List<int> ids = new List<int>();
            int a = empresaDAO.ProximoId();
            int b = estudosDAO.ProximoId();
            int c = idiomaDAO.ProximoId();

            ids.Add(a);
            ids.Add(b);
            ids.Add(c);

            return ids;
        }
    }
}
