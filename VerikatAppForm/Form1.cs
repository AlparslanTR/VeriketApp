using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VerikatAppForm
{
    public partial class Form1 : Form
    {
        private string logFilePath = $"C:\\Users\\byblu\\Desktop\\VeriketApp\\VeriketAppService\\bin\\Debug\\Logs\\VeriketAppTest_{DateTime.Now.Date.ToString("yyyy-MM-dd")}.txt";
        public Form1()
        {
            InitializeComponent();
            InitializeDataGrid();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InitializeDataGrid()
        {
            // DataGrid sütunlarını tanımla
            dataGridView1.Columns.Add("Tarih", "Tarih");
            dataGridView1.Columns.Add("BilgisayarAdi", "Bilgisayar Adı");
            dataGridView1.Columns.Add("KullaniciAdi", "Kullanıcı Adı");

            // Sütunları genişlet
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ReadAndPopulateDataGrid(string filePath)
        {
            try
            {
                // CSV dosyasındaki satırları oku
                List<string> lines = File.ReadAllLines(filePath).ToList();

                // Her bir satırı DataGrid'e ekle
                foreach (string line in lines)
                {
                    string[] values = line.Split(',');
                    dataGridView1.Rows.Add(values);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata oluştu: {ex.Message}");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Log dosyasındaki satırları oku ve DataGrid'e ekle
            ReadAndPopulateDataGrid(logFilePath);
        }
    }
}
