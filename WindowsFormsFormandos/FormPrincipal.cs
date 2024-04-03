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
    public partial class FormPrincipal : Form
    {
        //Instanciar os formulários que serão usados na aplicação.
        InserirFormandos formInserirFormandos = new InserirFormandos();
        EliminarFormandos formEliminarFormandos = new EliminarFormandos();
        AtualizarFormandos formAtualizarFormandos = new AtualizarFormandos();
        ListarFormandos formListarFormandos = new ListarFormandos();

        InserirNacionalidade formAdicionarNacionalidade = new InserirNacionalidade();
        EliminarNacionalidade formApagarNacionalidade = new EliminarNacionalidade();
        AtualizarNacionalidade formModificarNacionalidade = new AtualizarNacionalidade();
        ListarNacionalidade formMostrarNacionalidade = new ListarNacionalidade();

        InserirFormadores formInserirFormadores = new InserirFormadores();
        ListarFormadores formListarFormadores = new ListarFormadores();

        InserirArea formInserirArea = new InserirArea();
        AtualizarArea formAtualizarArea = new AtualizarArea();
        EliminarArea formEliminarArea = new EliminarArea();
        ListarArea formListarArea = new ListarArea();

        AutenticacaoUtilizador formAutenticacaoUtilizador = new AutenticacaoUtilizador();


        public FormPrincipal()
        {
            InitializeComponent(); // função definida no programa.cs
        }


        /* As próximas funções definem métodos onde cada um é um manipulador de eventos para ositens do menu. Eles controlam o que acontece quando um item de menu é clicado.
        Cada método define que quando um item de menu é clicado, o código verifica se o formulário associado já foi descartado.
        Se sim, cria uma nova instância dele. Em seguida, define o formulário principal como o pai do formulário em questão (MdiParente), define sua posição na tela e o exibe.
        */

        /* O formulário pai é o FormPrincipal. Ele é o formulário principal da aplicação, que contém a barra de menu e serve como o contêiner para os formulários filhos.
        Os formulários filhos são aqueles que são instanciados dentro dos manipuladores de eventos dos itens de menu, 
        como formInserirFormandos, formApagarFormandos, formAtualizarFormandos, formListarFormandos, formAdicionarNacionalidade, 
        formApagarNacionalidade, formAtualizarNacionalidade e formListarNacionalidade. Esses formulários são exibidos dentro do formulário principal (FormPrincipal).
        */

        // Formandos
        private void inserirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formInserirFormandos.IsDisposed)
            {
                formInserirFormandos = new InserirFormandos();
            }
            formInserirFormandos.MdiParent = this;
            formInserirFormandos.StartPosition = FormStartPosition.Manual;
            formInserirFormandos.Location = new Point((this.ClientSize.Width - formInserirFormandos.Width) / 2,
                (this.ClientSize.Height - formInserirFormandos.Height) / 3);
            formInserirFormandos.Show();
            formInserirFormandos.Activate();
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formEliminarFormandos.IsDisposed)
            {
                formEliminarFormandos = new EliminarFormandos();
            }
            formEliminarFormandos.MdiParent = this;
            formEliminarFormandos.StartPosition = FormStartPosition.Manual;
            formEliminarFormandos.Location = new Point((this.ClientSize.Width - formEliminarFormandos.Width) / 2,
                (this.ClientSize.Height - formEliminarFormandos.Height) / 3);
            formEliminarFormandos.Show();
            formEliminarFormandos.Activate();

        }

        private void atualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAtualizarFormandos.IsDisposed)
            {
                formAtualizarFormandos = new AtualizarFormandos();
            }
            formAtualizarFormandos.MdiParent = this;
            formAtualizarFormandos.StartPosition = FormStartPosition.Manual;
            formAtualizarFormandos.Location = new Point((this.ClientSize.Width - formAtualizarFormandos.Width) / 2,
                (this.ClientSize.Height - formAtualizarFormandos.Height) / 3);
            formAtualizarFormandos.Show();
            formAtualizarFormandos.Activate();
        }

        private void listarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formListarFormandos.IsDisposed)
            {
                formListarFormandos = new ListarFormandos();
            }
            formListarFormandos.MdiParent = this;
            formListarFormandos.StartPosition = FormStartPosition.Manual;
            formListarFormandos.Location = new Point((this.ClientSize.Width - formListarFormandos.Width) / 2,
                (this.ClientSize.Height - formListarFormandos.Height) / 3);
            formListarFormandos.Show();
            formListarFormandos.Activate();
        }


        // Nacionalidade
        private void adicionarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formAdicionarNacionalidade.IsDisposed)
            {
                formAdicionarNacionalidade = new InserirNacionalidade();
            }

            formAdicionarNacionalidade.MdiParent = this;
            formAdicionarNacionalidade.StartPosition = FormStartPosition.Manual;
            formAdicionarNacionalidade.Location = new Point((this.ClientSize.Width - formAdicionarNacionalidade.Width) / 2,
                (this.ClientSize.Height - formAdicionarNacionalidade.Height) / 3);
            formAdicionarNacionalidade.Show();
            formAdicionarNacionalidade.Activate();
        }

        private void modificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formModificarNacionalidade.IsDisposed)
            {
                formModificarNacionalidade = new AtualizarNacionalidade();
            }

            formModificarNacionalidade.MdiParent = this;
            formModificarNacionalidade.StartPosition = FormStartPosition.Manual;
            formModificarNacionalidade.Location = new Point((this.ClientSize.Width - formModificarNacionalidade.Width) / 2,
                (this.ClientSize.Height - formModificarNacionalidade.Height) / 3);
            formModificarNacionalidade.Show();
            formModificarNacionalidade.Activate();
        }

        private void apagarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (formApagarNacionalidade.IsDisposed)
            {
                formApagarNacionalidade = new EliminarNacionalidade();
            }

            formApagarNacionalidade.MdiParent = this;
            formApagarNacionalidade.StartPosition = FormStartPosition.Manual;
            formApagarNacionalidade.Location = new Point((this.ClientSize.Width - formApagarNacionalidade.Width) / 2,
                (this.ClientSize.Height - formApagarNacionalidade.Height) / 3);
            formApagarNacionalidade.Show();
            formApagarNacionalidade.Activate();
        }

        private void mostrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (formMostrarNacionalidade.IsDisposed)
            {
                formMostrarNacionalidade = new ListarNacionalidade();
            }
            formMostrarNacionalidade.MdiParent = this;
            formMostrarNacionalidade.StartPosition = FormStartPosition.Manual;
            formMostrarNacionalidade.Location = new Point((this.ClientSize.Width - formMostrarNacionalidade.Width) / 2,
                (this.ClientSize.Height - formMostrarNacionalidade.Height) / 3);
            formMostrarNacionalidade.Show();
            formMostrarNacionalidade.Activate();
        }


        //Autenticação
        private void FormPrincipal_Load(object sender, EventArgs e)
        {
            toolStriplblUser.Text = "";
            formAutenticacaoUtilizador.ShowDialog();
            toolStriplblUser.Text = "Perfil: " + Geral.id_user;

        }

        private void toolStripbtnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja efetuar Logout?\nTodas as janelas serão fechadas", "LogOut", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                foreach (Form frm in this.MdiChildren)
                {
                    frm.Dispose();
                    frm.Close();
                }

                toolStriplblUser.Text = "";
                toolStripbtnLogOut.Enabled = false;
                formAutenticacaoUtilizador.ShowDialog();
                toolStriplblUser.Text = "Pergil: " + Geral.id_user;
                toolStripbtnLogOut.Enabled = true;
            }
        }

       
        // Formadores

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            if (formInserirFormadores.IsDisposed)
            {
                formInserirFormadores = new InserirFormadores();
            }
            formInserirFormadores.MdiParent = this;
            formInserirFormadores.StartPosition = FormStartPosition.Manual;
            formInserirFormadores.Location = new Point((this.ClientSize.Width - formInserirFormadores.Width) / 2,
                (this.ClientSize.Height - formInserirFormadores.Height) / 3);
            formInserirFormadores.Show();
            formInserirFormadores.Activate();
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            if (formListarFormadores.IsDisposed)
            {
                formListarFormadores = new ListarFormadores();
            }
            formListarFormadores.MdiParent = this;
            formListarFormadores.StartPosition = FormStartPosition.Manual;
            formListarFormadores.Location = new Point((this.ClientSize.Width - formListarFormadores.Width) / 2,
                (this.ClientSize.Height - formListarFormadores.Height) / 3);
            formListarFormadores.Show();
            formListarFormadores.Activate();
        }


        //Area
        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (formInserirArea.IsDisposed)
            {
                formInserirArea = new InserirArea();
            }
            formInserirArea.MdiParent = this;
            formInserirArea.StartPosition = FormStartPosition.Manual;
            formInserirArea.Location = new Point((this.ClientSize.Width - formInserirArea.Width) / 2,
                (this.ClientSize.Height - formInserirArea.Height) / 3);
            formInserirArea.Show();
            formInserirArea.Activate();
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            if (formAtualizarArea.IsDisposed)
            {
                formAtualizarArea = new AtualizarArea();
            }
            formAtualizarArea.MdiParent = this;
            formAtualizarArea.StartPosition = FormStartPosition.Manual;
            formAtualizarArea.Location = new Point((this.ClientSize.Width - formAtualizarArea.Width) / 2,
                (this.ClientSize.Height - formAtualizarArea.Height) / 3);
            formAtualizarArea.Show();
            formAtualizarArea.Activate();
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            if (formEliminarArea.IsDisposed)
            {
                formEliminarArea = new EliminarArea();
            }
            formEliminarArea.MdiParent = this;
            formEliminarArea.StartPosition = FormStartPosition.Manual;
            formEliminarArea.Location = new Point((this.ClientSize.Width - formEliminarArea.Width) / 2,
                (this.ClientSize.Height - formEliminarArea.Height) / 3);
            formEliminarArea.Show();
            formEliminarArea.Activate();
        }

        private void toolStripMenuItem15_Click(object sender, EventArgs e)
        {
            if (formListarArea.IsDisposed)
            {
                formListarArea = new ListarArea();
            }
            formListarArea.MdiParent = this;
            formListarArea.StartPosition = FormStartPosition.Manual;
            formListarArea.Location = new Point((this.ClientSize.Width - formListarArea.Width) / 2,
                (this.ClientSize.Height - formListarArea.Height) / 3);
            formListarArea.Show();
            formListarArea.Activate();
        }

        
    }
}
