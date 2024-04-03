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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsFormandos
{
    public partial class EliminarFormador : Form
    {
        DBConnect ligacao = new DBConnect();
        public EliminarFormador()
        {
            InitializeComponent();
        }

        private void EliminarFormador_Load(object sender, EventArgs e)
        {
            gbDadosPessoais.Enabled = false;
            gbDadosAcesso.Enabled = false;
            btnEliminar.Enabled = false;
            nudID.Focus();

            this.AcceptButton = this.btnPesquisa;
        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {
            //string ID_Formador, ref string nome, ref string nif, ref string idUser, ref string userName, ref string password, ref string userRole, ref string data_nascimento, ref string id_area

            string nome = "", nif = "", id_user = "", user_name = "", password = "", user_role = "", data_nascimento = "", area = "", id_area = "";


            if (ligacao.PesquisaFormadores(nudID.Value.ToString(), ref nome, ref nif, ref id_user, ref user_name, ref password, ref user_role, ref data_nascimento, ref area, ref id_area))
            {

                txtNome.Text = nome;
                txtNIF.Text = nif;
                txtIdUser.Text = id_user;
                txtUserName.Text = user_name;
                txtPassword.Text = password;
                txtUserRole.Text = user_role;
                txtArea.Text = id_area + "-" + area;
                mtxtDataNascimento.Text = data_nascimento;
                

                
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
            txtNome.Text = string.Empty;
            txtNIF.Text = string.Empty;
            txtIdUser.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserRole.Text = string.Empty;
            txtArea.Text = string.Empty;
            mtxtDataNascimento.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            btnEliminar.Enabled = false;
            nudID.Focus();
            Limpar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // confirmar com o utilizador se quer apagar os dados selecionados (y/n)
            var confirmacao = MessageBox.Show("Confirma que deseja apagar o formador selecionado?", "", MessageBoxButtons.YesNo);

            if (confirmacao == DialogResult.Yes)
            {
                if (ligacao.DeleteFormador(nudID.Value.ToString(), txtIdUser.Text)) // eliminar dados e mostrar mensagem de sucesso ou de erro
                {
                    MessageBox.Show("Formador apagado com sucesso.");
                    btnEliminar.Enabled = false;
                    nudID.Focus();
                    Limpar();
                }
                else
                {
                    MessageBox.Show("Erro ao apagar formador.");
                }
            }
        }
    }
}
