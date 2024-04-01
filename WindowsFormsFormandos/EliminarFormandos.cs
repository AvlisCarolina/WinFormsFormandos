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
    public partial class EliminarFormandos : Form
    {
        DBConnect ligacao = new DBConnect();
        public EliminarFormandos()
        {
            InitializeComponent();
        }

        private void ApagarFormandos_Load(object sender, EventArgs e)
        {

            txtNome.ReadOnly = true;
            txtMorada.ReadOnly = true;
            txtContacto.ReadOnly = true;
            mTxtIban.ReadOnly = true;
            rbFeminino.Enabled = false;
            rbMasculino.Enabled = false;
            rbOutro.Enabled = false;
            mtxtDataNascimento.ReadOnly = true;
            txtNacionalidade.Enabled = false;


            btnEliminar.Enabled = false;

            this.AcceptButton = this.btnPesquisa;

        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            
            string nome = "", morada = "", contacto = "", iban = "", data_nascimento = "", nacionalidade = "", iso2 = "", id ="";
            char genero = ' ';

            if (ligacao.PesquisaFormando(nudID.Value.ToString(), ref nome, ref morada, ref contacto, ref iban, ref genero, ref data_nascimento,ref iso2, ref nacionalidade, ref id))
            {
                txtNome.Text = nome;
                txtMorada.Text = morada;
                txtContacto.Text = contacto;
                mTxtIban.Text = iban;
                txtNacionalidade.Text = id + " - " + iso2 + " - " + nacionalidade;

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
                btnEliminar.Enabled = true;
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
            txtContacto.Text = string.Empty;
            txtMorada.Text = string.Empty;
            txtNome.Text = string.Empty;
            mTxtIban.Text = string.Empty;
            rbFeminino.Checked = false;
            rbMasculino.Checked = false;
            rbOutro.Checked = false;
            txtNacionalidade.Text = string.Empty;
            mtxtDataNascimento.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            groupBox3.Enabled = true;
            btnEliminar.Enabled = false;
            nudID.Focus();
            Limpar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // confirmar com o utilizador se quer apagar os dados selecionados (y/n)
            var confirmacao = MessageBox.Show("Confirma que deseja apagar o formando selecionado?", "", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                if (ligacao.DeleteFormando(nudID.Value.ToString())) // eliminar dados e mostrar mensagem de sucesso ou de erro
                {
                    MessageBox.Show("Formando apagado com sucesso.");
                    groupBox3.Enabled = true;
                    btnEliminar.Enabled = false;
                    nudID.Focus();
                    Limpar();
                }
                else
                {
                    MessageBox.Show("Erro ao apagar formando.");
                }
            }
        }
    }
}
