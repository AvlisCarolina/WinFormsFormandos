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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace WindowsFormsFormandos
{
    public partial class AtualizarFormadores : Form
    {
        DBConnect ligacao = new DBConnect();
        public AtualizarFormadores()
        {
            InitializeComponent();
        }

        private void AtualizarFormadores_Load(object sender, EventArgs e)
        {
            gbDadosPessoais.Enabled = false;
            btnAtualizar.Enabled = false;
            nudID.Focus();

            this.AcceptButton = this.btnPesquisa;
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
    
            string nome = "", nif = "", id_user = "", user_name = "", password = "", user_role = "", data_nascimento = "", area = "", id_area ="";


            if (ligacao.PesquisaFormadores(nudID.Value.ToString(), ref nome, ref nif, ref id_user, ref user_name, ref password, ref user_role, ref data_nascimento, ref area, ref id_area))
            {

                txtNome.Text = nome;
                txtNIF.Text = nif;
                txtUserRole.Text = user_role;
                cbArea.Text = id_area + "-" + area;
                mtxtDataNascimento.Text = data_nascimento;


                gbDadosPessoais.Enabled = true;
                gbPesquisa.Enabled = false;
                btnAtualizar.Enabled = true;
                PesquisaArea();
            }
            else
            {
                MessageBox.Show("Formando não encontrado!");
                Limpar();
            }
        }

        private void Limpar()
        {
            nudID.Value = 0;
            txtNome.Text = string.Empty;
            txtNIF.Text = string.Empty;
            txtUserRole.Text = string.Empty;
            cbArea.Text = string.Empty;
            mtxtDataNascimento.Clear();
            gbDadosPessoais.Enabled = false;
            gbPesquisa.Enabled = true;
            btnAtualizar.Enabled = false;

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            nudID.Focus();
            Limpar();
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


            if (mtxtDataNascimento.Text.Length != 10 || Geral.CheckDate(mtxtDataNascimento.Text) == false)
            {
                MessageBox.Show("Erro no campo Data de Nascimento!");
                mtxtDataNascimento.Focus();
                return false;
            }

            return true;
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (VerificarCampos())
            {
                string[] texto2 = cbArea.Text.Split(new Char[] { '-' });
                string id_area = texto2[0];

                if (ligacao.UpdateFormador(nudID.Value.ToString(), txtNome.Text, txtNIF.Text, txtUserRole.Text,
                    DateTime.Parse(mtxtDataNascimento.Text).ToString("yyyy-MM-dd"), id_area))
                {
                    MessageBox.Show("Atualizado com sucesso!");
                    btnCancelar_Click(sender, e);

                }
                else
                {
                    MessageBox.Show("Erro na atualização do registo!");
                }


            }
        }
    }
}