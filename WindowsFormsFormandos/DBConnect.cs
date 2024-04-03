using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Mysqlx.Sql;
using System.Diagnostics.Contracts;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace WindowsFormsBDGestaoFormandos
{
    internal class DBConnect
    {
        public MySqlConnection connection { get; private set; }
        private string server;
        private string username;
        private string password;
        private string database;
        private string port;

        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            // Parametros de conexão com a base de dados
            server = "192.168.1.78";
            database = "gestaoformandos";
            username = "avlis";
            password = "kuVBNZ6b";
            port = "3306";

            // string de conexão no formato exigido pelo MySql Connector.
            string connectionString = "Server = " + server + "; Port = " + port + "; Database = " + database + "; Uid = " + username + "; Pwd = " + password + ";";

            // instanciação do objeto de conexão que irá ser utilizado para executar comandos no banco de dados.
            connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            // Método responsável por abrir a conexão com o banco de dados.
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public bool CloseConnection()
        {
            // Método responsável por fechar a conexão com o banco de dados.
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public string StatusConnection()
        {
            // Método responsável por verificar o estado atual da conexão com o banco de dados e retornar uma representação em forma de string desse estado.
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                else
                {
                    connection.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return connection.State.ToString();
        }

        //--------------- INÍCIO PARTE FORMANDOS DO FORM PRINCIPAL
        public bool InsertFormando(string ID, string nome, string morada, string contacto, string iban, char sexo, string data_nascimento, string id_nacionalidade) //Insert para InserirFormandos
        {
            // Este método é responsável por inserir um novo registro na tabela formando do banco de dados.
            string query = "Insert into formando (id_formando, nome, morada, contacto, iban, sexo, data_nascimento, id_Nacionalidade) values ('" +
                 ID + "', '" + nome + "', '" + morada + "', '" + contacto + "', '" + iban + "', '" + sexo + "', '" + data_nascimento + "','" + id_nacionalidade + "');";

            bool flag = true;

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }

            finally
            {
                CloseConnection();
            }

            return flag;

            /*
            A string de consulta SQL, query, será executada para inserir os dados na tabela. 
            Esta string contém instruções SQL para inserir os valores fornecidos nos parâmetros na tabela formando. 

            A variável flag é usada para indicar se a operação de inserção dos dados foi ou não bem sucedida.
            É feita a tentativa de ligação com a base de dados e, se for conseguido é executada a consulta.

            Se ocorrer uma exceção durante a execução da consulta, a exceção é capturada e tratada dentro do bloco catch. 
            Uma caixa de diálogo é exibida mostrando a mensagem de erro correspondente e a variável flag é definida como false para indicar que ocorreu um erro durante a operação.

            Independentemente de a operação ter sido bem-sucedida ou não, a conexão com o banco de dados é fechada usando o método CloseConnection no bloco finally.
            */
        }

        public bool DeleteFormando(string id) // Aplica-se a mesma lógica do método InserirFormando, mudando apenas a query.
        {
            string query = "Delete from formando where id_formando = '" + id + "';";

            bool flag = true;

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }

            finally
            {
                CloseConnection();
            }

            return flag;
        }

        public bool UpdateFormando(string ID, string nome, string morada, string contacto, string iban, char sexo, string data_nascimento, string id_nacionalidade) // Aplica-se a mesma lógica do método InserirFormando, mudando apenas a query.
        {
            string query = "update formando set nome = '" + nome + "', morada = '" + morada + "', contacto = '" +
                contacto + "', iban = '" + iban + "', sexo = '" + sexo + "', data_nascimento = '" + data_nascimento + "', id_nacionalidade = " + id_nacionalidade +
                " where id_formando = " + ID;

            bool flag = true;

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }

            finally
            {
                CloseConnection();
            }

            return flag;
        }

        public int DevolveUltimoID()
        {
            int ultimoID = 0;

            string query = "select max(id_formando) from formando;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int.TryParse(cmd.ExecuteScalar().ToString(), out ultimoID);
                    ultimoID = ultimoID + 1;
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
            
            finally
            {
                CloseConnection();
            }

            return ultimoID;
        }

        public bool PesquisaFormando(string ID_pesquisa, ref string nome, ref string morada, ref string contacto, ref string iban, ref char genero, ref string data_nascimento, ref string iso2, ref string nacionalidade, ref string id_nacionalidade)
        {
            bool flag = false;

            string query = "Select nome, morada, contacto, iban, sexo, data_nascimento, nac.alf2, nac.nacionalidade, nac.id_nacionalidade" +
                " from formando " +
                "join Nacionalidade as nac on nac.id_nacionalidade = formando.id_nacionalidade " +
                "where id_formando = '" + ID_pesquisa + "';";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        nome = dataReader[0].ToString();
                        morada = dataReader["morada"].ToString();
                        contacto = dataReader[2].ToString();
                        iban = dataReader[3].ToString();
                        genero = dataReader["sexo"].ToString()[0];
                        data_nascimento = dataReader[5].ToString();
                        iso2 = dataReader[6].ToString();
                        nacionalidade = dataReader[7].ToString();
                        id_nacionalidade = dataReader[8].ToString();

                        flag = true;
                    }
                    dataReader.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return flag;
        }

        public void PreencherDataGridViewFormandos(ref DataGridView dgv)
        {
            string query = "select id_formando, nome, iban, contacto, sexo, nac.alf2, nac.nacionalidade " +
                "from formando " +
                "join Nacionalidade as nac on nac.id_nacionalidade = formando.id_nacionalidade order by nome;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    int idxLinha = 0;
                    while (dr.Read())
                    {
                        dgv.Rows.Add();
                        dgv.Rows[idxLinha].Cells["codID"].Value = dr[0].ToString();
                        dgv.Rows[idxLinha].Cells["Nome"].Value = dr[1].ToString();
                        dgv.Rows[idxLinha].Cells["IBAN"].Value = dr[2].ToString();
                        dgv.Rows[idxLinha].Cells["Contacto"].Value = dr[3].ToString();
                        dgv.Rows[idxLinha].Cells["Genero"].Value = dr[4].ToString();
                        dgv.Rows[idxLinha].Cells["Nacionalidade"].Value = dr[5].ToString()+" - " + dr[6].ToString();
                        idxLinha++;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void PreencherDataGridViewFormandosPesquisa(ref DataGridView dgv, char genero, string nome, string id)
        {
            string query = "select id_formando, nome, iban, contacto, sexo, nac.alf2, nac.nacionalidade " +
                "from formando " +
                "join Nacionalidade as nac on nac.id_nacionalidade = formando.id_nacionalidade "+
                $"where nome like '{nome}%'" +
                (id != "" ? $" and nac.id_nacionalidade = {id} ": "")+
                (genero != 'T' ? $" and sexo = '{genero}'" : "" )+
                " order by nome;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    int idxLinha = 0;
                    while (dr.Read())
                    {
                        dgv.Rows.Add();
                        dgv.Rows[idxLinha].Cells["codID"].Value = dr[0].ToString();
                        dgv.Rows[idxLinha].Cells["Nome"].Value = dr[1].ToString();
                        dgv.Rows[idxLinha].Cells["IBAN"].Value = dr[2].ToString();
                        dgv.Rows[idxLinha].Cells["Contacto"].Value = dr[3].ToString();
                        dgv.Rows[idxLinha].Cells["Genero"].Value = dr[4].ToString();
                        dgv.Rows[idxLinha].Cells["Nacionalidade"].Value = dr[5].ToString() + " - " + dr[6].ToString();
                        idxLinha++;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }


        //--------------- FIM PARTE FORMANDOS DO FORM PRINCIPAL


        //--------------- INÍCIO PARTE NACIONALIDADE DO FORM PRINCIPAL

        public bool InsertNacionalidade(string iso2, string nacionalidade) //O método recebe dois parâmetros - iso2, que é o código ISO de duas letras da nacionalidade, e nacionalidade, que é o nome da nacionalidade.
        {
            bool flag = true; //esta variável será usada para determinar se a operação de inserção foi bem-sucedida ou não. Se for true a operação de inserção foi bem-sucedida, se false ocorreu um erro.

            string query = "insert into Nacionalidade values (0, '" + iso2 + "','" + nacionalidade + "');";
            //A string query é construída para inserir os valores fornecidos na tabela Nacionalidade do banco de dados.

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }
            CloseConnection();
            return flag; // retorna o valor da variável flag

            /*
            O método OpenConnection() é chamado para abrir a conexão com o banco de dados. 
            Se a conexão for aberta com sucesso, uma instância de MySqlCommand é criada com a query SQL e a conexão. 
            A função ExecuteNonQuery() é chamada para executar a query e inserir os dados na tabela.
            Depois que a operação de inserção é concluída ou se ocorrer um erro, o método CloseConnection() é chamado para fechar a conexão com o banco de dados.
            */
        }

        public bool UpdateNacionalidade(string id_nacionalidade, string iso2, string nacionalidade)
        {
            string query = " update Nacionalidade set alf2 = '" + iso2 + "', nacionalidade = '" + nacionalidade + "' where id_nacionalidade = '" + id_nacionalidade + "';";
            /*
            A variável query contém a consulta SQL que será executada para atualizar a entrada na tabela "Nacionalidade". 
            Esta consulta atualiza os valores das colunas "alf2" e "nacionalidade" onde o "id_nacionalidade" corresponde ao valor fornecido.
            */

            bool flag = true;

            try
            {
                if (this.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }
            finally { CloseConnection(); }
            return flag;

            /*
            Este método, UpdateNacionalidade, é responsável por atualizar uma entrada na tabela "Nacionalidade" do banco de dados. 
            A lógica aplicada é igual ao método anterior mudando apenas a variável query.
            */
        }

        public bool DeleteNacionalidade(string nacionalidade) // Idêntico ao método InserirNacionalidade.
        {
            bool flag = true;
            string query = "Delete from Nacionalidade where nacionalidade = '" + nacionalidade + "';"; //Muda a query, passando para delete.

            try
            {
                if (this.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }
            CloseConnection();
            return flag;
        }

        public void PreencherDataGridViewNacionalidade(ref DataGridView dgv)
        {
            string query = "select id_nacionalidade, alf2, nacionalidade from nacionalidade order by nacionalidade;";
            // Esta variável é definida para selecionar os campos id_nacionalidade, alf2 e nacionalidade da tabela nacionalidade, ordenados pelo campo nacionalidade.
            
            
            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    int idxLinha = 0;
                    while (dr.Read())
                    {
                        dgv.Rows.Add();
                        dgv.Rows[idxLinha].Cells["id_nacionalidade"].Value = dr[0].ToString();
                        dgv.Rows[idxLinha].Cells["alf2"].Value = dr[1].ToString();
                        dgv.Rows[idxLinha].Cells["nacionalidade"].Value = dr[2].ToString();
                        idxLinha++;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            /*
            Este método é responsável por executar uma consulta ao banco de dados para obter os dados das nacionalidades e 
            preencher um DataGridView com esses dados, garantindo o tratamento adequado de exceções e a correta liberação 
            dos recursos de conexão com o banco de dados. 

            A consulta é executada dentro de um bloco try-catch. 
            Se a conexão com o banco de dados for aberta com sucesso (OpenConnection()), a consulta é executada e os resultados são armazenados em um MySqlDataReader.

            Um loop while é utilizado para percorrer cada linha do resultado da consulta. 
            Para cada linha, uma nova linha é adicionada ao DataGridView (dgv.Rows.Add()), e os valores das colunas id_nacionalidade, alf2 e nacionalidade são atribuídos às células correspondentes da nova linha.
            
            Qualquer exceção MySQL é capturada pelo bloco catch, e uma mensagem de erro é exibida. 
            Em seguida, a conexão com o banco de dados é fechada (CloseConnection()).
            */
        }

        //--------------- FIM PARTE NACIONALIDADE DO FORM PRINCIPAL
       

        //--------------- INÍCIO PARTE AUTENTICAÇÃO UTILIZADOR

        public bool ValidateUser(string username, string password, ref string id_user)
        {
            bool flag = false;

            try
            {
                string query = "Select userRole from utilizador where nome_utilizador = '" + username + "' and palavra_passe = sha2('" + password + "', 512) and estado = 'A';";

                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    if (cmd.ExecuteScalar() != null)
                    {
                        id_user = cmd.ExecuteScalar().ToString();
                        flag = true;

                        string procedimentoS = $"call pUSuccessLogin ('{username}', 'S');";
                        new MySqlCommand(procedimentoS, connection).ExecuteScalar();
                    }

                    else
                    {
                        string procedimentoS = $"call pUSuccessLogin ('{username}', 'U');";
                        new MySqlCommand(procedimentoS, connection).ExecuteScalar();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return flag;
        }

        public bool UserAtivo(string username)
        {
            bool flag = false;

            try
            {
                string query = "Select * from utilizador where nome_utilizador = '" + username + "' and estado = 'A';";

                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    if (cmd.ExecuteScalar() != null)
                    {

                        flag = true;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }

            return flag;
        }

        //--------------- FIM PARTE AUTENTICAÇÃO UTILIZADOR


        //--------------- INÍCIO PARTE FORMADORES DO FORM PRINCIPAL

        public int DevolveUltimoIDFormadores()
        {
            int ultimoIDFormadores = 0;

            string query = "select max(id_formador) from formador;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int.TryParse(cmd.ExecuteScalar().ToString(), out ultimoIDFormadores);
                    ultimoIDFormadores = ultimoIDFormadores + 1;
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }

            finally
            {
                CloseConnection();
            }

            return ultimoIDFormadores;
        }

        public int DevolveUltimoIDUtilizador()
        {
            int ultimoIDUser = 0;

            string query = "select max(id_utilizador) from utilizador;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int.TryParse(cmd.ExecuteScalar().ToString(), out ultimoIDUser);
                    ultimoIDUser = ultimoIDUser + 1;
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }

            finally
            {
                CloseConnection();
            }

            return ultimoIDUser;
        }

        public bool PesquisaFormadores(string ID_Formador, ref string nome, ref string nif, ref string idUser, ref string userName, ref string password, ref string userRole, ref string data_nascimento, ref string area, ref string id_area)
        {
            bool flag = false;

            string query = $@"
                Select nome, nif, dataNascimento, area, user.id_utilizador, user.nome_utilizador, user.palavra_passe, user.userRole, area.id_area
                from formador
                join utilizador as user on user.id_utilizador = formador.id_utilizador
                join area on area.id_area = formador.id_area
                where formador.id_formador = {ID_Formador};";


            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    while (dataReader.Read())
                    {
                        nome = dataReader[0].ToString();
                        nif = dataReader[1].ToString();
                        idUser = dataReader[4].ToString();
                        userName = dataReader[5].ToString();
                        password = dataReader[6].ToString();
                        userRole = dataReader[7].ToString();
                        data_nascimento = dataReader[2].ToString();
                        area = dataReader[3].ToString();
                        id_area = dataReader[8].ToString();

                        flag = true;
                    }
                    dataReader.Close();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return flag;
        }

        public bool InsertFormador(string ID, string nome, string nif, string idUser, string userName, string password, string userRole, string data_nascimento, string id_area) //Insert para InserirFormandos
        {
            // Este método é responsável por inserir um novo registro na tabela formando do banco de dados.
            string query1 = "Insert into utilizador (id_utilizador, nome_utilizador, palavra_passe, userRole) values ("
                + $"'{idUser}', '{userName}', sha2('{password}',512), '{userRole}');";
            string query2 = "Insert into formador (id_formador, nome, nif, dataNascimento, id_area, id_utilizador) values (" 
                + $"'{ID}', '{nome}', '{nif}', '{data_nascimento}', '{id_area}', '{idUser}');";

            bool flag = true;

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query1, connection);
                    cmd.ExecuteNonQuery();

                    MySqlCommand cmd2 = new MySqlCommand(query2, connection);
                    cmd2.ExecuteNonQuery();
                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }

            finally
            {
                CloseConnection();
            }

            return flag;

        }

        public bool DeleteFormador(string id_formador, string id_utilizador) // Aplica-se a mesma lógica do método InserirFormando, mudando apenas a query.
        {
            string query = $@"Delete from formador where id_formador = {id_formador};
                Delete from utilizador where id_utilizador = {id_utilizador};";

            bool flag = true;

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }

            finally
            {
                CloseConnection();
            }

            return flag;
        }
        public bool UpdateFormador(string ID, string nome, string NIF, string userRole, string data_nascimento, string id_area) // Aplica-se a mesma lógica do método InserirFormando, mudando apenas a query.
        {
            
            string query = "update formador set nome = '" + nome + "', nif = '" + NIF + "', dataNascimento = '" +
                data_nascimento + "', id_area = " + id_area +
                " where id_formador = " + ID;

            bool flag = true;

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();

                }
            }

            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }

            finally
            {
                CloseConnection();
            }

            return flag;
        }


        public void PreencherDataGridViewFormadores(ref DataGridView dgv)
        {
            string query = "select id_formador, nome, nif, area.area " +
                "from formador " +
                "join area on area.id_area = formador.id_area order by nome;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    int idxLinha = 0;
                    while (dr.Read())
                    {
                        dgv.Rows.Add();
                        dgv.Rows[idxLinha].Cells["ID_Formador"].Value = dr[0].ToString();
                        dgv.Rows[idxLinha].Cells["Nome"].Value = dr[1].ToString();
                        dgv.Rows[idxLinha].Cells["NIF"].Value = dr[2].ToString();
                        dgv.Rows[idxLinha].Cells["Area"].Value = dr[3].ToString();
                        idxLinha++;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void PreencherDataGridViewFormadoresPesquisa(ref DataGridView dgv, string nome, string id)
        {
            string query = "select id_formador, nome, nif, area.area " +
                "from formador " +
                "join area on area.id_area = formador.id_area " +
                $"where nome like '{nome}%'" +
                (id != "" ? $" and area.id_area = {id}" : "") +
                " order by nome;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    int idxLinha = 0;
                    while (dr.Read())
                    {
                        dgv.Rows.Add();
                        dgv.Rows[idxLinha].Cells["id_formador"].Value = dr[0].ToString();
                        dgv.Rows[idxLinha].Cells["Nome"].Value = dr[1].ToString();
                        dgv.Rows[idxLinha].Cells["NIF"].Value = dr[2].ToString();
                        dgv.Rows[idxLinha].Cells["Area"].Value = dr[3].ToString();
                        idxLinha++;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }


        //--------------- FIM PARTE FORMANDOS DO FORM PRINCIPAL


        //--------------- INÍCIO PARTE AREA DO FORM PRINCIPAL

        public int DevolveUltimoIDArea()
        {
            int ultimoIDArea = 0;

            string query = "select max(id_area) from area;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    int.TryParse(cmd.ExecuteScalar().ToString(), out ultimoIDArea);
                    ultimoIDArea = ultimoIDArea + 1;
                }
            }
            catch (MySqlException ex)
            {

                MessageBox.Show(ex.Message);
            }

            finally
            {
                CloseConnection();
            }

            return ultimoIDArea;
        }
        public bool InsertArea(string area) 
        {
            bool flag = true; //esta variável será usada para determinar se a operação de inserção foi bem-sucedida ou não. Se for true a operação de inserção foi bem-sucedida, se false ocorreu um erro.

            string query = "insert into area values (0, '" + area + "');";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }
            CloseConnection();
            return flag; // retorna o valor da variável flag
        }

        public bool UpdateArea(string id_area, string area)
        {
            string query = " update area set area = '" + area + "' where id_area = '" + id_area + "';";

            bool flag = true;

            try
            {
                if (this.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }
            finally { CloseConnection(); }
            return flag;

        }

        public bool DeleteArea(string area) 
        {
            bool flag = true;
            string query = "Delete from area where area = '" + area + "';";

            try
            {
                if (this.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                flag = false;
            }
            CloseConnection();
            return flag;
        }

        public void PreencherDataGridViewArea(ref DataGridView dgv)
        {
            string query = "select id_area, area from area order by id_area;";

            try
            {
                if (OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader dr = cmd.ExecuteReader();

                    int idxLinha = 0;
                    while (dr.Read())
                    {
                        dgv.Rows.Add();
                        dgv.Rows[idxLinha].Cells["id_area"].Value = dr[0].ToString();
                        dgv.Rows[idxLinha].Cells["area"].Value = dr[1].ToString();
                        idxLinha++;

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                CloseConnection();
            }
        }


        //--------------- FIM PARTE AREA DO FORM PRINCIPAL



    }

    internal class Geral
    {
        public static string id_user = "";
        public static string TirarEspacos(string texto)
        {
            texto = texto.Trim();
            texto = Regex.Replace(texto, @"\s+", " ");
            return texto;
        }

        public static bool CheckDate(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
