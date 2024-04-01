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
    public partial class AtualizarNacionalidade : Form
    {
        DBConnect ligacao = new DBConnect(); //conexão com o banco de dados.
        public AtualizarNacionalidade()
        {
            InitializeComponent();
        }

        private void AtualizarNacionalidade_Load(object sender, EventArgs e)
        {
            PesquisaCampos();
            Limpar();

            // Chama PesquisaCampos e define os campos de entrada como somente leitura.
        }

        public bool PesquisaCampos()
        {
            comboBox1.Items.Clear(); // para evitar duplicação de nacionalidades
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
                        comboBox1.Items.Add(texto);
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
            Este método PesquisaCampos() é responsável por preencher o comboBox1 com os dados das nacionalidades existentes no banco de dados.
            As variáveis id, iso e nacionalidade são variáveis locais que armazenam temporariamente os valores das colunas da tabela "Nacionalidade" durante a leitura do banco de dados. 
            A variável texto será usada para construir a string que será exibida em cada item do ComboBox.
            A variável query contém a consulta SQL que seleciona todas as colunas da tabela "Nacionalidade".

            No bloco try-catch-finally o código da consulta SQL é colocado dentro de um bloco try onde passa por um loop while usado para iterar sobre os resultados da consulta. 
            Para cada linha no resultado, os valores das colunas são lidos e adicionados ao ComboBox na forma de uma string concatenada (id-iso-nacionalidade). 
            A variável flag é definida como true para indicar que pelo menos uma nacionalidade foi encontrada e adicionada ao ComboBox.
            Após o loop, o método Close() é chamado no objeto MySqlDataReader para fechar o leitor de dados.

            Se ocorrer uma exceção do tipo MySqlException, a mensagem de erro será exibida em uma caixa de diálogo MessageBox. 
            O bloco finally garante que a conexão com o banco de dados seja fechada mesmo que ocorra uma exceção, pela utilização do método CloseConection().
            */
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] texto2 = comboBox1.Text.Split(new Char[] { '-' }); // Separa o texto presente na comboBox1 numa array (lista) com 3 índices.
            //string id_nacionalidade = texto2[0];
            string iso2 = texto2[1]; // define o valor da váriável igual ao 2º indice da array texto2 para mostrar o dado na textBox txtISO2
            string nacionalidade = texto2[2];

            txtISO2.Text = iso2;
            txtNacionalidade.Text = nacionalidade;

            txtISO2.ReadOnly = false;
            txtNacionalidade.ReadOnly = false;
            btnAtualizar.Enabled = true; // habilita a utilização do botão Atualizar.


            /*
            Este método atualiza as caixas de texto txtISO2 e txtNacionalidade com os valores correspondentes ao item selecionado no comboBox1, 
            e habilita o botão "Atualizar" para permitir que o usuário atualize os valores desses campos.
            */
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            var confirmacao = MessageBox.Show("Confirma que deseja atualizar a nacionalidade selecionada?", "", MessageBoxButtons.YesNo); // MessageBox de YesNo
            string[] texto2 = comboBox1.Text.Split(new Char[] { '-' });
            string id_nacionalidade = texto2[0];


            if (confirmacao == DialogResult.Yes)
            {
                /*
                Se a confirmação do usuário for "Sim", então a função UpdateNacionalidade da classe DBConnect é chamada para atualizar a nacionalidade no banco de dados. 
                Os novos valores para a ISO2 e a nacionalidade são obtidos dos campos de texto txtISO2 e txtNacionalidade, respectivamente. 
                Se a atualização for bem-sucedida, uma mensagem de sucesso é exibida e os controles são atualizados para permitir a inserção de uma nova nacionalidade. 
                Caso contrário, uma mensagem de erro é exibida.
                */

                if (ligacao.UpdateNacionalidade(id_nacionalidade, txtISO2.Text, txtNacionalidade.Text))
                {
                    MessageBox.Show("Nacionalidade atualizada com sucesso.");
                    groupBox3.Enabled = true;
                    Limpar();
                    PesquisaCampos();

                }
                else
                {
                    MessageBox.Show("Erro ao apagar nacionalidade.");
                }
            }

            /*
            Este método permite ao utilizador confirmar a atualização de uma nacionalidade selecionada, 
            e então chama a função apropriada para realizar a atualização no banco de dados. 
            Após a atualização, os controles são atualizados para permitir a inserção de uma nova nacionalidade.
            */
        }

        private void Limpar()
        {
            comboBox1.Focus();
            comboBox1.Text = string.Empty;
            txtISO2.Text = string.Empty;
            txtNacionalidade.Text = string.Empty;
            txtISO2.ReadOnly = true;
            txtNacionalidade.ReadOnly = true;
            btnAtualizar.Enabled = false;

            // limpa o conteúdo dos campos de entrada, colocando os mesmos em readonly e desativando o botão atualizar.
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            PesquisaCampos();
            Limpar();
        }
    }
}
