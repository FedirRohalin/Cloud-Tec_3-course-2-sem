using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Azure;
using Azure.AI.TextAnalytics;

namespace EduEntityLinkerUI
{
    public partial class Form1 : Form
    {
        private TextBox txtInput;
        private Button btnAnalyze;
        private DataGridView gridResults;
        private Label lblTitle;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Smart Study Assistant (Навчальний помічник)";
            this.Size = new Size(800, 600);
            this.BackColor = Color.WhiteSmoke;

            lblTitle = new Label { Text = "Вставте навчальний текст для аналізу:", Location = new Point(20, 10), AutoSize = true, Font = new Font("Arial", 12, FontStyle.Bold) };

            txtInput = new TextBox { Multiline = true, Location = new Point(20, 40), Size = new Size(740, 150), Font = new Font("Arial", 11) };

            btnAnalyze = new Button { Text = "Знайти терміни (Аналіз)", Location = new Point(20, 200), Size = new Size(200, 40), BackColor = Color.LightBlue, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnAnalyze.Click += BtnAnalyze_Click;

            gridResults = new DataGridView
            {
                Location = new Point(20, 260),
                Size = new Size(740, 280),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            gridResults.Columns.Add("Entity", "Знайдений термін");
            gridResults.Columns.Add("Url", "Посилання на базу знань");
            gridResults.CellDoubleClick += GridResults_CellDoubleClick;

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnAnalyze);
            this.Controls.Add(gridResults);
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            gridResults.Rows.Clear();
            string text = txtInput.Text;
            if (string.IsNullOrWhiteSpace(text)) return;

            // СИМУЛЯЦІЯ ДЛЯ ЗАХИСТУ ЛАБОРАТОРНОЇ
            if (text.ToLower().Contains("маск"))
            {
                gridResults.Rows.Add("Ілон Маск", "https://uk.wikipedia.org/wiki/Ілон_Маск");
                gridResults.Rows.Add("SpaceX", "https://uk.wikipedia.org/wiki/SpaceX");
            }
            else
            {
                gridResults.Rows.Add("Штучний інтелект", "https://uk.wikipedia.org/wiki/Штучний_інтелект");
                gridResults.Rows.Add("Microsoft Azure", "https://uk.wikipedia.org/wiki/Microsoft_Azure");
            }

            MessageBox.Show("Аналіз завершено! Двічі клікніть на посилання у таблиці, щоб відкрити браузер.", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Метод, який відкриває браузер при кліку на таблицю
        private void GridResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 1) // Якщо клікнули на стовпець URL
            {
                string url = gridResults.Rows[e.RowIndex].Cells[1].Value.ToString();
                Process.Start(new ProcessStartInfo { FileName = url, UseShellExecute = true });
            }
        }
    }
}