Projeto Gestão de Formandos.

Âmbito do projeto:

NuGet Packages:
- MySql.Data by Oracle;
- iTextSharp by iText Software;
- BouncyCastle by Bouncy Castle Project Contributors;

Base de Dados:
A base de dados foi feita pelo MySql Workbench com o objetivo de fazer ligação remota (a uma Máquina Virtual presente na mesma LAN).

- Ligação à base de dados:
	* server = "192.168.1.78";
    * database = "gestaoformandos";
    * username = "avlis";
    * password = "kuVBNZ6b";
    * port = "3306";


Distribuição projeto por ficheiro:
- Program.cs
    * Ponto de entrada pincipal para o aplicativo.

- DB.Connects.cs
    * Integra métodos que são utilizados em vários formulários:
        - DBConnect (classe que chama o método Initialize)
        - Initialize (métodos de inicialização de conexão com o Banco de Dados usando a biblioteca MySql Connector)
        - OpenConnection (método de abrir a ligação com a base de dados)
        - CloseConnection (método de fechar a ligação à base de dados)
        - SatusConnectios (método que indica o estado da ligação)
        - InsertFormando (método que define como deve ser feita a inserção de um formando)
        - DeleteFormando (método que define como deve ser feita a eliminação de um formando)
        - UpdateFormando (método que define como deve ser feita a atualização de dados de um formando)
        - DevolveUltimoID (método devolve o primeiro ID disponível em ordem crescente, de acordo com os registos da base de dados)
        - PesquisaFormando (método utilizado para pesquisar um determinado formando com base no seu ID)
        - InsertNacionalidade (método que define como deve ser feita a inserção na base de dados)
        - DeleteNacionalidade (método que define como deve ser eliminada uma nacionalidade na base de dados)
        - UpdateNacionalidade (método que define como deve ser atualizada uma nacionalidade na base de dados)
        - PreencherDataGridViewNacionalidade (método que lista todas as nacionalidades registadas na base de dados)
        - PreencherDataGridViewFormandos
        - PreencherDataGridViewFormandosPesquisa
        - ValidateUser
        - UserAtivo
        - Geral
        

- Apresentação dos Formulários:
    * FormPrincipal: formulário inicial que contem um Menu Strip onde se fará o acesso aos outros formulários, sendo dividido em:
        1. Formandos:
            1.1: Inserir (InserirFormandos)
            1.2: Atualizar (AtualizarFormandos)
            1.3: Eliminar (EliminarFormandos)
            1.4: Listar (ListarFormandos)
        2. Nacionalidade:
            2.1: Adicionar (InserirNacionalidade)
            2.2: Modificar (AtualizarNacionalidade)
            2.3: Apagar (ELiminarNacionalidade)
            2.4: Mostrar (ListarNacionalidade)
        3. Formadores:
            3.1: Inserir (InserirFormadores)
            3.2: Atualizar (AtualizarFormadores)
            3.3: Eliminar (EliminarFormadores)
            3.4: Listar (ListarFormadores)
        4. Área:
            4.1: Inserir (InserirArea)
            4.2: Atualizar (AtualizarArea)
            4.3: Eliminar (EliminarArea)
            4.4: Listar (ListarArea)
        5. AutenticacaoUtilizador


Implementação por formulário:

* FormPrincipal
    - Design: contém um MenuStrip com os itens mencionados anteriormente, tal como se apresentam nomeados.
    Deve-se mudar a propriedade IsMdiContainer para true, de forma a habilitar que os outros formulários apareçam dentro do form principal.
    (No menu strip existe um traço como separador que é adicionado clicando com o botão direito-inserir-separador. 
    Sublinhar as letras implica a utilização do caractere & antes da mesma e serve para selecionar a opção ao precionar a respetiva letra no teclado.)

    - Código: contém a inicialização dos formulários com os eventos de MenuItem_Click respetivos a cada um dos formulários.

* InserirFormandos
    - Design: Dividido em duas groupBox
        * ID, selecionado através de uma NumericUpDown;
        * 3 TextBox para definir dados do formando Nome, Morada e Contacto;
        * MaskedTextBox para o IBAN;
        * 3 RadioButtons para definir o género (Feminino, Masculino ou Outro);
        * MaskedTextBox e DateTimePiker para a data de nascimento;
        * ComboBox para a Nacionalidade;
        * 2 buttons (Cancelar e Gravar).

    - Código:
        * Classe InserirFormandos: Esta classe define o formulário que permite ao utilizador adicionar um novo formando
        * Evento InserirFormandos_Load: Este evento é chamado quando o formulário é carregado. 
        * Evento btnGravar_Click: É o evento associado ao botão "Gravar".
        * Evento dateTimePicker1_ValueChanged: Atualiza o campo de texto mtxtDataNascimento quando o valor do DateTimePicker é alterado.
        * Evento mtxtDataNascimento_TextChanged: Converte a data inserida no campo mtxtDataNascimento em um DateTime e define o valor do DateTimePicker.
        * Evento btnCancelar_Click: Fecha o formulário.
        * Método PesquisaNacionalidade: Este método consulta todas as nacionalidades disponíveis no banco de dados e preenche o ComboBox de nacionalidades com os resultados.
        * Método VerificarCampos: Verifica se todos os campos obrigatórios estão preenchidos corretamente.
        * Método Genero: Retorna o género selecionado (F, M ou O) com base nos botões de opção de género.
        * Método Limpar: Limpa todos os campos do formulário e redefine o valor do campo nudID com o último ID disponível no banco de dados.

* AtualizarFormandos
    - Design: Idêntico ao InserirFormandos, mas está dividido em 3 groupBox
        * ID, selecionado através de uma NumericUpDown;
        * 3 TextBox para definir dados do formando Nome, Morada e Contacto;
        * MaskedTextBox para o IBAN;
        * 3 RadioButtons para definir o género (Feminino, Masculino ou Outro);
        * MaskedTextBox e DateTimePiker para a data de nascimento;
        * ComboBox para a Nacionalidade;
        * 2 buttons (Cancelar e Atualizar).

    - Código:

* EliminarFormandos
    - Design: Idêntico ao AtualizarFormandos, mas está dividido em 3 groupBox
        * ID, selecionado através de uma NumericUpDown;
        * 3 TextBox para definir dados do formando Nome, Morada e Contacto;
        * MaskedTextBox para o IBAN;
        * 3 RadioButtons para definir o género (Feminino, Masculino ou Outro);
        * MaskedTextBox e DateTimePiker para a data de nascimento;
        * ComboBox para a Nacionalidade;
        * 2 buttons (Cancelar e Atualizar).

    - Código:

* ListarFormandos
    - Design:
    - Código:



* InserirNacionalidade
    - Design: 
        * Contém duas groupBox.
        * Na primeira constam 2 labels e 2 textBox. Os nomes das textBox são txtISO2 e txtNacionalidade~.
        * Na segunda constam 2 buttons, btnCancelar e btnInserir.

    - Código: 
        * Classe AdicionarNacionalidade: Esta classe define o formulário que permite aos usuários adicionar uma nova nacionalidade.
        * Inicialização de Conexão com Banco de Dados: O código cria uma instância da classe DBConnect que lida com a conexão com o banco de dados.
        * Evento Load do Formulário: Este evento é acionado quando o formulário é carregado. Ele define o foco no campo txtISO2.
        * Evento Click do botão "Gravar": Este evento é acionado quando o botão "Gravar" é clicado. 
        * Método VerificarCampos: Este método verifica se os campos de entrada estão preenchidos corretamente.
        * Método Limpar: Este método limpa os campos do formulário.
        * Evento Click do botão "Cancelar": Este evento é acionado quando o botão "Cancelar" é clicado.

* AtualizarNacionalidade
    - Design: Semelhante ao design do form InserirNacionalidade
        * Contém 3 group box
        * Na groupbox1 consta uma comboBox
        * Na groupbox2 constam 2 labels e 2 textBox. Os nomes das textBox são txtISO2 e txtNacionalidade.
        * Na groupBox3 constam 2 buttons, btnCancelar e btnAtualizar.

    - Código:
        * Classe AtualizarNacionalidade: Define o formulário que permite aos usuários atualizar uma nacionalidade.
        * Inicialização da Conexão com o Banco de Dados: Cria uma instância da classe DBConnect para lidar com a conexão com o banco de dados.
        * Evento Load do Formulário: Este evento é acionado quando o formulário é carregado. Chama o método PesquisaCampos e define os campos de entrada somente como leitura.
        * Evento SelectedIndexChanged do comboBox1: Este evento é acionado quando o item selecionado no comboBox1 é alterado. 
        * Método PesquisaCampos: Este método executa uma consulta SQL para obter todas as nacionalidades do banco de dados e preenche o comboBox1 com os resultados.
        * Método UpdateNacionalidade: Este método da classe DBConnect é responsável por executar a consulta SQL para atualizar a nacionalidade no banco de dados.
        * Método Limpar: Este método limpa os campos do formulário.
        * Evento Click do botão "Atualizar": Este evento é acionado quando o botão "Atualizar" é clicado.
        * Evento Click do botão "Cancelar": Este evento é acionado quando o botão "Cancelar" é clicado. Chama o método limpar e coloca o foco na comboBox.

* EliminarNacionalidade
    - Design: Idêntico ao de AtualizarNacionalidade, só muda um botão, passando de atualizar para Eliminar.
    - Código: Métodos identicos ao form anterior.
        * Classe EliminarNacionalidade: Define o formulário que permite aos utilizadores eliminar uma nacionalidade.
        * Inicialização da Conexão com o Banco de Dados: Cria uma instância da classe DBConnect para lidar com a conexão com o banco de dados.
        * Evento Load do Formulário: Este evento é acionado quando o formulário é carregado. 
        * Método PesquisaCampos: Este método executa uma consulta SQL para obter todas as nacionalidades do banco de dados e preenche o comboBox1 com os resultados.
        * Evento SelectedIndexChanged do comboBox1: Este evento é acionado quando o item selecionado no comboBox1 é alterado.
        * Evento Click do botão "Eliminar": Este evento é acionado quando o botão "Eliminar" é clicado. 
        * Método Limpar: Este método limpa os campos do formulário.
        * Evento Click do botão "Cancelar": Este evento é acionado quando o botão "Cancelar" é clicado.

* ListarNacionalidade
    - Design: 
        * DataGridView: mostrará a lista das nacionalidades
        * Label: nº de registos (lblRegistos)
        * 2 buttons: um de imprimir (button1) e outro de refresh (btnRefresh)

    - Código:
        * Evento Load do Formulário: Este evento é acionado quando o formulário é carregado.
        * Evento Click do botão "Refresh": Este evento é acionado quando o botão "Refresh" é clicado.
        * Evento Click do botão 1 "Imprimir": Este evento é acionado quando o botão "Imprimir" é clicado. 

* AutenticacaoUtilizador
    - Design:

    - Código: