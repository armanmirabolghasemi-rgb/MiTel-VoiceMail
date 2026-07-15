using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Genration.Data;
using System.Diagnostics;

namespace Genration.Controllers
{
    public class VoiceController : Controller
    {
        private VoiceRepository repository = new VoiceRepository();

        public ActionResult Index()
        {
            var list = repository.GetVoiceMessages();
            return View(list);
        }

        // Player
        public ActionResult Play(string referenceCode)
        {
            var voice = repository
                .GetVoiceMessages()
                .FirstOrDefault(x => x.RefrenceCode == referenceCode);

            if (voice == null)
            {
                return new HttpStatusCodeResult(404);
            }

            if (string.IsNullOrEmpty(voice.VoicePath))
            {
                return new HttpStatusCodeResult(404);
            }

            if (!System.IO.File.Exists(voice.VoicePath))
            {
                return new HttpStatusCodeResult(404);
            }

            return File(voice.VoicePath, "audio/wav");
        }

        [HttpPost]
        public ActionResult UploadReplyVoice(HttpPostedFileBase voiceFile, string referenceCode)
        {
            if (voiceFile != null && voiceFile.ContentLength > 0)
            {
                string folder = @"\\192.168.0.221\ReplayVoice";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string tempFile = Path.Combine(folder, "Temp_" + referenceCode + ".wav");

                voiceFile.SaveAs(tempFile);

                string fileName = referenceCode + ".wav";

                string outputFile = Path.Combine(folder, fileName);

                string ffmpegPath = @"C:\Users\myhome\Desktop\Cs projects\Genration\FFmpeg\ffmpeg.exe";

                Process process = new Process();

                process.StartInfo.FileName = ffmpegPath;

                //16bit 8000gh mono
                process.StartInfo.Arguments =
                    $"-y -i \"{tempFile}\" -acodec pcm_mulaw -ar 8000 -ac 1 \"{outputFile}\"";

                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;

                process.Start();

                process.WaitForExit();

                if (System.IO.File.Exists(tempFile))
                {
                    System.IO.File.Delete(tempFile);
                }

                repository.UpdateReplyVoice(referenceCode, fileName);
            }

            return RedirectToAction("Index");
        }
    }
}