using Mysqlx.Crud;
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
    public partial class InserirNacionalidade : Form
    {
        DBConnect ligacao = new DBConnect(); // funcão definida no DBConnect.cs. Serve para conectar à base de dados.
        public InserirNacionalidade()
        {
            InitializeComponent();
        }

        private void AdicionarNacionalidade_Load(object sender, EventArgs e) // evento de carregar o formulário onde é definido o foco para o campo txtISO2
        {
            txtISO2.Focus();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //Este evento é acionado quando o botão "Cancelar" é clicado. Ele simplesmente limpa os campos do formulário.

            Limpar();
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            /*
            Este evento é acionado quando o botão "Gravar" é clicado.
            Ele verifica se os campos estão preenchidos corretamente e, em seguida, chama um método da classe DBConnect para inserir a nova nacionalidade no banco de dados.
            Se a inserção for bem-sucedida, exibe uma mensagem de sucesso e limpa os campos do formulário.
            */

            if (VerificarCampos())
            {
                if (ligacao.InsertNacionalidade(txtISO2.Text, txtNacionalidade.Text))
                {
                    MessageBox.Show("Inserido com Sucesso");
                    Limpar();
                    txtISO2.Focus();
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
            Este método verifica se os campos de entrada estão preenchidos corretamente.
            Ele verifica se o campo txtISO2 tem exatamente 2 caracteres e se o campo txtNacionalidade não está vazio.
            */

            if (txtISO2.Text.Length != 2)
            {
                MessageBox.Show("Erro no preenchimento dos campos!");
                txtISO2.Focus();
                return false;
            }

            if (txtNacionalidade.Text.Length == 0)
            {
                MessageBox.Show("Erro no preenchimento dos campos!");
                txtNacionalidade.Focus();
                return false;
            }

            return true;
        }

        private void Limpar()
        {
            // Este método limpa todos os campos do formulário.

            txtISO2.Text = string.Empty;
            txtNacionalidade.Text = string.Empty;
        }
    }
}
