using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace son
{
    public partial class Form1 : Form
    {
        //sql ba�lant�s� i�in komutlar
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
            //apimin ba�lant�s�
            string connection = "http://api.openweathermap.org/data/2.5/weather?q=istanbul&mode=xml&lang=tr&units=metric&appid=35e94a0fea42d6ba4a7c413a392728e4";

            XDocument weather = XDocument.Load(connection);
            //s�cakl�k de�eri, bas�n�,g�ne� y�kseli�i ve �ehiri tan�mlad�m.
            var temp  =  weather.Descendants("temperature").ElementAt(0).Attribute("value").Value;
            var pre   = weather.Descendants("pressure").ElementAt(0).Attribute("value").Value;
            var gunes = weather.Descendants("sun").ElementAt(0).Attribute("rise").Value;
            var hiz   = weather.Descendants("speed").ElementAt(0).Attribute("value").Value;
            var sehir = weather.Descendants("city").ElementAt(0).Attribute("name").Value;
            // Buras� butona t�kland���nda apiden �ekti�i veriyi database' e kaydediyor.
            cmd = new SqlCommand();
            con.Open();
            MessageBox.Show("Ba�ar�yla eklendi.");
            cmd.Connection = con;
            cmd.CommandText = "insert into durum(s�cakl�k,bas�n�,gunes,h�z,sehir) values (" + temp + ",'" + pre + "','" + gunes+ "','" + hiz + "','"+ sehir+"')";
            cmd.ExecuteNonQuery();
            con.Close();
            // bu method veritaban�n�n datagridview da g�sterilmesini sa�lar.
            giris();


            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //a��l�� ekran�nda datagridview da database verilerini �a��rd�m.
            giris();    
        }
        void giris()
        {
            //sql ba�lant�s�n� kurdum.
            con = new SqlConnection("server=localhost; Database=havadurumu; Integrated Security=true;");
            // tablomu se�tim.
            da = new SqlDataAdapter("Select * From durum", con);
            ds = new DataSet();
            con.Open();

            da.Fill(ds, "durum");
            //datagridview ad� neyse onu yazd�m.

            dataGridView1.DataSource = ds.Tables["durum"];
            con.Close();
        }
    }
}