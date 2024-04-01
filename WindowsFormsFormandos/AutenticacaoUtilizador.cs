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
    public partial class AutenticacaoUtilizador : Form
    {
        DBConnect ligacao = new DBConnect();
        public AutenticacaoUtilizador()
        {
            InitializeComponent();
        }

        private void AutenticacaoUtilizador_Load(object sender, EventArgs e)
        {
            txtPassword.Text = "";
            txtUtilizador.Text = "";
            ControlBox = false;
            AcceptButton = btnEntrar;
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (ligacao.UserAtivo(txtUtilizador.Text))
            {

                if (ligacao.ValidateUser(txtUtilizador.Text, txtPassword.Text, ref Geral.id_user))
                {
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Erro na autenticação! Password incorreta.");
                }
            }
            else
            {
                MessageBox.Show("Utilizador bloqueado ou inativo. Contacte o departamento de TI.");
            }
        }
    }
}
