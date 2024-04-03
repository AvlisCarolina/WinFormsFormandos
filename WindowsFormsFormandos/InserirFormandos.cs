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
    public partial class InserirFormandos : Form
    {
        DBConnect ligacao = new DBConnect();
        public InserirFormandos()
        {
            InitializeComponent();
        }

        private void InserirFormandos_Load(object sender, EventArgs e)
        {
            nudID.Value = ligacao.DevolveUltimoID(); // método definido no DBConnect
            nudID.Enabled = false;
            PesquisaNacionalidade(); // método definido no DBConnect

            //Este evento define o valor do campo nudID com o último ID disponível no banco de dados e preenche o ComboBox de nacionalidades.
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            /*
            Este evento verifica se os campos estão preenchidos corretamente e, se estiverem, 
            insere os dados do formando no banco de dados usando o método InsertFormando da classe DBConnect.
            */

            if (VerificarCampos()) // só executa este bloco se todos os campos estiverem devidamente preenchidos
            {                
                string[] texto2 = cbNacionalidade.Text.Split(new Char[] { '-' }); //divide o texto selecionado no ComboBox de nacionalidade em três partes 
                string id_nacionalidade = texto2[0]; //Extrai o ID da nacionalidade do array texto2.

                if (ligacao.InsertFormando(nudID.Value.ToString(), txtNome.Text, txtMorada.Text, txtContacto.Text, mTxtIban.Text, Genero(), DateTime.Parse(mtxtDataNascimento.Text).ToString("yyyy-MM-dd"), id_nacionalidade)) //Chama o método InsertFormando
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

        public bool PesquisaNacionalidade()
        {
            // Este método  é responsável por preencher o ComboBox de nacionalidades com os dados recuperados do banco de dados.

            cbNacionalidade.Items.Clear(); // Limpa combobox para evitar duplicação de dados
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

            /*
            O código dentro do loop while é executado para cada registro retornado pela consulta. 
            Para cada registro, os valores das colunas id, iso e nacionalidade são lidos do MySqlDataReader, concatenados em uma string texto e adicionados como um item ao ComboBox de nacionalidades. 
            Além disso, a variável flag é definida como true para indicar que pelo menos um registro foi encontrado.
            
            No final, o método retorna o valor da variável flag, que indica se pelo menos um registro foi encontrado durante a pesquisa no banco de dados. 
            Isso pode ser útil para determinar se o ComboBox foi preenchido com sucesso. 
            */
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

            /*
            Este método é responsável por determinar o gênero selecionado pelo usuário no formulário de inserção de formandos. 
            Inicializa a variável genero com o valor 'T', que representa "indefinido" ou "não especificado".
            Depois identifica qual das opções foi escolhida pelo utilizador e retorna o valor da variável genero, que representa o género selecionado pelo utilizador no formulário.
            
            Assim é possível garantir que o sistema capture corretamente as informações fornecidas pelo usuário, mantendo a precisão dos dados.
            */
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

            nudID.Value = ligacao.DevolveUltimoID();
            txtContacto.Text = string.Empty;
            txtMorada.Text = string.Empty;
            txtNome.Text = string.Empty;
            mTxtIban.Text = string.Empty;
            rbFeminino.Checked = false;
            rbMasculino.Checked = false;
            rbOutro.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            mtxtDataNascimento.Clear();
            cbNacionalidade.Text = string.Empty;
            cbNacionalidade.Items.Clear();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Limpar();
            PesquisaNacionalidade();

            //Limpa o formulário e chama o método PesquisaNacionalidade
        }
    }
}
