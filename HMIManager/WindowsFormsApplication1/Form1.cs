using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EasyModbus;
using System.Data.SqlClient;
using System.Threading;

namespace testpanel
{
    public partial class Form1 : Form
    {
        int CounterList = 0;
        bool timerActive = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIP.Text != "")
                {
                    
                    ModbusClient svimaster = new ModbusClient(txtIP.Text, 502);
                    lblError.Text = "connect " + txtIP.Text;
                    svimaster.Connect();
                    int[] Main_ProductCode;
                    lblError.Text = "connect "+ txtIP.Text+" ok";
                    bool[] checkbit = svimaster.ReadCoils(0, 1);// Check bit send from HMI
                    //Read Data From HMI
                    if (checkbit[0] == true)
                    {
                        int[] Main_Product_Code = svimaster.ReadHoldingRegisters(2, 12);
                        int[] Weight = svimaster.ReadHoldingRegisters(105, 2);
                        int[] Datee = svimaster.ReadHoldingRegisters(101, 2);
                        int[] Timee = svimaster.ReadHoldingRegisters(103, 2);
                        int[] Oerator_num = svimaster.ReadHoldingRegisters(111, 1);
                        int[] Kind = svimaster.ReadHoldingRegisters(112, 1);//نوع محصول
                        int[] Samplee = svimaster.ReadHoldingRegisters(109, 2);//نمونه
                        int[] Order_numberr = svimaster.ReadHoldingRegisters(107, 2);
                        int[] Product_Code = svimaster.ReadHoldingRegisters(19, 12);
                        int[] Max_Address = svimaster.ReadHoldingRegisters(99, 1);//ماکزیمم آدرس استفاده شده
                        int[] Max_send = svimaster.ReadHoldingRegisters(100, 1);//ماکزیمم آدرس ارسال شده
                        float NetWeightHMI = ModbusClient.ConvertRegistersToFloat(Weight);//وزن 
                        float SampleHMI = ModbusClient.ConvertRegistersToFloat(Samplee);//نمونه
                        double DateHMI = ModbusClient.ConvertRegistersToDouble(Datee);//تاریخ
                        double TimeHMI = ModbusClient.ConvertRegistersToDouble(Timee);//ساعت
                        string productcodeHMI = ModbusClient.ConvertRegistersToString(Product_Code, 0, 12);//کد محصول
                        float Order_numberHMI = ModbusClient.ConvertRegistersToFloat(Order_numberr);//شماره سفارش
                        DateTime Date = Convert.ToDateTime("20" + DateHMI.ToString().Substring(0, 2) + "/" + DateHMI.ToString().Substring(2, 2) + "/" + DateHMI.ToString().Substring(4, 2) + " " + TimeHMI.ToString().Substring(2, 2) + ":" + TimeHMI.ToString().Substring(4, 2) + ":" + TimeHMI.ToString().Substring(6, 2));
                        /*Reply From SQL Sever
                        Status=0 Unable to Saved to SQL Server
                        Status=1 Save to SQL Server
                        Status=2 Error in Data*/
                       
                        int Status = 1;//جواب سرور
                        svimaster.WriteSingleRegister(113, Status);
                        lblError.Text = "  Product =" + productcodeHMI + "Net Weight=" + NetWeightHMI.ToString();
                        checkbit[0] = false;

                    }
                    // Write to Panels
                    int Remain_Product = 100;//محصول باقیمانده
                    svimaster.WriteSingleRegister(114, Remain_Product);
                    float Main_order = 1;//شماره سفارش از سروربه پنل
                    int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_order);
                    svimaster.WriteMultipleRegisters(0, floatreg);
                    string Main_prod_code = "uyuy";//کد کالا از سرور به پنل
                    Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                    svimaster.WriteMultipleRegisters(2, Main_ProductCode);
                }
                else
                {
                    lblError.Text = "Please Fill IP Address";
                }
            }
            catch
            {
                lblError.Text = "Error Read Data From Device";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456"))
                using (SqlCommand command = new SqlCommand("select * from Devices", connection))
                {
                    connection.Open();
                    listBox1.Items.Clear();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["IP"].ToString());

                        }
                    }
                }
                CounterList = 0;
            }
            catch
            {
                lblError.Text = "Error Read Device From Database";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (listBox1.Items.Count != 0 &  CounterList==0)
            {
                CounterList = listBox1.Items.Count;
            }
            try
            {
               if (CounterList != 0 & timerActive==true)
                {
                    ModbusClient svimaster = new ModbusClient(listBox1.Items[CounterList - 1].ToString(), 502);
                    lblError.Text = "Read Device " + listBox1.Items[CounterList - 1].ToString();
                    CounterList = CounterList - 1;
                }
            }
                
            catch
            {
                lblError.Text = "Error Read Data From Device";    
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456"))
                using (SqlCommand command = new SqlCommand("select * from Devices", connection))
                {
                    connection.Open();
                    listBox1.Items.Clear();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader["IP"].ToString());

                        }
                    }
                }
                CounterList = 0;
            }
            catch
            {
                lblError.Text = "Error Read Device From Database";
            }
        }

        private void btnStartAutoRead_Click(object sender, EventArgs e)
        {
            
            if(btnStartAutoRead.Text=="Start")
            {
                btnStartAutoRead.BackColor = Color.Red;
                timerActive = true;
                btnStartAutoRead.Text = "Stop";
               
            }  
            else
            {
                timerActive = false;
                btnStartAutoRead.BackColor = Color.White;
                btnStartAutoRead.Text = "Start";
                lblError.Text = "";
                 
            }
        }
    }
}
