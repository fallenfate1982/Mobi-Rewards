using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows;
using System.IO;
using System.Drawing.Imaging;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace MobileReward.Controllers
{
    public class testController : Controller
    {
      public ActionResult index(string barcode="gomen")
      {
        
        ViewData["data"]= barcode ;        
        return View();
      }

      [HttpPost]
      public ActionResult index()
      {

        
        return View();
      }

      public virtual FileResult BarcodeImage(String barcodeText)
      {
        // generating a barcode here. Code is taken from QrCode.Net library
        QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
        QrCode qrCode = new QrCode();
        qrEncoder.TryEncode(barcodeText, out qrCode);
        var renderer = new GraphicsRenderer(new FixedCodeSize(400, QuietZoneModules.Zero), Brushes.Black, Brushes.White);        

        // write to file if required for auditing
        //renderer.CreateImageFile(qrCode.Matrix, String.Format(@"d:\tmp\{0}.png", barcodeText), ImageFormat.Png);

        Stream memoryStream = new MemoryStream();
        renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, memoryStream);

        // very important to reset memory stream to a starting position, otherwise you would get 0 bytes returned
        memoryStream.Position = 0;

        var resultStream = new FileStreamResult(memoryStream, "image/png");
        resultStream.FileDownloadName = String.Format("{0}.png", barcodeText);

        return resultStream;
      }
        
        //
        // GET: /test/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /test/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /test/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /test/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /test/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /test/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /test/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
