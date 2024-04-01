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
    public partial class InserirArea : Form
    {
        DBConnect ligacao = new DBConnect();
        public InserirArea()
        {
            InitializeComponent();
        }

        private void InserirArea_Load(object sender, EventArgs e)
        {
            txtID.Text = ligacao.DevolveUltimoIDArea().ToString();
            txtID.Enabled = false;
            txtArea.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
 
            if (VerificarCampos())
            {
                if (ligacao.InsertArea( txtArea.Text))
                {
                    MessageBox.Show("Inserido com Sucesso");
                    Limpar();
                    txtArea.Focus();

                }
                else
                {
                    MessageBox.Show("Erro ao Inserir");
                }
            }
        }

        private bool VerificarCampos()
        {

            if (txtArea.Text.Length == 0)
            {
                MessageBox.Show("Erro no preenchimento dos campos!");
                txtArea.Focus();
                return false;
            }

            return true;
        }

        private void Limpar()
        {
            // Este método limpa todos os campos do formulário.

            txtID.Text = ligacao.DevolveUltimoIDArea().ToString();
            txtArea.Text = string.Empty;
        }
    }
}
