using OgrenciYonetim;

namespace OgrenciWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IVeriKaynagi veriKaynagi;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected = comboBox1.SelectedItem.ToString();

            switch (selected)
            {
                case "SQLite":
                    veriKaynagi = new SQLiteOgrenci();
                    break;
                case "XML":
                    veriKaynagi = new XMLOgrenci();
                    break;
                default:
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ogrenci ogrenci = new Ogrenci
            {
                No = textBox4.Text,
                Adi = textBox1.Text,
                Soyadi = textBox2.Text,
                Bolum = textBox3.Text

            };

            if (veriKaynagi == null)
            {
                eksikVeriKaynagiHatasi();
            }
            else if (veriKaynagi.Kaydet(ogrenci, out string hataMesaji))
            {
                MessageBox.Show("Kayit Olusturuldu", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(hataMesaji, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (veriKaynagi == null)
            {
                eksikVeriKaynagiHatasi();
            }
            else if (veriKaynagi.Sil(textBox5.Text))
            {
                MessageBox.Show("Kayit Silindi", "Basarili", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Silerken Bir Hata Olustu", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(veriKaynagi != null) { 
                listBox1.Items.Clear(); 

                var veriler = veriKaynagi.VeriCek();
                foreach (var dict in veriler)
                {
                    foreach (var item in dict)
                    {
                        listBox1.Items.Add($"{item.Key}: {item.Value}");
                    }

                    listBox1.Items.Add("------------------------");
                }
            }
            else
            {
                eksikVeriKaynagiHatasi();
            }

        }


        void eksikVeriKaynagiHatasi()
        {
            MessageBox.Show("Lutfen Veri Kaynagi Seciniz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
