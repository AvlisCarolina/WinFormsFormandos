using iTextSharp.text.pdf;
using iTextSharp.text;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsBDGestaoFormandos;

namespace WindowsFormsFormandos
{
    public partial class ListarFormandos : Form
    {
        DBConnect ligacao = new DBConnect();
        public ListarFormandos()
        {
            InitializeComponent();
        }

        private void ListarFormandos_Load(object sender, EventArgs e)
        {
            PesquisaNacionalidade();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;

            dataGridView1.Columns.Add("codID", "ID");
            dataGridView1.Columns.Add("Nome", "Nome");
            dataGridView1.Columns.Add("Iban", " IBAN");
            dataGridView1.Columns.Add("Contacto", "Contacto");
            dataGridView1.Columns.Add("Genero", "Género");
            dataGridView1.Columns.Add("Nacionalidade", "Nacionalidade");

            ligacao.PreencherDataGridViewFormandos(ref dataGridView1);

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();

            rbTodos.Checked = true;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            txtNome.Text = "";
            rbTodos.Checked = true;
            cbNacionalidade.Text = string.Empty;

            ligacao.PreencherDataGridViewFormandos(ref dataGridView1);

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();

        }

        private void btnPesquisa_Click(object sender, EventArgs e)
        {

            dataGridView1.Rows.Clear();

            txtNome.Text = Geral.TirarEspacos(txtNome.Text);
            string[] texto2 = cbNacionalidade.Text.Split(new Char[] { '-' });
            string id_nacionalidade = texto2[0];

            ligacao.PreencherDataGridViewFormandosPesquisa(ref dataGridView1, Genero(), txtNome.Text, id_nacionalidade);

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Formandos.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("Impossível de apagar o ficheiro!");
                        }
                    }
                    //if (!fileError == true)
                    //if (fileError == false)
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfPTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfPTable.DefaultCell.Padding = 3;
                            pdfPTable.WidthPercentage = 100;
                            pdfPTable.HorizontalAlignment = Element.ALIGN_LEFT;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                pdfPTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfPTable.AddCell(cell.Value.ToString());
                                }
                            }

                            //using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))

                            FileStream stream = new FileStream(sfd.FileName, FileMode.Create);
                            //{
                            Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            pdfDoc.Add(pdfPTable);
                            pdfDoc.Close();
                            stream.Close();
                            //}

                            MessageBox.Show("Imprimiu com sucesso!");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("ERROR: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Não existe registos!");
            }
        }

        public bool PesquisaNacionalidade()
        {
            cbNacionalidade.Items.Clear();
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
        }
    }
}
