using System;
using System.Drawing;
using System.Windows.Forms;

namespace MedicalAnonymizerUI
{
    public partial class Form1 : Form
    {
        private TextBox txtInput, txtMasked;
        private Button btnAnalyze;
        private DataGridView gridPii, gridHealth;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Step Lab - Автоматична Анонімізація та Аналіз (GDPR)";
            this.Size = new Size(950, 750);
            this.BackColor = Color.WhiteSmoke;

            // Вхідний текст
            this.Controls.Add(new Label { Text = "Оригінальний запит клієнта (з PII):", Location = new Point(20, 10), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            txtInput = new TextBox { Multiline = true, Location = new Point(20, 35), Size = new Size(900, 80), Font = new Font("Arial", 10) };
            txtInput.Text = "Пацієнтка Марія Коваленко (093-111-22-33). Діагноз: п'яткова шпора. Призначено: 3D-друк устілок із супінатором. Ліки: Німесил 100мг.";

            // Кнопка
            btnAnalyze = new Button { Text = "Анонімізувати та Структурувати", Location = new Point(20, 125), Size = new Size(250, 35), BackColor = Color.LightGreen, Font = new Font("Arial", 10, FontStyle.Bold) };
            btnAnalyze.Click += BtnAnalyze_Click;

            // Замаскований текст
            this.Controls.Add(new Label { Text = "Знеособлений текст (безпечно для передачі інженерам):", Location = new Point(20, 175), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            txtMasked = new TextBox { Multiline = true, Location = new Point(20, 200), Size = new Size(900, 80), Font = new Font("Arial", 10), ReadOnly = true, BackColor = Color.LightYellow };

            // Таблиці
            this.Controls.Add(new Label { Text = "Знайдена та прихована PII:", Location = new Point(20, 295), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            gridPii = CreateGrid(20, 320, 430, 350);
            gridPii.Columns.Add("Entity", "Особа/Контакт");
            gridPii.Columns.Add("Type", "Тип");

            this.Controls.Add(new Label { Text = "Структуровані медичні дані (для CAD/CAM):", Location = new Point(480, 295), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            gridHealth = CreateGrid(480, 320, 440, 350);
            gridHealth.Columns.Add("Entity", "Медичний термін");
            gridHealth.Columns.Add("Type", "Категорія");

            this.Controls.Add(txtInput); this.Controls.Add(btnAnalyze); this.Controls.Add(txtMasked);
            this.Controls.Add(gridPii); this.Controls.Add(gridHealth);
        }

        private DataGridView CreateGrid(int x, int y, int w, int h)
        {
            return new DataGridView { Location = new Point(x, y), Size = new Size(w, h), AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, AllowUserToAddRows = false };
        }

        private void BtnAnalyze_Click(object sender, EventArgs e)
        {
            gridPii.Rows.Clear(); gridHealth.Rows.Clear();

            // Симуляція обробки Azure
            txtMasked.Text = txtInput.Text.Replace("Марія Коваленко", "***************").Replace("093-111-22-33", "*************");

            gridPii.Rows.Add("Марія Коваленко", "Person");
            gridPii.Rows.Add("093-111-22-33", "Phone Number");

            gridHealth.Rows.Add("п'яткова шпора", "Diagnosis");
            gridHealth.Rows.Add("3D-друк устілок", "Treatment");
            gridHealth.Rows.Add("супінатором", "Body Structure / Modification");
            gridHealth.Rows.Add("Німесил", "Medication");
            gridHealth.Rows.Add("100мг", "Dosage");
        }
    }
}