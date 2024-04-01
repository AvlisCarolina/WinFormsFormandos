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
    public partial class AtualizarFormandos : Form
    {
        DBConnect ligacao = new DBConnect();
        public AtualizarFormandos()
        {
            InitializeComponent();
        }

        private void AtualizarFormandos_Load(object sender, EventArgs e)
        {
            DesativarControlos();
            PesquisaNacionalidade();

            btnAtualizar.Enabled = false;

            this.AcceptButton = this.btnPesquisa;

        }

        private void DesativarControlos()
        {
            //groupBox1.Enabled = false;
            txtNome.ReadOnly = true;
            txtMorada.ReadOnly = true;
            txtContacto.ReadOnly = true;
            mTxtIban.ReadOnly = true;
            rbFeminino.Enabled = false;
            rbMasculino.Enabled = false;
            rbOutro.Enabled = false;
            mtxtDataNascimento.ReadOnly = true;
            dateTimePicker1.Visible = false;
            cbNacionalidade.Enabled = false;

        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {

            string nome = "", morada = "", contacto = "", iban = "", data_nascimento = "", nacionalidade = "", iso2 = "", id = "";
            char genero = ' ';

            if (ligacao.PesquisaFormando(nudID.Value.ToString(), ref nome, ref morada, ref contacto, ref iban, ref genero, ref data_nascimento, ref iso2, ref nacionalidade, ref id))
            {
                txtNome.Text = nome;
                txtMorada.Text = morada;
                txtContacto.Text = contacto;
                mTxtIban.Text = iban;
                cbNacionalidade.Text = id + " - " + iso2 + " - "+ nacionalidade;

                if (genero == 'F')
                {
                    rbFeminino.Checked = true;
                }
                else if (genero == 'M')
                {
                    rbMasculino.Checked = true;
                }
                else if (genero == 'O')
                {
                    rbOutro.Checked = true;
                }

                mtxtDataNascimento.Text = data_nascimento;

                groupBox3.Enabled = false;
                btnAtualizar.Enabled = true;

                txtNome.ReadOnly = false;
                txtMorada.ReadOnly = false;
                txtContacto.ReadOnly = false;
                mTxtIban.ReadOnly = false;
                rbFeminino.Enabled = true;
                rbMasculino.Enabled = true;
                rbOutro.Enabled = true;
                mtxtDataNascimento.ReadOnly = false;
                cbNacionalidade.Enabled = true;

            }
            else
            {
                MessageBox.Show("Formando não encontrado!");
                Limpar();
            }
        }

        public bool PesquisaNacionalidade()
        {
            cbNacionalidade.Items.Clear();
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
                        cbNacionalidade.Items.Add(texto);
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

        private void Limpar()
        {
            nudID.Value = 0;
            txtContacto.Text = string.Empty;
            txtMorada.Text = string.Empty;
            txtNome.Text = string.Empty;
            mTxtIban.Text = string.Empty;
            rbFeminino.Checked = false;
            rbMasculino.Checked = false;
            rbOutro.Checked = false;
            //dateTimePicker1.Value = DateTime.Now;
            mtxtDataNascimento.Clear();
            cbNacionalidade.Enabled = false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = true;
            btnAtualizar.Enabled = false;
            nudID.Focus();
            Limpar();
            DesativarControlos();

        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (VerificarCampos())
            {
                string[] texto2 = cbNacionalidade.Text.Split(new Char[] { '-' });
                string id_nacionalidade = texto2[0];

                if (ligacao.UpdateFormando(nudID.Value.ToString(), txtNome.Text, txtMorada.Text, txtContacto.Text, mTxtIban.Text, Genero(),
                    DateTime.Parse(mtxtDataNascimento.Text).ToString("yyyy-MM-dd"), id_nacionalidade))
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

        private bool VerificarCampos()
        {
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

            txtMorada.Text = Geral.TirarEspacos(txtMorada.Text);
            if (txtMorada.Text.Length < 3)
            {
                MessageBox.Show("Erro no campo Morada!");
                txtMorada.Focus();
                return false;
            }

            txtContacto.Text = Geral.TirarEspacos(txtContacto.Text);
            if (txtContacto.Text.Length < 3)
            {
                MessageBox.Show("Erro no campo Contacto!");
                txtContacto.Focus();
                return false;
            }

            if (mTxtIban.Text.Length > 17)
            {
                MessageBox.Show("Erro no campo IBAN");
                mTxtIban.Focus();
                return false;
            }

            if (Genero() == 'T')
            {
                MessageBox.Show("Erro no campo Género!");
                rbFeminino.Focus();
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

        private char Genero()
        {
            char genero = 'T';

            if (rbFeminino.Checked)
            {
                genero = 'F';
            }
            else if (rbMasculino.Checked)
            {
                genero = 'M';
            }
            else if (rbOutro.Checked)
            {
                genero = 'O';
            }
            return genero;
        }
    }
}
