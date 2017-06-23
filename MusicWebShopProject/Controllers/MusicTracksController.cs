using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicWebShopProject.Models;
using System.IO;

namespace MusicWebShopProject.Controllers
{
    public class MusicTracksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MusicTracks
        public ActionResult Index()
        {
            return View(db.MusicTracks.ToList());
        }

        // GET: MusicTracks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicTrack musicTrack = db.MusicTracks.Find(id);
            if (musicTrack == null)
            {
                return HttpNotFound();
            }
            return View(musicTrack);
        }

        // GET: MusicTracks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MusicTracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AlbumID,Name,Genre,Format,Description,Length,Price,TrackImgUrl,TrackFileUrl")] MusicTrack musicTrack, HttpPostedFileBase trackFile, HttpPostedFileBase TrackImg)
        {
            if (ModelState.IsValid)
            {
                var albumToAddTo = db.Albums.Find(Convert.ToInt32(musicTrack.AlbumID));

                if (trackFile.ContentLength > 0)
                {
                    byte[] audio = new byte[trackFile.ContentLength];
                    trackFile.InputStream.Read(audio, 0, audio.Length);
                    musicTrack.TrackFile = audio;
                }
                if (TrackImg.ContentLength > 0) {
                    byte[] image = new byte[TrackImg.ContentLength];
                    TrackImg.InputStream.Read(image, 0, image.Length);
                    musicTrack.TrackImg = image;
                }

                if (albumToAddTo != null)
                {
                    albumToAddTo.Tracks.Add(musicTrack);
                }

                db.MusicTracks.Add(musicTrack);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(musicTrack);
        }

        // GET: MusicTracks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicTrack musicTrack = db.MusicTracks.Find(id);
            if (musicTrack == null)
            {
                return HttpNotFound();
            }
            return View(musicTrack);
        }

        // POST: MusicTracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AlbumID,Name,Genre,Format,Description,Length,Price,TrackImgUrl,TrackImg,TrackFileUrl,TrackFile")] MusicTrack musicTrack)
        {
            if (ModelState.IsValid)
            {
                db.Entry(musicTrack).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musicTrack);
        }

        // GET: MusicTracks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MusicTrack musicTrack = db.MusicTracks.Find(id);
            if (musicTrack == null)
            {
                return HttpNotFound();
            }
            return View(musicTrack);
        }

        // POST: MusicTracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MusicTrack musicTrack = db.MusicTracks.Find(id);
            db.MusicTracks.Remove(musicTrack);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetAudio(int id)
        {
            MusicTrack trackInfo = db.MusicTracks.Find(id);
            byte[] audio = trackInfo.TrackFile;
            long fSize = audio.Length;
            long startbyte = 0;
            long endbyte = fSize - 1;
            int statusCode = 200;
            if ((Request.Headers["Range"] != null))
            {
                //Get the actual byte range from the range header string, and set the starting byte.
                string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                startbyte = Convert.ToInt64(range[1]);
                if (range.Length > 2 && range[2] != "") endbyte = Convert.ToInt64(range[2]);
                //If the start byte is not equal to zero, that means the user is requesting partial content.
                if (startbyte != 0 || endbyte != fSize - 1 || range.Length > 2 && range[2] == "")
                { statusCode = 206; }//Set the status code of the response to 206 (Partial Content) and add a content range header.                                    
            }
            long desSize = endbyte - startbyte + 1;
            //Headers
            Response.StatusCode = statusCode;

            Response.ContentType = "audio/mp3";
            Response.AddHeader("Content-Accept", Response.ContentType);
            Response.AddHeader("Content-Length", desSize.ToString());
            Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startbyte, endbyte, fSize));
            //Data

            var stream = new MemoryStream(audio, (int)startbyte, (int)desSize);

            return new FileStreamResult(stream, Response.ContentType);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
