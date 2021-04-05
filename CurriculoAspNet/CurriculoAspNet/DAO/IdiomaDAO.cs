using CurriculoAspNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.DAO
{
    public class IdiomaDAO
    {
        private SqlParameter[] CriaParametros(IdiomaViewModel idioma)
        {
            SqlParameter[] p = {
                new SqlParameter("id", idioma.Id),
                new SqlParameter("cpf", idioma.CPF),
                new SqlParameter("idioma", idioma.Idioma),
                new SqlParameter("habilidade", idioma.Habilidade),
            };

            return p;
        }

        public void Inserir(List<IdiomaViewModel> idiomas)
        {
            foreach(IdiomaViewModel idioma in idiomas)
            {
                if(idioma.Idioma != null)
                {
                    idioma.Id = ProximoId();
                    string sql = "insert into Idioma (id, cpf, idioma, habilidade)" +
                    "values (@id, @cpf, @idioma, @habilidade)";

                    HelperDAO.ExecutaSQL(sql, CriaParametros(idioma));
                }
                
            }
            
        }

        public void Alterar(List<IdiomaViewModel> idiomas)
        {
            foreach(IdiomaViewModel idioma in idiomas)
            {
                string sql = "update Idioma set idioma = @idioma, habilidade = @habilidade " +
                "where id = @id";
                HelperDAO.ExecutaSQL(sql, CriaParametros(idioma));
            }
            
        }

        public void Excluir(int id)
        {
            string sql = "delete Idioma where id = " + id;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public void Excluir(string cpf)
        {
            string sql = "delete Idioma where cpf = " + cpf;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public IdiomaViewModel Consulta(int id)
        {
            string sql = "select * from Idioma where id = " + id;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        public List<IdiomaViewModel> Consulta(string cpf)
        {
            string sql = "select * from Idioma where cpf = " + cpf;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<IdiomaViewModel> retorno = new List<IdiomaViewModel>();
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

        public List<IdiomaViewModel> Lista()
        {
            string sql = "select * from Idioma";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<IdiomaViewModel> retorno = new List<IdiomaViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;
        }


        public int ProximoId()
        {
            string sql = "select isnull(max(id) +1, 1) as 'MAIOR' from Idioma";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }



        public static IdiomaViewModel MontaModel(DataRow registro)
        {
            IdiomaViewModel main = new IdiomaViewModel(registro["cpf"].ToString());
            main.Id = Convert.ToInt32(registro["id"]);
            main.CPF = registro["cpf"].ToString();
            main.Idioma = registro["idioma"].ToString();
            main.Habilidade = Convert.ToInt32(registro["habilidade"]);
            return main;
        }
    }
}
