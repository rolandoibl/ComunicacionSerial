using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

//El siguiente código implementa la comunicación serial y escritura de un DataGriedView con los datos recibidos
//en tiempo real desde un sensor de Arduino, falta implementar la escritura e importación de archivoc ".csv" 

//22
namespace ejemplo_comunicacionSerial
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        //Se crea nuestro objeto tipo SerialPort el cual guardara los datos recibidos de nuestro puerto serial
        SerialPort serialPort = new SerialPort();


        /// <summary>
        /// Función que nos provee de las propiedades del puerto serial y la lectura de datos del puerto
        /// </summary>
        public void ComunicacionPuertoSerie()
        {
            // Asignamos las propiedades
            serialPort.BaudRate = 9600;  //Velocidad en Baudios
            serialPort.PortName = "COM5";  //Nombre del puerto

            // Creamos el evento para recibir datos
            serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);

            // Controlamos que el puerto indicado esté operativo
            try
            {
                // Abrimos el puerto serie
                serialPort.Open();
            }
            catch
            {

            }

        }

        /// <summary>
        /// Evento que inicia el recibo de datos del puerto serial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Obtenemos el puerto serie que lanza el evento
            //SerialPort currentSerialPort = (SerialPort)sender;

            // Leemos el dato recibido del puerto serie
            /*string inData = currentSerialPort.ReadLine();
            string[] datos = inData.Split(',');
            label1.Text = datos[0];
            label2.Text = datos[1];
            label3.Text = datos[2];
            */
            //Cuenta el número de columnas

            tmrAdquisicion.Start(); //Iniciamos el timer para escribir en el dataGriedView

        }

        /// <summary>
        /// Función que imprime los datos en un dataGriedView
        /// </summary>
        /// <param name="inData">Cadena con datos separados por una coma</param>
        private void imprimirDatos(string inData)
        {
            string[] datos = inData.Split(',');
            dataGridView1.Rows.Add(datos); //Agregamos los datos a una fila nueva del dataGridView
        }

        /// <summary>
        /// Función de un reloj para actualizar un dataGriedView con datos leídos en tiempo real
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrAdquisicion_Tick(object sender, EventArgs e)
        {
            // dt.Rows.Add(time, lbSpd.Text, lbAlt.Text, lbLat.Text, lbLng.Text);
            string inData = serialPort.ReadLine(); //Guardamos los datos leídos del puerto en una cadena
            int columnLength = inData.Split(',').Length; //Guardamos el # de datos recibidos por lectura para conocer el # de columnas
            dataGridView1.ColumnCount = columnLength;  //Fijamos el número de columnnas que habrá por lectura
            imprimirDatos(inData); //Se llama la función para imprimir en un dataGriedView
        }

        #region Botones
        /// <summary>
        /// Botón que activa la función ComunicacionPuertoSeria para comenzar la lectura e impresión de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeer_Click(object sender, EventArgs e)
        {
            ComunicacionPuertoSerie();
        }

        /// <summary>
        /// Botón que cierra el puerto con la comunicación serial
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetener_Click(object sender, EventArgs e)
        {
            serialPort.Close();
        }
        #endregion

        
    }
}
