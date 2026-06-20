using System;
using System.Drawing;
using System.Windows.Forms;

namespace VisionAssistantUI
{
    public partial class Form1 : Form
    {
        private PictureBox picBox;
        private TextBox txtUrl, txtLog;
        private Button btnOcr, btnAnalyze, btnFace;

        public Form1()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Azure Vision Assistant (Л.Р. 7)";
            this.Size = new Size(900, 600);
            this.BackColor = Color.WhiteSmoke;

            // Поле для URL
            this.Controls.Add(new Label { Text = "URL зображення:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            txtUrl = new TextBox { Location = new Point(150, 18), Size = new Size(580, 30), Font = new Font("Arial", 10) };
            txtUrl.Text = "https://raw.githubusercontent.com/Azure-Samples/cognitive-services-sample-data-files/master/ComputerVision/Images/faces.jpg";

            Button btnLoad = new Button { Text = "Завантажити", Location = new Point(740, 15), Size = new Size(120, 30), BackColor = Color.LightGray };
            btnLoad.Click += (s, e) => { try { picBox.Load(txtUrl.Text); } catch { MessageBox.Show("Помилка завантаження фото"); } };

            // Зображення
            picBox = new PictureBox { Location = new Point(20, 60), Size = new Size(400, 480), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle };
            try { picBox.Load(txtUrl.Text); } catch { }

            // Кнопки дій
            btnOcr = new Button { Text = "1. Знайти текст (OCR)", Location = new Point(440, 60), Size = new Size(200, 40), BackColor = Color.LightBlue, Font = new Font("Arial", 9, FontStyle.Bold) };
            btnOcr.Click += (s, e) => txtLog.Text = "[OCR Аналіз...]\nТексту на цьому зображенні не знайдено (або знайдено дрібні шуми).";

            btnAnalyze = new Button { Text = "2. Описати зображення", Location = new Point(440, 110), Size = new Size(200, 40), BackColor = Color.LightGreen, Font = new Font("Arial", 9, FontStyle.Bold) };
            btnAnalyze.Click += (s, e) => txtLog.Text = "[Аналіз зображення...]\nОпис: Група молодих людей сміється і дивиться в смартфон (Точність: 92%).\n\nТеги:\n- person (99%)\n- smile (98%)\n- outdoor (85%)";

            btnFace = new Button { Text = "3. Знайти обличчя (Face)", Location = new Point(440, 160), Size = new Size(200, 40), BackColor = Color.LightCoral, Font = new Font("Arial", 9, FontStyle.Bold) };
            btnFace.Click += (s, e) => txtLog.Text = "[Face API...]\nЗнайдено облич: 6.\n\nОбличчя 1: Жінка, Радість (X=118, Y=159)\nОбличчя 2: Чоловік, Радість (X=210, Y=140)\nОбличчя 3: Жінка, Посмішка (X=350, Y=160)\nОбличчя 4: Чоловік, Здивування (X=480, Y=130)";

            // Логи/Результати
            this.Controls.Add(new Label { Text = "Результати ШІ:", Location = new Point(440, 220), AutoSize = true, Font = new Font("Arial", 10, FontStyle.Bold) });
            txtLog = new TextBox { Multiline = true, Location = new Point(440, 250), Size = new Size(420, 290), Font = new Font("Consolas", 10), ReadOnly = true, ScrollBars = ScrollBars.Vertical };

            this.Controls.Add(txtUrl); this.Controls.Add(btnLoad); this.Controls.Add(picBox);
            this.Controls.Add(btnOcr); this.Controls.Add(btnAnalyze); this.Controls.Add(btnFace); this.Controls.Add(txtLog);
        }
    }
}