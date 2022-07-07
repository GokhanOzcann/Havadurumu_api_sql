using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace son
{
    public partial class Form1 : Form
    {
        //sql baðlantýsý için komutlar
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader dr;
        SqlDataAdapter da;

        DataSet ds;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // apimin keyi
            string api = "35e94a0fea42d6ba4a7c413a392728e4";
            //apimin baðlantýsý
            string connection = "http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=35e94a0fea42d6ba4a7c413a392728e4";

            XDocument weather = XDocument.Load(connection);
            //sýcaklýk deðeri, basýnç,güneþ yükseliþi ve þehiri tanýmladým.
            var temp  =  weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            var pre   = weather.Descendants("pressure").ElementAt(0).Attribute("value").Value;
            var gunes = weather.Descendants("sun").ElementAt(0).Attribute("rise").Value;
            var hiz   = weather.Descendants("speed").ElementAt(0).Attribute("value").Value;
            var sehir = weather.Descendants("city").ElementAt(0).Attribute("name").Value;
            // Burasý butona týklandýðýnda apiden çektiði veriyi database' e kaydediyor.
            cmd = new SqlCommand();
            con.Open();
            MessageBox.Show("Baþarýyla eklendi.");
            cmd.Connection = con;
            cmd.CommandText = "insert into durum(sýcaklýk,basýnç,gunes,hýz,sehir) values (" + temp + ",'" + pre + "','" + gunes+ "','" + hiz + "','"+ sehir+"')";
            cmd.ExecuteNonQuery();
            con.Close();
            // bu method veritabanýnýn datagridview da gösterilmesini saðlar.
            giris();


            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //açýlýþ ekranýnda datagridview da database verilerini çaðýrdým.
            giris();    
        }
        void giris()
        {
            //sql baðlantýsýný kurdum.
            con = new SqlConnection("server=localhost; Database=havadurumu; Integrated Security=true;");
            // tablomu seçtim.
            da = new SqlDataAdapter("Select * From durum", con);
            ds = new DataSet();
            con.Open();

            da.Fill(ds, "durum");
            //datagridview adý neyse onu yazdým.

            dataGridView1.DataSource = ds.Tables["durum"];
            con.Close();
        }
    }
}