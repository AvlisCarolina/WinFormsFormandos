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
    public partial class AtualizarArea : Form
    {
        DBConnect ligacao = new DBConnect();
        public AtualizarArea()
        {
            InitializeComponent();
        }

        private void AtualizarArea_Load(object sender, EventArgs e)
        {
            PesquisaCamposArea();
            Limpar();
        }

        public bool PesquisaCamposArea()
        {
            comboBox1.Items.Clear();
            string texto;
            string id, area;
            bool flag = false;
            string query = "Select * from area;";

            try
            {
                if (ligacao.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, ligacao.connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        id = dataReader[0].ToString();
                        area = dataReader[1].ToString();
                        flag = true;
                        texto = id +"-"+ area;
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
            string[] texto2 = comboBox1.Text.Split(new Char[] { '-' }); // Separa o texto presente na comboBox1 numa array (lista) com 2 índices.

            string area = texto2[1];

            txtArea.Text = area;

            txtArea.ReadOnly = false;
            btnAtualizar.Enabled = true; 

        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show("Confirma que deseja atualizar a area profissional selecionada?", "", MessageBoxButtons.YesNo); // MessageBox de YesNo
            string[] texto2 = comboBox1.Text.Split(new Char[] { '-' });
            string id_area = texto2[0];


            if (confirmacao == DialogResult.Yes)
            {

                if (ligacao.UpdateArea(id_area,txtArea.Text))
                {
                    MessageBox.Show("Área profissional atualizada com sucesso.");
                    groupBox3.Enabled = true;
                    Limpar();
                    PesquisaCamposArea();

                }
                else
                {
                    MessageBox.Show("Erro ao apagar área.");
                }
            }
        }

        private void Limpar()
        {
            comboBox1.Focus();
            comboBox1.Text = string.Empty;
            txtArea.Text = string.Empty;
            txtArea.ReadOnly = true;
            btnAtualizar.Enabled = false;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            PesquisaCamposArea();
            Limpar();
        }
    }
}
