using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsBDGestaoFormandos;

namespace WindowsFormsFormandos
{
    public partial class EliminarNacionalidade : Form
    {
        DBConnect ligacao = new DBConnect();
        public EliminarNacionalidade()
        {
            InitializeComponent();
        }

        private void EliminarNacionalidade_Load(object sender, EventArgs e) 
        {
            //chama o método PesquisaCampos para preencher o comboBox1 com os dados
            //das nacionalidades existentes e define os campos de entrada como somente leitura.
            
            PesquisaCampos();
            comboBox1.Focus();
            txtISO2.ReadOnly = true;
            txtNacionalidade.ReadOnly = true;
            btnEliminar.Enabled = false;

        }

        public bool PesquisaCampos() // explicação no form AtualizarNacionalidade
        {
            comboBox1.Items.Clear();
            string texto;
            string id, iso, nacionalidade;
            bool flag = false;
            string query = "Select * from Nacionalidade;";

            try
            {
                if (ligacao.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, ligacao.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        id = dataReader[0].ToString();
                        iso = dataReader[1].ToString();
                        nacionalidade = dataReader[2].ToString();
                        flag = true;
                        texto = id + "-" + iso + "-" + nacionalidade;
                        comboBox1.Items.Add(texto);
                    }
                    dataReader.Close();

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                ligacao.CloseConnection();
            }
            return flag;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) 
        {
            //atualiza os campos de entrada com os valores da nacionalidade selecionada
            //e habilita o botão "Eliminar".

            string[] texto2 = comboBox1.Text.Split(new Char[] { '-' });
            //string id_nacionalidade = texto2[0];
            string iso2 = texto2[1];
            string nacionalidade = texto2[2];

            txtISO2.Text = iso2;
            txtNacionalidade.Text = nacionalidade;

            btnEliminar.Enabled = true;
        }

        private void Limpar()
        {
            comboBox1.Text = string.Empty;
            txtISO2.Text = string.Empty;
            txtNacionalidade.Text = string.Empty;
            btnEliminar.Enabled = false;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            comboBox1.Focus();
            Limpar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // confirmar com o utilizador se quer apagar os dados selecionados (y/n)
            //chama o método DeleteNacionalidade da classe DBConnect para eliminar a nacionalidade do banco de dados.

            var confirmacao = MessageBox.Show("Confirma que deseja apagar a nacionalidade selecionado?", "", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                if (ligacao.DeleteNacionalidade(txtNacionalidade.Text)) // explicação do método delete nacionalidade na classe DBConnect
                {
                    MessageBox.Show("Nacionalidade apagada com sucesso.");
                    groupBox3.Enabled = true;
                    btnEliminar.Enabled = false;
                    comboBox1.Focus();
                    Limpar();
                    PesquisaCampos();
                }
                else
                {
                    MessageBox.Show("Erro ao apagar nacionalidade.");
                }

                /*
                Este método da classe DBConnect é responsável por executar a consulta SQL para eliminar a nacionalidade do banco de dados. 
                Retorna true se a eliminação for bem-sucedida e false caso contrário.
                 */
            }
        }
    }
}
