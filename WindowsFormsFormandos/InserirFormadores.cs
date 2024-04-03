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
    public partial class InserirFormadores : Form
    {
        DBConnect ligacao = new DBConnect();
        public InserirFormadores()
        {
            InitializeComponent();
        }

        private void InserirFormadores_Load(object sender, EventArgs e)
        {
            nudID.Value = ligacao.DevolveUltimoIDFormadores();
            txtIdUser.Text = ligacao.DevolveUltimoIDUtilizador().ToString();
            nudID.Enabled = false;
            txtIdUser.Enabled = false;

            PesquisaArea();

        }

        public bool PesquisaArea()
        {
            // Este método  é responsável por preencher o ComboBox de area com os dados recuperados do banco de dados.

            cbArea.Items.Clear(); // Limpa combobox para evitar duplicação de dados
            string texto;
            string id_area, area;
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
                        id_area = dataReader[0].ToString();                      
                        area = dataReader[1].ToString();
                        flag = true;
                        texto = id_area + "-" + area;
                        cbArea.Items.Add(texto);
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

        private void btnGravar_Click(object sender, EventArgs e)
        {
            /*
            Este evento verifica se os campos estão preenchidos corretamente e, se estiverem, 
            insere os dados do formando no banco de dados usando o método InsertFormador da classe DBConnect.
            */

            if (VerificarCampos()) // só executa este bloco se todos os campos estiverem devidamente preenchidos
            {
                string[] texto2 = cbArea.Text.Split(new Char[] { '-' }); //divide o texto selecionado no ComboBox de area em três partes 
                string id_area = texto2[0]; //Extrai o ID da area do array texto2.

                if (ligacao.InsertFormador(nudID.Value.ToString(), txtNome.Text, txtNIF.Text, txtIdUser.Text, txtUserName.Text, txtPassword.Text, txtUserRole.Text, DateTime.Parse(mtxtDataNascimento.Text).ToString("yyyy-MM-dd"), id_area)) //Chama o método InsertFormador
                {
                    MessageBox.Show("Inserido com Sucesso");
                    Limpar();
                    txtNome.Focus();
                }
                else
                {
                    MessageBox.Show("Erro ao Inserir");
                }
            }
        }

        private bool VerificarCampos()
        {
            /*
            Este método  é responsável por validar se os campos do formulário estão preenchidos corretamente
            antes de inserir os dados do formando no banco de dados, tendo em consideração parametros
            específicos para cada campo, com base na sua predifinição na base de dados.

            Este método recorre a um outro método definido na classe Geral (encontra-se no DBConnect)
            */

            if (nudID.Value == 0)
            {
                MessageBox.Show("Erro no campo ID!");
                nudID.Focus();
                return false;
            }

            txtNome.Text = Geral.TirarEspacos(txtNome.Text);
            if (txtNome.Text.Length < 3)
            {
                MessageBox.Show("Erro no campo Nome!");
                txtNome.Focus();
                return false;
            }

            txtNIF.Text = Geral.TirarEspacos(txtNIF.Text);
            if (txtNIF.Text.Length < 3)
            {
                MessageBox.Show("Erro no campo NIF!");
                txtNIF.Focus();
                return false;
            }

            txtIdUser.Text = Geral.TirarEspacos(txtIdUser.Text);
            if (txtIdUser.Text.Length == 0)
            {
                MessageBox.Show("Erro no campo ID Utilizador!");
                txtIdUser.Focus();
                return false;
            }


            if (mtxtDataNascimento.Text.Length != 10 || Geral.CheckDate(mtxtDataNascimento.Text) == false)
            {
                MessageBox.Show("Erro no campo Data de Nascimento!");
                mtxtDataNascimento.Focus();
                return false;
            }

            return true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //Atualiza o campo de texto mtxtDataNascimento quando o valor do DateTimePicker é alterado.

            mtxtDataNascimento.Text = dateTimePicker1.Value.ToShortDateString();
        }

        private void mtxtDataNascimento_TextChanged(object sender, EventArgs e)
        {
            //Converte a data inserida no campo mtxtDataNascimento em um DateTime e define o valor do DateTimePicker.

            int dia, mes, ano;
            string textoData;
            DateTime data;

            if (mtxtDataNascimento.MaskCompleted == true)
            {
                textoData = mtxtDataNascimento.Text;
                dia = int.Parse(textoData.Substring(0, 2));
                mes = int.Parse(textoData.Substring(3, 2));
                ano = int.Parse(textoData.Substring(6));

                try
                {
                    data = DateTime.Parse(dia + "-" + mes + "-" + ano);
                    dateTimePicker1.Value = data;
                }

                catch
                {
                    MessageBox.Show("Data incorreta!", "Erro!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mtxtDataNascimento.Focus();
                }
            }
        }
        private void Limpar()
        {
            //Limpa todos os campos do formulário e redefine o valor do campo nudID com o último ID disponível no banco de dados.

            nudID.Value = ligacao.DevolveUltimoIDFormadores();
            txtIdUser.Text = ligacao.DevolveUltimoIDUtilizador().ToString();
            txtNIF.Text = string.Empty;
            txtNome.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtUserRole.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            mtxtDataNascimento.Clear();
            cbArea.Text = string.Empty;
            cbArea.Items.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpar();
            PesquisaArea();

            //Limpa o formulário e chama o método PesquisaNacionalidade
        }
    }
}
