using CurriculoAspNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.DAO
{
    public class EstudosDAO
    {
        private SqlParameter[] CriaParametros(EstudosViewModel estudo)
        {
            SqlParameter[] p = {
                new SqlParameter("id", estudo.Id),
                new SqlParameter("cpf", estudo.CPF),
                new SqlParameter("curso", estudo.Curso),
                new SqlParameter("instituicao", estudo.Instituicao),
            };

            return p;
        }

        public void Inserir(List<EstudosViewModel> estudos)
        {
            foreach(EstudosViewModel estudo in estudos)
            {
                if(estudo.Curso != null)
                {
                    estudo.Id = ProximoId();
                    string sql = "insert into Curso (id, cpf, curso, instituicao)" +
                    "values (@id, @cpf, @curso, @instituicao)";

                    HelperDAO.ExecutaSQL(sql, CriaParametros(estudo));
                }
                
            }
            
        }

        public void Alterar(List<EstudosViewModel> estudos)
        {
            foreach(EstudosViewModel estudo in estudos)
            {
                string sql = "update Curso set curso = @curso," +
               "instituicao = @instituicao " +
               "where id = @id";
                HelperDAO.ExecutaSQL(sql, CriaParametros(estudo));
            }
           
        }

        public void Excluir(int id)
        {
            string sql = "delete Curso where id = " + id;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public void Excluir(string cpf)
        {
            string sql = "delete Curso where cpf = " + cpf;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public EstudosViewModel Consulta(int id)
        {
            string sql = "select * from Curso where id = " + id;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public List<EstudosViewModel> Consulta(string cpf)
        {
            string sql = "select * from Curso where cpf = " + cpf;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<EstudosViewModel> retorno = new List<EstudosViewModel>();
            if (tabela.Rows.Count == 0)
                return null;
            else
            {
                foreach (DataRow registro in tabela.Rows)
                {
                    retorno.Add(MontaModel(registro));
                }

                return retorno;
            }
        }


        public List<EstudosViewModel> Lista()
        {
            string sql = "select * from Curso";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<EstudosViewModel> retorno = new List<EstudosViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;
        }


        public int ProximoId()
        {
            string sql = "select isnull(max(id) +1, 1) as 'MAIOR' from Curso";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }



        public static EstudosViewModel MontaModel(DataRow registro)
        {
            EstudosViewModel main = new EstudosViewModel(registro["cpf"].ToString());
            main.Id = Convert.ToInt32(registro["id"]);
            main.CPF = registro["cpf"].ToString();
            main.Curso = registro["curso"].ToString();
            main.Instituicao = registro["instituicao"].ToString();
            return main;
        }
    }
}
