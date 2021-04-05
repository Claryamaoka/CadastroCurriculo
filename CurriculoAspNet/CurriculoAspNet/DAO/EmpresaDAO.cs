using CurriculoAspNet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CurriculoAspNet.DAO
{
    public class EmpresaDAO
    {
        private SqlParameter[] CriaParametros(EmpresaViewModel empresa)
        {
            SqlParameter[] p = {
                new SqlParameter("id", empresa.Id),
                new SqlParameter("cpf", empresa.CPF),
                new SqlParameter("empresa", empresa.Empresa),
                new SqlParameter("cargo", empresa.Cargo),
            };

            return p;
        }

        public void Inserir(List<EmpresaViewModel> empresas)
        {
            foreach(EmpresaViewModel empresa in empresas)
            {
                if(empresa.Empresa != null)
                {
                    empresa.Id = ProximoId();
                    string sql = "insert into Profissional (id, cpf, cargo, empresa)" +
                    "values (@id, @cpf, @cargo, @empresa)";

                    HelperDAO.ExecutaSQL(sql, CriaParametros(empresa));
                }
                
            }
            
        }

        public void Alterar(List<EmpresaViewModel> empresas)
        {
            foreach(EmpresaViewModel empresa in empresas)
            {
                string sql = "update Profissional set cargo = @cargo," +
                "empresa = @empresa" +
                " where id = @id";
                HelperDAO.ExecutaSQL(sql, CriaParametros(empresa));
            }
            
        }

        public void Excluir(int id)
        {
            string sql = "delete Profissional where id = " + id;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public void Excluir(string cpf)
        {
            string sql = "delete Profissional where cpf = " + cpf;
            HelperDAO.ExecutaSQL(sql, null);
        }

        public EmpresaViewModel Consulta(int id)
        {
            string sql = "select * from Profissional where id = " + id;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            if (tabela.Rows.Count == 0)
                return null;
            else
                return MontaModel(tabela.Rows[0]);
        }

        //retornar uma lista de Empresa
        public List<EmpresaViewModel> Consulta(string cpf)
        {
            string sql = "select * from Profissional where cpf = " + cpf;
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<EmpresaViewModel> retorno = new List<EmpresaViewModel>();
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


        public List<EmpresaViewModel> Lista()
        {
            string sql = "select * from Profissional";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            List<EmpresaViewModel> retorno = new List<EmpresaViewModel>();

            foreach (DataRow registro in tabela.Rows)
            {
                retorno.Add(MontaModel(registro));
            }

            return retorno;
        }


        public int ProximoId()
        {
            string sql = "select isnull(max(id) +1, 1) as 'MAIOR' from Profissional";
            DataTable tabela = HelperDAO.ExecutaSelect(sql, null);
            return Convert.ToInt32(tabela.Rows[0]["MAIOR"]);
        }



        public static EmpresaViewModel MontaModel(DataRow registro)
        {
            EmpresaViewModel main = new EmpresaViewModel(registro["cpf"].ToString());
            main.Id = Convert.ToInt32(registro["id"]);
            main.CPF = registro["cpf"].ToString();
            main.Empresa = registro["empresa"].ToString();
            main.Cargo = registro["cargo"].ToString();
            return main;
        }
    }
}
