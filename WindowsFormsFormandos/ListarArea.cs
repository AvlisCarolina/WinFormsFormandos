using iTextSharp.text.pdf;
using iTextSharp.text;
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
    public partial class ListarArea : Form
    {
        DBConnect ligacao = new DBConnect();
        public ListarArea()
        {
            InitializeComponent();
        }

        private void ListarArea_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;

            dataGridView1.Columns.Add("id_area", "ID");
            dataGridView1.Columns.Add("area", "Area");

            ligacao.PreencherDataGridViewArea(ref dataGridView1);

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();

            /*
            Este evento configura o DataGridView para exibir as colunas "ID", "ALF2" e "Nacionalidade" 
            e preenche o DataGridView com os dados das nacionalidades utilizando o método PreencherDataGridViewNacionalidade da classe DBConnect.
            */
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            ligacao.PreencherDataGridViewArea(ref dataGridView1);

            lblRegistos.Text = "Nº Registos: " + dataGridView1.RowCount.ToString();

            /*
            Este evento limpa o DataGridView e preenche novamente com os dados atualizados das nacionalidades.
            Também atualiza a contagem de nº de registos.
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            Este evento inicia um SaveFileDialog para que o usuário escolha onde salvar o arquivo PDF. 
            Se o usuário confirmar a seleção, os dados do DataGridView são exportados para um arquivo PDF utilizando a biblioteca iTextSharp.
            O código cria um objeto PdfPTable e adiciona cada célula do DataGridView ao PDF. 
            Em seguida, ele cria um documento PDF utilizando a biblioteca iTextSharp e adiciona a tabela ao documento. 
            Finalmente, o documento é fechado e salvo no local especificado pelo usuário.
            */

            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "AreaFormadores.pdf";
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
    }
}
