
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

    namespace BMP_Console {

        public sealed class logger {
            private static volatile logger instance;
            private static object syncRoot = new Object();
            private static StreamWriter sw;
            private static FileStream fs;
            private static FileInfo finfo;
            private static int counter;
            private static string logfilename;

            private logger() {
                counter = 0;
                //fn = "Error log " + System.Diagnostics.Process.GetCurrentProcess().Id.ToString() + ".txt";
                logfilename = Form1.strErrorLog;
                fs = new FileStream(logfilename, FileMode.Append, FileAccess.Write);
                sw = new StreamWriter(fs);
                finfo = new FileInfo(logfilename);
            }

            public static logger Instance {
                get {
                    if (instance == null) {
                        lock (syncRoot) {
                            if (instance == null)
                                instance = new logger();
                        }
                    }

                    return instance;
                }
            }
            public void write(string data) {

                lock (syncRoot) {
                    if (sw.BaseStream.Length > (Form1.nLogFilesSize)) // 10MByte                             
                    {
                        try {
                            sw.Close();
                            string suffix = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()
                                            + " " + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString();
                            //System.IO.File.Move(logfilename, logfilename + "--" + suffix);
                            int lenght = finfo.Name.Length;
                            //string driveLetter = finfo.Directory.ToString().Substring(0, 1);
                            //string back_logfilename = driveLetter + @":\Logs\NCPDPSniffer\" + finfo.Name.Substring(0, lenght - 4) + "--" + suffix + ".log";
                            string back_logfilename = @".\Logs\" + finfo.Name.Substring(0, lenght - 4) + "--" + suffix + ".log";
                            System.IO.File.Move(logfilename, back_logfilename);
                            fs = new FileStream(logfilename, FileMode.Append, FileAccess.Write);
                            sw = new StreamWriter(fs);
                            finfo = new FileInfo(logfilename);
                        } catch (Exception e) {
                            Console.WriteLine("Exception caught at LogRename: " + e.Message);
                            sw.Write(DateTime.Now.ToString() + "::Exception caught at LogRename: " + e.Message + Environment.NewLine);
                        }
                    }
                    //sw.Write(DateTime.Now.ToString() + "::" + data + Environment.NewLine);
                    sw.Write(DateTime.Now.ToString() + "::" + data + Environment.NewLine);
                    //Console.WriteLine(DateTime.Now.ToString() + "::" + data);
                    sw.Flush();
                    counter++;
                }

            }
            public void closelog(string closing_data) {
                //Console.WriteLine(counter.ToString());
                //logger.Instance.write("Main::Stopping gracefully.");
                sw.Write(closing_data);
                sw.Flush();
                sw.Close();
                fs.Close();
                if (counter == 0)
                    System.IO.File.Delete(logfilename);
            }

        }

    }



