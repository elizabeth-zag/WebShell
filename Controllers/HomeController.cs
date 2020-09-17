using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebShell.Models;

namespace WebShell.Controllers
{
    public class HomeController : Controller
    {
        RequestContext db;
        public HomeController(RequestContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View(db.Requests.ToList());
        }

        public ActionResult Delete(int id)
        {
            Request b = db.Requests.Find(id);
            if (b != null)
            {
                db.Requests.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        public ActionResult Take(Request request)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            request.Text = request.Text != null ? request.Text : "";
            string[] input = request.Text.Split("%20");
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
                request.Text = request.Text.Replace("%20", " ");
                db.Requests.Add(request);
                db.SaveChanges();
                ViewBag.Message = output;
                return PartialView();
            }
            else if (erOutput.Length > 0)
            {
                request.Text = request.Text.Replace("%20", " ");
                db.Requests.Add(request);
                db.SaveChanges();
                ViewBag.Message = erOutput;
                return PartialView();
            }
            else
            {
                ViewBag.Message = "";
                return PartialView();
            }
            

        }
    }
}
