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
using Modbus;
using System.Diagnostics;

namespace testpanel
{
    public partial class Form1 : Form
    {
        long SuccessCheckHMI = 0;
        long wrongRead = 0;
        long SuccessInsert = 0;
        DateTime startTime;

        ModbusClient svimaster = new ModbusClient();
        SqlConnection connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456");
        SqlCommand command;
        /// شمارنده لیست باکس ای پی ها
        int CounterList = 0;
        int CounterListSql = 0;
        /// برای فعال یا غیر فعال کردن تایمر
        bool timerActive = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        ///لود کردن ای پی ها به صورت دستی
        private void button2_Click(object sender, EventArgs e)
        {
        }

        ///تایمر مربوط به خواندن هر دستگاه
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timerActive == true)
            {
                try
                {
                    connection = new SqlConnection(@"Data Source=192.168.1.11\Towzin;Initial Catalog=Towzin;User ID=towzin;Password=123456;MultipleActiveResultSets=true");
                    connection.Close();
                    connection.Open();

                    /// مربوط به الگوریتم خواندن کلیه ای پی ها از لیست 
                    if (ListIP.Items.Count != 0 & CounterList == 0)
                    {
                        CounterList = ListIP.Items.Count;
                        CounterListSql = CounterList;
                    }


                    if (CounterList != 0 & timerActive == true)
                    {
                        lblCorrect.Text = (SuccessCheckHMI = SuccessCheckHMI + 1).ToString();
                        ///کد مر بوط به پینگ کردن ای پی مقصد
                        Ping p = new Ping();
                        PingReply r;
                        string s;
                        s = ListIP.Items[CounterList - 1].ToString();


                        r = p.Send(s, 1);


                        if (r.Status == IPStatus.Success)
                        {


                            lblStatus.Text = "connect " + ListIP.Items[CounterList - 1].ToString();
                            svimaster.ConnectionTimeout = 1;
                            s = svimaster.ToString();

                            svimaster.Connect(ListIP.Items[CounterList - 1].ToString(), 502);

                            //  ListErrors.Items.Add(CounterList.ToString() + "s" + DateTime.Now.TimeOfDay.Seconds.ToString() + ":" + DateTime.Now.TimeOfDay.Milliseconds.ToString());
                            int[] Main_ProductCode;
                            // lblStatus.Text = "connect " + ListIP.Items[CounterList - 1].ToString() + " ok";
                            bool[] checkbit = svimaster.ReadCoils(0, 1);// Check bit send from HMI
                                                                        //Read Data From HMI
                            if (checkbit[0] == true)
                            {
                                ///خواندن اطلاعات از HMI
                                ///{
                                ///int[] productCodeHMI = svimaster.ReadHoldingRegisters(2, 12);
                                int[] amountHMI = svimaster.ReadHoldingRegisters(105, 2);//مقدار واحد اول
                                int[] amount1HMI = svimaster.ReadHoldingRegisters(109, 2);//مقدار واحد دوم
                                int[] dateHMI = svimaster.ReadHoldingRegisters(101, 2);//تاریخ اضافه شدن
                                int[] timeHMI = svimaster.ReadHoldingRegisters(103, 2);//ساعت اضافه شدن
                                int[] opratorCodeHMI = svimaster.ReadHoldingRegisters(111, 1);//کد اپراتور

                                ///1 محصول
                                ///2-49 ضایعات
                                ///50-100 توقف
                                int[] kindHMI = svimaster.ReadHoldingRegisters(112, 1);//نوع محصول
                                int[] sourceOrderCodeHMI = svimaster.ReadHoldingRegisters(107, 2);//کدسفارش مبدا
                                int[] sourceProductCodeHMI = svimaster.ReadHoldingRegisters(19, 15);///کد کالای مبدا
                                int[] destinationOrderCodeHMI = svimaster.ReadHoldingRegisters(118, 2);///کد سفارش مقصد
                                int[] destinationProductCodeHMI = svimaster.ReadHoldingRegisters(40, 15);//کد کالای مقصد
                                ///}


                                ////تبدیل باینری به متغیر های قابل استفاده
                                ////{
                                float amount = ModbusClient.ConvertRegistersToFloat(amountHMI);//وزن 
                                float amount1 = ModbusClient.ConvertRegistersToFloat(amount1HMI);//نمونه
                                double dateInsert = ModbusClient.ConvertRegistersToDouble(dateHMI);//تاریخ
                                double timeInsert = ModbusClient.ConvertRegistersToDouble(timeHMI);//ساعت
                                float sourceOrderCode = ModbusClient.ConvertRegistersToFloat(sourceOrderCodeHMI);//کد شماره سفارش مبدا
                                string sourceProductCode = ModbusClient.ConvertRegistersToString(sourceProductCodeHMI, 0, 15);//کد محصول مبدا
                                float destinationOrderCode = ModbusClient.ConvertRegistersToFloat(destinationOrderCodeHMI);//شماره سفارش مقصد
                                string destinationPartCode = ModbusClient.ConvertRegistersToString(destinationProductCodeHMI, 0, 15);//کد محصول مقصد
                                DateTime dateTime;
                                if (dateInsert > 999999)
                                {
                                    dateTime = Convert.ToDateTime("20" + dateInsert.ToString().Substring(1, 2) + "-" + dateInsert.ToString().Substring(3, 2) + "-" + dateInsert.ToString().Substring(5, 2) + " " + timeInsert.ToString().Substring(2, 2) + ":" + timeInsert.ToString().Substring(4, 2) + ":" + timeInsert.ToString().Substring(6, 2));
                                }
                                else
                                {
                                    dateTime = Convert.ToDateTime("20" + dateInsert.ToString().Substring(0, 2) + "-" + dateInsert.ToString().Substring(2, 2) + "-" + dateInsert.ToString().Substring(4, 2) + " " + timeInsert.ToString().Substring(2, 2) + ":" + timeInsert.ToString().Substring(4, 2) + ":" + timeInsert.ToString().Substring(6, 2));
                                }
                                ////}


                                /////چک کردن اطلاعات دریافتی با دیتابیس جهت تایید اطلاعات گرفته شده
                                ////{
                                bool partError = false;
                                bool orderError = false;
                                int status = 0; //جواب سرور دیتابیس


                                ////چک کردن کد سفارش مبدا در دیتا بیس
                                ////{
                                command = new SqlCommand("SELECT OrderCode FROM [Order] where OrderCode=" + sourceOrderCode.ToString(), connection);
                                var tempSourceOrderCode = command.ExecuteScalar();
                                if (tempSourceOrderCode == null)
                                {
                                    sourceOrderCode = 99999;
                                //    orderError = true;
                                }
                                ////}

                                ///چک کردن کد سفارش مقصد در دیتا بیس
                                ///{
                                //شرط جهت چک کردن اینکه اصلا کد سفارش مقصد وارد شده است یا نه
                                if (destinationOrderCode > 0)
                                {
                                    //check Order Code Destination
                                    command = new SqlCommand("SELECT OrderCode FROM [Order] where OrderCode=" + destinationOrderCode.ToString(), connection);
                                    var tempDestinationOrderCode = command.ExecuteScalar();

                                    if (tempDestinationOrderCode == null)
                                    {
                                        destinationOrderCode = 99999;
                                        //orderError = true;
                                    }

                                }

                                ///}


                                ////چک کردن خالی بودن کد کالای مبدا
                                ///{
                                int temp = sourceProductCode.IndexOf("\0");
                                if (temp < 0)
                                {
                                    sourceProductCode = sourceProductCode.Substring(0, 11);
                                }

                                else if (temp == 0)
                                {
                                    sourceProductCode = "99999";
                                }

                                else if (temp > 0)
                                {
                                    sourceProductCode = sourceProductCode.Substring(0, temp);
                                }

                                ////}


                                ////چک کردن کد کالای مقصد
                                ///{
                        //        temp = destinationPartCode.IndexOf("\0");
                          //      if (temp < 0)
                            //    {
                              //      destinationPartCode = destinationPartCode.Substring(0, 11);
                                //}
                                //else if (temp == 0)
                                //{
                                    
                                  //      destinationPartCode = "99999";
                                    
                                    

                               // }
                              //  else if (temp > 0)
                            //    {
                          //          destinationPartCode = destinationPartCode.Substring(0, temp);
                         //       }
                                ////}




                                ////چک کردن کد کالای سفارش مبدا در دیتا بیس   
                                ////{                             
                                long sourcePartID = 0;
                                if (sourceProductCode != "99999")
                                {
                                    command = new SqlCommand("select ID from Part where PartCode='" + sourceProductCode + "'", connection);
                                    var tempPartID = command.ExecuteScalar();
                                    ////آیا کد کالا در دیتابیس وجود دارد
                                    if (tempPartID != null)
                                    {
                                        sourcePartID = (long)tempPartID;
                                    }
                                    else
                                    {
                                        ///در صورتی که وجود ندارد کد پیش فرض تعلق میگیرد
                                        sourcePartID = 10011;
                                    }
                                }
                                else
                                {
                                    sourcePartID = 10011;
                                }
                                ///}


                               
                                //long destinationpartid = 0;
                                /////چک کردن کد کالای سفارش مقصد
                                //if (destinationpartcode.trim() != "")
                                //{
                                //    command = new sqlcommand("select id from part where partcode='" + destinationpartcode + "'", connection);
                                //    var temppartid = command.executescalar();
                                //    ////آیا کد کالا در دیتابیس وجود دارد
                                //    if (temppartid != null)
                                //    {
                                //        destinationpartid = (long)temppartid;
                                //    }
                                //    else
                                //    {
                                //        ///در صورتی که وجود ندارد کد پیش فرض تعلق میگیرد
                                //        destinationpartid = 10011;
                                //    }

                                //}
                                //else
                                //{
                                //    destinationpartid = 10011;
                                //}
                                    
                                ////}پایان چک کردن در دیتا بیس 
                               

                                ////گرفتن کد کالای ضایعاتی در صورتی وجود کد کالای مبدا
                                /////{
                                long partWasteID = 0;
                                if (sourcePartID != 10011)
                                {
                                    command = new SqlCommand("select PartWesteID from Part where ID='" + sourcePartID + "'", connection);
                                    if (command.ExecuteScalar() != null)
                                    {

                                            var tempPartWasteID = command.ExecuteScalar();
                                            ////آیا کد کالای ضایعاتی در دیتابیس وجود دارد
                                            if (tempPartWasteID != null)
                                            {
                                                partWasteID = (long)tempPartWasteID;
                                            }

                                        }
                                    
                                    if (partWasteID == 0)
                                    {
                                        ///در صورتی که وجود ندارد کد پیش فرض تعلق میگیرد
                                        partWasteID = 30025;
                                    }
                                }
                                else
                                {
                                    partWasteID = 30025;
                                }
                                ////}



                                string Creator = "2b2f093d-19c0-4abd-b4b8-512cdacd97ab";
                                string modifier = "2b2f093d-19c0-4abd-b4b8-512cdacd97ab";

                                ///بررسی اینکه تا الان رکوردی با این زمان درج شده است یا خبر
                                command = new SqlCommand("SELECT OrderCodeID FROM ProductiveDetails where AddDate='" + dateTime + "'", connection);
                                var tempProductiveDetails = command.ExecuteScalar();

                                ///بررسی اینکه تا الان رکوردی با این زمان در جدول توقفات درج شده است یا خبر
                                command = new SqlCommand("SELECT OrderCodeID FROM ProductiveStoppages where AddDate='" + dateTime + "'", connection);
                                var tempProductiveStopages = command.ExecuteScalar();

                                ////گرفتن شماره خط تولید از ای پی
                                string ProductionLineID = ListProductionLine.Items[CounterList - 1].ToString();
                                string destinationProductionLineID = "";
                                command = new SqlCommand("SELECT ProductionLineID FROM [Order] where OrderCode=" + destinationOrderCode + "", connection);
                                var tempDestinationProductionLineID = command.ExecuteScalar();
                                if(tempDestinationProductionLineID!=null)
                                {
                                    destinationProductionLineID = tempDestinationProductionLineID.ToString();
                                }
                                else
                                {
                                    destinationProductionLineID = "20006";
                                }

                                if (kindHMI[0] == 1 & tempProductiveDetails == null)
                                {
                                    int Result1 = 1;
                                    int Result = 1;
                                    if (destinationOrderCode == 0)
                                    {
                                        command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,OperatorID,IO,Waste,Amount,Amount1,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + sourceOrderCode + "," + ProductionLineID + "," + sourcePartID + "," + "10006" + "," + 1 + "," + 0 + "," + amount + "," + amount1 + "," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);
                                    }
                                    else
                                    {
                                        //with Destination Code
                                        //command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,OperatorID,IO,Waste,Amount,Amount1,ToOrderCodeID,ToPartID,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + sourceOrderCode + "," + ProductionLineID + "," + sourcePartID + "," + "10006" + "," + 1 + "," + 0 + "," + amount + "," + amount1 + "," + destinationOrderCode + "," + destinationPartID + "," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);

                                        command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,OperatorID,IO,Waste,Amount,Amount1,ToOrderCodeID,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + sourceOrderCode + "," + ProductionLineID + "," + sourcePartID + "," + "10006" + "," + 1 + "," + 0 + "," + amount + "," + amount1 + "," + destinationOrderCode + "," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);
                                        Result1 = command.ExecuteNonQuery();
                                        command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,OperatorID,IO,Waste,Amount,Amount1,FromOrderCodeID,FromPartID,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + destinationOrderCode + "," + destinationProductionLineID + "," + sourcePartID + "," + "10006" + "," + 0 + "," + 0 + "," + amount + "," + amount1 + "," + sourceOrderCode + "," + sourcePartID + "," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);
                                        Result = command.ExecuteNonQuery();
                                    }
                                        

                                        if (Result != 0 & orderError == false & partError == false & Result1 != 0)
                                        {
                                            status = 1;
                                        }
                                        else
                                        {
                                            status = 2;
                                        }
                                    
                                }
                                else if (kindHMI[0] >= 2 & kindHMI[0] <= 49 & tempProductiveDetails == null)
                                {
                                    command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,ProductionLineID,PartID,FromPartID,OperatorID,IO,Waste,Amount,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + sourceOrderCode + "," + ProductionLineID + "," + partWasteID + "," + sourcePartID + "," + "10006" + "," + 1 + "," + 1 + "," + amount + "," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);

                                    int Result = command.ExecuteNonQuery();

                                    if (Result != 0 & partError == false & orderError == false)
                                    {
                                        status = 1;
                                    }
                                    else
                                    {
                                        status = 2;
                                    }

                                }

                                else if (kindHMI[0] >= 50 & kindHMI[0] <= 100 & tempProductiveStopages == null)
                                {


                                    command = new SqlCommand("SELECT ID FROM Stoppages where Description='" + kindHMI[0] + "'", connection);

                                    var tempStoppagesID = command.ExecuteScalar();
                                    long StoppagesID = 0;
                                    if (tempStoppagesID == null)
                                    {
                                        StoppagesID = 99;

                                    }
                                    else
                                    {

                                        StoppagesID = (long)tempStoppagesID;
                                    }

                                    if (kindHMI[0] < 100)
                                    {
                                        command = new SqlCommand("INSERT INTO [dbo].[ProductiveStoppages] ([StoppagesID],[OrderCodeID],[OperatorID],[StartTime],[State],[Creator],[AddDate],[LastModifier],[LastModificationDate]) VALUES(" + StoppagesID + "," + sourceOrderCode + "," + "10006" + ",'" + dateTime + "'," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);
                                    }
                                    else
                                    {
                                        command = new SqlCommand("INSERT INTO [dbo].[ProductiveStoppages] ([StoppagesID],[OrderCodeID],[OperatorID],[EndTime],[State],[Creator],[AddDate],[LastModifier],[LastModificationDate]) VALUES(" + StoppagesID + "," + sourceOrderCode + "," + "10006" + ",'" + dateTime + "'," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);
                                    }
                                    int Result = command.ExecuteNonQuery();

                                    if (Result != 0 & partError == false)
                                    {
                                        status = 1;
                                    }
                                    else
                                    {
                                        status = 2;
                                    }

                                }
                                else if (kindHMI[0] == 101 & tempProductiveDetails == null)
                                {

                                    command = new SqlCommand("insert into ProductiveDetails (OrderCodeID,PartID,OperatorID,IO,Waste,Amount,State,Creator,AddDate,LastModifier,LastModificationDate) VALUES (" + sourceOrderCode + "," + sourcePartID + "," + "10006" + "," + 0 + "," + 0 + "," + amount * (-1) + "," + 1 + ",'" + Creator + "','" + dateTime + "','" + modifier + "','" + dateTime + "')", connection);

                                    int Result = command.ExecuteNonQuery();

                                    if (Result != 0 & partError == false & orderError == false)
                                    {
                                        status = 1;
                                    }
                                    else
                                    {
                                        status = 2;
                                    }

                                }


                                if (kindHMI[0] == 0)
                                {
                                    status = 2;
                                }


                                if (tempProductiveDetails != null || tempProductiveStopages != null)
                                {
                                    status = 1;
                                }

                                SuccessInsert = SuccessInsert + 1;
                                lblRead.Text = SuccessInsert.ToString();

                                /*Reply From SQL Sever
                                Status=0 Unable to Saved to SQL Server
                                Status=1 Save to SQL 
                                Status=2 Error in Data*/
                                svimaster.WriteSingleRegister(113, status);
                                checkbit[0] = false;
                                ///}
                                ///پایان خواندن از HMI
                            }

                            long OrderCodeSource = 0;
                            long OrderCodeDestination = 0;
                            ///خواندن اینکه دستگاه تازه روشن شده است یا خیر اگه تازه روشن شده باشد 0 است
                            bool[] RestartHMI = svimaster.ReadCoils(5, 1);

                            command = new SqlCommand("SELECT SendOrderCode FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                            var varSendOrderCode = command.ExecuteScalar();
                            bool SendOrderCode = (bool)varSendOrderCode;
                            if (SendOrderCode == true || RestartHMI[0] == false)
                            {
                                ////ارسال لیست شماره شماره سفارش مبدا
                                int addressOrder = 371;
                                int addressLine = 299;
                                command = new SqlCommand("SELECT OrderCode,ProductionLineLatinName FROM vwDeviceOrders where IP='" + ListIP.Items[CounterList - 1].ToString() + "' and SendOrderCode=1", connection);

                                using (var reader = command.ExecuteReader())
                                {

                                    while (reader.Read())
                                    {
                                        OrderCodeSource = (long)reader.GetInt64(reader.GetOrdinal("OrderCode"));
                                        float Main_order = OrderCodeSource;//شماره سفارش از سروربه پنل
                                        int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_order);
                                        svimaster.WriteMultipleRegisters(addressOrder, floatreg);
                                        addressOrder = addressOrder + 2;
                                        string Main_ProductionLineLatinName = (string)reader.GetString(reader.GetOrdinal("ProductionLineLatinName"));//شماره سفارش از سروربه پنل
                                        int[] ProductionLineLatinName = ModbusClient.ConvertStringToRegisters(Main_ProductionLineLatinName);
                                        svimaster.WriteMultipleRegisters(addressLine, ProductionLineLatinName);
                                        addressLine = addressLine + 12;
                                    }
                                }
                                //command = new SqlCommand("Update Devices set SendOrderCode=0 where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                                //command.ExecuteNonQuery();
                            }

                            /////ارسال لیست شماره سفارش مقصد
                            command = new SqlCommand("SELECT SendDestinationOrder FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                            var varSendDestinationOrder = command.ExecuteScalar();
                            bool SendDestinationOrder = (bool)varSendDestinationOrder;

                            if (SendDestinationOrder == true || RestartHMI[0] == false)
                            {
                                int addressLine = 399;
                                int addressOrder = 519;


                                command = new SqlCommand("SELECT OrderCode,ProductionLineLatinName FROM vwDeviceOrders where IP!='" + ListIP.Items[CounterList - 1].ToString() + "' and Region=" + ListRegion.Items[CounterList - 1].ToString(), connection);
                            
                                using (var reader = command.ExecuteReader())
                                {

                                    while (reader.Read())
                                    {
                                        OrderCodeDestination = (long)reader.GetInt64(reader.GetOrdinal("OrderCode"));
                                        float Main_order = OrderCodeDestination;//شماره سفارش از سروربه پنل
                                        int[] floatreg = ModbusClient.ConvertFloatToTwoRegisters(Main_order);
                                        svimaster.WriteMultipleRegisters(addressOrder, floatreg);
                                        addressOrder = addressOrder + 2;

                                        string Main_ProductionLineLatinName = (string)reader.GetString(reader.GetOrdinal("ProductionLineLatinName"));//شماره سفارش از سروربه پنل
                                        int[] ProductionLineLatinName = ModbusClient.ConvertStringToRegisters(Main_ProductionLineLatinName);
                                        svimaster.WriteMultipleRegisters(addressLine, ProductionLineLatinName);
                                        addressLine = addressLine + 12;
                                    }
                                }
                                //command = new SqlCommand("Update Devices set SendDestinationOrder=0 where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                                //command.ExecuteNonQuery();
                            }


                            //////نوشتن شماره گاری و وزن آنها رو HMI
                            command = new SqlCommand("SELECT SendGari FROM Devices where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                            var varSendGari = command.ExecuteScalar();
                            bool SendGari = (bool)varSendGari;


                           if (SendGari == true || RestartHMI[0]==false)

                            //if (SendGari == true)
                            {
                                int addressContainerCode = 800;
                                int addressContainerNetWeight = 600;
                                command = new SqlCommand("SELECT ContainerCode,NetWeight FROM Container", connection);

                                using (var reader = command.ExecuteReader())
                                {

                                    while (reader.Read())
                                    {
                                        int ContainerCode = reader.GetInt32(reader.GetOrdinal("ContainerCode"));
                                        svimaster.WriteSingleRegister(addressContainerCode, ContainerCode);


                                        float Main_NetWeight = (float)reader.GetDouble(reader.GetOrdinal("NetWeight"));
                                        int[] NetWeight = ModbusClient.ConvertFloatToTwoRegisters(Main_NetWeight);
                                        svimaster.WriteMultipleRegisters(addressContainerNetWeight, NetWeight);

                                        addressContainerCode = addressContainerCode + 1;
                                        addressContainerNetWeight = addressContainerNetWeight + 2;
                                    }
                                }
                                command = new SqlCommand("Update Devices set SendGari=0 where IP='" + ListIP.Items[CounterList - 1].ToString() + "'", connection);
                                command.ExecuteNonQuery();

                                ///ست کردن بیت ارسال مجدد گاری برای بارگزاری مجدد دیتا از دیتا بیس
                                svimaster.WriteSingleCoil(7, true);
                            }
                       




                        ////ارسال کد کالاهای سفارش مبدا در صورت تغییر در سفارش پنل 

                        bool[] ChangeOrderCodeSource = svimaster.ReadCoils(4, 1);//تغییری در کد سفارش اتفاق افتاده است یا خیر : خیر=0

                            if (ChangeOrderCodeSource[0] == true)
                            {
                                int[] OrderCodeSourceChange = svimaster.ReadHoldingRegisters(0, 2);
                                OrderCodeSource = (long)ModbusClient.ConvertRegistersToFloat(OrderCodeSourceChange);


                                int Address = 200;

                                using (connection)
                                using (command = new SqlCommand("select * from vwOrderParts where OrderCode=" + OrderCodeSource.ToString(), connection))
                                {
                                    {
                                        ///پاک کردن ردیف کالاها
                                        ///for (int j = 199; j <= 251; j = j + 13)
                                        ///{
                                        // svimaster.WriteMultipleRegisters(j, ModbusClient.ConvertStringToRegisters("            "));
                                        ///}


                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                string PartCodeSource = (string)reader.GetString(reader.GetOrdinal("PartCode"));
                                                string Main_prod_code = PartCodeSource;//کد کالا از سرور به پنل
                                                Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                                                svimaster.WriteMultipleRegisters(Address, Main_ProductCode);
                                                Address = Address + 15;
                                            }

                                        }
                                    }

                                    svimaster.WriteSingleCoil(4, false);
                                }
                            }




                            ////ارسال کد کالاهای سفارش مقصد در صورت تغییر در سفارش پنل 

                            bool[] ChangeOrderCodeDestination = svimaster.ReadCoils(6, 1);//تغییری در کد سفارش اتفاق افتاده است یا خیر : خیر=0

                            if (ChangeOrderCodeDestination[0] == true)
                            {
                                int[] OrderCodeDestinationChange = svimaster.ReadHoldingRegisters(549, 2);
                                OrderCodeDestination = (long)ModbusClient.ConvertRegistersToFloat(OrderCodeDestinationChange);


                                int addressPartCode = 1100;
                                int addressPartName = 1300;
                                using (connection)
                                using (command = new SqlCommand("select * from vwOrderParts where OrderCode=" + OrderCodeDestination.ToString(), connection))
                                {
                                    {
                                        ////پاک کردن ردیف کالاها
                                        ///for (int j = 199; j <= 251; j = j + 13)
                                        ///{
                                        /// svimaster.WriteMultipleRegisters(j, ModbusClient.ConvertStringToRegisters("                "));
                                        ///}


                                        using (SqlDataReader reader = command.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                string tempSourcePartCode = (string)reader.GetString(reader.GetOrdinal("PartCode"));
                                                ///string Main_prod_code = tempSourcePartCode;//کد کالا از سرور به پنل
                                                Main_ProductCode = ModbusClient.ConvertStringToRegisters(tempSourcePartCode);
                                                svimaster.WriteMultipleRegisters(addressPartCode, Main_ProductCode);
                                                addressPartCode = addressPartCode + 15;

                                                string tempSourcePartName = (string)reader.GetString(reader.GetOrdinal("LatinName"));
                                                string Main_prod_code = tempSourcePartName;//کد کالا از سرور به پنل
                                                Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                                                svimaster.WriteMultipleRegisters(addressPartName, Main_ProductCode);
                                                addressPartName = addressPartName + 12;
                                            }
                                        }
                                    }

                                    svimaster.WriteSingleCoil(6, false);
                                }
                            }





                            //string Main_prod_code = "uyuy";//کد کالا از سرور به پنل
                            // Main_ProductCode = ModbusClient.ConvertStringToRegisters(Main_prod_code);
                            // svimaster.WriteMultipleRegisters(2, Main_ProductCode);               
                            lblStatus.Text = "Read Device " + ListIP.Items[CounterList - 1].ToString();
                            ///مربوط به الگوریتم خواندن کلیه ای پی ها از لیست
                           
                           
                                svimaster.WriteSingleCoil(5, true);
                            svimaster.WriteSingleCoil(8, true);


                        }
                        CounterList = CounterList - 1;
                        //// برای قسمت تازه روشن دستگاه و یک کردن آن


                        connection.Close();
                        svimaster.Disconnect();
                        int i = DateTime.Compare(DateTime.Now, startTime);
                        TimeSpan avgTime = DateTime.Now.Subtract(startTime);
                        lblAvarage.Text = (avgTime.TotalSeconds / SuccessCheckHMI).ToString();
                    }

                    connection.Close();
                    svimaster.Disconnect();
                    
                }


                catch (Exception ex)
                {

                    ListErrors.Items.Add(ListIP.Items[CounterList - 1].ToString() + ex.Message);
                    connection.Close();
                    svimaster.Disconnect();
                    lblWrong.Text = (wrongRead = wrongRead + 1).ToString();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Close();
                connection.Open();
                using (connection)
                using (command = new SqlCommand("select * from Devices", connection))
                {

                    ListIP.Items.Clear();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListIP.Items.Add(reader["IP"].ToString());
                            ListRegion.Items.Add(reader["Region"].ToString());
                            ListProductionLine.Items.Add(reader["ProductionLineID"].ToString());
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


            if (btnStartAutoRead.Text == "Start")
            {

                SuccessCheckHMI = 0;
                startTime = DateTime.Now;
                btnStartAutoRead.BackColor = Color.Red;
                timerActive = true;
                btnStartAutoRead.Text = "Stop";
                /*                var worker = new BackgroundWorker();
                                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                                worker.RunWorkerAsync();*/

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

        private void ListIP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
