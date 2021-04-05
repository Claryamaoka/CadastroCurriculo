using CurriculoAspNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.DAO
{
    public class MainDAO
    {
        private SqlParameter[] CriaParametros(MainViewModel main)
        {
            SqlParameter[] p = {
                new SqlParameter("nome", main.Nome ),
                new SqlParameter("cpf", main.CPF),
                new SqlParameter("email", main.Email),
                new SqlParameter("telefone", main.Telefone),
                new SqlParameter("cargo", main.CargoPretendido),
                new SqlParameter("cep", main.endereco.CEP),
            };

            return p;
        }

        public void Inserir(MainViewModel curriculo)
        {
            string sql = "insert into Pessoal (nome, cpf, email, cargo, telefone, cep) " +
                "values (@nome, @cpf, @email, @cargo, @telefone, @cep)";

            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo));
        }

        public void Alterar(MainViewModel curriculo)
        {
            string sql = "update Pessoal set nome = @nome, " +
                "email = @email, cargo = @cargo, telefone = @telefone," +
                "cep = @cep where cpf = @cpf";
            HelperDAO.ExecutaSQL(sql, CriaParametros(curriculo));
        }

        public void Excluir(string cpf)
        {
            string sql = "delete Pessoal where cpf = " + cpf;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public MainViewModel Consulta(string cpf)
        {
            string sql = "select * from Pessoal where cpf = " + cpf;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }


        public List<MainViewModel> Lista()
        {
            string sql = "select * from Pessoal";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<MainViewModel> retorno = new List<MainViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;
        }

        public List<CurriculoViewModel> ListaPrincipal()
        {
            string sql = "select p.nome, p.cpf, p.cep, e.rua from Pessoal p inner join Endereco e on e.cep = p.cep";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<CurriculoViewModel> retorno = new List<CurriculoViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModelLista(registro));
            }

            return retorno;
        }


        public int ProximoId()
        {
            string sql = "select isnull(max(id) +1, 1) as 'MAIOR' from Pessoal";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }



        public static MainViewModel MontaModel(DataRow registro)
        {
            MainViewModel main = new MainViewModel(registro["cpf"].ToString());
            main.CPF = registro["cpf"].ToString();
            main.Nome = registro["nome"].ToString();
            main.Telefone = (registro["telefone"]).ToString();
            main.Email = registro["email"].ToString();
            main.CargoPretendido = registro["cargo"].ToString();
            main.endereco.CEP = Convert.ToInt32( registro["cep"].ToString());
            return main;
        }

        public static CurriculoViewModel MontaModelLista(DataRow registro)
        {
            try
            {
                CurriculoViewModel main = new CurriculoViewModel();
                main.CPF = registro["cpf"].ToString();
                main.main.Nome = registro["nome"].ToString();
                main.main.endereco.Street = registro["rua"].ToString();
                main.main.endereco.CEP = Convert.ToInt32(registro["cep"]);
                
                return main;
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }
            
        }
    }
}
