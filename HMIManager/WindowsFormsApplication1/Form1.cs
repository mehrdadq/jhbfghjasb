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
using System.Net.NetworkInformation;

namespace testpanel
{
    public partial class Form1 : Form
    {

        SqlConnection connection= new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456");
        SqlCommand command;
        /// شمارنده لیست باکس ای پی ها
        int CounterList = 0;
        /// برای فعال یا غیر فعال کردن تایمر
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
                    lblStatus.Text = "connect " + txtIP.Text;
                    svimaster.Connect();
                    int[] Main_ProductCode;
                    lblStatus.Text = "connect "+ txtIP.Text+" ok";
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
                       // int[] Max_Address = svimaster.ReadHoldingRegisters(99, 1);//ماکزیمم آدرس استفاده شده
                        //int[] Max_send = svimaster.ReadHoldingRegisters(100, 1);//ماکزیمم آدرس ارسال شده
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
                        lblStatus.Text = "  Product =" + productcodeHMI + "Net Weight=" + NetWeightHMI.ToString();
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
                    lblStatus.Text = "Please Fill IP Address";
                }
            }
            catch
            {
                lblStatus.Text = "Error Read Data From Device";
            }
        }

        ///لود کردن ای پی ها به صورت دستی
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (connection)
                using (SqlCommand command = new SqlCommand("select * from Devices", connection))
                {
                    connection.Open();
                    ListIP.Items.Clear();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListIP.Items.Add(reader["IP"].ToString());

                        }
                    }
                    
                }
                CounterList = 0;
            }
            catch
            {
                lblStatus.Text = "Error Read Device From Database";
            }
        }

        ///تایمر مربوط به خواندن هر دستگاه
        private void timer1_Tick(object sender, EventArgs e)
        {
            try { 
            connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456;MultipleActiveResultSets=true");
            connection.Close();
            connection.Open();
                /// مربوط به الگوریتم خواندن کلیه ای پی ها از لیست 
                if (ListIP.Items.Count != 0 & CounterList == 0)
                {
                    CounterList = ListIP.Items.Count;
                }           
            
               if (CounterList != 0 & timerActive==true)
                {

            ///کد مر بوط به پینگ کردن ای پی مقصد
                        Ping p = new Ping();
                        PingReply r;
                        string s;
                        s = ListIP.Items[CounterList - 1].ToString();
                        r = p.Send(s);
               

                if (r.Status == IPStatus.Success)
                {
                        ModbusClient svimaster = new ModbusClient(ListIP.Items[CounterList - 1].ToString(), 502);
                        lblStatus.Text = "connect " + ListIP.Items[CounterList - 1].ToString();
                        svimaster.ConnectionTimeout = 100;
                        svimaster.Connect();
                        
                    int[] Main_ProductCode;
                   // lblStatus.Text = "connect " + ListIP.Items[CounterList - 1].ToString() + " ok";
                    bool[] checkbit = svimaster.ReadCoils(0, 1);// Check bit send from HMI
                    //Read Data From HMI
                    if (checkbit[0] == true)
                    {

                        int[] Main_Product_Code = svimaster.ReadHoldingRegisters(2, 12);
                        int[] Weight = svimaster.ReadHoldingRegisters(105, 2);
                        int[] Datee = svimaster.ReadHoldingRegisters(101, 2);
                        int[] Timee = svimaster.ReadHoldingRegisters(103, 2);
                        int[] Oerator_num = svimaster.ReadHoldingRegisters(111, 1);

                        ///1 محصول
                        ///2-49 ضایعات
                        ///50-100 توقف
                        int[] Kind = svimaster.ReadHoldingRegisters(112, 1);//نوع محصول

                        ///کد نمونه
                        int[] Samplee = svimaster.ReadHoldingRegisters(109, 2);//نمونه

                        ///کد سفارش
                        int[] Order_numberr = svimaster.ReadHoldingRegisters(107, 2);

                        ///کد محصول
                        int[] Product_Code = svimaster.ReadHoldingRegisters(19, 12);


                        ////در صورتی که ماکزیمم آدرس استفاده شده با ماکزیمم آدرس ارسال شده برابری کند 
                        ////یعنی دستگاه اطلاعاتی برای ارسال ندارد

                       int[] Max_Address = svimaster.ReadHoldingRegisters(99, 1);//ماکزیمم آدرس استفاده شده
                        int[] Max_send = svimaster.ReadHoldingRegisters(100, 1);//ماکزیمم آدرس ارسال شده


                     
                        float NetWeightHMI = ModbusClient.ConvertRegistersToFloat(Weight);//وزن 
                        float SampleHMI = ModbusClient.ConvertRegistersToFloat(Samplee);//نمونه
                        double DateHMI = ModbusClient.ConvertRegistersToDouble(Datee);//تاریخ
                        double TimeHMI = ModbusClient.ConvertRegistersToDouble(Timee);//ساعت
                        string productcodeHMI = ModbusClient.ConvertRegistersToString(Product_Code, 0, 12);//کد محصول
                        float Order_numberHMI = ModbusClient.ConvertRegistersToFloat(Order_numberr);//شماره سفارش
                        DateTime DateFromHMI;

                                          
                            
                        if (DateHMI > 999999)
                        {
                            DateFromHMI= Convert.ToDateTime("20" + DateHMI.ToString().Substring(1, 2) + "-" + DateHMI.ToString().Substring(3, 2) + "-" + DateHMI.ToString().Substring(5, 2) + " " + TimeHMI.ToString().Substring(2, 2) + ":" + TimeHMI.ToString().Substring(4, 2) + ":" + TimeHMI.ToString().Substring(6, 2));
                        }
                        else
                        {
                            DateFromHMI = Convert.ToDateTime("20" + DateHMI.ToString().Substring(0, 2) + "-" + DateHMI.ToString().Substring(2, 2) + "-" + DateHMI.ToString().Substring(4, 2) + " " + TimeHMI.ToString().Substring(2, 2) + ":" + TimeHMI.ToString().Substring(4, 2) + ":" + TimeHMI.ToString().Substring(6, 2));
                        }

                        
                        int temp=productcodeHMI.IndexOf("\0");
                        productcodeHMI = productcodeHMI.Substring(0, temp);
                             
                        if (productcodeHMI == "\0\0\0\0\0\0\0\0\0\0\0\0")
                        {
                            productcodeHMI = "99999";
                        }



                           command = new SqlCommand("select ID from Part where PartCode='" + productcodeHMI + "'", connection);

                        var varPartID = command.ExecuteScalar();
                        long PartID=0;
                        if (varPartID!=null)
                        {
                             PartID = (long)varPartID;
                        }
                        else
                        {
                            PartID = 10011;
                        }

                        int status = 0; //جواب سرور دیتابیس
                        if (PartID == 0)
                        {
                            PartID = 10011;
                            status = 2;
                        }


                        if (Order_numberHMI == 0)
                        {
                            Order_numberHMI = 99999;
                            status = 2;
                        }


                        string Creator = "2b2f093d-19c0-4abd-b4b8-512cdacd97ab";
                        string modifier = "2b2f093d-19c0-4abd-b4b8-512cdacd97ab";


                            if (Kind[0] == 1)
                            {
                                command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,PartID,OperatorID,IO,Waste,Amount,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + Order_numberHMI + "," + PartID + "," + "10006" + "," + 1 + "," + 0 + "," + NetWeightHMI + "," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);

                                int Result = command.ExecuteNonQuery();

                                if (Result != 0 & status == 0)
                                {
                                    status = 1;
                                }
                                else if (status == 0)
                                {
                                    status = 0;
                                }

                            }
                            if (Kind[0] >= 2 & Kind[0] <= 49)
                            {
                                command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,PartID,OperatorID,IO,Waste,Amount,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + Order_numberHMI + "," + PartID + "," + "10006" + "," + 1 + "," + 1 + "," + NetWeightHMI + "," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);

                                int Result = command.ExecuteNonQuery();

                                if (Result != 0 & status == 0)
                                {
                                    status = 1;
                                }
                                else if (status == 0)
                                {
                                    status = 0;
                                }

                            }
                            if (Kind[0] >= 50 & Kind[0] <= 100)
                            {

                 
                                 command = new SqlCommand("SELECT ID FROM Stoppages where Description='" + Kind[0] + "'", connection);
                 
                                var VarStoppagesID = command.ExecuteScalar();
                          
                                long StoppagesID = (long)VarStoppagesID;


                                if (Kind[0] < 100)
                                {
                                    command = new SqlCommand("INSERT INTO [dbo].[ProductiveStoppages] ([StoppagesID],[OrderCodeID],[OperatorID],[StartTime],[State],[Creator],[AddDate],[LastModifier],[LastModificationDate]) VALUES(" + StoppagesID + "," + Order_numberHMI + "," + "10006" + ",'" + DateFromHMI + "'," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);
                                }
                                else
                                {
                                    command = new SqlCommand("INSERT INTO [dbo].[ProductiveStoppages] ([StoppagesID],[OrderCodeID],[OperatorID],[EndTime],[State],[Creator],[AddDate],[LastModifier],[LastModificationDate]) VALUES(" + StoppagesID + "," + Order_numberHMI + "," + "10006" + ",'" + DateFromHMI + "'," + 1 + ",'" + Creator + "','" + DateFromHMI + "','" + modifier + "','" + DateFromHMI + "')", connection);
                                }
                                int Result = command.ExecuteNonQuery();

                                if (Result != 0 & status == 0)
                                {
                                    status = 1;
                                }
                                else if (status == 0)
                                {
                                    status = 0;
                                }

                            }

                       

                        /*Reply From SQL Sever
                        Status=0 Unable to Saved to SQL Server
                        Status=1 Save to SQL 
                        Status=2 Error in Data*/


                        svimaster.WriteSingleRegister(113, status);
                      
                        checkbit[0] = false;

                    }
                        // Write to Panels
                        //  int Remain_Product = 100;//محصول باقیمانده
                        // svimaster.WriteSingleRegister(114, Remain_Product);
                        long ProductionLineID = 0;
                    long OrderCode=0;
                        int[] SendOrderCode= svimaster.ReadHoldingRegisters(5, 1);//کد سفارش به صورت دستی است یا اتواتیک 0=اتوماتیک
                        if (SendOrderCode[0] == 0)
                        {
                 
                            command = new SqlCommand("SELECT ProductionLineID FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "' and SendOrderCode=1", connection);
                 
                            var VarProductionLineID = command.ExecuteScalar();

                            ProductionLineID = (long)VarProductionLineID;
                            if (ProductionLineID != 0)
                            {
                 
                                command = new SqlCommand("SELECT OrderCode FROM [Order] where ProductionLineID=" + ProductionLineID + " and OrderStatusID=2", connection);
                 
                                var VarOrderCode = command.ExecuteScalar();

                               OrderCode = (long)VarOrderCode;


                                float Main_order = OrderCode;//شماره سفارش از سروربه پنل
                                int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_order);
                            svimaster.WriteMultipleRegisters(0, floatreg);
                            }
                        }

                        int[] ChangeOrderCode = svimaster.ReadHoldingRegisters(4, 1);//تغییری در کد سفارش اتفاق افتاده است یا خیر : خیر=0
                        if (ChangeOrderCode[0] == 0)
                        {
                            int Address = 199;

                 
                  



                            using (connection)
                            using ( command = new SqlCommand("select * from OrderPart where OrderCodeID=" + OrderCode.ToString() , connection))
                            {
                                connection.Close();
                                connection.Open();
                                
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        
                                        command = new SqlCommand("SELECT PartCode FROM Part where ID=" + reader["PartID"].ToString(), connection);
                                        var VarPartCode = command.ExecuteScalar();

                                        string PartCode = (string)VarPartCode;
                                       
                                       
                                        string Main_prod_code = PartCode;//کد کالا از سرور به پنل
                                         Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                                        svimaster.WriteMultipleRegisters(Address, Main_ProductCode);
                                        Address=Address+13;
                                        
                                    }
                                    if(Address==199)
                                    {
                                        svimaster.WriteMultipleRegisters(199, ModbusClient.ConvertStringToRegisters(""));
                                    }
                                    if (Address == 212)
                                    {
                                        svimaster.WriteMultipleRegisters(212, ModbusClient.ConvertStringToRegisters(""));
                                    }
                                    if (Address == 225)
                                    {
                                        svimaster.WriteMultipleRegisters(225, ModbusClient.ConvertStringToRegisters(""));
                                    }
                                    if (Address == 238)
                                    {
                                        svimaster.WriteMultipleRegisters(238, ModbusClient.ConvertStringToRegisters(""));
                                    }
                                    if (Address == 251)
                                    {
                                        svimaster.WriteMultipleRegisters(251, ModbusClient.ConvertStringToRegisters(""));
                                    }
                                }
                            }
                        }
                            //string Main_prod_code = "uyuy";//کد کالا از سرور به پنل
                            // Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                            // svimaster.WriteMultipleRegisters(2, Main_ProductCode);               
                            lblStatus.Text = "Read Device " + ListIP.Items[CounterList - 1].ToString();
                        ///مربوط به الگوریتم خواندن کلیه ای پی ها از لیست
                    
                }
                    CounterList = CounterList - 1;
                }
           }
                
            catch(Exception ex)
            {
                ListErrors.Items.Add(ex.Message);
               
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Close();
                connection.Open();
                using (connection )
                using (command = new SqlCommand("select * from Devices", connection))
                {
                    
                    ListIP.Items.Clear();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListIP.Items.Add(reader["IP"].ToString());

                        }
                    }
                }
                CounterList = 0;
            }
            catch
            {
                lblStatus.Text = "Error Read Device From Database";
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
                lblStatus.Text = "";
                 
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            
            SqlCommand command = new SqlCommand("select ID from Part where PartCode='" + "35wr66" + "'", connection);
            
            long ID = (long)command.ExecuteScalar();
        }
    }
}
